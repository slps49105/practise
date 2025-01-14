using System.Collections.Generic;
using System.Diagnostics;
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
            var Daily = _context.Daily.ToList();

            return View(Daily);
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

            //SQL:INSERT INTO Daily (Id, ItemName, ItemStatus, UserName)
            //VALUES(@Id, @ItemName, @ItemStatus, @UserName);
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

            //SQL:UPDATE Daily SET ItemStatus = @updatedDaily.ItemStatus WHERE Id = @Id;
            daily.ItemName = updatedDaily.ItemName;
            _context.SaveChanges();

            return Ok("Item updated successfully.");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
