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
                    // Generar el bitmap de QR code
                    using (var bitmap = qrCode.GetGraphic(20))
                    {
                        // Convertir el bitmap de System.Drawing a ImageSharp
                        using (var memoryStream = new MemoryStream())
                        {
                            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                            memoryStream.Position = 0; // Reiniciar la posición del stream

                            // Cargar la imagen desde el MemoryStream
                            using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(memoryStream.ToArray()))
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
