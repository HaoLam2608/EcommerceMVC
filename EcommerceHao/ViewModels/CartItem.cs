namespace EcommerceHao.ViewModels
{
    public class CartItem
    {
        public int maHH { get; set; }
        public string tenHH { get; set; }
        public string hinh { get; set; }
        public double donGia { get; set; }
        public int soLuong {  get; set; }
        public double thanhTien =>soLuong*donGia;

    }
}
