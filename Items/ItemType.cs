using System;
using System.IO;
using AlbLib.Texts;
using Texts = AlbLib.Texts;

namespace AlbLib.Items
{
	/// <summary>
	/// Structure representing static information about item type.
	/// </summary>
	[Serializable]
	public class ItemType : GameResource, IGameResource
	{
		private static ItemType[] ItemStates;
		
		public ItemType(Stream stream, int type) : this(new BinaryReader(stream), type){}
		
		public ItemType(BinaryReader reader, int type)
		{
			Type = type;
			unknown1 = reader.ReadByte();
			Class = (ItemClass)reader.ReadByte();
			Slot = (ItemSlot)reader.ReadByte();
			BreakRate = reader.ReadByte();
			Gender = (Gender)reader.ReadByte();
			FreeHands = reader.ReadByte();
			LifePointsBonus = reader.ReadByte();
			SpellPointsBonus = reader.ReadByte();
			AttributeType = (AttributeType)reader.ReadByte();
			AttributeBonus = reader.ReadByte();
			SkillTypeBonus = (SkillType)reader.ReadByte();
			SkillBonus = reader.ReadByte();
			PhysicalDamageProtection = reader.ReadByte();
			PhysicalDamageCaused = reader.ReadByte();
			AmmunitionType = reader.ReadByte();
			SkillType1Tax = (SkillType)reader.ReadByte();
			SkillType2Tax = (SkillType)reader.ReadByte();
			Skill1Tax = reader.ReadSByte();
			Skill2Tax = reader.ReadSByte();
			TorchIntensity = reader.ReadByte();
			AmmoAnimation = reader.ReadByte();
			Spell = (ItemSpellType)reader.ReadByte();
			SpellID = reader.ReadByte();
			Charges = reader.ReadByte();
			NumRecharged = reader.ReadByte();
			MaxNumRecharged = reader.ReadByte();
			MaxCharges = reader.ReadByte();
			Count1 = reader.ReadByte();
			Count2 = reader.ReadByte();
			IconAnim = reader.ReadByte();
			Weight = reader.ReadInt16();
			FixedValue = reader.ReadInt16();
			Icon = reader.ReadInt16();
			UsingClass = reader.ReadInt16();
			UsingRace = reader.ReadInt16();
		}
		
		public int Save(Stream output)
		{
			throw new NotImplementedException();
		}
		
		private static void LoadItemStates()
		{
			using(FileStream stream = new FileStream(Paths.ItemList, FileMode.Open))
			{
				BinaryReader reader = new BinaryReader(stream);
				int count = (int)(stream.Length/40);
				ItemType[] itemStates = new ItemType[count];
				for(int i = 0; i < count; i++)
				{
					itemStates[i] = new ItemType(reader, (short)(i+1));
				}
				ItemStates = itemStates;
			}
		}
		
		/// <summary>
		/// Find specified item state for <paramref name="type"/>.
		/// </summary>
		/// <param name="type">
		/// Type ID.
		/// </param>
		/// <returns>
		/// Item state bound to the <paramref name="type"/>.
		/// </returns>
		public static ItemType GetItemType(int type)
		{
			if(type == 0)return default(ItemType);
			if(ItemStates == null)
			{
				LoadItemStates();
			}
			return ItemStates[type-1];
		}
		
		public static ItemType[] GetItemTypes()
		{
			if(ItemStates == null)
			{
				LoadItemStates();
			}
			return (ItemType[])(ItemStates.Clone());
		}
		
		/// <summary>
		/// Type ID.
		/// </summary>
		[Skip]
		public int Type{get; set;}
		
		private byte unknown1;
		
		/// <summary>
		/// Item class.
		/// </summary>
		public ItemClass Class{get; set;}
		
		/// <summary>
		/// Item slot that can hold the item.
		/// </summary>
		public ItemSlot Slot{get; set;}
		
		/// <summary>
		/// Chance to break the item.
		/// </summary>
		public byte BreakRate{get; set;}
		
		/// <summary>
		/// Gender limitation.
		/// </summary>
		public Gender Gender{get; set;}
		
		/// <summary>
		/// Number of free hands needed.
		/// </summary>
		public byte FreeHands{get; set;}
		
		/// <summary>
		/// Bonus value for life points.
		/// </summary>
		public byte LifePointsBonus{get; set;}
		
		/// <summary>
		/// Bonus value for spell points.
		/// </summary>
		public byte SpellPointsBonus{get; set;}
		
