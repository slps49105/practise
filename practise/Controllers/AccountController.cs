using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practise.data;
using practise.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;

namespace practise.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            //宣告_context=連線字串
            _context = context;
        }

        // 密碼哈希處理（使用 SHA256）
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        // 前往註冊頁面
        public IActionResult Register()
        {
            return View(new MemberContext());
        }

        // 註冊邏輯
        [HttpPost]
        public async Task<IActionResult> Register(string userName, string password)
        {
            // 檢查重複使用者
            var existingUser = await _context.Member.FirstOrDefaultAsync(m => m.UserName == userName);
            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "此帳號已存在";
                return View();
            }


            // 密碼加密（使用 SHA256 或其他加密演算法）
            var hashedPassword = HashPassword(password);

            // 儲存新使用者
            var newMember = new MemberContext
            {
                UserName = userName,
                Password = hashedPassword
            };

            _context.Member.Add(newMember);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        // 前往登入頁面
        public IActionResult Login()
        {
            return View();
        }

        // 登入邏輯
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var user = await _context.Member.FirstOrDefaultAsync(m => m.UserName == userName);

            if (user == null || user.Password != HashPassword(password))
            {
                ViewBag.ErrorMessage = "無效的使用者名稱或密碼";
                return View();
            }

            // 註冊使用者身份
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // 登入並設定 Cookie
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        // 登出
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
