/*
 * Created by SharpDevelop.
 * User: Illidan
 * Date: 28.9.2012
 * Time: 13:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace AlbLib.Mapping
{
	/// <summary>
	/// Description of Position.
	/// </summary>
	public struct Position
	{
		public byte X{get;set;}
		public byte Y{get;set;}
		
		public Position(byte x, byte y) : this()
		{
			X = x;
			Y = y;
		}
	}
}
