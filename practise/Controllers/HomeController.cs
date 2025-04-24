using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practise.data;
using practise.Models;

namespace practise.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            //宣告_context=連線字串
            _context = context;
        }

        public IActionResult Index()
        {
            string? userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserName = userName;
            var dailyItems = _context.Daily.Where(d => d.UserName == userName).ToList();
            return View(dailyItems);
        }

        public IActionResult GetData(string type)
        {
            string? userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userName))
            {
                return Content("請先登入");
            }

            switch (type)
            {
                case "daily":
                    var dailyItems = _context.Daily.Where(d => d.UserName == userName).ToList();
                    return PartialView("_DailyPartial", dailyItems); // 返回 PartialView 的 HTML

                case "weekly":
                    var weeklyItems = _context.Weekly.Where(d => d.UserName == userName).ToList();
                    return PartialView("_WeeklyPartial", weeklyItems);
            }
            return Content("沒有資料");
        }

        //[FromBody]將獲取資料的方式改為Ajax
        //且轉換為DailyContext(自定義的Model)類型
        //再將資料置於updatedDaily(自定義參數)參數
        [HttpPost]
        public IActionResult Checked([FromBody] DailyContext updatedDaily)
        {
            //SQL:SELECT TOP 1 * FROM Daily WHERE Id = @updatedDaily.Id;
            var daily = _context.Daily.FirstOrDefault(d => d.Id == updatedDaily.Id);
            if (daily == null)
            {
                return NotFound("Record not found.");
            }

            //SQL:UPDATE Daily SET ItemStatus = @updatedDaily.ItemStatus WHERE Id = @Id;
            daily.ItemStatus = updatedDaily.ItemStatus;
            _context.SaveChanges();

            return Ok("Item updated successfully.");
        }

        [HttpPost]
        public IActionResult Create([FromBody] DailyContext newItem)
        {
            if (string.IsNullOrWhiteSpace(newItem.ItemName))
            {
                return BadRequest("Item name cannot be empty.");
            }

            string? userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("User is not logged in.");
            }

            newItem.UserName = userName;

            //SQL:INSERT INTO Daily (Id, ItemName, ItemStatus, UserName ,ResetTime)
            //VALUES(@Id, @ItemName, @ItemStatus, @UserName, @ResetTime);
            _context.Daily.Add(newItem);
            _context.SaveChanges();

            return Ok("Item create successfully.");
        }

        [HttpPost]
        public IActionResult Delete([FromBody] DailyContext deleteItem)
        {
            // 查找要刪除的資料
            var daily = _context.Daily.FirstOrDefault(d => d.Id == deleteItem.Id);

            if (daily == null)
            {
                return NotFound("Record not found.");
            }

            // 刪除資料
            _context.Daily.Remove(daily);
            _context.SaveChanges();

            return Ok("Item deleted successfully.");
        }

        [HttpPost]
        public IActionResult Update([FromBody] DailyContext updatedDaily)
        {
            //SQL:SELECT TOP 1 * FROM Daily WHERE Id = @updatedDaily.Id;
            var daily = _context.Daily.FirstOrDefault(d => d.Id == updatedDaily.Id);
            if (daily == null)
            {
                return NotFound("Record not found.");
            }

            if (daily.ItemName == updatedDaily.ItemName && daily.ResetTime == updatedDaily.ResetTime)
            {
                return Ok("No changes detected.");
            }

            //SQL:UPDATE Daily SET ItemStatus = @updatedDaily.ItemStatus WHERE Id = @Id;
            daily.ItemName = updatedDaily.ItemName;
            daily.ResetTime = updatedDaily.ResetTime;
            try
            {
                _context.SaveChanges();
                return Ok("Item updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Database update failed: " + ex.Message);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
