using System.ComponentModel.DataAnnotations;

namespace Shared.DeviceModels
{
    public class DeviceInput : GardenUserModel
    {
        [Required]
        public string ApiKey { get; init; }
    }
}