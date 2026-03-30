using Microsoft.AspNetCore.Mvc;
using SmartMeal.MockServer.Services;
using SmartMealLib.Abstractions.Models;

namespace SmartMeal.MockServer.Controllers;

[ApiController]
[Route("api")]
public class HttpMockController : ControllerBase
{
    [HttpPost("main")]
    public IActionResult MainEndpoint([FromBody] RequestModel request)
    {
        try
        {
            return request.Command switch
            {
                "GetMenu" => Ok(HttpMockService.GetMenu()),
                "SendOrder" => Ok(HttpMockService.SendOrder()),
                _ => Ok(new { Success = false, ErrorMessage = "Неизвестная команда" })
            };
        }
        catch (Exception e)
        {
            return Ok(new { Success = false, ErrorMessage = $"{e.Message}" });
        }
    }
}