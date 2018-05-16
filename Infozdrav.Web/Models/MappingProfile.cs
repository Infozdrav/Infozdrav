using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Data.Manage;
using Infozdrav.Web.Helpers;
using Infozdrav.Web.Models.Manage;

namespace Infozdrav.Web.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserViewModel, User>();
            CreateMap<User, UserViewModel>()
                .Ignore(o => o.Password);
            CreateMap<User, UserEditViewModel>();
            CreateMap<Role, RoleViewModel>();
        }
    }
}