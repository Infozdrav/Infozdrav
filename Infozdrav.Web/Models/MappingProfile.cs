using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Models.Manage;

namespace Infozdrav.Web.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserViewModel, User>();
        }
    }
}