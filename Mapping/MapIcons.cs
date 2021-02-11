using System.Collections.Generic;
using System.IO;
using AlbLib.Caching;
using AlbLib.Imaging;
using AlbLib.XLD;

namespace AlbLib.Mapping
{
	/// <summary>
	/// This class represents static tile data.
	/// </summary>
	public static class MapIcons
	{
		/// <summary>
		/// Returns data array for tileset.
		/// </summary>
		/// <param name="index">
		/// One-based tileset index.
		/// </param>
		public static TileData[] GetIconData(int index)
		{
			return GameData.MapIcons.Open(index);
		}
		
		public static TileData[] ReadIconData(Stream input, int length)
		{
			TileData[] tileset = new TileData[length/8];
			for(int i = 0; i < tileset.Length; i++)
			{
				tileset[i] = new TileData(i+2, input);
			}
			return tileset;
		}
		
		/// <summary>
		/// Returns tile data from tileset.
		/// </summary>
		/// <param name="tileset">
		/// One-based tileset index.
		/// </param>
		/// <param name="index">
		/// Tile index.
		/// </param>
		public static TileData GetTileData(int tileset, int index)
		{
			return GetIconData(tileset)[index];
		}
		/// <summary>
		/// Loads tileset as an array of RawImages.
		/// </summary>
		/// <param name="index">
		/// Zero-based tileset index.
		/// </param>
		/// <returns>
		/// Array representing the tileset.
		/// </returns>
		public static RawImage[] GetIcons(int index)
		{
			return GameData.IconGraphics.Open(index);
		}
		
		public static RawImage[] ReadIconGraphics(Stream input, int length)
		{
			RawImage[] tileset = new RawImage[length/256];
			for(int i = 0; i < length/256; i++)
			{
				tileset[i] = new RawImage(input, 16, 16);
			}
			return tileset;
		}
		
		public static RawImage GetIconGraphics(int tileset, int grindex)
		{
			return GetIcons(tileset)[grindex];
		}

        /// <summary>
        /// Gets tile graphics using tile index.
        /// </summary>
        /// <param name="tileset">
        /// Zero-based tileset index.
        /// </param>
        /// <param name="index">
        /// Overlay or underlay.
        /// </param>
        public static IEnumerable<RawImage> GetTileGraphics(int tileset, int index)
        {
            if (index <= 1) yield break;
            TileData[] tiledata = MapIcons.GetIconData(tileset);
            TileData current = tiledata[index - 2];
            for (int i = 0; i < current.FramesCount; i++)
            {
                yield return GetIconGraphics(tileset, current.GrID + i);
            }

        }

        /// <summary>
		/// Returns image representing underlay portion of tile.
		/// </summary>
		/// <param name="tileset">
		/// Zero-based tileset index.
		/// </param>
		/// <param name="tile">
		/// Tile.
		/// </param>
		/// <returns>
		/// Underlay image.
		/// </returns>
		public static IEnumerable<RawImage> GetTileUnderlay(int tileset, Tile tile)
		{
			return GetTileGraphics(tileset, tile.Underlay);
		}
		
		/// <summary>
		/// Returns image representing overlay portion of tile.
		/// </summary>
		/// <param name="tileset">
		/// Zero-based tileset index.
		/// </param>
		/// <param name="tile">
		/// Tile.
		/// </param>
		/// <returns>
		/// Overlay image.
		/// </returns>
		public static IEnumerable<RawImage> GetTileOverlay(int tileset, Tile tile)
		{
			return GetTileGraphics(tileset, tile.Overlay);
		}
	}
}