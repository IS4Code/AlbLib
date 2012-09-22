using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace AlbLib
{
	namespace Internal
	{
		/// <summary>
		/// Class for accessing embedded XML resources.
		/// </summary>
		public static class Resources
		{
			/// <summary>
			/// When this set on, this class will load resources only from this assembly.
			/// </summary>
			public static bool AlwaysUseEmbedded{get;set;}
			
			/// <summary>
			/// Loads a XML resource.
			/// </summary>
			/// <param name="name">
			/// Resource name.
			/// </param>
			/// <returns>
			/// XML document.
			/// </returns>
			public static XDocument GetResource(string name)
			{
				if(File.Exists(name) && !AlwaysUseEmbedded)
				{
					return XDocument.Load(name);
				}else{
					return XDocument.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream(name));
				}
			}
			
			private static XDocument functions;
			
			/// <summary>
			/// functions
			/// |- function:
			///      name = Function name.
			///      parameters = Number of parameters.
			/// </summary>
			public static XDocument Functions{
				get{
					if(functions == null)
					{
						functions = GetResource("Functions.xml");
					}
					return functions;
				}
			}
			
			private static XDocument data;
			
			/// <summary>
			/// files
			/// |- file:
			///      name = File name.
			///      type = File type.
			/// </summary>
			public static XDocument FileData{
				get{
					if(data == null)
					{
						data = GetResource("Data.xml");
					}
					return data;
				}
			}
			
			private static XDocument chartable;
			
			/// <summary>
			/// chartable
			/// |- [treename]:
			///      char = Unicode character.
			///      code = Byte code.
			/// </summary>
			public static XDocument CharTable{
				get{
					if(chartable == null)
					{
						chartable = GetResource("CharTable.xml");
					}
					return chartable;
				}
			}
		}
	}
}