﻿using System.ComponentModel.DataAnnotations;

namespace Shared.DeviceModels
{
    public class GardenUserModel
    {
        [Required]
        public string DeviceId { get; init; }

        [Required]
        public Dictionary<string, uint> Sensor { get; init; }

        [Required]
        public string GardenId { get; init; }
    }
}