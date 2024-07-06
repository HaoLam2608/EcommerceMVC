using EcommerceHao.Data;
using EcommerceHao.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace EcommerceHao.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly HaoLamShopContext db;

        public CategoriesViewComponent(HaoLamShopContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            var data = db.Loais.Select(Lo => new MenuLoaiVM
            {
                maLoai=Lo.MaLoai,
                tenLoai=Lo.TenLoai,
                soLuong = Lo.HangHoas.Count
            });
            return View("Defautl",data);
        }
    }
}
