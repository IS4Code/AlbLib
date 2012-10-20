using System;
using System.Drawing;

namespace AlbLib.Imaging
{
	/// <summary>
	/// When getting color from this palette, values are multiplied by specified modifier.
	/// </summary>
	[Serializable]
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
		
		public ModifierPalette(ImagePalette inner, params BlockModifier[] modifiers)
		{
			Inner = inner;
			Modifiers = modifiers;
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
				double A = c.A,R = c.R,G = c.G,B = c.B;
				foreach(BlockModifier mod in Modifiers)
				{
					if(mod==null)continue;
					if(mod.LowerIndex <= index && index <= mod.UpperIndex)
					{
						A *= mod.A;
						R *= mod.R;
						G *= mod.G;
						B *= mod.B;
					}
				}
				return Color.FromArgb((int)A,(int)R,(int)G,(int)B);
			}
		}
	}
}