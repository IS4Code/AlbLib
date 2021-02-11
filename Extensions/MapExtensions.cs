using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using AlbLib.Imaging;
using AlbLib.Mapping;

namespace AlbLib.Extensions
{
    public static class MapExtensions
    {
        private const int TileEdgeInPixels = 16;

        private static readonly int[] IgnoredTileIds = new[]
        {
            2763, // This is an invisible movement blocker in the game and must not be rendered; looks like a dashed square
            2764 // This is an invisible trigger in the game and must not be rendered; looks like a dotted square
        };

        /// <summary>
        /// Renders the entire map. The rendering is performed lazily one animation frame at a time.
        /// The method returns enough frames, so that a seamless, continuous animation can be created as a APNG, a GIF, or whatever else you wish to use.
        /// If only a static image is required, it's most efficient to only take the first element from the enumeration.
        ///
        /// Hint: when creating animations, you should set the delay between frames to 1/10th of a second
        /// </summary>
        /// <param name="map">The map to render the full images from</param>
        /// <returns>An enumeration of all animation frames for the whole map</returns>
        public static IEnumerable<Bitmap> RenderFullMap(this Map map)
        {
            if (map == null)
            {
                throw new ArgumentNullException($"Parameter '{nameof(map)}' must not be null");
            }

            RenderOptions underlayOptions =
                new RenderOptions(ImagePalette.GetFullPalette(map.Palette));

            RenderOptions overlayOptions =
                new RenderOptions(underlayOptions) {TransparentIndex = 0};

            IDictionary<short, RawImage[]> cache = BuildCache(map);
            var totalMapFramesForSeamlessAnimation = CalculateLeastCommonMultiple(cache);

            // Creating new bitmaps is an expensive operation so we will just reuse this object to draw all tiles; reduces the time to render the whole map significantly.
            Bitmap tileCache = new Bitmap(TileEdgeInPixels, TileEdgeInPixels, PixelFormat.Format8bppIndexed);

            for (var mapFrame = 0; mapFrame < totalMapFramesForSeamlessAnimation; mapFrame++)
            {
                Bitmap mapBitmap = new Bitmap(map.Width * TileEdgeInPixels, map.Height * TileEdgeInPixels);
                Graphics mapGraphics = Graphics.FromImage(mapBitmap);

                foreach (Tile tile in map.TileData)
                {
                    DrawTile(tile.Underlay, cache, mapFrame, mapGraphics, underlayOptions, tileCache, tile);
                    DrawTile(tile.Overlay, cache, mapFrame, mapGraphics, overlayOptions, tileCache, tile);
                }

                yield return mapBitmap;
            }
        }

        private static void DrawTile(short tileId, IDictionary<short, RawImage[]> cache, int mapFrame,
            Graphics mapGraphics,
            RenderOptions renderOptions, Bitmap tileCache, Tile tile)
        {
            if (tileId <= 1 || IgnoredTileIds.Contains(tileId))
                return;

            // retrieves the correct animated frame for this tile based on the map frame
            // one tile animation may play many times before all map frames are completed
            // in order to allow a seamless animation of the entire map
            RawImage localTileFrame = cache[tileId][mapFrame % cache[tileId].Length];
            mapGraphics.DrawImageUnscaled(localTileFrame.Render(renderOptions, tileCache), tile.X * TileEdgeInPixels,
                tile.Y * TileEdgeInPixels);
        }

        private static int CalculateLeastCommonMultiple(IDictionary<short, RawImage[]> cache)
        {
            var uniqueCounts = (from frames in cache
                select frames.Value.Length).Distinct().ToArray();

            var result = uniqueCounts[0];

            for (var i = 1; i < uniqueCounts.Length; i++)
            {
                result = CalculateLeastCommonMultiple(result, uniqueCounts[i]);
            }

            return result;
        }

        private static int CalculateLeastCommonMultiple(int a, int b)
        {
            return a * b / CalculateGreatestCommonFactor(a, b);
        }

        private static int CalculateGreatestCommonFactor(int a, int b)
        {
            while (b != 0)
            {
                var newA = b;
                b = a % b;
                a = newA;
            }

            return a;
        }

        /// <summary>
        /// Builds a cache of all tile graphics on the map. This helps to avoid redrawing graphics that occur more than once.
        /// </summary>
        /// <param name="map">The map to build the cache from</param>
        /// <returns>The filled cache</returns>
        private static IDictionary<short, RawImage[]> BuildCache(Map map)
        {
            Dictionary<short, RawImage[]>
                cache = new Dictionary<short, RawImage[]>(
                    map.TileData
                        .Length); // will pretty much always be much shorter since tiles will repeat often. But serves well as a maximum size.

            foreach (Tile tile in map.TileData)
            {
                var underlay = tile.Underlay;
                if (underlay > 1 && !cache.ContainsKey(underlay))
                {
                    cache[underlay] = MapIcons.GetTileUnderlay(map.Tileset, tile).ToArray();
                }


                var overlay = tile.Overlay;
                if (overlay > 1 && !cache.ContainsKey(overlay))
                {
                    cache[overlay] = MapIcons.GetTileOverlay(map.Tileset, tile).ToArray();
                }
            }

            return cache;
        }
    }
}
