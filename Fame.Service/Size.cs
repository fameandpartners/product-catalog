using System.Collections.Generic;

namespace Fame.Service
{
    public struct Size
    {
        public const int FULL_QUALITY = 100;
        public const int QUALITY = 95;

        public static Size SIZE_OPTION_4048 = new Size(4048, 4048, QUALITY);
        public static Size SIZE_OPTION_2816 = new Size(2816, 2816, QUALITY);
        public static Size SIZE_OPTION_1408 = new Size(1408, 1408, QUALITY);
        public static Size SIZE_OPTION_1056 = new Size(1056, 1056, QUALITY);
        public static Size SIZE_OPTION_704 = new Size(704, 704, QUALITY);
        public static Size SIZE_OPTION_528 = new Size(528, 528, QUALITY);
        public static Size SIZE_OPTION_352 = new Size(352, 352, QUALITY);

        public static Size ORIGINAL_RENDER_SIZE = new Size(6096, 6096, FULL_QUALITY);
        
        public static Size[] OPTION_SIZES = { SIZE_OPTION_352, SIZE_OPTION_528, SIZE_OPTION_704 };

        public static Size[] PRODUCT_SIZES = { SIZE_OPTION_704, SIZE_OPTION_1056, SIZE_OPTION_1408, SIZE_OPTION_2816, SIZE_OPTION_4048 };

        public static Size[] ALL_SIZES = { SIZE_OPTION_352, SIZE_OPTION_528, SIZE_OPTION_704, SIZE_OPTION_1056, SIZE_OPTION_1408, SIZE_OPTION_2816, SIZE_OPTION_4048 };
        
        public static MaxSize MAX_XL = new MaxSize { ShortSide = 1536, LongSide = 2048 };
        public static MaxSize MAX_L = new MaxSize { ShortSide = 1200, LongSide = 1600 };
        public static MaxSize MAX_M = new MaxSize { ShortSide = 768, LongSide = 1024 };
        public static MaxSize MAX_S = new MaxSize { ShortSide = 480, LongSide = 640 };
        public static MaxSize MAX_XS = new MaxSize { ShortSide = 320, LongSide = 480 };

        private static MaxSize[] MAX_SIZES = { MAX_XL, MAX_L, MAX_M, MAX_S, MAX_S, MAX_XS };

        public static IEnumerable<Size> CreateVariations(int width, int height)
        {
            var sizes = new List<Size>();
            var originalSize = new Size(width, height, FULL_QUALITY, true);
            sizes.Add(originalSize);
            foreach (var maxSize in MAX_SIZES)
            {
                var dimensions = CalculateDimensions(originalSize, maxSize);
                if (!dimensions.HasValue) continue;
                sizes.Add(dimensions.Value);
            }
            return sizes;
        } 
        
        public int Width { get; }
        public int Height { get; }
        public int Quality { get; }
        public bool IsOriginal { get; }

        public override string ToString()
        {
            return $"{Width}x{Height}";
        }

        public Size(int width, int height, int quality, bool isOriginal = false)
        {
            Width = width;
            Height = height;
            Quality = quality;
            IsOriginal = isOriginal;
        }

        public static Size? CalculateDimensions(Size currentSize, MaxSize maxSize) {
            var sourceWidth = (double)currentSize.Width;
            var sourceHeight = (double)currentSize.Height;
            var targetWidth = sourceWidth < sourceHeight ? maxSize.ShortSide : maxSize.LongSide;
            var targetHeight = sourceWidth < sourceHeight ? maxSize.LongSide : maxSize.ShortSide;

            if (sourceWidth < targetWidth && sourceHeight < targetHeight) return null;

            var widthPercent = targetWidth / sourceWidth;
            var heightPercent = targetHeight / sourceHeight;

            var percent = heightPercent < widthPercent ? heightPercent : widthPercent;

            var destWidth = (int)(sourceWidth * percent);
            var destHeight = (int)(sourceHeight * percent);

            return new Size(destWidth, destHeight, currentSize.Quality);
        }
        
        public struct MaxSize
        {
            public bool IsOriginal { get; set; }
            public int ShortSide { get; set; }
            public int LongSide { get; set; }
        }
    }
}
