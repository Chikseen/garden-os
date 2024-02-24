﻿using System.ComponentModel.DataAnnotations;

namespace Shared.DeviceModels
{
    public class Device
    {
        [Required]
        public string DeviceId { get; init; }

        [Required]
        public Dictionary<string, int> Sensor { get; init; }

        [Required]
        public string GardenId { get; init; }
    }
}