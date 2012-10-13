using System;

namespace AlbLib.Mapping
{
	/// <summary>
	/// Description of IMinimapVisible.
	/// </summary>
	public interface IMinimapVisible
	{
		byte MinimapType{get;}
		bool VisibleOnMinimap{get;}
	}
}
