using Microsoft.AspNetCore.Mvc;
using ZXing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using System.Net.NetworkInformation;

[ApiController]
[Route("api/[controller]")]
public class QRCodeController : ControllerBase
{
    [HttpGet("generate")]
    public IActionResult GenerateQRCode(string text)
    {
        try
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 300
                }
            };

            var pixelData = writer.Write(text);
            using (var image = SixLabors.ImageSharp.Image.LoadPixelData<Rgba32>(pixelData.Pixels, pixelData.Width, pixelData.Height))
            {
                using (var outputStream = new MemoryStream())
                {
                    image.SaveAsPng(outputStream); // Guardar como PNG
                    return File(outputStream.ToArray(), "image/png");
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