		/// <summary>
		/// Bonus attribute type.
		/// </summary>
		/// <seealso cref="AttributeBonus"/>
		public AttributeType AttributeType{get; set;}
		
		/// <summary>
		/// Bonus attribute value for type.
		/// </summary>
		/// <seealso cref="AttributeType"/>
		public byte AttributeBonus{get; set;}
		
		/// <summary>
		/// Bonus skill type.
		/// </summary>
		/// <seealso cref="SkillBonus"/>
		public SkillType SkillTypeBonus{get; set;}
		
		/// <summary>
		/// Bonus skill value for type.
		/// </summary>
		/// <seealso cref="SkillTypeBonus"/>
		public byte SkillBonus{get; set;}
		
		/// <summary>
		/// Protection from physical damage.
		/// </summary>
		public byte PhysicalDamageProtection{get; set;}
		
		/// <summary>
		/// Caused physical damage.
		/// </summary>
		public byte PhysicalDamageCaused{get; set;}
		
		/// <summary>
		/// Type of ammunition.
		/// </summary>
		public byte AmmunitionType{get; set;}
		
		/// <summary>
		/// Skill 1 tax type.
		/// </summary>
		/// <seealso cref="Skill1Tax"/>
		public SkillType SkillType1Tax{get; set;}
		
		/// <summary>
		/// Skill 2 tax type.
		/// </summary>
		/// <seealso cref="Skill2Tax"/>
		public SkillType SkillType2Tax{get; set;}
		
		/// <summary>
		/// Skill 1 tax value.
		/// </summary>
		/// <seealso cref="SkillType1Tax"/>
		public sbyte Skill1Tax{get; set;}
		
		/// <summary>
		/// Skill 2 tax value.
		/// </summary>
		/// <seealso cref="SkillType2Tax"/>
		public sbyte Skill2Tax{get; set;}
		
		/// <summary>
		/// If <see cref="Class"/> is <see cref="ItemClass.Staff"/>, intensity of torch.
		/// </summary>
		public byte TorchIntensity{get; set;}
		
		/// <summary>
		/// If <see cref="Class"/> is <see cref="ItemClass.Special"/>, what this item activates.
		/// </summary>
		public ItemActivates Activates{
			get{return (ItemActivates)TorchIntensity;}
			set{TorchIntensity = (byte)value;}
		}
		
		/// <summary>
		/// Ammo combat animation.
		/// </summary>
		public byte AmmoAnimation{get; set;}
		
		/// <summary>
		/// Spell type.
		/// </summary>
		public ItemSpellType Spell{get; set;}
		
		/// <summary>
		/// Spell ID.
		/// </summary>
		public byte SpellID{get; set;}
		
		/// <summary>
		/// If <see cref="Class"/> is <see cref="ItemClass.Staff"/>, torch lifetime. Otherwise charges left in item.
		/// </summary>
		public byte Charges{get; set;}
		
		/// <summary>
		/// How many this item was recharged.
		/// </summary>
		public byte NumRecharged{get; set;}
		
		/// <summary>
		/// How many this item can be recharged.
		/// </summary>
		public byte MaxNumRecharged{get; set;}
		
		/// <summary>
		/// Maximum possible charges.
		/// </summary>
		public byte MaxCharges{get; set;}
		
		/// <summary></summary>
		public byte Count1{get; set;}
		/// <summary></summary>
		public byte Count2{get; set;}
		
		/// <summary>
		/// Number of animated images in icon.
		/// </summary>
		public byte IconAnim{get; set;}
		
		/// <summary>
		/// Item weight in grammes.
		/// </summary>
		public short Weight{get; set;}
		
		private short FixedValue{get; set;}
		
		/// <summary>
		/// Item sell value.
		/// </summary>
		[Skip]
		public float Value{
			get{
				return FixedValue/10f;
			}
			set{
				FixedValue = (short)(value*10);
			}
		}
		
		/// <summary>
		/// Icon ID.
		/// </summary>
		public short Icon{get; set;}
		
		/// <summary>
		/// Which class can use it.
		/// </summary>
		public short UsingClass{get; set;}
		
		/// <summary>
		/// Which race can use it. Maybe broken.
		/// </summary>
		public short UsingRace{get; set;}
		
		/// <summary>
		/// Localized type name.
		/// </summary>
		public string TypeName{
			get{
				return TextCore.GetItemName(Type);
			}
		}
		
