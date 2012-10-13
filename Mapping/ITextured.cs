using System;

namespace AlbLib.Mapping
{
	public interface ITextured
	{
		short Texture{get;}
		byte AnimationsCount{get;}
		short TextureWidth{get;}
		short TextureHeight{get;}
		bool IsTransparent{get;}
	}
}
