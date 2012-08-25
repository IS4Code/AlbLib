using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AlbLib.Items;
using AlbLib.Localization;

namespace AlbLib
{
	namespace SaveGame
	{
		/// <summary>
		/// Information about saved game.
		/// </summary>
		public class SaveGameInfo
		{
			private short unknown1;
			
			/// <summary>
			/// Name of save game.
			/// </summary>
			public string Name{get; set;}
			
			private int unknown2;
			
			/// <summary>
			/// Game version.
			/// </summary>
			public Version Version{get; set;}
			
			private byte[] unknown3;//[7]
			
			/// <summary>
			/// Game days.
			/// </summary>
			public short Days{get; set;}
			
			/// <summary>
			/// Game hours.
			/// </summary>
			public short Hours{get; set;}
			
			/// <summary>
			/// Game minutes.
			/// </summary>
			public short Minutes{get; set;}
			
			/// <summary>
			/// Party map ID.
			/// </summary>
			public short MapID{get; set;}
			
			/// <summary>
			/// Party X position.
			/// </summary>
			public short PartyX{get; set;}
			
			/// <summary>
			/// Party Y position.
			/// </summary>
			public short PartyY{get; set;}
			
			/// <summary>
			/// Loads saved game from specified file.
			/// </summary>
			/// <param name="path">
			/// File where saved game is stored.
			/// </param>
			public SaveGameInfo(string path)
			{
				using(FileStream stream = new FileStream(path, FileMode.Open))
				{
					BinaryReader reader = new BinaryReader(stream);
					short length = reader.ReadInt16();
					unknown1 = reader.ReadInt16();
					Name = Encoding.ASCII.GetString(reader.ReadBytes(length));
					unknown2 = reader.ReadInt32();
					byte version = reader.ReadByte();
					Version = new Version(version/100, version%100);
					unknown3 = reader.ReadBytes(7);
					Days = reader.ReadInt16();
					Hours = reader.ReadInt16();
					Minutes = reader.ReadInt16();
					MapID = reader.ReadInt16();
					PartyX = reader.ReadInt16();
					PartyY = reader.ReadInt16();
				}
			}
			
			/// <summary>
			/// Loads saved game from specified <paramref name="stream"/>.
			/// </summary>
			/// <param name="stream">
			/// Input stream.
			/// </param>
			public SaveGameInfo(Stream stream)
			{
				BinaryReader reader = new BinaryReader(stream);
				short length = reader.ReadInt16();
				unknown1 = reader.ReadInt16();
				Name = Encoding.ASCII.GetString(reader.ReadBytes(length));
				unknown2 = reader.ReadInt32();
				byte version = reader.ReadByte();
				Version = new Version(version/100, version%100);
				unknown3 = reader.ReadBytes(7);
				Days = reader.ReadInt16();
				Hours = reader.ReadInt16();
				Minutes = reader.ReadInt16();
				MapID = reader.ReadInt16();
				PartyX = reader.ReadInt16();
				PartyY = reader.ReadInt16();
			}
			
			/// <summary>
			/// Writes saved game header to a stream.
			/// </summary>
			/// <param name="output">
			/// Output stream.
			/// </param>
			public void Write(Stream output)
			{
				BinaryWriter writer = new BinaryWriter(output, Encoding.ASCII);
				writer.Write((short)Name.Length);
				writer.Write(unknown1);
				writer.Write(Name.ToCharArray());
				writer.Write(unknown2);
				writer.Write((byte)(Version.Major*100+Version.Minor));
				writer.Write(unknown3);
				writer.Write(Days);
				writer.Write(Hours);
				writer.Write(Minutes);
				writer.Write(MapID);
				writer.Write(PartyX);
				writer.Write(PartyY);
			}
			
			/// <summary>
			/// Converts saved game header to byte array.
			/// </summary>
			/// <returns>
			/// Converted saved game.
			/// </returns>
			public byte[] ToRawData()
			{
				MemoryStream stream = new MemoryStream();
				Write(stream);
				return stream.ToArray();
			}
		}
		
		/// <summary>
		/// Stores information about game character.
		/// </summary>
		public class Character
		{
			private byte[] source;
			
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
			public Class Class{get; set;}
			
			/// <summary>
			/// Magic type of character.
			/// </summary>
			public Magic Magic{get; set;}
			
			/// <summary>
			/// Level of character.
			/// </summary>
			public byte Level{get; set;}
			
