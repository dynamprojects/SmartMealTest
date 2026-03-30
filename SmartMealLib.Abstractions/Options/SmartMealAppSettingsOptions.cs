using System.ComponentModel.DataAnnotations;

namespace SmartMealLib.Abstractions.Options
{
    public enum SmartMealProtocol
    {
        Http,
        Grpc
    }

    public enum SmartMealAuthenticationType
    {
        None = 0,
        Basic = 1
    }
    
    public class SmartMealAppSettingsOptions
    {
        [Required]
        [Url]
        public string BaseUrl { get; set; } = string.Empty;
    
        [Required]
        public SmartMealProtocol Protocol { get; set; } = SmartMealProtocol.Http;
        
        public SmartMealAuthenticationType AuthenticationType { get; set; } = SmartMealAuthenticationType.Basic;
        
        public string? Username { get; set; }
        public string? Password { get; set; }
        
        public int HttpPort { get; set; }
        public int GrpcPort { get; set; } 
    }
}