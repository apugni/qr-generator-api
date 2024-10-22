using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace QRCodeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRCodeController : ControllerBase
    {
        [HttpGet]
        [Route("generate")]
        public IActionResult GenerateQRCode([FromQuery] string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return BadRequest("Text parameter is required.");
            }

            using (var qrGenerator = new QRCodeGenerator())
            {
                using (var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q))
                {
                    using (var qrCode = new QRCoder.QRCode(qrCodeData)) // Especificar el espacio de nombres QRCoder
                    {
                        using (Bitmap bitmap = qrCode.GetGraphic(20))
                        {
                            using (var ms = new MemoryStream())
                            {
                                bitmap.Save(ms, ImageFormat.Png);
                                var byteImage = ms.ToArray();
                                return File(byteImage, "image/png");
                            }
                        }
                    }
                }
            }
        }
    }
}
