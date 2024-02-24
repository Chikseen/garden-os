using System.ComponentModel.DataAnnotations;

namespace Shared.DeviceModels
{
    public class DeviceInput : Device
    {
        [Required]
        public string ApiKey { get; init; }
    }
}