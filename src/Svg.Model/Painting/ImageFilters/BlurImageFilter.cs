﻿
namespace Svg.Model.Painting.ImageFilters
{
    public sealed class BlurImageFilter : SKImageFilter
    {
        public float SigmaX { get; set; }
        public float SigmaY { get; set; }
        public SKImageFilter? Input { get; set; }
        public SKCropRect? CropRect { get; set; }
    }
}
