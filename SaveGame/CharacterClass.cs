using System;
namespace AlbLib.SaveGame
{
	/// <summary>
	/// Class of character.
	/// </summary>
	[Serializable]
	public enum CharacterClass : byte
	{
		/// <summary>
		/// Pilot.
		/// </summary>
		Pilot = 0,
		/// <summary>
		/// Scientist.
		/// </summary>
		Scientist = 1,
		/// <summary>
		/// Warrior.
		/// </summary>
		IskaiWarrior = 2,
		/// <summary>
		/// Dji Kas mage.
		/// </summary>
		DjiKasMage = 3,
		/// <summary>
		/// Druid.
		/// </summary>
		Druid = 4,
		/// <summary>
		/// Enlightened one.
		/// </summary>
		EnlightenedOne = 5,
		/// <summary>
		/// Technician.
		/// </summary>
		Technician = 6,
		/// <summary>
		/// Oqulo Kamulos.
		/// </summary>
		OquloKamulos = 8,
		/// <summary>
		/// Warrior.
		/// </summary>
		HumanWarrior = 9,
		/// <summary>
		/// Monster.
		/// </summary>
		Monster = 31
	}
}