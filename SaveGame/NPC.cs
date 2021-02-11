using System;
using System.IO;
using System.Text;
using AlbLib.Texts;

namespace AlbLib.SaveGame
{
	/// <summary>
	/// Stores information about game NPC.
	/// </summary>
	[Serializable]
	public class NPC : IGameResource
	{
		public NPCType Type{get; set;}
		
		/// <summary>
		/// Gender of character.
		/// </summary>
		public Gender Gender{get; set;}
		
		/// <summary>
		/// Race of character.
		/// </summary>
		public Race Race{get; set;}
		
		/// <summary>
		/// Class of character.
		/// </summary>
		public CharacterClass Class{get; set;}
		
		/// <summary>
		/// Magic type of character.
		/// </summary>
		public MagicFlags Magic{get; set;}
		
		/// <summary>
		/// Level of character.
		/// </summary>
		public byte Level{get; set;}
		
		/// <summary>
		/// Speaking language of character.
		/// </summary>
		public LanguageFlags Language{get; set;}
		
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
		public PlayableCharacter NamedAppearance{
			get{return (PlayableCharacter)Appearance;}
			set{Appearance = (byte)value;}
		}
		
		/// <summary>
		/// Actual character face.
		/// </summary>
		public PlayableCharacter NamedFace{
			get{return (PlayableCharacter)Face;}
			set{Face = (byte)value;}
		}
		
		/// <summary>
		/// Actual character inventory picture.
		/// </summary>
		public PlayableCharacter NamedInventoryPicture{
			get{return (PlayableCharacter)InventoryPicture;}
			set{InventoryPicture = (byte)value;}
		}
		
		/// <summary>
		/// Combat action points.
		/// </summary>
		public byte ActionPoints{get; set;}
		
		public short DialogueOptions{get; set;}
		
		public short ResponseOptions{get; set;}
		
		/// <summary>
		/// Training points.
		/// </summary>
		public short TrainingPoints{get; set;}
		
		/// <summary>
		/// Gold in inventory.
		/// </summary>
		public decimal Gold{get; set;}
		
		/// <summary>
		/// Rations in inventory.
		/// </summary>
		public short Rations{get; set;}
		
		/// <summary>
		/// Character conditions.
		/// </summary>
		public ConditionFlags Conditions{get; set;}
		
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
		/// Character magic talent stat.
		/// </summary>
		public CharacterAttribute MagicTalent{get; set;}
		
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
		
		public short BaseProtection{get; set;}
		public short Protection{get; set;}
		public short BaseDamage{get; set;}
		public short Damage{get; set;}
		
		/// <summary>
		/// Character experience points.
		/// </summary>
		public int Experience{get; set;}
		
		/// <summary>
		/// Character class-relative spells.
		/// </summary>
		public int[] Spells{get; private set;}
		
		/// <summary>
		/// Character name.
		/// </summary>
		public LanguageTerm Name{get; set;}
		
		public short[,] SpellStrengths{get; private set;}
		
		private readonly byte[][] unknown;
		
		/// <summary>
		/// Creates new character.
		/// </summary>
		public NPC()
		{
			Spells = new int[7];
			SpellStrengths = new short[7,30];
			unknown = new byte[21][];
			unknown[1] = new byte[2];
			unknown[2] = new byte[5];
			unknown[3] = new byte[2];
			unknown[4] = new byte[10];
			unknown[5] = new byte[4];
			unknown[6] = new byte[4];
			unknown[7] = new byte[4];
			unknown[8] = new byte[4];
			unknown[9] = new byte[4];
			unknown[10] = new byte[4];
			unknown[11] = new byte[4];
			unknown[12] = new byte[4];
			unknown[13] = new byte[14];
			unknown[14] = new byte[4];
			unknown[15] = new byte[4];
			unknown[16] = new byte[4];
			unknown[17] = new byte[52];
			unknown[18] = new byte[2];
			unknown[19] = new byte[18];
			unknown[20] = new byte[4];
		}
		
		/// <summary>
		/// Loads NPC.
		/// </summary>
		/// <param name="data">
		/// Byte array containing character data.
		/// </param>
		public NPC(byte[] data) : this(new MemoryStream(data))
		{}
		
		public NPC(Stream stream) : this(new BinaryReader(stream, TextCore.DefaultEncoding))
		{}
		
