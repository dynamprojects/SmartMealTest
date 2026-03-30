namespace SmartMealLib.Http.Dto;

public class SendOrderResponse
{
    public bool Success { get; set; }

    public string ErrorMessage { get; set; }

    public OrderResponseData Data { get; set; }
}

public class OrderResponseData
{
    public Guid OrderId { get; set; }
}