using System;
using System.Linq.Expressions;
using AutoMapper;

namespace Infozdrav.Web.Helpers
{
    public static class MapperHelpers
    {
        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, object>> selector)
        {
            map.ForMember(selector, config => config.Ignore());
            return map;
        }
    }
}
