using AutoMapper;
﻿using System.Linq;
using Infozdrav.Web.Abstractions;
using Infozdrav.Web.Data;
using Infozdrav.Web.Data.Manage;
using Infozdrav.Web.Data.Trbovlje;
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
                .ForMember( x => x.NumberOfAvailableUnits, opt => opt.MapFrom( s => s.NumberOfUnits - s.ArticleUses.Count() - s.Lends.Sum(l => l.UnitsUsed)));
            CreateMap<Trbovlje.ArticleReceptionViewModel, Trbovlje.ArticleReceptionViewModel>()
                .ForMember( x=> x.ShowIgnoreBadLot, opt => opt.Ignore());

            CreateMap<Trbovlje.CatalogArticleFullViewModel, CatalogArticle>();
            CreateMap<CatalogArticle, Trbovlje.CatalogArticleFullViewModel>(MemberList.Source);
            CreateMap<Trbovlje.CatalogArticleAddViewModel, CatalogArticle>();
            CreateMap<CatalogArticle, Trbovlje.CatalogArticleAddViewModel>(MemberList.Source);
            CreateMap<Trbovlje.CatalogArticleEditViewModel, CatalogArticle>();
            CreateMap<CatalogArticle, Trbovlje.CatalogArticleEditViewModel>(MemberList.Source);
            CreateMap<Trbovlje.CatalogArticleTableViewModel, CatalogArticle>();
            CreateMap<CatalogArticle, Trbovlje.CatalogArticleTableViewModel>(MemberList.Source);

            CreateMap<Trbovlje.OrderCatalogArticleFullViewModel, OrderCatalogArticle>();
            CreateMap<OrderCatalogArticle, Trbovlje.OrderCatalogArticleFullViewModel>(MemberList.Source);
            CreateMap<OrderCatalogArticle, Trbovlje.OrderCatalogArticleTableViewModel>();
            CreateMap<Trbovlje.OrderCatalogArticleFullViewModel, CatalogArticle>();
            CreateMap<CatalogArticle, Trbovlje.OrderCatalogArticleFullViewModel>(MemberList.Source);
            CreateMap<Trbovlje.NewOrderCatalogArticleViewModel, OrderCatalogArticle>();
            CreateMap<OrderCatalogArticle, Trbovlje.OrderCatalogArticleEditViewModel>(MemberList.Source);
            CreateMap<Trbovlje.OrderCatalogArticleEditViewModel, OrderCatalogArticle>(MemberList.Destination);

            CreateMap<Trbovlje.BufferViewModel, Buffer>();
            CreateMap<Buffer, Trbovlje.BufferViewModel>(MemberList.Source);

            
        }
    }
}