			/// <summary>
			/// Speaking language of character.
			/// </summary>
			public Language Language{get; set;}
			
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
				
				Gender = (Gender)data[1];
				Race = (Race)data[2];
				Class = (Class)data[3];
				Magic = (Magic)data[4];
				Level = data[5];
				Language = (Language)data[8];
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
				
				byte[] name = Encoding.ASCII.GetBytes(Name.ToCharArray());
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
		
		/// <summary>
		/// Character attribute stat.
		/// </summary>
		public struct CharacterAttribute
		{
			/// <summary>
			/// Current value.
			/// </summary>
			public short Value{get;private set;}
			
			/// <summary>
			/// Maximum available value.
			/// </summary>
			public short MaximumValue{get;private set;}
			
			/// <summary>
			/// Creates structure using actual values.
			/// </summary>
			/// <param name="value">
			/// Current value.
			/// </param>
			/// <param name="maxvalue">
			/// Maximum available value.
			/// </param>
			public CharacterAttribute(short value, short maxvalue) : this()
			{
				Value = value;
				MaximumValue = maxvalue;
			}
			
			/// <summary>
			/// Creates structure using byte array and start index.
			/// </summary>
			/// <param name="data">
			/// Byte array containing attribute.
			/// </param>
			/// <param name="startIndex">
			/// Index in array where attribute lies.
			/// </param>
			public CharacterAttribute(byte[] data, int startIndex) : this()
			{
				Value = BitConverter.ToInt16(data, startIndex);
				MaximumValue = BitConverter.ToInt16(data, startIndex+2);
			}
			
			/// <summary>
			/// Converts structure to byte array.
			/// </summary>
			/// <returns>
			/// Structure as array of bytes.
			/// </returns>
			public byte[] ToRawData()
			{
				byte[] data = new byte[4];
				BitConverter.GetBytes(Value).CopyTo(data, 0);
				BitConverter.GetBytes(MaximumValue).CopyTo(data, 2);
				return data;
			}
			
			/// <summary></summary>
			public override string ToString()
			{
				return Value+"/"+MaximumValue;
			}
		}
		
		/// <summary>
		/// Character backpack items.
		/// </summary>
		public class Backpack : Inventory
		{
			private ItemStack[] inventory = new ItemStack[24];
			
			/// <summary>
			/// Creates empty inventory.
			/// </summary>
			public Backpack(){}
			
			/// <summary>
			/// Loads inventory from byte array.
			/// </summary>
			/// <param name="data">
			/// Byte array containing inventory items.
			/// </param>
			/// <param name="offset">
			/// Position of inventory.
			/// </param>
			public Backpack(byte[] data, int offset)
			{
				for(int i = 0; i < 24; i++)
				{
					inventory[i] = new ItemStack(data, offset+i*6);
				}
			}
			
			/// <summary>
			/// Converts items to byte array.
			/// </summary>
			/// <returns>
			/// Array containg items.
			/// </returns>
			public override byte[] ToRawData()
			{
				byte[] data = new byte[144];
				for(int i = 0; i < 24; i++)
				{
					inventory[i].ToRawData().CopyTo(data, i*6);
				}
				return data;
			}
			
			/// <summary>
			/// Count of item slots.
			/// </summary>
			public override int Capacity{
				get{return 24;}
			}
			
			/// <summary>
			/// Accesses item using item <paramref name="index"/>.
			/// </summary>
			/// <param name="index">
			/// Item index in range from 0 to 23.
			/// </param>
			public override SaveGame.ItemStack this[int index]
			{
				get{
					return inventory[index];
				}
				set {
					inventory[index] = value;
				}
			}
			
			/// <summary>
			/// Accesses item using <paramref name="x"/> and <paramref name="y"/> displayed position.
			/// </summary>
			/// <param name="x">
			/// Item X position between 0 and 3.
			/// </param>
			/// <param name="y">
			/// Item Y position between 0 and 5.
			/// </param>
			public SaveGame.ItemStack this[int x, int y]
			{
				get{
					return inventory[y*4+x];
				}
				set {
					inventory[y*4+x] = value;
				}
			}
			
			/// <summary>
			/// Removes all items from inventory.
			/// </summary>
			public override void Clear()
			{
				for(int i = 0; i < 24; i++)
				{
					inventory[i] = default(ItemStack);
				}
			}
			
