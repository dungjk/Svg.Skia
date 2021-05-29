﻿using Svg.Model.Primitives;

namespace Svg.Model.Painting.ImageFilters
{
    public sealed class PointLitSpecularImageFilter : SKImageFilter
    {
        public SKPoint3 Location { get; set; }
        public SKColor LightColor { get; set; }
        public float SurfaceScale { get; set; }
        public float Ks { get; set; }
        public float Shininess { get; set; }
        public SKImageFilter? Input { get; set; }
        public SKCropRect? CropRect { get; set; }
    }
}
