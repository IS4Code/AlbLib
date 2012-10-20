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
	public struct ItemState
	{
		private static ItemState[] ItemStates;
		
		public ItemState(Stream stream, short type) : this(new BinaryReader(stream), type){}
		
		public ItemState(BinaryReader reader, short type) : this()
		{
			Type = type;
			reader.ReadByte();
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
			Value = reader.ReadInt16()/10f;
			Icon = reader.ReadInt16();
			UsingClass = reader.ReadInt16();
			UsingRace = reader.ReadInt16();
		}
		
		private static void LoadItemStates()
		{
			using(FileStream stream = new FileStream(Paths.ItemList, FileMode.Open))
			{
				BinaryReader reader = new BinaryReader(stream);
				int count = (int)(stream.Length/40);
				ItemState[] itemStates = new ItemState[count];
				for(int i = 0; i < count; i++)
				{
					itemStates[i] = new ItemState(reader, (short)(i+1));
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
		public static ItemState GetItemState(short type)
		{
			if(type == 0)return default(ItemState);
			if(ItemStates == null)
			{
				LoadItemStates();
			}
			return ItemStates[type-1];
		}
		
		/// <summary>
		/// Type ID.
		/// </summary>
		public short Type{get; set;}
		
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
		
		/// <summary>
		/// Item sell value.
		/// </summary>
		public float Value{get; set;}
		
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
	}
}