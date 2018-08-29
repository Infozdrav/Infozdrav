using Infozdrav.Web.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Infozdrav.Web.Data.Manage
{
    public class Role : IdentityRole<int>, IEntity
    {
    }

    public sealed class Roles
    {
        public const string Administrator = "Administrator";

        public const string ArticleView = "Pregled artiklov";
        public const string ArticleReception = "Sprejema artiklov";
        public const string ArticleWriteOff = "Odpis artiklov";
        public const string ArticleEdit = "Urejanje artiklov";
        public const string ArticleLend = "Sposojanje artiklov";
        public const string ArticleLendRecive = "Vrnitev sposojenih artiklov";
        public const string ArticleUse = "Uporaba artiklov";
        public const string BufferCreate = "Ustvarjanje pufrov";
        public const string Statistics = "Statistika";
        public const string StockView = "Pregled zaloge";
        public const string ArticleCatalogEdit= "Urejanje kataloga artiklov";
        public const string CatalogArticleOrder = "Naročanje artiklov";
        public const string OrderConfirm = "Sprejem naročil";
        public const string CatalogArticleConfirmOrder = "Potrjevanje artiklov";
    }
}