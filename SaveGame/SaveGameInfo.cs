using System;
using System.IO;
using AlbLib.Localization;

namespace AlbLib.SaveGame
{
	/// <summary>
	/// Information about saved game.
	/// </summary>
	public class SaveGameInfo
	{
		private short unknown1;
		
		/// <summary>
		/// Name of save game.
		/// </summary>
		public string Name{get; set;}
		
		private int unknown2;
		
		/// <summary>
		/// Game version.
		/// </summary>
		public Version Version{get; set;}
		
		private byte[] unknown3;//[7]
		
		/// <summary>
		/// Game days.
		/// </summary>
		public short Days{get; set;}
		
		/// <summary>
		/// Game hours.
		/// </summary>
		public short Hours{get; set;}
		
		/// <summary>
		/// Game minutes.
		/// </summary>
		public short Minutes{get; set;}
		
		/// <summary>
		/// Party map ID.
		/// </summary>
		public short MapID{get; set;}
		
		/// <summary>
		/// Party X position.
		/// </summary>
		public short PartyX{get; set;}
		
		/// <summary>
		/// Party Y position.
		/// </summary>
		public short PartyY{get; set;}
		
		/// <summary>
		/// Loads saved game from specified file.
		/// </summary>
		/// <param name="path">
		/// File where saved game is stored.
		/// </param>
		public SaveGameInfo(string path)
		{
			using(FileStream stream = new FileStream(path, FileMode.Open))
			{
				BinaryReader reader = new BinaryReader(stream, Texts.DefaultEncoding);
				short length = reader.ReadInt16();
				unknown1 = reader.ReadInt16();
				Name = new String(reader.ReadChars(length));
				unknown2 = reader.ReadInt32();
				byte version = reader.ReadByte();
				Version = new Version(version/100, version%100);
				unknown3 = reader.ReadBytes(7);
				Days = reader.ReadInt16();
				Hours = reader.ReadInt16();
				Minutes = reader.ReadInt16();
				MapID = reader.ReadInt16();
				PartyX = reader.ReadInt16();
				PartyY = reader.ReadInt16();
			}
		}
		
		/// <summary>
		/// Loads saved game from specified <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream">
		/// Input stream.
		/// </param>
		public SaveGameInfo(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream, Texts.DefaultEncoding);
			short length = reader.ReadInt16();
			unknown1 = reader.ReadInt16();
			Name = new String(reader.ReadChars(length));
			unknown2 = reader.ReadInt32();
			byte version = reader.ReadByte();
			Version = new Version(version/100, version%100);
			unknown3 = reader.ReadBytes(7);
			Days = reader.ReadInt16();
			Hours = reader.ReadInt16();
			Minutes = reader.ReadInt16();
			MapID = reader.ReadInt16();
			PartyX = reader.ReadInt16();
			PartyY = reader.ReadInt16();
		}
		
		/// <summary>
		/// Writes saved game header to a stream.
		/// </summary>
		/// <param name="output">
		/// Output stream.
		/// </param>
		public void Write(Stream output)
		{
			BinaryWriter writer = new BinaryWriter(output, Texts.DefaultEncoding);
			writer.Write((short)Name.Length);
			writer.Write(unknown1);
			writer.Write(Name.ToCharArray());
			writer.Write(unknown2);
			writer.Write((byte)(Version.Major*100+Version.Minor));
			writer.Write(unknown3);
			writer.Write(Days);
			writer.Write(Hours);
			writer.Write(Minutes);
			writer.Write(MapID);
			writer.Write(PartyX);
			writer.Write(PartyY);
		}
		
		/// <summary>
		/// Converts saved game header to byte array.
		/// </summary>
		/// <returns>
		/// Converted saved game.
		/// </returns>
		public byte[] ToRawData()
		{
			MemoryStream stream = new MemoryStream();
			Write(stream);
			return stream.ToArray();
		}
	}
}