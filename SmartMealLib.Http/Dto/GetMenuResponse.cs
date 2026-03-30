namespace SmartMealLib.Http.Dto;

public class GetMenuResponse
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public MenuDataDto Data { get; set; }
}

public class MenuDataDto
{
    public List<DishDto> MenuItems { get; set; }
}

public class DishDto
{
    public string Id { get; set; }
    public string Article { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
}