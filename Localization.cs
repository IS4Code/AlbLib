using System.IO;
using System.Text;

namespace AlbLib
{
	namespace Localization
	{
		/// <summary>
		/// Core localization class.
		/// </summary>
		public static class Texts
		{
			private static string[][] ItemNames;
			
			/// <summary>
			/// Default language in common localization operations.
			/// </summary>
			public static Language DefaultLanguage{
				get;set;
			}
			
			static Texts()
			{
				DefaultLanguage = Language.English;
			}
			
			private static void LoadItemNames()
			{
				using(FileStream stream = new FileStream(Paths.ItemName, FileMode.Open))
				{
					BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
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
				if(ItemNames == null)
				{
					LoadItemNames();
				}
				return ItemNames[type-1][(int)language];
			}
		}
		
		/// <summary>
		/// Used game languages.
		/// </summary>
		public enum Language
		{
			/// <summary>German.</summary>
			German = 0,
			/// <summary>English.</summary>
			English = 1,
			/// <summary>French.</summary>
			French = 2
		}
	}
}