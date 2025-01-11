using AutoMapper;
using EcommerceHao.Data;
using EcommerceHao.Helpers;
using EcommerceHao.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;

namespace EcommerceHao.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly HaoLamShopContext db;
        private readonly IMapper _mapper;

        public KhachHangController(HaoLamShopContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DangKy(RegisterVM model, IFormFile Hinh)
        {
            if (ModelState.IsValid)
            {
                var khachhang = _mapper.Map<KhachHang>(model);
                khachhang.RandomKey = Util.GenerateRandomKey();
                khachhang.MatKhau = model.MatKhau.ToMd5Hash(khachhang.RandomKey);
                khachhang.HieuLuc = true;
                khachhang.VaiTro = 0;

                if (Hinh != null)
                {
                    khachhang.Hinh = Util.UploadHinh(Hinh, "KhachHang");
                }
                db.Add(khachhang);
                db.SaveChanges();
                return RedirectToAction("Index", "HangHoa");
            }
            return View();
        }



        #region Login
        [HttpGet]
        public IActionResult DangNhap(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(LogInVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.UserName);
                if (khachHang == null)
                {
                    ModelState.AddModelError("loi", "Không có khách hàng này");
                }
                else
                {
                    if (!khachHang.HieuLuc)
                    {
                        ModelState.AddModelError("loi", "Tài khoản đã bị khóa. Vui lòng liên hệ Admin.");
                    }
                    else
                    {
                        var test = model.Password.ToMd5Hash(khachHang.RandomKey);
                        Console.WriteLine(test);
                        Console.WriteLine(khachHang.MatKhau);
                        if (khachHang.MatKhau != model.Password.ToMd5Hash(khachHang.RandomKey))
                        {
                            ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                        }
                        else
                        {
                            var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, khachHang.Email),
                                new Claim(ClaimTypes.Name, khachHang.HoTen),
                                new Claim(MyConst.Claims_CusID, khachHang.MaKh),

								//claim - role động
								new Claim(ClaimTypes.Role, "Customer")
                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal);

                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                return Redirect("/");
                            }
                        }
                    }
                }
            }
            return View();
        }
        #endregion

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
