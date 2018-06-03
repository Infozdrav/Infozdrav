using System.Linq;
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
            CreateMap<Manage.UserViewModel, User>();
            CreateMap<UserViewModel, User>();
            CreateMap<User, UserViewModel>()
                .Ignore(o => o.Password);
            CreateMap<User, UserEditViewModel>();
            CreateMap<Role, RoleViewModel>();
            CreateMap<DataFileViewModel, DataFile>();

            CreateMap<Trbovlje.ArticleReceptionViewModel, Article>()
                .ForMember(x => x.Certificate, opt => opt.Ignore())
                .ForMember(x => x.SafteyList, opt => opt.Ignore());
            CreateMap<Article, Trbovlje.ArticleEditViewModel>(MemberList.Source);
            CreateMap<Trbovlje.ArticleEditViewModel, Article>(MemberList.Destination);
            CreateMap<Trbovlje.ArticleUseViewModel, ArticleUse>();
            CreateMap<Article, Trbovlje.ArticleTableViewModel>()
                .ForMember( x => x.NumberOfAvailableUnits, opt => opt.MapFrom( s => s.NumberOfUnits - s.ArticleUses.Count()));
            CreateMap<Trbovlje.ArticleReceptionViewModel, Trbovlje.ArticleReceptionViewModel>()
                .ForMember(x => x.ShowIgnoreBadLot, opt => opt.Ignore());

        }
    }
}