		public NPC(BinaryReader reader) : this()
		{
			Type = (NPCType)reader.ReadByte();
			Gender = (Gender)reader.ReadByte();
			Race = (Race)reader.ReadByte();
			Class = (CharacterClass)reader.ReadByte();
			Magic = (MagicFlags)reader.ReadByte();
			Level = reader.ReadByte();
			reader.Read(unknown[1], 0, 2);
			Language = (LanguageFlags)reader.ReadByte();
			Appearance = reader.ReadByte();
			Face = reader.ReadByte();
			InventoryPicture = reader.ReadByte();
			
			reader.Read(unknown[2], 0, 5);
			
			ActionPoints = reader.ReadByte();
			DialogueOptions = reader.ReadInt16();
			ResponseOptions = reader.ReadInt16();
			TrainingPoints = reader.ReadInt16();
			Gold = reader.ReadInt16()/10M;
			Rations = reader.ReadInt16();
			
			reader.Read(unknown[3], 0, 2);
			
			Conditions = (ConditionFlags)reader.ReadInt16();
			reader.Read(unknown[4], 0, 10);
			Strength = new CharacterAttribute(reader);
			reader.Read(unknown[5], 0, 4);
			Intelligence = new CharacterAttribute(reader);
			reader.Read(unknown[6], 0, 4);
			Dexterity = new CharacterAttribute(reader);
			reader.Read(unknown[7], 0, 4);
			Speed = new CharacterAttribute(reader);
			reader.Read(unknown[8], 0, 4);
			Stamina = new CharacterAttribute(reader);
			reader.Read(unknown[9], 0, 4);
			Luck = new CharacterAttribute(reader);
			reader.Read(unknown[10], 0, 4);
			MagicResistance = new CharacterAttribute(reader);
			reader.Read(unknown[11], 0, 4);
			MagicTalent = new CharacterAttribute(reader);
			reader.Read(unknown[12], 0, 4);
			Age = reader.ReadInt16();
			reader.Read(unknown[13], 0, 14);
			CloseRangeCombat = new CharacterAttribute(reader);
			reader.Read(unknown[14], 0, 4);
			LongRangeCombat = new CharacterAttribute(reader);
			reader.Read(unknown[15], 0, 4);
			CriticalHit = new CharacterAttribute(reader);
			reader.Read(unknown[16], 0, 4);
			Lockpicking = new CharacterAttribute(reader);
			reader.Read(unknown[17], 0, 52);
			LifePoints = new CharacterAttribute(reader);
			reader.Read(unknown[18], 0, 2);
			SpellPoints = new CharacterAttribute(reader);
			BaseProtection = reader.ReadInt16();
			Protection = reader.ReadInt16();
			BaseDamage = reader.ReadInt16();
			Damage = reader.ReadInt16();
			reader.Read(unknown[19], 0, 18);
			Experience = reader.ReadInt32();
			for(int i = 0; i < 7; i++)
			{
				Spells[i] = reader.ReadInt32();
			}
			reader.Read(unknown[20], 0, 4);
			Name = new LanguageTerm(reader);
			for(int i = 0; i < 7; i++)
			{
				for(int j = 0; j < 30; j++)
				{
					SpellStrengths[i,j] = reader.ReadInt16();
				}
			}
		}
		
		/// <summary>
		/// Saves character to byte array.
		/// </summary>
		/// <returns>
		/// Saved character.
		/// </returns>
		/*public byte[] ToRawData()
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
			
			byte[] name = TextCore.DefaultEncoding.GetBytes(Name.ToCharArray());
			for(int i = 0; i < name.Length; i++)
			{
				source[274+i] = name[i];
			}
			int j = 274+Name.Length;
			while(source[j] != 0)
			{
				source[j++] = 0;
			}
			
			return (byte[])source.Clone();
		}*/
		
		int IGameResource.Save(Stream output)
		{
			throw new NotImplementedException();
		}
		
		public bool Equals(IGameResource other)
		{
			return Equals((object)other);
		}
		