			/// <summary>
			/// Enumerates through all items in inventory.
			/// </summary>
			/// <returns>
			/// Item enumerator.
			/// </returns>
			public override IEnumerator<ItemStack> GetEnumerator()
			{
				return (IEnumerator<ItemStack>)inventory.GetEnumerator();
			}
		}
		
		/// <summary>
		/// Character equipment items.
		/// </summary>
		public class Equipment : Inventory
		{
			/// <summary>
			/// Item on neck.
			/// </summary>
			public ItemStack Neck;
			
			/// <summary>
			/// Item on head.
			/// </summary>
			public ItemStack Head;
			
			/// <summary>
			/// Item in tail.
			/// </summary>
			public ItemStack Tail;
			
			/// <summary>
			/// Item in right hand.
			/// </summary>
			public ItemStack RightHand;
			
			/// <summary>
			/// Item on chest.
			/// </summary>
			public ItemStack Chest;
			
			/// <summary>
			/// Item in left hand.
			/// </summary>
			public ItemStack LeftHand;
			
			/// <summary>
			/// Item on right finger.
			/// </summary>
			public ItemStack RightFinger;
			
			/// <summary>
			/// Item on feet.
			/// </summary>
			public ItemStack Feet;
			
			/// <summary>
			/// Item on left finger.
			/// </summary>
			public ItemStack LeftFinger;
			
			/// <summary>
			/// Creates empty inventory.
			/// </summary>
			public Equipment(){}
			
			/// <summary>
			/// Loads inventory from byte array.
			/// </summary>
			/// <param name="data">
			/// Byte array containing inventory items.
			/// </param>
			/// <param name="offset">
			/// Position of inventory.
			/// </param>
			public Equipment(byte[] data, int offset)
			{
				Neck = new ItemStack(data, offset);
				Head = new ItemStack(data, offset+6);
				Tail = new ItemStack(data, offset+12);
				RightHand = new ItemStack(data, offset+18);
				Chest = new ItemStack(data, offset+24);
				LeftHand = new ItemStack(data, offset+30);
				RightFinger = new ItemStack(data, offset+36);
				Feet = new ItemStack(data, offset+42);
				LeftFinger = new ItemStack(data, offset+48);
			}
			
			/// <summary>
			/// Converts items to byte array.
			/// </summary>
			/// <returns>
			/// Array containg items.
			/// </returns>
			public override byte[] ToRawData()
			{
				byte[] data = new byte[54];
				Neck.ToRawData().CopyTo(data, 0);
				Head.ToRawData().CopyTo(data, 6);
				Tail.ToRawData().CopyTo(data, 12);
				RightHand.ToRawData().CopyTo(data, 18);
				Chest.ToRawData().CopyTo(data, 24);
				LeftHand.ToRawData().CopyTo(data, 30);
				RightFinger.ToRawData().CopyTo(data, 36);
				Feet.ToRawData().CopyTo(data, 42);
				LeftFinger.ToRawData().CopyTo(data, 48);
				return data;
			}
			
			/// <summary>
			/// Accesses item using item <paramref name="index"/>.
			/// </summary>
			/// <param name="index">
			/// Item index in range from 0 to 8.
			/// </param>
			public override ItemStack this[int index]
			{
				get{
					switch(index)
					{
						case 0: return Neck;
						case 1: return Head;
						case 2: return Tail;
						case 3: return RightHand;
						case 4: return Chest;
						case 5: return LeftHand;
						case 6: return RightFinger;
						case 7: return Feet;
						case 8: return LeftFinger;
						default: throw new IndexOutOfRangeException();
					}
				}
				set{
					switch(index)
					{
						case 0: Neck = value; break;
						case 1: Head = value; break;
						case 2: Tail = value; break;
						case 3: RightHand = value; break;
						case 4: Chest = value; break;
						case 5: LeftHand = value; break;
						case 6: RightFinger = value; break;
						case 7: Feet = value; break;
						case 8: LeftFinger = value; break;
						default: throw new IndexOutOfRangeException();
					}
				}
			}
			
			/// <summary>
			/// Count of item slots.
			/// </summary>
			public override int Capacity{
				get{return 9;}
			}
			
			/// <summary>
			/// Enumerates through all items in inventory.
			/// </summary>
			/// <returns>
			/// Item enumerator.
			/// </returns>
			public override IEnumerator<ItemStack> GetEnumerator()
			{
				yield return Neck;
				yield return Head;
				yield return Tail;
				yield return RightHand;
				yield return Chest;
				yield return LeftHand;
				yield return RightFinger;
				yield return Feet;
				yield return LeftFinger;
			}
			
