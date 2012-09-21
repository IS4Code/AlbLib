using System.Drawing;
namespace AlbLib.Imaging
{
	/// <summary>
	/// When getting color from this palette, values are multiplied by specified modifier.
	/// </summary>
	public class ModifierPalette : ImagePalette
	{
		/// <summary>
		/// Source palette.
		/// </summary>
		public readonly ImagePalette Inner;
		
		/// <summary>
		/// Modifiers to apply.
		/// </summary>
		public BlockModifier[] Modifiers{get;set;}
		
		/// <summary>
		/// Creates new instance using source palette.
		/// </summary>
		/// <param name="inner">Souce palette.</param>
		public ModifierPalette(ImagePalette inner)
		{
			Inner = inner;
		}
		
		/// <summary>
		/// Gets count of all colors.
		/// </summary>
		public override int Length{
			get{
				return Inner.Length;
			}
		}
		
		/// <summary>
		/// Returns Color at index in palette.
		/// </summary>
		public override Color this[int index]{
			get{
				Color c = Inner[index];
				if(Modifiers==null)return c;
				foreach(BlockModifier mod in Modifiers)
				{
					if(mod==null)continue;
					if(mod.LowerIndex <= index && index <= mod.UpperIndex)
					{
						c = Color.FromArgb((int)(c.A*mod.A), (int)(c.R*mod.R), (int)(c.G*mod.G), (int)(c.B*mod.B));
					}
				}
				return c;
			}
		}
	}
}