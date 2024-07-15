using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace EcommerceHao.ViewModels
{
    public class LogInVM
    {
        [Display(Name ="Tên đăng nhập")]
        [Required(ErrorMessage ="*")]
        [MaxLength(20,ErrorMessage ="Tối đa 20 kí tự")]
        public string UserName {  get; set; }


        [Display(Name ="Mật khẩu")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
