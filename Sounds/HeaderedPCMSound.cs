using System;
using System.Collections.Generic;
using System.IO;

namespace AlbLib.Sounds
{
	[Serializable]
	public class HeaderedPCMSound : SoundBase
	{
		public List<Sample> Samples{get; private set;}
		
		public HeaderedPCMSound(Stream stream)
		{
			Samples = new List<Sample>(512);
			int maxsample = 0;
			for(int i = 0; i < 512; i++)
			{
				Sample s = new Sample(stream);
				if(s.Used)
				{
					maxsample = i;
					Samples.Add(s);
				}
			}
			Samples.Sort((s1,s2)=>s1.StartOffset.CompareTo(s2.StartOffset));
			int pos = 0x4000;
			for(int i = 0; i <= maxsample; i++)
			{
				if(Samples[i].Used)
				{
					if(pos == Samples[i].StartOffset)
					{
						Samples[i].Sound = new RawPCMSound(stream, Samples[i].Length);
						Samples[i].Sound.Rate = Samples[i].Rate;
						pos += Samples[i].Length;
					}else{
						
					}
				}
			}
		}
	}
}