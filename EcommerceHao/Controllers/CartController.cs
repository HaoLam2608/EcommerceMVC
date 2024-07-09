using EcommerceHao.Data;
using EcommerceHao.ViewModels;
using Microsoft.AspNetCore.Mvc;
using EcommerceHao.Helpers;

namespace EcommerceHao.Controllers
{
    public class CartController : Controller
    {
        private readonly HaoLamShopContext db;

        public CartController(HaoLamShopContext context)
        {
            db = context;
        }
        const string cartKey = "mycart";
        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(cartKey) ?? new List<CartItem>();
        public IActionResult Index()
        {
            return View(Cart);
        }
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.maHH == id);
            if (item == null)
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)
                {
                    return Redirect("/404");
                }
                item = new CartItem
                {
                    maHH = hangHoa.MaHh,
                    tenHH = hangHoa.TenHh,
                    donGia = hangHoa.DonGia ?? 0,
                    hinh = hangHoa.Hinh ?? string.Empty,
                    soLuong = quantity

                };
                gioHang.Add(item);

            }
            else
            {
                item.soLuong+=quantity;
            }
            HttpContext.Session.Set(cartKey, gioHang);

            return RedirectToAction("Index");
        }
    }
}
