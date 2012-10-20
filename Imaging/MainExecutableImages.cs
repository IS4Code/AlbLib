using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace AlbLib.Imaging
{
	/// <summary>
	/// Contains all interface images located in the main game executable.
	/// </summary>
	public static partial class MainExecutableImages
	{
		/// <summary>
		/// Checks if images have been loaded.
		/// </summary>
		public static bool Loaded{
			get{
				return images!=null;
			}
		}
		
		private static RawImage[] images;
		
		/// <summary>
		/// Returns list of all found images.
		/// </summary>
		public static IList<RawImage> Images
		{
			get{
				if(images==null)return null;
				return new ReadOnlyCollection<RawImage>(images);
			}
		}
		
		/// <summary>
		/// Loads all images.
		/// </summary>
		public static void Load()
		{
			images = new RawImage[infos.Length];
			using(FileStream stream = new FileStream(Paths.Main, FileMode.Open))
			{
				for(int i = 0; i < infos.Length; i++)
				{
					ImageLocationInfo info = infos[i];
					if(stream.Position != info.Position)
					{
						stream.Seek(info.Position, SeekOrigin.Begin);
					}
					images[i] = new RawImage(stream, info.Width, info.Height);
				}
			}
		}
		
		static ImageLocationInfo[] infos = {
			new ImageLocationInfo(1031768,14,14),//defcur
			new ImageLocationInfo(1031964,16,16),//3dmovcur
			new ImageLocationInfo(1032220,16,16),//3dmovcur
			new ImageLocationInfo(1032476,16,16),//3dmovcur
			new ImageLocationInfo(1032732,16,16),//3dmovcur
			new ImageLocationInfo(1032988,16,16),//3dmovcur
			new ImageLocationInfo(1033244,16,16),//3dmovcur
			new ImageLocationInfo(1033500,16,16),//3dmovcur
			new ImageLocationInfo(1033756,16,16),//3dmovcur
			new ImageLocationInfo(1034012,16,16),//2dmovcur
			new ImageLocationInfo(1034268,16,16),//2dmovcur
			new ImageLocationInfo(1034524,16,16),//2dmovcur
			new ImageLocationInfo(1034780,16,16),//2dmovcur
			new ImageLocationInfo(1035036,16,16),//2dmovcur
			new ImageLocationInfo(1035292,16,16),//2dmovcur
			new ImageLocationInfo(1035548,16,16),//2dmovcur
			new ImageLocationInfo(1035804,16,16),//2dmovcur
			new ImageLocationInfo(1036060,14,12),//invcur
			new ImageLocationInfo(1036216,24,15),//cdcur
			new ImageLocationInfo(1036576,16,19),//hourglass
			new ImageLocationInfo(1036880,18,25),//mouse
			new ImageLocationInfo(1037330,8,8),//itemcur
			new ImageLocationInfo(1037394,20,19),//3dpntcuract
			new ImageLocationInfo(1037774,22,21),//3dpntcur
			new ImageLocationInfo(1038236,28,21),//chip
			new ImageLocationInfo(1038796,16,16),//3dmovcur
			new ImageLocationInfo(1039052,16,16),//3dmovcur
			new ImageLocationInfo(1039632,32,64),//background
			new ImageLocationInfo(1041680,3,16),//vertline1
			new ImageLocationInfo(1041728,3,16),//vertline2
			new ImageLocationInfo(1041776,3,16),//vertline3
			new ImageLocationInfo(1041824,3,16),//vertline4
			new ImageLocationInfo(1041872,16,3),//horzline1
			new ImageLocationInfo(1041920,16,3),//horzline2
			new ImageLocationInfo(1041968,16,3),//horzline3
			new ImageLocationInfo(1042016,16,3),//horzline4
			new ImageLocationInfo(1042064,16,16),//tlcor
			new ImageLocationInfo(1042320,16,16),//trcor
			new ImageLocationInfo(1042576,16,16),//blcor
			new ImageLocationInfo(1042832,16,16),//brcor
			new ImageLocationInfo(1043088,56,16),//exit1
			new ImageLocationInfo(1043984,56,16),//exit2
			new ImageLocationInfo(1044880,56,16),//exit3
			new ImageLocationInfo(1045776,8,8),//dmg
			new ImageLocationInfo(1045840,6,8),//arm
			new ImageLocationInfo(1045888,12,10),//gold
			new ImageLocationInfo(1046008,20,10),//rations
			new ImageLocationInfo(1046208,16,16),//block
			new ImageLocationInfo(1046464,16,16),//dmgditem
			new ImageLocationInfo(1046720,50,8),//bar
			new ImageLocationInfo(1047120,16,16),//cmbmove
			new ImageLocationInfo(1047376,16,16),//cmbmelee
			new ImageLocationInfo(1047632,16,16),//cmbrange
			new ImageLocationInfo(1047888,16,16),//cmbflee
			new ImageLocationInfo(1048144,16,16),//cmbcast
			new ImageLocationInfo(1048400,16,16),//cmtitem
			new ImageLocationInfo(1048656,32,27),//monster
			new ImageLocationInfo(1049520,32,27),//monsteractive
			new ImageLocationInfo(1050384,32,25),//watch
			new ImageLocationInfo(1051604,30,29),//compassDE
			new ImageLocationInfo(1052474,30,29),//compassEN
			new ImageLocationInfo(1053344,30,29),//compassFR
			new ImageLocationInfo(1054214,6,6),//compassAnim
			new ImageLocationInfo(1054250,6,6),//compassAnim
			new ImageLocationInfo(1054286,6,6),//compassAnim
			new ImageLocationInfo(1054322,6,6),//compassAnim
			new ImageLocationInfo(1054358,6,6),//compassAnim
			new ImageLocationInfo(1054394,6,6),//compassAnim
			new ImageLocationInfo(1054430,6,6),//compassAnim
			new ImageLocationInfo(1054466,6,6),//compassAnim
			new ImageLocationInfo(1054502,18,18),//tileselect
			new ImageLocationInfo(1054826,22,22),//doorlock
			new ImageLocationInfo(1055309,34,48),//herzler
			new ImageLocationInfo(1056941,32,32),//damaged
			new ImageLocationInfo(1057965,32,32),//healed
			new ImageLocationInfo(1058989,14,13),//ill
			new ImageLocationInfo(1059171,16,16),//3dturn
			new ImageLocationInfo(1059427,16,16),//3dturn
			new ImageLocationInfo(1059683,16,16),//3dturn
			new ImageLocationInfo(1059939,16,16),//3dturn
			new ImageLocationInfo(1060195,16,16),//3dlook
			new ImageLocationInfo(1060451,16,16)//3dlook
		};
	}
}