using System;

namespace AlbLib.Mapping
{
	/// <summary>
	/// Description of Position.
	/// </summary>
	[Serializable]
	public struct Position
	{
		public byte X{get;set;}
		public byte Y{get;set;}
		
		public Position(byte x, byte y) : this()
		{
			X = x;
			Y = y;
		}
	}
}
