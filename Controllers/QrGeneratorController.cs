

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
        try
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

        catch (Exception ex)
        {
            // Log del error para obtener más información
            return BadRequest($"Error generating QR code: {ex.Message}");
        }
    }
}
