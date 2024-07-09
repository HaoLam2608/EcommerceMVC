using EcommerceHao.Data;
using EcommerceHao.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var hangHoas = db.HangHoas.AsQueryable();
            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
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
        public IActionResult search(string? query)
        {
            var hangHoas = db.HangHoas.AsQueryable();
            if (query != null)
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

        public IActionResult Detail(int id)
        {
            var data = db.HangHoas
                .Include(p => p.MaLoaiNavigation)
                .SingleOrDefault(p => p.MaHh == id);
            if (data == null)
            {
                TempData["Message"] = $"Không thấy sản phẩm có mã {id}";
                return Redirect("/404");
            }

            var result = new ChiTietHangHoaVM
            {
                maHh = data.MaHh,
                tenHH = data.TenHh,
                donGia = data.DonGia ?? 0,
                Chitiet = data.MoTa ?? string.Empty,
                hinh = data.Hinh ?? string.Empty,
                shortDesc = data.MoTaDonVi ?? string.Empty,
                tenLoai = data.MaLoaiNavigation.TenLoai,
                SoLuongTon = 10,//tính sau
                DiemDanhGia = 5,//check sau
            };
            return View(result);


        }
    } 
    
}
