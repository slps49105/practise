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
            //�ŧi_context=�s�u�r��
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
                return Content("�Х��n�J");
            }

            switch (type)
            {
                case "daily":
                    var dailyItems = _context.Daily.Where(d => d.UserName == userName).ToList();
                    return PartialView("_DailyPartial", dailyItems); // ��^ PartialView �� HTML

                case "weekly":
                    var weeklyItems = _context.Weekly.Where(d => d.UserName == userName).ToList();
                    return PartialView("_WeeklyPartial", weeklyItems);
            }
            return Content("�S�����");
        }

        //[FromBody]�N�����ƪ��覡�אּAjax
        //�B�ഫ��DailyContext(�۩w�q��Model)����
        //�A�N��Ƹm��updatedDaily(�۩w�q�Ѽ�)�Ѽ�
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
            // �d��n�R�������
            var daily = _context.Daily.FirstOrDefault(d => d.Id == deleteItem.Id);

            if (daily == null)
            {
                return NotFound("Record not found.");
            }

            // �R�����
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
