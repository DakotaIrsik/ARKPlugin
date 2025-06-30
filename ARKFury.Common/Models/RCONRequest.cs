using ArkFury.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ArkFury.Common.Models
{
    public class RCONRequest
    {
        public string Command { get; set; }
        public int Port { get; set; } 
        public string Server { get; set; }
        public string Password { get; set; }
    }
}
