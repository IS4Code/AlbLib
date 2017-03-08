using System;
using System.IO;

namespace AlbLib.Mapping
{
	[Serializable]
	public class MapEvent
	{
		public readonly int Id;
		
		public EventType Type{get;set;}
		public byte Byte1{get;set;}
		public byte Byte2{get;set;}
		public byte Byte3{get;set;}
		public byte Byte4{get;set;}
		public byte Byte5{get;set;}
		public short Word6{get;set;}
		public short Word8{get;set;}
		public short Next{get;set;}
		
		public IArgument Argument{
			get{
				switch(Type)
				{
					case EventType.Text:
						return new Text(this);
					case EventType.MapExit:
						return new MapExit(this);
					case EventType.DoScript:
						return new DoScript(this);
					default:
						return null;
				}
			}
			set{
				switch(Type)
				{
					case EventType.Text:
						((Text)value).Restore(this);
						break;
					case EventType.MapExit:
						((MapExit)value).Restore(this);
						break;
					case EventType.DoScript:
						((DoScript)value).Restore(this);
						break;
					default:
						((IArgument)value).Restore(this);
						break;
				}
			}
		}
		
		public MapEvent(int id, Stream stream) : this(id, new BinaryReader(stream))
		{
			
		}
		
		public MapEvent(int id, BinaryReader reader)
		{
			Id = id;
			
			Type = (EventType)reader.ReadByte();
			Byte1 = reader.ReadByte();
			Byte2 = reader.ReadByte();
			Byte3 = reader.ReadByte();
			Byte4 = reader.ReadByte();
			Byte5 = reader.ReadByte();
			Word6 = reader.ReadInt16();
			Word8 = reader.ReadInt16();
			Next = reader.ReadInt16();
		}
		
		public interface IArgument
		{
			void Store(MapEvent evnt);
			void Restore(MapEvent evnt);
		}
		
		public class DoScript : IArgument
		{
			public short ScriptId{get;set;}
			
			public DoScript()
			{
				
			}
			
			public DoScript(MapEvent evnt)
			{
				Store(evnt);
			}
			
			public void Store(MapEvent evnt)
			{
				ScriptId = evnt.Word6;
			}
			
			public void Restore(MapEvent evnt)
			{
				evnt.Word6 = ScriptId;
			}
		}
		
		public class Text : IArgument
		{
			public byte TextId{get;set;}
			
			public Text()
			{
				
			}
			
			public Text(MapEvent evnt)
			{
				Store(evnt);
			}
			
			public void Store(MapEvent evnt)
			{
				TextId = evnt.Byte5;
			}
			
			public void Restore(MapEvent evnt)
			{
				evnt.Byte5 = TextId;
			}
		}
		
		public class MapExit : IArgument
		{
			public short MapId{get;set;}
			public byte X{get;set;}
			public byte Y{get;set;}
			
			public MapExit()
			{
				
			}
			
			public MapExit(MapEvent evnt)
			{
				Store(evnt);
			}
			
			public void Store(MapEvent evnt)
			{
				X = evnt.Byte1;
				Y = evnt.Byte2;
				MapId = evnt.Word6;
			}
			
			public void Restore(MapEvent evnt)
			{
				evnt.Byte1 = X;
				evnt.Byte2 = Y;
				evnt.Word6 = MapId;
			}
		}
	}
}
