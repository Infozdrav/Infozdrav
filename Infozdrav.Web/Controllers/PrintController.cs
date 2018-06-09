using System.IO;
using Infozdrav.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infozdrav.Web.Controllers
{
    public class PrintController : Controller
    {
        private readonly QRCodeService _qrCodeService;

        public PrintController(QRCodeService qrCodeService)
        {
            _qrCodeService = qrCodeService;
        }

        public IActionResult QrCodePdf(string code)
        {
            return new FileStreamResult(_qrCodeService.GetQrPdf(code, code), "application/pdf");
        }
    }
}