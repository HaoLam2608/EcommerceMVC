using EcommerceHao.Data;
using EcommerceHao.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceHao.Controllers
{
    public class HangHoaController : Controller

    {
        private readonly HaoLamShopContext db;

        public HangHoaController(HaoLamShopContext context) 
        {
            db = context;
        }
        public IActionResult Index(int? loai)
        {
            var hangHoas=db.HangHoas.AsQueryable();
            if (loai.HasValue) 
            { 
                    hangHoas=hangHoas.Where(p=>p.MaLoai == loai.Value);
            }
            var result = hangHoas.Select(p => new HangHoaVM
            {
                maHH=p.MaHh,
                tenHH=p.TenHh,
                hinh=p.Hinh,
                donGia=p.DonGia ?? 0,
                shortDesc=p.MoTaDonVi ?? " ",
                tenLoai=p.MaLoaiNavigation.TenLoai
   
            });
            return View(result);
        }
        public IActionResult search(string? query)
        {
            var hangHoas = db.HangHoas.AsQueryable();
            if (query!=null)
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
            }
            var result = hangHoas.Select(p => new HangHoaVM
            {
                maHH = p.MaHh,
                tenHH = p.TenHh,
                hinh = p.Hinh,
                donGia = p.DonGia ?? 0,
                shortDesc = p.MoTaDonVi ?? " ",
                tenLoai = p.MaLoaiNavigation.TenLoai

            });
            return View(result);
        }
    }
}
