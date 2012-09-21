namespace AlbLib.Items
{
	/// <summary>
	/// Skill type for item.
	/// </summary>
	public enum SkillType : byte
	{
		/// <summary>Close-range combat.</summary>
		CloseRangeCombat = 0,
		/// <summary>Long-range combat.</summary>
		LongRangeCombat = 1,
		/// <summary>Critical hit.</summary>
		CriticalHit = 2,
		/// <summary>Lockpicking.</summary>
		Lockpicking = 3
	}
}