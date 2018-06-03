using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Infozdrav.Web.TagHelpers
{
    [HtmlTargetElement("barcode-scanner")]
    public class BarcodeScannerHelper : TagHelper
    {
        public string Callback { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";
            output.Attributes.SetAttribute("class", "btn btn-info barcode-scanner");
            output.Attributes.SetAttribute("data-callback", Callback);
        }
    }
}