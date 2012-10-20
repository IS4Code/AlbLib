using System;
using System.Collections.Generic;
using System.IO;

namespace AlbLib.Mapping
{
	/// <summary>
	/// Map NPC/monster.
	/// </summary>
	[Serializable]
	public class NPC
	{
		public byte ID{get;set;}
		public byte SoundFX{get;set;}
		public short Event{get;set;}
		public short ObjectID{get;set;}
		public byte Interaction{get;set;}
		public byte Movement{get;set;}
		private short unk1;
		
		public Position[] Positions{get;set;}
		
		public bool IsEmpty{
			get{
				return ID==0;
			}
		}
		
		public NPC(Stream stream) : this(new BinaryReader(stream))
		{}
		
		public NPC(BinaryReader reader)
		{
			ID = reader.ReadByte();
			SoundFX = reader.ReadByte();
			Event = reader.ReadInt16();
			ObjectID = reader.ReadInt16();
			Interaction = reader.ReadByte();
			Movement = reader.ReadByte();
			unk1 = reader.ReadInt16();
			
			Positions = new Position[0];
		}
	}
}
