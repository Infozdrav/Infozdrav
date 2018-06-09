using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Infozdrav.Web.TagHelpers
{
    [HtmlTargetElement("print-qr")]
    public class PrintQrHelper : TagHelper
    {
        private readonly IUrlHelper _urlHelper;
        public string Code { get; set; }

        public PrintQrHelper(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("target", "_blank");
            output.Attributes.SetAttribute("href", _urlHelper.Content($"~/Print/QrCodePdf?code={Code}"));
            output.Attributes.SetAttribute("class", "btn btn-info print-qr");
        }
    }
}