		public override bool Equals(object obj)
		{
			NPC other = obj as NPC;
			if (other == null)
				return false;
			return object.Equals(this.unknown, other.unknown) && this.Type == other.Type && this.Gender == other.Gender && this.Race == other.Race && this.Class == other.Class && this.Magic == other.Magic && this.Level == other.Level && this.Language == other.Language && this.Appearance == other.Appearance && this.Face == other.Face && this.InventoryPicture == other.InventoryPicture && this.ActionPoints == other.ActionPoints && this.DialogueOptions == other.DialogueOptions && this.ResponseOptions == other.ResponseOptions && this.TrainingPoints == other.TrainingPoints && object.Equals(this.Gold, other.Gold) && this.Rations == other.Rations && this.Conditions == other.Conditions && this.Strength == other.Strength && this.Intelligence == other.Intelligence && this.Dexterity == other.Dexterity && this.Speed == other.Speed && this.Stamina == other.Stamina && this.Luck == other.Luck && this.MagicResistance == other.MagicResistance && this.MagicTalent == other.MagicTalent && this.CloseRangeCombat == other.CloseRangeCombat && this.LongRangeCombat == other.LongRangeCombat && this.CriticalHit == other.CriticalHit && this.Lockpicking == other.Lockpicking && this.LifePoints == other.LifePoints && this.SpellPoints == other.SpellPoints && this.Age == other.Age && this.BaseProtection == other.BaseProtection && this.Protection == other.Protection && this.BaseDamage == other.BaseDamage && this.Damage == other.Damage && this.Experience == other.Experience && object.Equals(this.Spells, other.Spells) && object.Equals(this.Name, other.Name) && object.Equals(this.SpellStrengths, other.SpellStrengths);
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (unknown != null)
					hashCode += 1000000007 * unknown.GetHashCode();
				hashCode += 1000000009 * Type.GetHashCode();
				hashCode += 1000000021 * Gender.GetHashCode();
				hashCode += 1000000033 * Race.GetHashCode();
				hashCode += 1000000087 * Class.GetHashCode();
				hashCode += 1000000093 * Magic.GetHashCode();
				hashCode += 1000000097 * Level.GetHashCode();
				hashCode += 1000000103 * Language.GetHashCode();
				hashCode += 1000000123 * Appearance.GetHashCode();
				hashCode += 1000000181 * Face.GetHashCode();
				hashCode += 1000000207 * InventoryPicture.GetHashCode();
				hashCode += 1000000223 * ActionPoints.GetHashCode();
				hashCode += 1000000241 * DialogueOptions.GetHashCode();
				hashCode += 1000000271 * ResponseOptions.GetHashCode();
				hashCode += 1000000289 * TrainingPoints.GetHashCode();
				hashCode += 1000000297 * Gold.GetHashCode();
				hashCode += 1000000321 * Rations.GetHashCode();
				hashCode += 1000000349 * Conditions.GetHashCode();
				hashCode += 1000000363 * Strength.GetHashCode();
				hashCode += 1000000403 * Intelligence.GetHashCode();
				hashCode += 1000000409 * Dexterity.GetHashCode();
				hashCode += 1000000411 * Speed.GetHashCode();
				hashCode += 1000000427 * Stamina.GetHashCode();
				hashCode += 1000000433 * Luck.GetHashCode();
				hashCode += 1000000439 * MagicResistance.GetHashCode();
				hashCode += 1000000447 * MagicTalent.GetHashCode();
				hashCode += 1000000453 * CloseRangeCombat.GetHashCode();
				hashCode += 1000000459 * LongRangeCombat.GetHashCode();
				hashCode += 1000000483 * CriticalHit.GetHashCode();
				hashCode += 1000000513 * Lockpicking.GetHashCode();
				hashCode += 1000000531 * LifePoints.GetHashCode();
				hashCode += 1000000579 * SpellPoints.GetHashCode();
				hashCode += 1000000007 * Age.GetHashCode();
				hashCode += 1000000009 * BaseProtection.GetHashCode();
				hashCode += 1000000021 * Protection.GetHashCode();
				hashCode += 1000000033 * BaseDamage.GetHashCode();
				hashCode += 1000000087 * Damage.GetHashCode();
				hashCode += 1000000093 * Experience.GetHashCode();
				if (Spells != null)
					hashCode += 1000000097 * Spells.GetHashCode();
				if (Name != null)
					hashCode += 1000000103 * Name.GetHashCode();
				if (SpellStrengths != null)
					hashCode += 1000000123 * SpellStrengths.GetHashCode();
			}
			return hashCode;
		}
		
		public static bool operator ==(NPC lhs, NPC rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(NPC lhs, NPC rhs)
		{
			return !(lhs == rhs);
		}

	}
}