using System;

namespace AlbLib.Mapping
{
	[Serializable]
	public class CombineArgs
	{
		public static readonly CombineArgs Default = new CombineArgs();
		
		//2D
		public bool ShowUnderlays{get;set;}
		public bool ShowOverlays{get;set;}
		public bool ShowNPCs2D{get;set;}
		
		//3D
		public bool ShowFloors{get;set;}
		public bool ShowWalls{get;set;}
		public bool ShowObjects{get;set;}
		public bool ShowNPCs3D{get;set;}
		public bool ShowGotoPoints{get;set;}
		
		public bool ShowHelpers{get;set;}
		public bool ShowDebug{get;set;}
		
		public CombineArgs()
		{
			ShowUnderlays = true;
			ShowOverlays = true;
			ShowFloors = true;
			ShowWalls = true;
			ShowObjects = true;
			ShowNPCs3D = true;
			ShowNPCs2D = true;
			ShowGotoPoints = true;
			ShowHelpers = false;
			ShowDebug = false;
		}
	}
}
