namespace AlbLib.Imaging
{
	/// <summary>
	/// Modifier can be used to multiply color values.
	/// </summary>
	public class BlockModifier
	{
		/// <summary>
		/// R multiplier.
		/// </summary>
		public readonly double R;
		
		/// <summary>
		/// G multiplier.
		/// </summary>
		public readonly double G;
		
		/// <summary>
		/// B multiplier.
		/// </summary>
		public readonly double B;
		
		/// <summary>
		/// Alpha multiplier.
		/// </summary>
		public readonly double A;
		
		/// <summary>
		/// Modifier will be applied on values going from <see cref="LowerIndex"/> to <see cref="UpperIndex"/>, inclusive.
		/// </summary>
		public readonly int LowerIndex;
		
		/// <summary>
		/// Modifier will be applied on values going from <see cref="LowerIndex"/> to <see cref="UpperIndex"/>, inclusive.
		/// </summary>
		public readonly int UpperIndex;
		
		/// <param name="r">R modifier.</param>
		/// <param name="g">G modifier.</param>
		/// <param name="b">B modifier.</param>
		/// <param name="a">Alpha modifier.</param>
		/// <param name="lower">Lower color index.</param>
		/// <param name="upper">Upper color index.</param>
		public BlockModifier(double r, double g, double b, double a, int lower, int upper)
		{
			R = r;
			G = g;
			B = b;
			A = a;
			LowerIndex = lower;
			UpperIndex = upper;
		}
		
		/// <param name="r">R modifier.</param>
		/// <param name="g">G modifier.</param>
		/// <param name="b">B modifier.</param>
		/// <param name="lower">Lower color index.</param>
		/// <param name="upper">Upper color index.</param>
		public BlockModifier(double r, double g, double b, int lower, int upper)
		{
			R = r;
			G = g;
			B = b;
			A = 1;
			LowerIndex = lower;
			UpperIndex = upper;
		}
		
		/// <param name="mod">All but alpha modifiers.</param>
		/// <param name="lower">Lower color index.</param>
		/// <param name="upper">Upper color index.</param>
		public BlockModifier(double mod, int lower, int upper)
		{
			R = mod;
			G = mod;
			B = mod;
			A = 1;
			LowerIndex = lower;
			UpperIndex = upper;
		}
		
		/// <param name="mod">All but alpha modifiers.</param>
		/// <param name="a">Alpha modifier.</param>
		/// <param name="lower">Lower color index.</param>
		/// <param name="upper">Upper color index.</param>
		public BlockModifier(double mod, double a, int lower, int upper)
		{
			R = mod;
			G = mod;
			B = mod;
			A = a;
			LowerIndex = lower;
			UpperIndex = upper;
		}
		
		/// <summary>
		/// Creates night modifier. Applies only to local palette.
		/// </summary>
		/// <param name="mod">
		/// 1 is day, 0.5 is midnight.
		/// </param>
		public static BlockModifier Night(double mod)
		{
			return new BlockModifier(mod, 0, 191);
		}
	}
}