using EcommerceHao.Helpers;
using EcommerceHao.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceHao.ViewComponents
{
    public class CartViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var count = HttpContext.Session.Get<List<CartItem>>(MyConst.cartKey)?? new List<CartItem>();
            return View("CartPanel",new CartModel 
            {
                quantity = count.Sum(p=>p.soLuong),
                total=count.Sum(p=>p.thanhTien)
            });

        }
    }
}
