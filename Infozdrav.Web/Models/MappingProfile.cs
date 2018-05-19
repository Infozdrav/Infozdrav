using System.Collections;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Models.Manage;
using Infozdrav.Web.Models.Trbovlje;

namespace Infozdrav.Web.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Manage.UserViewModel, User>();
            CreateMap<UserViewModel, User>();
            CreateMap<DataFileViewModel, DataFile>();

            CreateMap<Trbovlje.ArticleReceptionViewModel, Article>();
            CreateMap<Trbovlje.ArticleReceptionViewModel, Trbovlje.ArticleReceptionViewModel>()
                .ForMember( x=> x.ShowIgnoreBadLot, opt => opt.Ignore());

            CreateMap<Trbovlje.CatalogArticleViewModel, CatalogArticle>();
            CreateMap<CatalogArticle, Trbovlje.CatalogArticleViewModel>(MemberList.Source);

            CreateMap<Trbovlje.BufferViewModel, Buffer>();
            CreateMap<Buffer, Trbovlje.BufferViewModel>(MemberList.Source);

        }
    }
}