using System.Reflection;
using PdfSharp.Fonts;

class MyFontResolver : IFontResolver
{
    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        // Ignore case of font names.
        var name = familyName.ToLower().TrimEnd('#');

        // Deal with the fonts we know.
        switch (name)
        {
            case "arial":
                if (isBold)
                {
                    if (isItalic)
                        return new FontResolverInfo("Arial#bi");
                    return new FontResolverInfo("Arial#b");
                }
                if (isItalic)
                    return new FontResolverInfo("Arial#i");
                return new FontResolverInfo("Arial#");

            case "verdana":
                if (isBold)
                {
                    if (isItalic)
                        return new FontResolverInfo("Verdana#bi");
                    return new FontResolverInfo("Verdana#b");
                }
                if (isItalic)
                    return new FontResolverInfo("Verdana#i");
                return new FontResolverInfo("Verdana#");
        }

        // We pass all other font requests to the default handler.
        // When running on a web server without sufficient permission, you can return a default font at this stage.
        return PlatformFontResolver.ResolveTypeface(familyName, isBold, isItalic);
    }

    /// <summary>
    /// Return the font data for the fonts.
    /// </summary>
    public byte[] GetFont(string faceName)
    {
        switch (faceName)
        {
            case "Arial#":
                return FontHelper.Arial;

            case "Arial#b":
                return FontHelper.ArialBold;

            case "Arial#i":
                return FontHelper.ArialItalic;

            case "Arial#bi":
                return FontHelper.ArialBoldItalic;

            case "Verdana#":
                return FontHelper.Verdana;

            case "Verdana#b":
                return FontHelper.VerdanaBold;

            case "Verdana#i":
                return FontHelper.VerdanaItalic;

            case "Verdana#bi":
                return FontHelper.VerdanaBoldItalic;
        }

        return null;
    }


    internal static MyFontResolver OurGlobalFontResolver = null;

    /// <summary>
    /// Ensure the font resolver is only applied once (or an exception is thrown)
    /// </summary>
    internal static void Apply()
    {
        if (OurGlobalFontResolver == null || GlobalFontSettings.FontResolver == null)
        {
            if (OurGlobalFontResolver == null)
                OurGlobalFontResolver = new MyFontResolver();

            GlobalFontSettings.FontResolver = OurGlobalFontResolver;
        }
    }
}


/// <summary>
/// Helper class that reads font data from embedded resources.
/// </summary>
public static class FontHelper
{
    public static byte[] Arial
    {
        get { return LoadFontData("wwwroot/fonts/ARIAL.TTF"); }
    }

    public static byte[] ArialBold
    {
        get { return LoadFontData("wwwroot/fonts/ARIALBD.TTF"); }
    }

    public static byte[] ArialItalic
    {
        get { return LoadFontData("wwwroot/fonts/ARIALI.TTF"); }
    }

    public static byte[] ArialBoldItalic
    {
        get { return LoadFontData("wwwroot/fonts/ARIALBI.TTF"); }
    }

     public static byte[] Verdana
    {
        get { return LoadFontData("wwwroot/fonts/VERDANA.TTF"); }
    }

    public static byte[] VerdanaBold
    {
        get { return LoadFontData("wwwroot/fonts/VERDANAB.TTF"); }
    }

    public static byte[] VerdanaItalic
    {
        get { return LoadFontData("wwwroot/fonts/VERDANAI.TTF"); }
    }

    public static byte[] VerdanaBoldItalic
    {
        get { return LoadFontData("wwwroot/fonts/VERDANAZ.TTF"); }
    }

    /// <summary>
    /// Returns the specified font from an embedded resource.
    /// </summary>
    static byte[] LoadFontData(string name)
    {
        return File.ReadAllBytes(name);

        /* var assembly = Assembly.GetExecutingAssembly();

        // Test code to find the names of embedded fonts
        //var ourResources = assembly.GetManifestResourceNames();

        using (Stream stream = assembly.GetManifestResourceStream(name))
        {
            if (stream == null)
                throw new ArgumentException("No resource with name " + name);

            int count = (int)stream.Length;
            byte[] data = new byte[count];
            stream.Read(data, 0, count);
            return data;
        } */
    }
}