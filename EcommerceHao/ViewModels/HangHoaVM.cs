namespace EcommerceHao.ViewModels
{
    public class HangHoaVM
    {
        public int maHH {  get; set; }
        public string tenHH {  get; set; }
        public string hinh {  get; set; }
        public double donGia { get; set; }
        public string shortDesc {  get; set; }
        public string tenLoai {  get; set; }
    }
    public class ChiTietHangHoaVM
    {
        public int maHh { get; set; }
        public string tenHH { get; set; }
        public string hinh { get; set; }
        public double donGia { get; set; }
        public string shortDesc { get; set; }
        public string tenLoai { get; set; }
        public string Chitiet { get; set; }
        public int DiemDanhGia { get; set; }
        public int SoLuongTon { get; set; }

    }
}
