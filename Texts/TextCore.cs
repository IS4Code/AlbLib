using System;
using System.IO;
using System.Text;

namespace AlbLib.Texts
{
	/// <summary>
	/// Core texts class.
	/// </summary>
	public static class TextCore
	{
		private static string[][] ItemNames;
		
		/// <summary>
		/// Default language in common localization operations.
		/// </summary>
		public static Language DefaultLanguage{
			get;set;
		}
		
		/// <summary>
		/// Encoding used in all text-related readings.
		/// </summary>
		public static Encoding DefaultEncoding{
			get;set;
		}
		
		private static Encoding lastEncoding;
		
		static TextCore()
		{
			DefaultLanguage = Language.English;
			DefaultEncoding = Encoding.ASCII;
		}
		
		private static void LoadItemNames()
		{
			using(FileStream stream = new FileStream(Paths.ItemName, FileMode.Open))
			{
				BinaryReader reader = new BinaryReader(stream, TextCore.DefaultEncoding);
				int count = (int)(stream.Length/60);
				string[][] itemNames = new string[count][];
				for(int i = 0; i < count; i++)
				{
					string[] names = new string[3];
					names[0] = new string(reader.ReadChars(20)).TrimEnd('\0');
					names[1] = new string(reader.ReadChars(20)).TrimEnd('\0');
					names[2] = new string(reader.ReadChars(20)).TrimEnd('\0');
					itemNames[i] = names;
				}
				ItemNames = itemNames;
			}
			lastEncoding = DefaultEncoding;
		}
		
		/// <summary>
		/// Gets localized item name for <paramref name="type"/> using default language.
		/// </summary>
		/// <param name="type">
		/// Item type.
		/// </param>
		/// <returns>
		/// The localized name.
		/// </returns>
		public static string GetItemName(short type)
		{
			return GetItemName(type, DefaultLanguage);
		}
		
		/// <summary>
		/// Gets localized item name for <paramref name="type"/> and specified <paramref name="language"/>.
		/// </summary>
		/// <param name="type">
		/// Item type.
		/// </param>
		/// <param name="language">
		/// Language of the name.
		/// </param>
		/// <returns>
		/// The localized name.
		/// </returns>
		public static string GetItemName(short type, Language language)
		{
			if(type == 0)return String.Empty;
			if(ItemNames == null || DefaultEncoding != lastEncoding)
			{
				LoadItemNames();
			}
			return ItemNames[type-1][(int)language];
		}
		
		/// <summary>
		/// Trims a string to first null character.
		/// </summary>
		/// <param name="str">
		/// String to trim.
		/// </param>
		/// <returns>
		/// Trimmed string.
		/// </returns>
		public static string TrimNull(string str)
		{
			return str.Substring(0, str.IndexOf('\0'));
		}
		
		/// <summary>
		/// Creates a string from character array up to first null character.
		/// </summary>
		/// <param name="chars">
		/// Source character array.
		/// </param>
		/// <returns>
		/// New string.
		/// </returns>
		public static string TrimNull(char[] chars)
		{
			StringBuilder str = new StringBuilder(chars.Length);
			foreach(char ch in chars)
			{
				if(ch == '\0')break;
				str.Append(ch);
			}
			return str.ToString();
		}
	}
}