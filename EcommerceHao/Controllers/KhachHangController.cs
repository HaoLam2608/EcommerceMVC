using AutoMapper;
using EcommerceHao.Data;
using EcommerceHao.Helpers;
using EcommerceHao.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
                return RedirectToAction("Index","HangHoa");
            }
            return View();
        }
    }
}
