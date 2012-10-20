/*
 * Created by SharpDevelop.
 * User: Illidan
 * Date: 27.9.2012
 * Time: 13:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;

namespace AlbLib.Mapping
{
	[Serializable]
	public class EventHeader
	{
		public short XPos{get;set;}
		public short Trigger{get;set;}
		public short BlockNumber{get;set;}
		
		public EventHeader(Stream stream) : this(new BinaryReader(stream))
		{}
		
		public EventHeader(BinaryReader reader)
		{
			XPos = (short)(reader.ReadInt16()-1);
			Trigger = reader.ReadInt16();
			BlockNumber = reader.ReadInt16();
		}
	}
}
