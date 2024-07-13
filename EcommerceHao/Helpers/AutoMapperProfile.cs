using AutoMapper;
using EcommerceHao.Data;
using EcommerceHao.ViewModels;

namespace EcommerceHao.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<RegisterVM, KhachHang>();
                //.ForMember(kh => kh.HoTen, option => option.MapFrom(RegisterVM => RegisterVM.HoTen))
                //.ReverseMap();
        }
    }
}
