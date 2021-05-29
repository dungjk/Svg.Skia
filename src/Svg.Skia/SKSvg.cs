﻿using System;
using Svg.Model;
using Svg.Model.Drawables;
using Svg.Model.Primitives;

namespace Svg.Skia
{
    public class SKSvg : IDisposable
    {
        private static readonly IAssetLoader s_assetLoader = new SkiaAssetLoader();

        public static SkiaSharp.SKPicture? ToPicture(SvgFragment svgFragment)
        {
            var picture = SvgModelExtensions.ToModel(svgFragment, s_assetLoader);
            return picture?.ToSKPicture();
        }

        public static void Draw(SkiaSharp.SKCanvas skCanvas, SvgFragment svgFragment)
        {
            var size = SvgModelExtensions.GetDimensions(svgFragment);
            var bounds = SKRect.Create(size);
            var drawable = DrawableFactory.Create(svgFragment, bounds, null, s_assetLoader);
            if (drawable is { })
            {
                drawable.PostProcess(bounds);
                var picture = drawable.Snapshot(bounds);
                picture.Draw(skCanvas);
            }
        }

        public static void Draw(SkiaSharp.SKCanvas skCanvas, string path)
        {
            var svgDocument = SvgModelExtensions.Open(path);
            if (svgDocument is { })
            {
                Draw(skCanvas, svgDocument);
            }
        }

        public SKPicture? Model { get; set; }

        public SkiaSharp.SKPicture? Picture { get; set; }

        public SkiaSharp.SKPicture? Load(System.IO.Stream stream)
        {
            Reset();
            var svgDocument = SvgModelExtensions.Open(stream);
            if (svgDocument is { })
            {
                Model = SvgModelExtensions.ToModel(svgDocument, s_assetLoader);
                Picture = Model?.ToSKPicture();
                return Picture;
            }
            return null;
        }

        public SkiaSharp.SKPicture? Load(string path)
        {
            Reset();
            var svgDocument = SvgModelExtensions.Open(path);
            if (svgDocument is { })
            {
                Model = SvgModelExtensions.ToModel(svgDocument, s_assetLoader);
                Picture = Model?.ToSKPicture();
                return Picture;
            }
            return null;
        }

        public SkiaSharp.SKPicture? FromSvg(string svg)
        {
            Reset();
            var svgDocument = SvgModelExtensions.FromSvg(svg);
            if (svgDocument is { })
            {
                Model = SvgModelExtensions.ToModel(svgDocument, s_assetLoader);
                Picture = Model?.ToSKPicture();
                return Picture;
            }
            return null;
        }

        public SkiaSharp.SKPicture? FromSvgDocument(SvgDocument? svgDocument)
        {
            Reset();
            if (svgDocument is { })
            {
                Model = SvgModelExtensions.ToModel(svgDocument, s_assetLoader);
                Picture = Model?.ToSKPicture();
                return Picture;
            }
            return null;
        }

        public bool Save(System.IO.Stream stream, SkiaSharp.SKColor background, SkiaSharp.SKEncodedImageFormat format = SkiaSharp.SKEncodedImageFormat.Png, int quality = 100, float scaleX = 1f, float scaleY = 1f)
        {
            if (Picture is { })
            {
                return Picture.ToImage(stream, background, format, quality, scaleX, scaleY, SkiaSharp.SKColorType.Rgba8888, SkiaSharp.SKAlphaType.Premul, SKSvgSettings.s_srgb);
            }
            return false;
        }

        public bool Save(string path, SkiaSharp.SKColor background, SkiaSharp.SKEncodedImageFormat format = SkiaSharp.SKEncodedImageFormat.Png, int quality = 100, float scaleX = 1f, float scaleY = 1f)
        {
            if (Picture is { })
            {
                using var stream = System.IO.File.OpenWrite(path);
                return Picture.ToImage(stream, background, format, quality, scaleX, scaleY, SkiaSharp.SKColorType.Rgba8888, SkiaSharp.SKAlphaType.Premul, SKSvgSettings.s_srgb);
            }
            return false;
        }

        private void Reset()
        {
            Model = null;

            Picture?.Dispose();
            Picture = null;
        }

        public void Dispose()
        {
            Reset();
        }
    }
}
