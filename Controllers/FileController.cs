using System.Net.WebSockets;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using TestPdfSystem.Models;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IConfigurationRoot ConfigRoot;

        public FileController(IConfiguration configRoot)
        {
            ConfigRoot = (IConfigurationRoot)configRoot;
        }

        [HttpGet]
        [Route("CreatePDF")]
        public ActionResult CreatePDF()
        {
            MyFontResolver.Apply();

            string customerFirstName = "Jane", customerLastName = "Smith";
            string oldYear = "2017", oldMake = "Ford", oldModel = "F-150";
            string newYear = "2020", newMake = "Chevrolet", newModel = "Tahoe";

            LoanLetter letter = new();
            letter.SetFont(SupportedFonts.Arial);
            letter.AddHeader();
            letter.AddDateLine();
            letter.AddBlankParagraphs();
            letter.AddInfoLine($"Customer: {customerFirstName} {customerLastName}");
            letter.AddInfoLine($"Previous Existing Vehicle: {oldYear} {oldMake} {oldModel}");
            letter.AddInfoLine($"Current Upgrade Vehicle: {newYear} {newMake} {newModel}");
            letter.AddBlankParagraphs();

            string paragraph1 = $"Congratulations! We hope you’re enjoying your experience with your new {newYear} {newMake} {newModel}! We would like to thank you for being a DriveTime/Bridgecrest customer and giving us the opportunity to help you with your vehicle and financing needs.";
            letter.AddBodyParagraph(paragraph1);

            string paragraph2 = $"In connection with your paid account, you are receiving a pro rata refund of your ancillary products, which may include a Vehicle Service Contract (VSC), GPS services (GPS), and/or Guaranteed Asset Protection (GAP). The following refund amount(s) are being applied to your new {newYear} {newMake} {newModel} account as a credit to the principal amount financed:";
            letter.AddBodyParagraph(paragraph2);

            var ancillaryProducts = AncillaryProductGenerator.Generate();
            foreach(var product in ancillaryProducts)
            {
                string paragraphAncillary = $"{product.Name}: ${product.Amount} applied on {product.Date.ToShortDateString()}";
                letter.AddBoldBodyParagraph(paragraphAncillary);
            }

            string paragraph4 = $"If you have any questions about your refund or your account, please contact Bridgecrest at {ConfigRoot["BridgecrestPhoneNumber"]}.";
            letter.AddBodyParagraph(paragraph4);

            letter.AddBodyParagraph("You can also log into your account on our website or the Bridgecrest app to view your account details.");
            letter.AddBodyParagraph("We appreciate your business and look forward to continuing our journey together with you on the road to vehicle ownership!");
            letter.AddClosing("The Bridgecrest Team");

            var stream = letter.RenderDocument();

            return File(stream, "application/pdf", "testletter.pdf");
        }
    }
}
