using System;
using System.IO;

namespace AlbLib.Mapping
{
	[Serializable]
	public partial struct ObjectData : IMinimapVisible
	{
		public byte MinimapType{get;private set;}
		public SubObject[] SubObjects{get;private set;}
		
		public bool VisibleOnMinimap{get{return MinimapType>1;}}
		
		public ObjectData(Stream input) : this(new BinaryReader(input))
		{}
		
		public ObjectData(BinaryReader reader) : this()
		{
			MinimapType = reader.ReadByte();
			reader.ReadByte();
			
			SubObjects = new SubObject[8];
			for(int i = 0; i < 8; i++)
			{
				SubObjects[i] = new SubObject(reader);
			}
		}
		

	}
}
