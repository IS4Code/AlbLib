/* Date: 29.8.2014, Time: 17:28 */
using System;
using System.IO;

namespace AlbLib.SaveGame
{
	/// <summary>
	/// Information about a spell.
	/// </summary>
	public struct Spell : IEquatable<Spell>, IGameResource
	{
		public SpellType Type{get;set;}
		public byte Cost{get;set;}
		public byte MinimumLevel{get;set;}
		public byte NumTargets{get;set;}
		private byte unknown1{get;set;}
		
		public Spell(Stream input) : this()
		{
			BinaryReader reader = new BinaryReader(input);
			Type = (SpellType)reader.ReadByte();
			MinimumLevel = reader.ReadByte();
			NumTargets = reader.ReadByte();
			unknown1 = reader.ReadByte();
		}
		
		public static string GetName(Magic @class, int index)
		{
			return GetName((int)@class*30+index);
		}
		
		public static string GetName(int index)
		{
			return GameData.SystemTexts.Open(203+index);
		}
		
		public int Save(Stream output)
		{
			output.WriteByte((byte)Type);
			output.WriteByte(Cost);
			output.WriteByte(MinimumLevel);
			output.WriteByte(NumTargets);
			output.WriteByte(unknown1);
			return 5;
		}
		
		public static Spell[] ReadSpellClass(Stream input)
		{
			Spell[] spells = new Spell[30];
			for(int i = 0; i < 30; i++)
			{
				spells[i] = new Spell(input);
			}
			return spells;
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			return (obj is Spell) && Equals((Spell)obj);
		}
		
		public bool Equals(Spell other)
		{
			return this.Type == other.Type && this.Cost == other.Cost && this.MinimumLevel == other.MinimumLevel && this.NumTargets == other.NumTargets && this.unknown1 == other.unknown1;
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				hashCode += 1000000007 * Type.GetHashCode();
				hashCode += 1000000009 * Cost.GetHashCode();
				hashCode += 1000000021 * MinimumLevel.GetHashCode();
				hashCode += 1000000033 * NumTargets.GetHashCode();
				hashCode += 1000000087 * unknown1.GetHashCode();
			}
			return hashCode;
		}
		
		public static bool operator ==(Spell lhs, Spell rhs)
		{
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(Spell lhs, Spell rhs)
		{
			return !(lhs == rhs);
		}
		#endregion
	}
}
