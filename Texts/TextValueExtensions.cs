/* Date: 29.8.2014, Time: 15:39 */
using System;
using AlbLib.Items;
using AlbLib.Mapping;
using AlbLib.SaveGame;

namespace AlbLib.Texts
{
	/// <summary>
	/// Class containing extension methods to retrieve text values of objects.
	/// </summary>
	public static class TextValueExtensions
	{
		public static string GetName(this MapIcon icon)
		{
			return GetValue((int)icon+166);
		}
		
		public static string GetName(this Race race)
		{
			return GetValue((int)race+456);
		}
		
		public static string GetName(this Language language)
		{
			return GetValue((int)language+663);
		}
		
		public static string GetName(this CharacterClass @class)
		{
			return GetValue((int)@class+95);
		}
		
		public static string GetInsult(this Race race)
		{
			return GetValue((int)race+472);
		}
		
		public static string GetInsult(this CharacterClass @class)
		{
			return GetValue((int)@class+135);
		}
		
		public static string GetName(this AttributeType attr)
		{
			return GetValue((int)attr+635);
		}
		
		public static string GetName(this SkillType skill)
		{
			return GetValue((int)skill+643);
		}
		
		public static string GetNameAbbreviation(this AttributeType attr)
		{
			return GetValue((int)attr+649);
		}
		
		public static string GetNameAbbreviation(this SkillType skill)
		{
			return GetValue((int)skill+657);
		}
		
		private static string GetValue(int id)
		{
			return GameData.SystemTexts.Open(id);
		}
	}
}
