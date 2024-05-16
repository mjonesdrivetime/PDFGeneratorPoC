using MigraDoc.DocumentObjectModel;

public static class Formats{
    public static ParagraphFormat SingleLine(){
        return new ParagraphFormat()
        {
            Alignment = ParagraphAlignment.Left
        };
    }

    public static ParagraphFormat Body() {
        return new ParagraphFormat()
        {
            SpaceBefore = 10,
            SpaceAfter = 10,
            Alignment = ParagraphAlignment.Left
        };
    }

    public static ParagraphFormat Border()
    {
        Border newBorder = new() { Style = BorderStyle.Single, Color = new Color(6, 161, 227)};
        return new ParagraphFormat() {
            Borders = new Borders() {
                Bottom = newBorder
            }
        };
    }

     public static ParagraphFormat BodyBold() {
        return new ParagraphFormat()
        {
            Font = new Font(){ Bold = true },
            Alignment = ParagraphAlignment.Left
        };
    }
}