using System;
namespace AlbLib.SaveGame
{
	/// <summary>
	/// Character attribute stat.
	/// </summary>
	[Serializable]
	public struct CharacterAttribute
	{
		/// <summary>
		/// Current value.
		/// </summary>
		public short Value{get;private set;}
		
		/// <summary>
		/// Maximum available value.
		/// </summary>
		public short MaximumValue{get;private set;}
		
		/// <summary>
		/// Creates structure using actual values.
		/// </summary>
		/// <param name="value">
		/// Current value.
		/// </param>
		/// <param name="maxvalue">
		/// Maximum available value.
		/// </param>
		public CharacterAttribute(short value, short maxvalue) : this()
		{
			Value = value;
			MaximumValue = maxvalue;
		}
		
		/// <summary>
		/// Creates structure using byte array and start index.
		/// </summary>
		/// <param name="data">
		/// Byte array containing attribute.
		/// </param>
		/// <param name="startIndex">
		/// Index in array where attribute lies.
		/// </param>
		public CharacterAttribute(byte[] data, int startIndex) : this()
		{
			Value = BitConverter.ToInt16(data, startIndex);
			MaximumValue = BitConverter.ToInt16(data, startIndex+2);
		}
		
		/// <summary>
		/// Converts structure to byte array.
		/// </summary>
		/// <returns>
		/// Structure as array of bytes.
		/// </returns>
		public byte[] ToRawData()
		{
			byte[] data = new byte[4];
			BitConverter.GetBytes(Value).CopyTo(data, 0);
			BitConverter.GetBytes(MaximumValue).CopyTo(data, 2);
			return data;
		}
		
		/// <summary>
		/// Converts attribute to string.
		/// </summary>
		public override string ToString()
		{
			return Value+"/"+MaximumValue;
		}
	}
}