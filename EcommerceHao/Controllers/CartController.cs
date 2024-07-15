using EcommerceHao.Data;
using EcommerceHao.ViewModels;
using Microsoft.AspNetCore.Mvc;
using EcommerceHao.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceHao.Controllers
{
    public class CartController : Controller
    {
        private readonly HaoLamShopContext db;

        public CartController(HaoLamShopContext context)
        {
            db = context;
        }
       
        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MyConst.cartKey) ?? new List<CartItem>();
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
            HttpContext.Session.Set(MyConst.cartKey, gioHang);

            return RedirectToAction("Index");
        }
        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.maHH == id);
            if (item != null) 
            {   
                gioHang.Remove(item);
                HttpContext.Session.Set(MyConst.cartKey, gioHang);
            }
            return RedirectToAction("Index");
        }


        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {

            if (Cart.Count == 0)
            {
                return Redirect("/");
            }
            return View(Cart);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(CheckoutVM model)
        {   

            if (ModelState.IsValid)
            {
                var customerID = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MyConst.Claims_CusID).Value;
                var khachhang = new KhachHang();
                if (model.GiongKhachHang)
                {
                    khachhang = db.KhachHangs.SingleOrDefault(p => p.MaKh == customerID);
                }
                var hoadon = new HoaDon
                {
                    MaKh = customerID,
                    HoTen=model.HoTen ?? khachhang.HoTen,
                    DiaChi = model.DiaChi ?? khachhang.DiaChi,
                    DienThoai=model.DienThoai ?? khachhang.DienThoai,
                    NgayDat=DateTime.Now,
                    CachThanhToan="COD",
                    CachVanChuyen="GRAB",
                    MaTrangThai=0,
                    GhiChu=model.GhiChu
                    
                };
                db.Database.BeginTransaction();
                try
                {                   
                    db.Database.CommitTransaction();
                    db.Add(hoadon);
                    db.SaveChanges();

                    var cthd=new List<ChiTietHd>();
                    foreach(var item in Cart)
                    {
                        cthd.Add(new ChiTietHd 
                        { 
                            MaHd=hoadon.MaHd,
                            SoLuong=item.soLuong,
                            DonGia=item.donGia,
                            MaHh=item.maHH,
                            GiamGia=0,
                        });

                    }
                    db.SaveChanges();
                    HttpContext.Session.Set<List<CartItem>>(MyConst.cartKey,new List<CartItem>());

                    return View("Success");
                }
                catch
                {
                    db.Database.RollbackTransaction();
                }

            }
            return View(Cart);
        }
    }
}
