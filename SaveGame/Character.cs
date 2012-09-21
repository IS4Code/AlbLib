using System;
using System.Text;
using AlbLib.Localization;

namespace AlbLib.SaveGame
{
	/// <summary>
	/// Stores information about game character.
	/// </summary>
	public class Character
	{
		private byte[] source;
		
		/// <summary>
		/// Gender of character.
		/// </summary>
		public CharacterGender Gender{get; set;}
		
		/// <summary>
		/// Race of character.
		/// </summary>
		public CharacterRace Race{get; set;}
		
		/// <summary>
		/// Class of character.
		/// </summary>
		public CharacterClass Class{get; set;}
		
		/// <summary>
		/// Magic type of character.
		/// </summary>
		public CharacterMagic Magic{get; set;}
		
		/// <summary>
		/// Level of character.
		/// </summary>
		public byte Level{get; set;}
		
		/// <summary>
		/// Speaking language of character.
		/// </summary>
		public CharacterLanguage Language{get; set;}
		
		/// <summary>
		/// Character appearance.
		/// </summary>
		public byte Appearance{get; set;}
		
		/// <summary>
		/// Character face.
		/// </summary>
		public byte Face{get; set;}
		
		/// <summary>
		/// Character inventory picture.
		/// </summary>
		public byte InventoryPicture{get; set;}
		
		/// <summary>
		/// Actual character appearance.
		/// </summary>
		public CharacterPerson NamedAppearance{
			get{return (CharacterPerson)Appearance;}
			set{Appearance = (byte)value;}
		}
		
		/// <summary>
		/// Actual character face.
		/// </summary>
		public CharacterPerson NamedFace{
			get{return (CharacterPerson)Face;}
			set{Face = (byte)value;}
		}
		
		/// <summary>
		/// Actual character inventory picture.
		/// </summary>
		public CharacterPerson NamedInventoryPicture{
			get{return (CharacterPerson)InventoryPicture;}
			set{InventoryPicture = (byte)value;}
		}
		
		/// <summary>
		/// Training points.
		/// </summary>
		public short TrainingPoints{get; set;}
		
		/// <summary>
		/// Gold in inventory.
		/// </summary>
		public float Gold{get; set;}
		
		/// <summary>
		/// Rations in inventory.
		/// </summary>
		public short Rations{get; set;}
		
		/// <summary>
		/// Character conditions.
		/// </summary>
		public CharacterConditions Conditions{get; set;}
		
		/// <summary>
		/// Character strength stat.
		/// </summary>
		public CharacterAttribute Strength{get; set;}
		
		/// <summary>
		/// Character intelligence stat.
		/// </summary>
		public CharacterAttribute Intelligence{get; set;}
		
		/// <summary>
		/// Character dexterity stat.
		/// </summary>
		public CharacterAttribute Dexterity{get; set;}
		
		/// <summary>
		/// Character speed stat.
		/// </summary>
		public CharacterAttribute Speed{get; set;}
		
		/// <summary>
		/// Character stamina stat.
		/// </summary>
		public CharacterAttribute Stamina{get; set;}
		
		/// <summary>
		/// Character luck stat.
		/// </summary>
		public CharacterAttribute Luck{get; set;}
		
		/// <summary>
		/// Character magic resistance stat.
		/// </summary>
		public CharacterAttribute MagicResistance{get; set;}
		
		/// <summary>
		/// Character magic tallent stat.
		/// </summary>
		public CharacterAttribute MagicTallent{get; set;}
		
		/// <summary>
		/// Character close-range combat points.
		/// </summary>
		public CharacterAttribute CloseRangeCombat{get; set;}
		
		/// <summary>
		/// Character long-range combat points.
		/// </summary>
		public CharacterAttribute LongRangeCombat{get; set;}
		
		/// <summary>
		/// Character critical hit stat.
		/// </summary>
		public CharacterAttribute CriticalHit{get; set;}
		
		/// <summary>
		/// Character lockpicking stat.
		/// </summary>
		public CharacterAttribute Lockpicking{get; set;}
		
		/// <summary>
		/// Character life points.
		/// </summary>
		public CharacterAttribute LifePoints{get; set;}
		
		/// <summary>
		/// Character spell points.
		/// </summary>
		public CharacterAttribute SpellPoints{get; set;}
		
		/// <summary>
		/// Character age in years.
		/// </summary>
		public short Age{get; set;}
		
		/// <summary>
		/// Character experience points.
		/// </summary>
		public int Experience{get; set;}
		
		/// <summary>
		/// Character class-relative spells.
		/// </summary>
		public int[] ClassSpells{get; private set;}
		
		/// <summary>
		/// Character name.
		/// </summary>
		public string Name{get; set;}
		
		/// <summary>
		/// Character equipment items.
		/// </summary>
		public Equipment Equipment{get; private set;}
		
		/// <summary>
		/// Character backpack items.
		/// </summary>
		public Backpack Inventory{get; private set;}
		
		/// <summary>
		/// Creates new character.
		/// </summary>
		public Character()
		{
			source = new byte[940];
			ClassSpells = new int[7];
			Equipment = new Equipment();
		}
		
