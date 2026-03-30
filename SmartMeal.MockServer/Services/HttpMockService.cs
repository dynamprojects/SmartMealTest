namespace SmartMeal.MockServer.Services;

public static class HttpMockService
{
    public static object GetMenu()
    {
        return new
        {
            Command = "GetMenu",
            Success = true,
            ErrorMessage = "",
            Data = new
            {
                MenuItems = new[]
                {
                    new
                    {
                        Id = "5979224",
                        Article = "A1004292",
                        Name = "Каша гречневая",
                        Price = 50,
                        IsWeighted = false,
                        FullPath = "ПРОИЗВОДСТВО\\Гарниры",
                        Barcodes = new[] { "57890975627974236429" }
                    },
                    new
                    {
                        Id = "9084246",
                        Article = "A1004293",
                        Name = "Конфеты Коровка",
                        Price = 300,
                        IsWeighted = true,
                        FullPath = "ДЕСЕРТЫ\\Развес",
                        Barcodes = Array.Empty<string>()
                    }
                }
            }
        };
    }

    public static object SendOrder()
    {
        return new
        {
            Command = "SendOrder",
            Success = true,
            ErrorMessage = ""
        };
    }
}