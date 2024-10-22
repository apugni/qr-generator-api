//using Microsoft.AspNetCore.Mvc;
//using QRCoder;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.IO;

//namespace QRCodeApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class QRCodeController : ControllerBase
//    {
//        [HttpGet]
//        [Route("generate")]
//        public IActionResult GenerateQRCode([FromQuery] string text)
//        {
//            if (string.IsNullOrEmpty(text))
//            {
//                return BadRequest("Text parameter is required.");
//            }

//            using (var qrGenerator = new QRCodeGenerator())
//            {
//                using (var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q))
//                {
//                    using (var qrCode = new QRCoder.QRCode(qrCodeData)) // Especificar el espacio de nombres QRCoder
//                    {
//                        using (Bitmap bitmap = qrCode.GetGraphic(20))
//                        {
//                            using (var ms = new MemoryStream())
//                            {
//                                bitmap.Save(ms, ImageFormat.Png);
//                                var byteImage = ms.ToArray();
//                                return File(byteImage, "image/png");
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Drawing;
using System.IO;

[ApiController]
[Route("api/[controller]")]
public class QRCodeController : ControllerBase
{
    [HttpGet("generate")]
    public IActionResult GenerateQRCode(string text)
    {
        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        {
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            using (QRCode qrCode = new QRCode(qrCodeData))
            {
                using (var bitmap = qrCode.GetGraphic(20))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        // Guardar el Bitmap en el MemoryStream
                        bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        memoryStream.Position = 0; // Reiniciar la posición del stream

                        // Cargar la imagen desde el MemoryStream
                        using (var image = SixLabors.ImageSharp.Image.Load(memoryStream.ToArray())) // Cargar la imagen sin Rgba32
                        {
                            using (var outputStream = new MemoryStream())
                            {
                                image.SaveAsPng(outputStream); // Guardar como PNG
                                return File(outputStream.ToArray(), "image/png");
                            }
                        }
                    }
                }
            }
        }
    }
}
