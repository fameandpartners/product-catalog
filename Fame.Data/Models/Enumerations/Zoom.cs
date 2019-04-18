using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace Fame.Data.Models
{
    [Serializable]
    public enum Zoom
    {
        [EnumMember(Value = "none")] None,
        [EnumMember(Value = "top")] Top,
        [EnumMember(Value = "middle")] Middle,
        [EnumMember(Value = "bottom")] Bottom
    }

    public static class ZoomExtensions
    {
        public static Rectangle GetCroppingRectangle(this Zoom zoom, Orientation orientation, int canvasWidth, int canvasHeight)
        {
            if (orientation == Orientation.Cad || orientation == Orientation.Swatch)
            {
                return new Rectangle()
                {
                    Height = canvasHeight,
                    Width = canvasWidth,
                    X = 0,
                    Y = 0
                };
            }

            switch (zoom)
            {
                case Zoom.Top:
                    return new Rectangle()
                    {
                        Height = (int)(0.4 * canvasHeight),
                        Width = (int)(0.4 * canvasWidth),
                        X = (int)(0.3 * canvasWidth),
                        Y = (int)(0.05 * canvasHeight)
                    };

                case Zoom.Bottom:
                    return new Rectangle()
                    {
                        Height = (int)(0.7 * canvasHeight),
                        Width = (int)(0.7 * canvasWidth),
                        X = (int)(0.15 * canvasWidth),
                        Y = (int)(0.3 * canvasHeight)
                    };

                case Zoom.None:
                    return new Rectangle()
                    {
                        Height = (int)(0.9 * canvasHeight),
                        Width = (int)(0.9 * canvasWidth),
                        X = (int)(0.05 * canvasWidth),
                        Y = (int)(0.1 * canvasHeight)
                    };

                case Zoom.Middle:
                    return new Rectangle()
                    {
                        Height = (int)(0.4 * canvasHeight),
                        Width = (int)(0.4 * canvasWidth),
                        X = (int)(0.3 * canvasWidth),
                        Y = (int)(0.14 * canvasHeight)
                    };

            }

            throw new ArgumentException("Unknown zoom");
        }
    }
}