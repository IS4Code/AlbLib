using System.IO;
using AlbLib.Imaging;
using AlbLib.XLD;

namespace AlbLib.Mapping
{
	/// <summary>
	/// Used for loading tile images.
	/// </summary>
	public static class IconGraphics
	{
		private static readonly RawImage[][] tilesets = new RawImage[short.MaxValue][];
		private static readonly RawImage[][] tilesetssorted = new RawImage[short.MaxValue][];
		
		/// <summary>
		/// Loads tileset as an array of RawImages.
		/// </summary>
		/// <param name="index">
		/// Zero-based tileset index.
		/// </param>
		/// <returns>
		/// Array representing the tileset.
		/// </returns>
		public static RawImage[] GetTileset(int index)
		{
			index -= 1;
			if(tilesetssorted[index] == null)
			{
				if(tilesets[index] == null)
				{
					int fx = index/100;
					int tx = index%100;
					using(FileStream stream = new FileStream(Paths.IconGraphicsN.Format(fx), FileMode.Open))
					{
						int len = XLDFile.ReadToIndex(stream, tx);
						tilesets[index] = new RawImage[len/256];
						for(int i = 0; i < len/256; i++)
						{
							tilesets[index][i] = new RawImage(stream, 16, 16);
						}
					}
				}
				TileData[] data = IconData.GetTileset(index);
				tilesetssorted[index] = new RawImage[data.Length];
				foreach(TileData td in data)
				{
					tilesetssorted[index][td.Id] = tilesets[index][td.GrID];
				}
			}
			return tilesetssorted[index];
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
		public static RawImage GetTile(int tileset, int index)
		{
			if(index <= 1)return null;
			return GetTileset(tileset)[index-2];
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
		public static RawImage GetTileUnderlay(int tileset, Tile tile)
		{
			if(tile.Underlay <= 1)return null;
			return GetTileset(tileset)[tile.Underlay-2];
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
		public static RawImage GetTileOverlay(int tileset, Tile tile)
		{
			if(tile.Overlay <= 1)return null;
			return GetTileset(tileset)[tile.Overlay-2];
		}
	}
}