			/// <summary>
			/// Removes all items from inventory.
			/// </summary>
			public override void Clear()
			{
				Neck = default(ItemStack);
				Head = default(ItemStack);
				Tail = default(ItemStack);
				RightHand = default(ItemStack);
				Chest = default(ItemStack);
				LeftHand = default(ItemStack);
				RightFinger = default(ItemStack);
				Feet = default(ItemStack);
				LeftFinger = default(ItemStack);
			}
		}
		
		/// <summary>
		/// Inventory holding items.
		/// </summary>
		public abstract class Inventory : IList<ItemStack>
		{
			/// <summary>
			/// Accesses item using item <paramref name="index"/>.
			/// </summary>
			/// <param name="index">
			/// Item index from 0 to <see cref="Capacity"/>-1.
			/// </param>
			public abstract ItemStack this[int index]{
				get;set;
			}
			
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
			
			/// <summary>
			/// Enumerates through all items in inventory.
			/// </summary>
			/// <returns>
			/// Item enumerator.
			/// </returns>
			public virtual IEnumerator<ItemStack> GetEnumerator()
			{
				for(int i = 0; i < Capacity; i++)
				{
					ItemStack item = this[i];
					if(item!=null&&!item.IsEmpty)yield return item;
				}
			}
			
			bool ICollection<ItemStack>.IsReadOnly{
				get{return false;}
			}
			
			/// <summary>
			/// Count of item slots.
			/// </summary>
			public abstract int Capacity{
				get;
			}
			
			/// <summary>
			/// Counts all used item slots.
			/// </summary>
			public virtual int Count{
				get{
					int count = 0;
					foreach(ItemStack item in this)
					{
						count += 1;
					}
					return count;
				}
			}
			
			/// <summary>
			/// Checks if inventory is full.
			/// </summary>
			public bool IsFull{
				get{
					return Count == Capacity;
				}
			}
			
			/// <summary>
			/// Checks if inventory is empty.
			/// </summary>
			public bool IsEmpty{
				get{
					return Count == 0;
				}
			}
			
			bool ICollection<ItemStack>.Remove(ItemStack item)
			{
				throw new NotSupportedException();
			}
			
			/// <summary>
			/// Copies items to item array.
			/// </summary>
			/// <param name="array"></param>
			/// <param name="arrayIndex"></param>
			public void CopyTo(ItemStack[] array, int arrayIndex)
			{
				for(int i = 0; i < this.Count; i++)
				{
					array[arrayIndex+i] = this[i];
				}
			}
			
			/// <summary>
			/// Check if inventory contains given item stack.
			/// </summary>
			/// <param name="item"></param>
			/// <returns>
			/// True if inventory contains specified item.
			/// </returns>
			public bool Contains(ItemStack item)
			{
				foreach(ItemStack stack in this)
				{
					if(stack.Equals(item))return true;
				}
				return false;
			}
			
			/// <summary>
			/// Adds new <paramref name="item"/> stack to inventory.
			/// </summary>
			/// <param name="item">
			/// Item to add.
			/// </param>
			public virtual void Add(ItemStack item)
			{
				for(int i = 0; i < Capacity; i++)
				{
					if(this[i].IsEmpty)
					{
						this[i] = item;
						return;
					}
				}
				throw new Exception();//TODO
			}
			
			/// <summary>
			/// Removes item stack at specified <paramref name="index"/>.
			/// </summary>
			/// <param name="index">
			/// Index of item slot.
			/// </param>
			public virtual void RemoveAt(int index)
			{
				this[index] = null;
			}
			
			/// <summary>
			/// Decreases item count of stack at specified <paramref name="index"/>.
			/// </summary>
			/// <param name="index">
			/// Index of item slot.
			/// </param>
			public virtual void RemoveOne(int index)
			{
				if(this[index].Count > 1)this[index].Count -= 1;
				else this[index] = null;
			}
			
			void IList<ItemStack>.Insert(int index, ItemStack item)
			{
				throw new NotSupportedException();
			}
			
			int IList<ItemStack>.IndexOf(ItemStack item)
			{
				for(int i = 0; i < this.Count; i++)
				{
					if(this[i] == item)return i;
				}
				return -1;
			}
			
			/// <summary>
			/// Removes all items from inventory.
			/// </summary>
			public abstract void Clear();
			
