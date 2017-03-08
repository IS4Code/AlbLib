using System;
using System.IO;
using AlbLib.XLD;

namespace AlbLib.Imaging
{
	/// <summary>
	/// Transparency tables define color blending.
	/// </summary>
	[Serializable]
	public class TransparencyTable : IGameResource
	{
		byte[] dark;
		byte[] main;
		byte[] light;
		
		/// <summary>
		/// Palette which this table applies to.
		/// </summary>
		public int Palette{
			get;set;
		}
		
		/// <param name="source">
		/// Byte array containing the table.
		/// </param>
		public TransparencyTable(byte[] source) : this(-1, source)
		{
			
		}
		
		/// <param name="palette">
		/// Palette which this table applies to.
		/// </param>
		/// <param name="source">
		/// Byte array containing the table.
		/// </param>
		public TransparencyTable(int palette, byte[] source)
		{
			Palette = palette;
			dark = new byte[65536];
			main = new byte[65536];
			light = new byte[65536];
			Array.Copy(source, 0, dark, 0, 65536);
			Array.Copy(source, 65536, dark, 0, 65536);
			Array.Copy(source, 131072, dark, 0, 65536);
		}
		
		/// <param name="input">
		/// Stream containing the table.
		/// </param>
		public TransparencyTable(Stream input) : this(-1, input)
		{
			
		}
		
		/// <param name="palette">
		/// Palette which this table applies to.
		/// </param>
		/// <param name="input">
		/// Stream containing the table.
		/// </param>
		public TransparencyTable(int palette, Stream input)
		{
			Palette = palette;
			dark = new byte[65536];
			main = new byte[65536];
			light = new byte[65536];
			input.Read(dark, 0, 65536);
			input.Read(main, 0, 65536);
			input.Read(light, 0, 65536);
		}
		
		public int Save(Stream output)
		{
			output.Write(dark, 0, dark.Length);
			output.Write(main, 0, main.Length);
			output.Write(light, 0, light.Length);
			return dark.Length+main.Length+light.Length;
		}
		
		/// <summary>
		/// Gets resulting color when blending two others.
		/// </summary>
		/// <param name="overlaying">
		/// Foreground color.
		/// </param>
		/// <param name="underlaying">
		/// Background color.
		/// </param>
		/// <param name="type">
		/// Type of blending.
		/// </param>
		/// <returns>Blended color index.</returns>
		public byte GetResultingColorIndex(byte overlaying, byte underlaying, TransparencyType type)
		{
			switch(type)
			{
				case TransparencyType.None:
					return overlaying;
				case TransparencyType.Dark:
					return dark[underlaying*256+overlaying];
				case TransparencyType.Main:
					return main[underlaying*256+overlaying];
				case TransparencyType.Light:
					return light[underlaying*256+overlaying];
				default:
					throw new ArgumentException("Unknown type.", "type");
			}
		}
		
		/// <summary>
		/// Loads transparency table for palette.
		/// </summary>
		/// <param name="palette">
		/// One-based palette index.
		/// </param>
		/// <returns>
		/// Assigned transparency table.
		/// </returns>
		public static TransparencyTable GetTransparencyTable(int palette)
		{
			return GameData.TransparencyTables.Open(palette);
		}
	}
}