using System;
using System.IO;
using System.Text;
using AlbLib.Caching;

namespace AlbLib.Texts
{
	/// <summary>
	/// Core texts class.
	/// </summary>
	public static class TextCore
	{
		//private static string[][] ItemNames;
		private static IndexedCache<string[], RefEq<Encoding>> NameCache = new IndexedCache<string[], RefEq<Encoding>>(LoadItemName);
		
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
		
		static TextCore()
		{
			DefaultLanguage = Language.English;
			DefaultEncoding = Encoding.ASCII;
		}
		
		private static string ReadString(BinaryReader reader)
		{
			return TrimNull(reader.ReadChars(20));
		}
		
		private static string[] LoadItemName(int index, RefEq<Encoding> encoding)
		{
			if(index == 0)return new[]{"","",""};
			index -= 1;
			using(FileStream stream = new FileStream(Paths.ItemName, FileMode.Open))
			{
				stream.Seek(index*60, SeekOrigin.Begin);
				BinaryReader reader = new BinaryReader(stream, encoding.Value);
				return new string[]{ReadString(reader), ReadString(reader), ReadString(reader)};
			}
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
			return NameCache[type][(int)language];
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