			/// <summary>
			/// Converts items to byte array.
			/// </summary>
			/// <returns>
			/// Array containg items.
			/// </returns>
			public abstract byte[] ToRawData();
		}
		
		/// <summary>
		/// Class representing instance of item or items of same type.
		/// </summary>
		public class ItemStack
		{
			/// <summary>
			/// Count of items in stack.
			/// </summary>
			public byte Count{get;set;}
			
			/// <summary>
			/// Number of charges.
			/// </summary>
			public byte Charges{get;set;}
			
			/// <summary>
			/// How many times was item recharged.
			/// </summary>
			public byte NumRecharged{get;set;}
			
			/// <summary>
			/// Item flags.
			/// </summary>
			public ItemFlags Flags{get;set;}
			
			/// <summary>
			/// Item type.
			/// </summary>
			public short Type{get;set;}
			
			/*
			/// <summary>
			/// Checks if item stack actually exist.
			/// </summary>
			public bool Exists{
				get{return Count != 0 && Type != 0;}
			}*/
			
			/// <summary>
			/// Returns localized item name.
			/// </summary>
			public string TypeName{
				get{return Texts.GetItemName(Type);}
			}
			
			/// <summary>
			/// Returns information about item type.
			/// </summary>
			public Items.ItemState State{
				get{return ItemState.GetItemState(Type);}
			}
			
			/// <summary>
			/// Checks if item is not empty slot.
			/// </summary>
			public bool IsEmpty{
				get{
					return Count == 0 || Type == 0;
				}
			}
			
			/// <summary>
			/// Loads an item stack from byte array.
			/// </summary>
			/// <param name="data">
			/// Byte array containing item informations.
			/// </param>
			/// <param name="offset">
			/// Offset where informations are located.
			/// </param>
			public ItemStack(byte[] data, int offset)
			{
				Count = data[offset];
				Charges = data[offset+1];
				NumRecharged = data[offset+2];
				Flags = (ItemFlags)data[offset+3];
				Type = BitConverter.ToInt16(data, offset+4);
			}
			
			/// <summary>
			/// Creates new item stack from type and count.
			/// </summary>
			/// <param name="count">
			/// Count of items in stack.
			/// </param>
			/// <param name="type">
			/// Type of item or items.
			/// </param>
			public ItemStack(byte count, short type)
			{
				Count = count; Type = type;
			}
			
			/// <summary>
			/// Converts item stack to byte array.
			/// </summary>
			/// <returns>
			/// Converted item stack.
			/// </returns>
			public byte[] ToRawData()
			{
				byte[] data = new byte[6];
				data[0] = Count;
				data[1] = Charges;
				data[2] = NumRecharged;
				data[3] = (byte)Flags;
				BitConverter.GetBytes(Type).CopyTo(data, 4);
				return data;
			}
			
			/// <summary></summary>
			public override string ToString()
			{
				if(Count == 0 || Type == 0)
				{
					return "[ItemStack None]";
				}else{
					return string.Format("[ItemStack Count={0}, Type={1}, Flags={2}]", Count, TypeName+" ("+Type+")", Flags);
				}
			}
			
			/// <summary></summary>
			public override bool Equals(object obj)
			{
				if(obj is ItemStack)
				{
					return this.Equals((ItemStack)obj);
				}else{
					return false;
				}
			}
			
			/// <summary></summary>
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			/// <summary></summary>
			public bool Equals(ItemStack stack)
			{
				return this.Count == stack.Count &&
					   this.Charges == stack.Charges &&
					   this.NumRecharged == stack.NumRecharged &&
					   this.Flags == stack.Flags &&
					   this.Type == stack.Type;
			}
			
			/// <summary></summary>
			public static bool operator ==(ItemStack a, ItemStack b)
			{
				return a.Equals(b);
			}
			
			/// <summary></summary>
			public static bool operator !=(ItemStack a, ItemStack b)
			{
				return !a.Equals(b);
			}
		}
		
