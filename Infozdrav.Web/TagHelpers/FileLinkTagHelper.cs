using Infozdrav.Web.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Infozdrav.Web.TagHelpers
{
    [HtmlTargetElement("filelink")]
    public class FileLinkTagHelper : TagHelper
    {
        private readonly IOptions<FileSettings> _fileSettings;
        private readonly IUrlHelper _urlHelper;

        public FileLinkTagHelper(IOptions<FileSettings> fileSettings, 
                                 IUrlHelper urlHelper)
        {
            _fileSettings = fileSettings;
            _urlHelper = urlHelper;
        }

        public string FileName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", _urlHelper.Content($"~/{_fileSettings.Value.UploadDir}/{FileName}"));
        }
    }
}