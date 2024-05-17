

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;

namespace TestPdfSystem.Models;

public class LoanLetter
{
    protected Document Document { get; set; }

    public LoanLetter() {
        Document = new Document();

        DefineStyles();
        
        Document.AddSection();
    }

    void DefineStyles()
    {
        // Get the predefined style Normal.
        Style style = Document.Styles["Normal"];
        // Because all styles are derived from Normal, the next line changes the 
        // font of the whole document. Or, more exactly, it changes the font of
        // all styles and paragraphs that do not redefine the font.
        style.Font.Name = "Verdana";
        
        style = Document.Styles[StyleNames.Header];
        style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);
        
        style = Document.Styles[StyleNames.Footer];
        style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);
    }

    public void AddHeader() {
        var header = Document.LastSection.Headers.Primary;
        var img = header.AddImage("wwwroot/images/logo.png");
        img.LockAspectRatio = true;
        img.Height = new Unit(2.25, UnitType.Centimeter);
        img.RelativeVertical = RelativeVertical.Line;
        img.RelativeHorizontal = RelativeHorizontal.Margin;
        img.Top = ShapePosition.Top;
        img.Left = ShapePosition.Right;

        AddBlankParagraphs();

        AddParagraph(string.Empty, Formats.Border());

        AddBlankParagraphs();
    }

    public void SetFont(SupportedFonts fontName, int size = 12)
    {
        var style = Document.Styles["Normal"] ?? new Style();
        style.Font.Name = fontName switch
        {
            SupportedFonts.Arial => "arial",
            SupportedFonts.Verdana => "verdana",
            _ => "arial",
        };
        style.Font.Size = size;
    }

    private Paragraph AddParagraph(string text, ParagraphFormat format)
    {
        var section = Document.LastSection;
        var paragraph = section.AddParagraph(text);
        paragraph.Format = format;
        return paragraph;
    }

    public Paragraph AddInfoLine(string text)
    {
        return AddParagraph(text, Formats.SingleLine());
    }

    public void AddDateLine()
    {
        var date = DateTime.Today.ToString("dddd, MMMM dd, yyyy");

        AddParagraph(date, Formats.SingleLine());
    }

    public void AddDateLine(DateTime date)
    {
        var dateString = date.ToString("dddd, MMMM dd, yyyy");

        AddParagraph(dateString, Formats.SingleLine());
    }

    public Paragraph AddBodyParagraph(string text)
    {
        return AddParagraph(text, Formats.Body());
    }

    public Paragraph AddBoldBodyParagraph(string text)
    {
        return AddParagraph(text, Formats.BodyBold());
    }

    public void AddBlankParagraphs(int count = 1)
    {
        int i = 1;
        do{
            var section = Document.LastSection;
            section.AddParagraph();
            i++;
        }while( i <= count);
    }

    public void AddClosing(string name)
    {
        AddBlankParagraphs();
        AddParagraph("Sincerely,", Formats.SingleLine());
        AddBlankParagraphs(2);
        AddParagraph(name, Formats.SingleLine());
    }

    public MemoryStream RenderDocument()
    {
        PdfDocumentRenderer renderer = new()
        {
            Document = Document
        };

        renderer.RenderDocument();

        MemoryStream stream = new MemoryStream();
        renderer.Save(stream, false);

        return stream;
    }
}