		/// <summary>
		/// Gender of character.
		/// </summary>
		public enum Gender : byte
		{
			/// <summary>Male.</summary>
			Male = 0,
			/// <summary>Female.</summary>
			Female = 1
		}
		/// <summary>
		/// Race of character.
		/// </summary>
		public enum Race : byte
		{
			/// <summary>Terran.</summary>
			Terran = 0,
			/// <summary>Iskai.</summary>
			Iskai = 1,
			/// <summary>Celt.</summary>
			Celt = 2,
			/// <summary>Kenget Kamulos.</summary>
			KengetKamulos = 3,
			/// <summary>Dji-Cantos.</summary>
			DjiCantos = 4,
			/// <summary>Mahino.</summary>
			Mahino = 5,
			/// <summary>Decadent.</summary>
			Decadent = 6,
			/// <summary>Umajo.</summary>
			Umajo = 7,
			/// <summary>Monster.</summary>
			Monster = 15
		}
		/// <summary>
		/// Class of character.
		/// </summary>
		public enum Class : byte
		{
			/// <summary>Pilot.</summary>
			Pilot = 0,
			/// <summary>Scientist.</summary>
			Scientist = 1,
			/// <summary>Warrior.</summary>
			Warrior = 2,
			/// <summary>Dji Kas mage.</summary>
			DjiKasMage = 3,
			/// <summary>Druid.</summary>
			Druid = 4,
			/// <summary>Enlightened one.</summary>
			EnlightenedOne = 5,
			/// <summary>Technician.</summary>
			Technician = 6,
			/// <summary>Oqulo Kamulos.</summary>
			OquloWarrior = 7,
			/// <summary>Warrior 2.</summary>
			Warrior2 = 8
		}
		/// <summary>
		/// Character language.
		/// </summary>
		[Flags]
		public enum Language : byte
		{
			/// <summary>None learnt.</summary>
			None = 0,
			/// <summary>Terran only.</summary>
			Terran = 1,
			/// <summary>Iskai only.</summary>
			Iskai = 2,
			/// <summary>Celtic only.</summary>
			Celtic = 4
		}
		/// <summary>
		/// Character magic class.
		/// </summary>
		[Flags]
		public enum Magic : byte
		{
			/// <summary>Character can't do magic.</summary>
			None = 0,
			/// <summary>Dji Kas.</summary>
			DjiKas = 1,
			/// <summary>Enlightened one.</summary>
			EnlightenedOne = 2,
			/// <summary>Druid.</summary>
			Druid = 4,
			/// <summary>Oqulo.</summary>
			OquloKamulos = 8
		}
		/// <summary>
		/// Character condition states.
		/// </summary>
		[Flags]
		public enum CharacterConditions : ushort
		{
			/// <summary>Unconscious.</summary>
			Unconscious = 0x0100,
			/// <summary>Poisoned.</summary>
			Poisoned = 0x0200,
			/// <summary>Ill.</summary>
			Ill = 0x0400,
			/// <summary>Exhausted.</summary>
			Exhausted = 0x0800,
			/// <summary>Paralyzed.</summary>
			Paralyzed = 0x1000,
			/// <summary>Fleeing.</summary>
			Fleeing = 0x2000,
			/// <summary>Intoxicated.</summary>
			Intoxicated = 0x4000,
			/// <summary>Blind.</summary>
			Blind = 0x8000,
			/// <summary>Panicking.</summary>
			Panicking = 0x01,
			/// <summary>Asleep.</summary>
			Asleep = 0x02,
			/// <summary>Insane.</summary>
			Insane = 0x04,
			/// <summary>Irritated.</summary>
			Irritated = 0x08
		}
		/// <summary>
		/// Actual playable characters in game.
		/// </summary>
		public enum CharacterPerson : byte
		{
			/// <summary>Nobody.</summary>
			None = 0,
			/// <summary>Tom Driscoll.</summary>
			Tom = 1,
			/// <summary>Rainer Hofstedt.</summary>
			Rainer = 2,
			/// <summary>Drirr.</summary>
			Drirr = 3,
			/// <summary>Sira.</summary>
			Sira = 4,
			/// <summary>Mellthas.</summary>
			Mellthas = 5,
			/// <summary>Harriet.</summary>
			Harriet = 6,
			/// <summary>Joe Bernard.</summary>
			Joe = 7,
			/// <summary>Khunag.</summary>
			Khunag = 9,
			/// <summary>Siobhan.</summary>
			Siobhan = 10
		}
		
		/// <summary>
		/// Special item flags.
		/// </summary>
		[Flags]
		public enum ItemFlags : byte
		{
			/// <summary>Item has no special flags.</summary>
			None = 0,
			/// <summary>Showing detailed info is available.</summary>
			ShowMoreInfo = 1,
			/// <summary>Item is broken and cannot be used.</summary>
			Broken = 2,
			/// <summary>Item is cursed.</summary>
			Cursed = 4
		}
	}
}