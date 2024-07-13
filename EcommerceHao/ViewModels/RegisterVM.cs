using System.ComponentModel.DataAnnotations;

namespace EcommerceHao.ViewModels
{
    public class RegisterVM
    {
        [Key]
        [Display(Name="Tên đăng nhập")]
        [Required(ErrorMessage ="*")]
        [MaxLength(20,ErrorMessage ="Toi da 20 ky tu")]
        public string MaKh { get; set; }

        [Display(Name ="Mật khẩu")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Toi da 50 ky tu")]
        public string HoTen { get; set; } = null!;

        [Display(Name ="Giới tính")]
        public bool GioiTinh { get; set; } = true;

        [Display(Name ="Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [Display(Name = "Địa chỉ")]
        [MaxLength(60, ErrorMessage = "Toi da 60 ky tu")]
        public string DiaChi { get; set; }

        [Display(Name = "Điện thoại")]
        [MaxLength(10, ErrorMessage = "Toi da 10 ky tu")]
        [RegularExpression(@"0[9875]\d{8}",ErrorMessage ="Chua dung dinh dang so dien thoai VN")]
        public string DienThoai { get; set; }


        [EmailAddress(ErrorMessage ="Chua dung dinh dang email")]
        public string Email { get; set; }

        public string? Hinh { get; set; }
    }
}
