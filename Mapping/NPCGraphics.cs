using System;
using AlbLib.Caching;
using AlbLib.Imaging;
using AlbLib.XLD;

namespace AlbLib.Mapping
{
	public static class NPCGraphics
	{
		public static HeaderedImage GetNPCBig(int index)
		{
			return GameData.BigNPC.Open(index);
		}
		
		public static HeaderedImage GetNPCSmall(int index)
		{
			return GameData.SmallNPC.Open(index);
		}
	}
}