		/// <summary>
		/// Loads character.
		/// </summary>
		/// <param name="data">
		/// Byte array containing character data.
		/// </param>
		public Character(byte[] data)
		{
			source = (byte[])data.Clone();
			
			Gender = (CharacterGender)data[1];
			Race = (CharacterRace)data[2];
			Class = (CharacterClass)data[3];
			Magic = (CharacterMagic)data[4];
			Level = data[5];
			Language = (CharacterLanguage)data[8];
			Appearance = data[9];
			Face = data[10];
			InventoryPicture = data[11];
			TrainingPoints = BitConverter.ToInt16(data, 22);
			Gold = BitConverter.ToInt16(data, 24)/10f;
			Rations = BitConverter.ToInt16(data, 26);
			Conditions = (CharacterConditions)BitConverter.ToInt16(data, 30);
			
			Strength = new CharacterAttribute(data, 42);
			Intelligence = new CharacterAttribute(data, 50);
			Dexterity = new CharacterAttribute(data, 58);
			Speed = new CharacterAttribute(data, 66);
			Stamina = new CharacterAttribute(data, 74);
			Luck = new CharacterAttribute(data, 82);
			MagicResistance = new CharacterAttribute(data, 90);
			MagicTallent = new CharacterAttribute(data, 98);
			Age = BitConverter.ToInt16(data, 106);
			CloseRangeCombat = new CharacterAttribute(data, 122);
			LongRangeCombat = new CharacterAttribute(data, 130);
			CriticalHit = new CharacterAttribute(data, 138);
			Lockpicking = new CharacterAttribute(data, 146);
			LifePoints = new CharacterAttribute(data, 202);
			SpellPoints = new CharacterAttribute(data, 208);
			
			Experience = BitConverter.ToInt32(data, 238);
			
			ClassSpells = new int[7];
			ClassSpells[0] = BitConverter.ToInt32(data, 242);
			ClassSpells[1] = BitConverter.ToInt32(data, 246);
			ClassSpells[2] = BitConverter.ToInt32(data, 250);
			ClassSpells[3] = BitConverter.ToInt32(data, 254);
			ClassSpells[4] = BitConverter.ToInt32(data, 258);
			ClassSpells[5] = BitConverter.ToInt32(data, 262);
			ClassSpells[6] = BitConverter.ToInt32(data, 266);
			
			StringBuilder name = new StringBuilder(16);
			int i = 274;
			while(data[i] != 0)
			{
				name.Append((char)data[i++]);
			}
			Name = name.ToString();
			
			Equipment = new Equipment(data, 742);
			Inventory = new Backpack(data, 796);
		}
		
		/// <summary>
		/// Saves character to byte array.
		/// </summary>
		/// <returns>
		/// Saved character.
		/// </returns>
		public byte[] ToRawData()
		{
			source[1] = (byte)Gender;
			source[2] = (byte)Race;
			source[3] = (byte)Class;
			source[4] = (byte)Magic;
			source[5] = Level;
			source[8] = (byte)Language;
			source[9] = Appearance;
			source[10] = Face;
			source[11] = InventoryPicture;
			BitConverter.GetBytes(TrainingPoints).CopyTo(source, 22);
			BitConverter.GetBytes((short)(Gold*10)).CopyTo(source, 24);
			BitConverter.GetBytes(Rations).CopyTo(source, 26);
			BitConverter.GetBytes((short)Conditions).CopyTo(source, 30);
			Strength.ToRawData().CopyTo(source, 42);
			Intelligence.ToRawData().CopyTo(source, 50);
			Dexterity.ToRawData().CopyTo(source, 58);
			Speed.ToRawData().CopyTo(source, 66);
			Stamina.ToRawData().CopyTo(source, 74);
			Luck.ToRawData().CopyTo(source, 82);
			MagicResistance.ToRawData().CopyTo(source, 90);
			MagicTallent.ToRawData().CopyTo(source, 98);
			BitConverter.GetBytes(Age).CopyTo(source, 106);
			CloseRangeCombat.ToRawData().CopyTo(source, 122);
			LongRangeCombat.ToRawData().CopyTo(source, 130);
			CriticalHit.ToRawData().CopyTo(source, 138);
			Lockpicking.ToRawData().CopyTo(source, 146);
			LifePoints.ToRawData().CopyTo(source, 202);
			SpellPoints.ToRawData().CopyTo(source, 208);
			BitConverter.GetBytes(Experience).CopyTo(source, 239);
			
			BitConverter.GetBytes(ClassSpells[0]).CopyTo(source, 242);
			BitConverter.GetBytes(ClassSpells[1]).CopyTo(source, 246);
			BitConverter.GetBytes(ClassSpells[2]).CopyTo(source, 250);
			BitConverter.GetBytes(ClassSpells[3]).CopyTo(source, 254);
			BitConverter.GetBytes(ClassSpells[4]).CopyTo(source, 258);
			BitConverter.GetBytes(ClassSpells[5]).CopyTo(source, 262);
			BitConverter.GetBytes(ClassSpells[6]).CopyTo(source, 266);
			
			byte[] name = Texts.DefaultEncoding.GetBytes(Name.ToCharArray());
			for(int i = 0; i < name.Length; i++)
			{
				source[274+i] = name[i];
			}
			int j = 274+Name.Length;
			while(source[j] != 0)
			{
				source[j++] = 0;
			}
			
			Equipment.ToRawData().CopyTo(source, 742);
			Inventory.ToRawData().CopyTo(source, 796);
			
			return (byte[])source.Clone();
		}
	}
}