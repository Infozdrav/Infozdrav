using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Models.Manage;

namespace Infozdrav.Web.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Manage.UserViewModel, User>();
            CreateMap<Trbovlje.WorkLocationViewModel, WorkLocation>();
            CreateMap<Trbovlje.StorageLocationViewModel, StorageLocation>();
            CreateMap<UserViewModel, User>();
            CreateMap<DataFileViewModel, DataFile>();
            CreateMap<Trbovlje.ArticleReceptionViewModel, Article>();
        }
    }
}