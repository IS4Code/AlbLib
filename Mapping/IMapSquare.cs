using System;

namespace AlbLib.Mapping
{
	public interface IMapSquare
	{
		byte X{get;}
		byte Y{get;}
		EventHeader Event{get;}
		MapSquareType Type{get;}
	}
}
