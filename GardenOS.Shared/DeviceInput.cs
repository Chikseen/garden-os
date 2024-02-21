using System.ComponentModel.DataAnnotations;

namespace Shared
{
    public class DeviceInput
    {
        [Required]
        public float Value { get; set; }

        [Required]
        public string DeviceId { get; init; }

        [Required]
        public string GardenId { get; init; }

        [Required]
        public string ApiKey { get; init; }
    }
}