		public bool Equals(IGameResource obj)
		{
			return Equals((object)obj);
		}
		
		public override bool Equals(object obj)
		{
			ItemType other = obj as ItemType;
			if (other == null)
				return false;
			return this.Type == other.Type && this.Class == other.Class && this.Slot == other.Slot && this.BreakRate == other.BreakRate && this.Gender == other.Gender && this.FreeHands == other.FreeHands && this.LifePointsBonus == other.LifePointsBonus && this.SpellPointsBonus == other.SpellPointsBonus && this.AttributeType == other.AttributeType && this.AttributeBonus == other.AttributeBonus && this.SkillTypeBonus == other.SkillTypeBonus && this.SkillBonus == other.SkillBonus && this.PhysicalDamageProtection == other.PhysicalDamageProtection && this.PhysicalDamageCaused == other.PhysicalDamageCaused && this.AmmunitionType == other.AmmunitionType && this.SkillType1Tax == other.SkillType1Tax && this.SkillType2Tax == other.SkillType2Tax && this.Skill1Tax == other.Skill1Tax && this.Skill2Tax == other.Skill2Tax && this.TorchIntensity == other.TorchIntensity && this.AmmoAnimation == other.AmmoAnimation && this.Spell == other.Spell && this.SpellID == other.SpellID && this.Charges == other.Charges && this.NumRecharged == other.NumRecharged && this.MaxNumRecharged == other.MaxNumRecharged && this.MaxCharges == other.MaxCharges && this.Count1 == other.Count1 && this.Count2 == other.Count2 && this.IconAnim == other.IconAnim && this.Weight == other.Weight && object.Equals(this.Value, other.Value) && this.Icon == other.Icon && this.UsingClass == other.UsingClass && this.UsingRace == other.UsingRace;
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				hashCode += 1000000007 * Type.GetHashCode();
				hashCode += 1000000009 * Class.GetHashCode();
				hashCode += 1000000021 * Slot.GetHashCode();
				hashCode += 1000000033 * BreakRate.GetHashCode();
				hashCode += 1000000087 * Gender.GetHashCode();
				hashCode += 1000000093 * FreeHands.GetHashCode();
				hashCode += 1000000097 * LifePointsBonus.GetHashCode();
				hashCode += 1000000103 * SpellPointsBonus.GetHashCode();
				hashCode += 1000000123 * AttributeType.GetHashCode();
				hashCode += 1000000181 * AttributeBonus.GetHashCode();
				hashCode += 1000000207 * SkillTypeBonus.GetHashCode();
				hashCode += 1000000223 * SkillBonus.GetHashCode();
				hashCode += 1000000241 * PhysicalDamageProtection.GetHashCode();
				hashCode += 1000000271 * PhysicalDamageCaused.GetHashCode();
				hashCode += 1000000289 * AmmunitionType.GetHashCode();
				hashCode += 1000000297 * SkillType1Tax.GetHashCode();
				hashCode += 1000000321 * SkillType2Tax.GetHashCode();
				hashCode += 1000000349 * Skill1Tax.GetHashCode();
				hashCode += 1000000363 * Skill2Tax.GetHashCode();
				hashCode += 1000000403 * TorchIntensity.GetHashCode();
				hashCode += 1000000409 * AmmoAnimation.GetHashCode();
				hashCode += 1000000411 * Spell.GetHashCode();
				hashCode += 1000000427 * SpellID.GetHashCode();
				hashCode += 1000000433 * Charges.GetHashCode();
				hashCode += 1000000439 * NumRecharged.GetHashCode();
				hashCode += 1000000447 * MaxNumRecharged.GetHashCode();
				hashCode += 1000000453 * MaxCharges.GetHashCode();
				hashCode += 1000000459 * Count1.GetHashCode();
				hashCode += 1000000483 * Count2.GetHashCode();
				hashCode += 1000000513 * IconAnim.GetHashCode();
				hashCode += 1000000531 * Weight.GetHashCode();
				hashCode += 1000000579 * Value.GetHashCode();
				hashCode += 1000000007 * Icon.GetHashCode();
				hashCode += 1000000009 * UsingClass.GetHashCode();
				hashCode += 1000000021 * UsingRace.GetHashCode();
			}
			return hashCode;
		}
		
		public static bool operator ==(ItemType lhs, ItemType rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(ItemType lhs, ItemType rhs)
		{
			return !(lhs == rhs);
		}

	}
}