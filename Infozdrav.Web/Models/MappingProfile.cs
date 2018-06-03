using System.Collections;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
﻿using System.Linq;
using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Data.Manage;
using Infozdrav.Web.Helpers;
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
            CreateMap<User, UserViewModel>()
                .Ignore(o => o.Password);
            CreateMap<User, UserEditViewModel>();
            CreateMap<Role, RoleViewModel>();
            CreateMap<DataFileViewModel, DataFile>();

            CreateMap<Trbovlje.ArticleReceptionViewModel, Article>();
            CreateMap<Trbovlje.ArticleUseViewModel, ArticleUse>();
            CreateMap<Article, Trbovlje.ArticleTableViewModel>()
                .ForMember( x => x.NumberOfAvailableUnits, opt => opt.MapFrom( s => s.NumberOfUnits - s.ArticleUses.Count()));
            CreateMap<Trbovlje.ArticleReceptionViewModel, Trbovlje.ArticleReceptionViewModel>()
                .ForMember( x=> x.ShowIgnoreBadLot, opt => opt.Ignore());

            CreateMap<Trbovlje.CatalogArticleViewModel, CatalogArticle>();
            CreateMap<CatalogArticle, Trbovlje.CatalogArticleViewModel>(MemberList.Source);

            CreateMap<Trbovlje.OrderCatalogArticleViewModel, OrderCatalogArticle>();
            CreateMap<OrderCatalogArticle, Trbovlje.OrderCatalogArticleViewModel>(MemberList.Source);

            CreateMap<Trbovlje.BufferViewModel, Buffer>();
            CreateMap<Buffer, Trbovlje.BufferViewModel>(MemberList.Source);

            CreateMap<Trbovlje.OrderCatalogArticleViewModel, CatalogArticle>();
            CreateMap<CatalogArticle, Trbovlje.OrderCatalogArticleViewModel>(MemberList.Source);

                .ForMember( x => x.ShowIgnoreBadLot, opt => opt.Ignore());
        }
    }
}