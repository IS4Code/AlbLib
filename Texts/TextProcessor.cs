/* Date: 22.8.2014, Time: 12:35 */
using System;
using System.IO;
using System.Text;

namespace AlbLib.Texts
{
	public static class TextProcessor
	{
		private static string nameformat = "&lt;{0} name&gt;";
		
		public static string TextToHtml(string text)
		{
			string selected = null;
			
			StringBuilder output = new StringBuilder();
			StringReader reader = new StringReader(text);
			char[] buf = new char[4];
			int r;
			while((r = reader.Read()) != -1)
			{
				char c = (char)r;
				switch(c)
				{
					case '^':
						output.Append("<br>");
						break;
					case '{':
						if(reader.ReadBlock(buf, 0, 4) != 4) throw new InvalidDataException("Bad formatting.");
						string code = new string(buf);
						switch(code)
						{
							case "INK ":
								if(reader.ReadBlock(buf, 0, 3) != 3) throw new InvalidDataException("Bad formatting.");
								buf[3] = '\0';
								int ink = Int32.Parse(new string(buf));
								break;
							case "LEAD":
								selected = "leader";
								break;
							case "NAME":
								output.Append(String.Format(nameformat, selected));
								break;
							default:
								break;
						}
						if(reader.Read() != (int)'}') throw new InvalidDataException("Bad formatting.");
						break;
					default:
						output.Append(c);
						break;
				}
			}
			return output.ToString();
		}
	}
}
