using System;
using System.Drawing;
using System.Linq;

namespace AlbLib.Imaging
{
    /// <summary>
    /// When getting color from this palette, values are multiplied by specified modifier.
    /// </summary>
    [Serializable]
    public class ModifierPalette : ImagePalette
    {
        /// <summary>
        /// Modifiers to apply.
        /// </summary>
        public BlockModifier[] Modifiers { get; }

        public ModifierPalette(ImagePalette inner, params BlockModifier[] modifiers)
        {
            ColorArray = inner.Select((color, i) => Apply(i, color, modifiers)).ToArray();
            Modifiers = modifiers;
        }

        private Color Apply(int index, Color color, BlockModifier[] modifiers)
        {
            if (Modifiers == null) return color;
            double A = color.A, R = color.R, G = color.G, B = color.B;
            foreach (BlockModifier mod in Modifiers)
            {
                if (mod == null) continue;
                if (mod.LowerIndex <= index && index <= mod.UpperIndex)
                {
                    A *= mod.A;
                    R *= mod.R;
                    G *= mod.G;
                    B *= mod.B;
                }
            }

            return Color.FromArgb((int) A, (int) R, (int) G, (int) B);
        }
    }
}