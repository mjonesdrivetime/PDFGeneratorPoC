public class AncillaryProduct{
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}

public static class AncillaryProductGenerator
{
    public static List<AncillaryProduct> Generate()
    {
        var random = new Random(DateTime.Now.Millisecond);

        //First, generate number of ancillary products
        var productCount = random.Next(1, 9);

        //Foreach product, generate a name and a decimal amount
        List<AncillaryProduct> products = [];
        for(int i = 1; i <= productCount; i++ )
        {
            var newProduct = new AncillaryProduct(){
                Name = GenerateThreeLetterName(),
                Amount = random.Next(40, 200),
                Date = DateTime.Today.AddDays(-2)
            };

            products.Add(newProduct);
        }

        return products;
    }

    public static string GenerateThreeLetterName()
    {
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        string name = "";
        Random random = new();

        while(name.Length < 3)
        {
            var character = random.Next(0,26);
            name += alphabet[character];
        }

        return name;
    }
}