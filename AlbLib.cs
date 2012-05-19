using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AlbLib
{
	public static class Paths
	{
		public static string Palette0;
		public static string GlobalPalette;
		public static string ScriptsN;
		public static string Scripts0;
		public static string Scripts2;
		public static string ItemName;
		public static string ItemList;
		
		public static void SetXLDLIBS(string path)
		{
			Palette0 = Path.Combine(path, "PALETTE0.XLD");
			GlobalPalette = Path.Combine(path, "PALETTE.000");
			ScriptsN = Path.Combine(path, "SCRIPT{0}.XLD");
			Scripts0 = Path.Combine(path, "SCRIPT0.XLD");
			Scripts2 = Path.Combine(path, "SCRIPT2.XLD");
			ItemName =  Path.Combine(path, "ITEMNAME.DAT");
			ItemList =  Path.Combine(path, "ITEMLIST.DAT");
		}
	}

	public static class XLD
	{
		public delegate int SubfileHandler(short index, int length, BinaryReader reader);
		
		public static BinaryReader ReadToIndex(Stream stream, int index)
		{
			int length;
			return ReadToIndex(stream, index, out length);
		}
		
		public static BinaryReader ReadToIndex(Stream stream, int index, out int length)
		{
			int lastEntry = -1;
			BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
			short nentries = 0;
			int[] entrylen = null;
			
			string sig = new string(reader.ReadChars(6));
			if(sig != "XLD0I\0")
			{
				throw new Exception("This is not valid XLD file.");
			}
			nentries = reader.ReadInt16();
			entrylen = new int[nentries];
			for(int i = 0; i < nentries; i++)
			{
				entrylen[i] = reader.ReadInt32();
			}
			for(int i = 0; i < nentries; i++)
			{
				lastEntry = i;
				if(i == index)
				{
					length = entrylen[i];
					return reader;
				}else{
					reader.ReadBytes(entrylen[i]);
				}
			}
			length = -1;
			return null;
		}
		
		public static byte[] ReadSubfile(Stream stream, short index)
		{
			int lastEntry = -1;
			BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
			short nentries = 0;
			int[] entrylen = null;
			
			string sig = new string(reader.ReadChars(6));
			if(sig != "XLD0I\0")
			{
				throw new Exception("This is not valid XLD file.");
			}
			nentries = reader.ReadInt16();
			entrylen = new int[nentries];
			for(int i = 0; i < nentries; i++)
			{
				entrylen[i] = reader.ReadInt32();
			}
			for(int i = 0; i < nentries; i++)
			{
				lastEntry = i;
				if(i == index)
				{
					return reader.ReadBytes(entrylen[i]);;
				}else{
					reader.ReadBytes(entrylen[i]);
				}
			}
			return null;
		}
		
		public static void ForEachSubfile(Stream stream, SubfileHandler handler)
		{
			int lastEntry = -1;
			BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
			short nentries = 0;
			int[] entrylen = null;
			
			string sig = new string(reader.ReadChars(6));
			if(sig != "XLD0I\0")
			{
				throw new Exception("This is not valid XLD file.");
			}
			nentries = reader.ReadInt16();
			entrylen = new int[nentries];
			for(short i = 0; i < nentries; i++)
			{
				entrylen[i] = reader.ReadInt32();
			}
			for(short i = 0; i < nentries; i++)
			{
				lastEntry = i;
				int read = handler(i, entrylen[i], reader);
				if(read < entrylen[i])
				{
					reader.ReadBytes(entrylen[i]-read);
				}else if(read > entrylen[i])
				{
					throw new Exception();
				}
			}
		}
		
		public static XLDInfo Parse(byte[] data)
		{
			using(MemoryStream stream = new MemoryStream(data))
			{
				return Parse(stream);
			}
		}
		
		public static XLDInfo Parse(string path)
		{
			using(FileStream stream = new FileStream(path, FileMode.Open))
			{
				return Parse(stream);
			}
		}
		
		public static XLDInfo Parse(Stream stream)
		{
			int lastEntry = -1;
			BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
			short nentries = 0;
			int[] entrylen = null;
			string sig = new string(reader.ReadChars(6));
			if(sig != "XLD0I\0")
			{
				throw new Exception("This is not valid XLD file.");
			}
			nentries = reader.ReadInt16();
			entrylen = new int[nentries];
			for(int i = 0; i < nentries; i++)
			{
				entrylen[i] = reader.ReadInt32();
			}
			byte[][] entries = new byte[nentries][];
			for(int i = 0; i < nentries; i++)
			{
				lastEntry = i;
				entries[i] = reader.ReadBytes(entrylen[i]);
			}
			return new XLDInfo(nentries, entries);
		}
		
		public static bool ReadToXLD(Stream stream)
		{
			if(!stream.CanSeek)
				throw new ArgumentException("This stream cannot seek, operation is not possible.");
			BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
			while(stream.Position < stream.Length)
			{
				if(reader.ReadChar() == 'X' &&
				   reader.ReadChar() == 'L' &&
				   reader.ReadChar() == 'D' &&
				   reader.ReadChar() == '0' &&
				   reader.ReadChar() == 'I' &&
				   reader.ReadChar() == '\0')
				{
					stream.Seek(-6, SeekOrigin.Current);
					return true;
				}
			}
			return false;
		}
		
		public static List<XLDInfo> FindXLDs(Stream stream)
		{
			List<XLDInfo> entries = new List<XLD.XLDInfo>();
			while(ReadToXLD(stream))
			{
				entries.Add(XLD.Parse(stream));
			}
			return entries;
		}
		
		public static XLD.XLDInfo FindXLD(Stream stream, int order)
		{
			int offset;
			return FindXLD(stream, order, out offset);
		}
		
		public static XLDInfo FindXLD(Stream stream, int order, out int offset)
		{
			BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
			int pos = 0;
			while(ReadToXLD(stream))
			{
				if(pos++ == order)
				{
					offset = (int)stream.Position;
					return XLD.Parse(stream);
				}else{
					reader.ReadByte();
				}
			}
			offset = -1;
			return null;
		}
		
		public class XLDInfo : IEnumerable<byte[]>
		{
			public short Count{get;private set;}
			public IList<byte[]> Subfiles{get;private set;}
			
			public XLDInfo(short count, IList<byte[]> subfiles)
			{
				Count = count;
				Subfiles = subfiles;
			}
			
			public IEnumerator<byte[]> GetEnumerator()
			{
				return Subfiles.GetEnumerator();
			}
			IEnumerator IEnumerable.GetEnumerator()
			{
				return Subfiles.GetEnumerator();
			}
			
			public int Write(Stream output)
			{
				BinaryWriter writer = new BinaryWriter(output, Encoding.ASCII);
				writer.Write(new[]{'X','L','D','0','I','\0'});
				writer.Write(Count);
				int written = 8;
				foreach(byte[] subfile in Subfiles)
				{
					writer.Write(subfile.Length);
					written += 4 + subfile.Length;
				}
				foreach(byte[] subfile in Subfiles)
				{
					writer.Write(subfile);
				}
				return written;
			}
			
			public byte[] ToRawData()
			{
				MemoryStream stream = new MemoryStream();
				Write(stream);
				return stream.ToArray();
			}
		}
	}
	
	public static class SaveGame
	{
		public class SaveGameInfo
		{
			private short unknown1;
			public string Name{get; set;}
			private int unknown2;
			public Version Version{get; set;}
			private byte[] unknown3;//[7]
			public short Days{get; set;}
			public short Hours{get; set;}
			public short Minutes{get; set;}
			public short MapID{get; set;}
			public short PartyX{get; set;}
			public short PartyY{get; set;}
			
			public int Size{
				get{
					return Name.Length+28;
				}
			}
			
			public SaveGameInfo(Stream stream)
			{
				BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
				short length = reader.ReadInt16();
				unknown1 = reader.ReadInt16();
				Name = new string(reader.ReadChars(length));
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
			
			public byte[] ToRawData()
			{
				MemoryStream stream = new MemoryStream();
				Write(stream);
				return stream.ToArray();
			}
		}
		
		public class Character
		{
			private byte[] source;
			
			public Gender Gender{get; set;}
			public Race Race{get; set;}
			public Class Class{get; set;}
			public Magic Magic{get; set;}
			public byte Level{get; set;}
			public Language Language{get; set;}
			public byte Appearance{get; set;}
			public byte Face{get; set;}
			public byte InventoryPicture{get; set;}
			public CharacterPerson NamedAppearance{
				get{return (CharacterPerson)Appearance;}
				set{Appearance = (byte)value;}
			}
			public CharacterPerson NamedFace{
				get{return (CharacterPerson)Face;}
				set{Face = (byte)value;}
			}
			public CharacterPerson NamedInventoryPicture{
				get{return (CharacterPerson)InventoryPicture;}
				set{InventoryPicture = (byte)value;}
			}
			public short TrainingPoints{get; set;}
			public float Gold{get; set;}
			public short Rations{get; set;}
			public CharacterConditions Conditions{get; set;}
			
			public CharacterAttribute Strength{get; set;}
			public CharacterAttribute Intelligence{get; set;}
			public CharacterAttribute Dexterity{get; set;}
			public CharacterAttribute Speed{get; set;}
			public CharacterAttribute Stamina{get; set;}
			public CharacterAttribute Luck{get; set;}
			public CharacterAttribute MagicResistance{get; set;}
			public CharacterAttribute MagicTallent{get; set;}
			public CharacterAttribute CloseRangeCombat{get; set;}
			public CharacterAttribute LongRangeCombat{get; set;}
			public CharacterAttribute CriticalHit{get; set;}
			public CharacterAttribute Lockpicking{get; set;}
			public CharacterAttribute LifePoints{get; set;}
			public CharacterAttribute SpellPoints{get; set;}
			
			public short Age{get; set;}
			
			public int Experience{get; set;}
			
			public int[] ClassSpells{get; private set;}
			
			public string Name{get; set;}
			
			public Equipment Equipment{get; private set;}
			public Backpack Inventory{get; private set;}
			
			public Character()
			{
				source = new byte[940];
				ClassSpells = new int[7];
				Equipment = new Equipment();
			}
			
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
			
			public struct CharacterAttribute
			{
				public short Value{get;private set;}
				public short MaximumValue{get;private set;}
				public CharacterAttribute(short value, short maxvalue) : this()
				{
					Value = value;
					MaximumValue = maxvalue;
				}
				
				public CharacterAttribute(byte[] value, int startIndex) : this()
				{
					Value = BitConverter.ToInt16(value, startIndex);
					MaximumValue = BitConverter.ToInt16(value, startIndex+2);
				}
				
				public byte[] ToRawData()
				{
					byte[] data = new byte[4];
					BitConverter.GetBytes(Value).CopyTo(data, 0);
					BitConverter.GetBytes(MaximumValue).CopyTo(data, 2);
					return data;
				}
				
				public override string ToString()
				{
					return Value+"/"+MaximumValue;
				}

			}
		}
		
		public class Backpack : Inventory
		{
			private ItemStack[] inventory = new ItemStack[24];
			
			public Backpack(){}
			
			public Backpack(byte[] data, int offset)
			{
				for(int i = 0; i < 24; i++)
				{
					inventory[i] = ItemStack.FromRawData(data, offset+i*6);
				}
			}
			
			public override byte[] ToRawData()
			{
				byte[] data = new byte[144];
				for(int i = 0; i < 24; i++)
				{
					inventory[i].ToRawData().CopyTo(data, i*6);
				}
				return data;
			}
			
			public override int Count{
				get{return 24;}
			}
			
			public override SaveGame.ItemStack this[int index]
			{
				get{
					return inventory[24];
				}
				set {
					inventory[24] = value;
				}
			}
			
			public SaveGame.ItemStack this[int x, int y]
			{
				get{
					return inventory[y*4+x];
				}
				set {
					inventory[y*4+x] = value;
				}
			}
			
			public override void Clear()
			{
				for(int i = 0; i < 24; i++)
				{
					inventory[i] = default(ItemStack);
				}
			}
			
			public override IEnumerator<ItemStack> GetEnumerator()
			{
				return (IEnumerator<ItemStack>)inventory.GetEnumerator();
			}
		}
		
		public class Equipment : Inventory
		{
			public ItemStack Neck;
			public ItemStack Head;
			public ItemStack Tail;
			public ItemStack RightHand;
			public ItemStack Chest;
			public ItemStack LeftHand;
			public ItemStack RightFinger;
			public ItemStack Feet;
			public ItemStack LeftFinger;
			
			public Equipment(){}
			
			public Equipment(byte[] data, int offset)
			{
				Neck = ItemStack.FromRawData(data, offset);
				Head = ItemStack.FromRawData(data, offset+6);
				Tail = ItemStack.FromRawData(data, offset+12);
				RightHand = ItemStack.FromRawData(data, offset+18);
				Chest = ItemStack.FromRawData(data, offset+24);
				LeftHand = ItemStack.FromRawData(data, offset+30);
				RightFinger = ItemStack.FromRawData(data, offset+36);
				Feet = ItemStack.FromRawData(data, offset+42);
				LeftFinger = ItemStack.FromRawData(data, offset+48);
			}
			
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
			
			public override int Count{
				get{return 9;}
			}
			
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
		
		public abstract class Inventory : ICollection<ItemStack>, IList<ItemStack>
		{
			public abstract ItemStack this[int index]{
				get;set;
			}
			
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
			
			public abstract IEnumerator<ItemStack> GetEnumerator();
			
			bool ICollection<ItemStack>.IsReadOnly{
				get{return false;}
			}
			
			public abstract int Count{
				get;
			}
			
			bool ICollection<ItemStack>.Remove(ItemStack item)
			{
				throw new NotSupportedException();
			}
			
			public void CopyTo(ItemStack[] array, int arrayIndex)
			{
				for(int i = 0; i < this.Count; i++)
				{
					array[arrayIndex+i] = this[i];
				}
			}
			
			public bool Contains(ItemStack item)
			{
				foreach(ItemStack stack in this)
				{
					if(stack.Equals(item))return true;
				}
				return false;
			}
			
			void ICollection<ItemStack>.Add(ItemStack item)
			{
				throw new NotSupportedException();
			}
			
			void IList<ItemStack>.RemoveAt(int index)
			{
				throw new NotSupportedException();
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
			
			public abstract void Clear();
			
			public abstract byte[] ToRawData();
		}
		
		public struct ItemStack
		{
			public byte Count{get;set;}
			public byte Charges{get;set;}
			public byte NumRecharged{get;set;}
			public ItemFlags Flags{get;set;}
			public short Type{get;set;}
			
			public bool Existing{
				get{return Count != 0 && Type != 0;}
			}
			
			public string TypeName{
				get{return Texts.GetItemName(Type);}
			}
			
			public Items.ItemState State{
				get{return Items.GetItemState(Type);}
			}
			
			public ItemStack(byte[] data, int offset) : this()
			{
				Count = data[offset];
				Charges = data[offset+1];
				NumRecharged = data[offset+2];
				Flags = (ItemFlags)data[offset+3];
				Type = BitConverter.ToInt16(data, offset+4);
			}
			
			public ItemStack(byte count, short type) : this()
			{
				Count = count; Type = type;
			}
			
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
			
			public override string ToString()
			{
				if(Count == 0 || Type == 0)
				{
					return "[ItemStack None]";
				}else{
					return string.Format("[ItemStack Count={0}, Type={1}, Flags={2}]", Count, Texts.ItemNamesLoaded?(object)("'"+TypeName+"'"):Type, Flags);
				}
			}
			
			public override bool Equals(object obj)
			{
				if(obj is ItemStack)
				{
					return this.Equals((ItemStack)obj);
				}else{
					return false;
				}
			}
			
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			public bool Equals(ItemStack stack)
			{
				return this.Count == stack.Count &&
					   this.Charges == stack.Charges &&
					   this.NumRecharged == stack.NumRecharged &&
					   this.Flags == stack.Flags &&
					   this.Type == stack.Type;
			}
			
			public static bool operator ==(ItemStack a, ItemStack b)
			{
				return a.Equals(b);
			}
			
			public static bool operator !=(ItemStack a, ItemStack b)
			{
				return !a.Equals(b);
			}
			
			public static ItemStack FromRawData(byte[] data, int offset)
			{
				return new ItemStack(data, offset);
			}
		}
		
		public enum Gender : byte
		{
			Male = 0, Female = 1
		}
		public enum Race : byte
		{
			Terran = 0, Iskai = 1, Celt = 2, KengetKamulos = 3, DjiCantos = 4,
			Mahino = 5, Decadent = 6, Umajo = 7, Monster = 15
		}
		public enum Class : byte
		{
			Pilot = 0, Scientist = 1, Warrior = 2, DjiKasMage = 3, Druid = 4,
			EnlightenedOne = 5, Technician = 6, OquloWarrior = 7, Warrior2 = 8
		}
		[Flags]
		public enum Language : byte
		{
			None = 0, Terran = 1, Iskai = 2, Celtic = 4
		}
		[Flags]
		public enum Magic : byte
		{
			None = 0, DjiKas = 1, EnlightenedOne = 2, Druid = 4, OquloKamulos = 8
		}
		[Flags]
		public enum CharacterConditions : ushort
		{
			Unconscious = 0x0100, Poisoned = 0x0200, Ill = 0x0400, Exhausted = 0x0800,
			Paralyzed = 0x1000, Fleeing = 0x2000, Intoxicated = 0x4000, Blind = 0x8000,
			Panicking = 0x01, Asleep = 0x02, Insane = 0x04, Irritated = 0x08
		}
		public enum CharacterPerson : byte
		{
			None = 0, Tom = 1, Rainer = 2, Drirr = 3, Sira = 4, Mellthas = 5,
			Harriet = 6, Joe = 7, Khunag = 9, Siobhan = 10
		}
		[Flags]
		public enum ItemFlags : byte
		{
			None = 0, ShowMoreInfo = 1, Broken = 2, Cursed = 4
		}
	}
	
	public static class Items
	{
		private static ItemState[] ItemStates;
		
		public static bool ItemStatesLoaded{
			get{return ItemStates != null;}
		}
		
		public static void LoadItemStates()
		{
			using(FileStream stream = new FileStream(Paths.ItemList, FileMode.Open))
			{
				BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
				int count = (int)(stream.Length/40);
				ItemState[] itemStates = new ItemState[count];
				for(int i = 0; i < count; i++)
				{
					ItemState state = new ItemState();
					state.Type = (short)(i+1);
					reader.ReadByte();
					state.Class = (ItemClass)reader.ReadByte();
					state.Slot = (ItemSlot)reader.ReadByte();
					state.BreakRate = reader.ReadByte();
					state.Gender = (Gender)reader.ReadByte();
					state.FreeHands = reader.ReadByte();
					state.LifePointsBonus = reader.ReadByte();
					state.SpellPointsBonus = reader.ReadByte();
					state.AttributeType = (AttributeType)reader.ReadByte();
					state.AttributeBonus = reader.ReadByte();
					state.SkillTypeBonus = (SkillType)reader.ReadByte();
					state.SkillBonus = reader.ReadByte();
					state.PhysicalDamageProtection = reader.ReadByte();
					state.PhysicalDamageCaused = reader.ReadByte();
					state.AmmunitionType = reader.ReadByte();
					state.SkillType1Tax = (SkillType)reader.ReadByte();
					state.SkillType2Tax = (SkillType)reader.ReadByte();
					state.Skill1Tax = reader.ReadSByte();
					state.Skill2Tax = reader.ReadSByte();
					state.TorchIntensity = reader.ReadByte();
					state.AmmoAnimation = reader.ReadByte();
					state.Spell = (ItemSpellType)reader.ReadByte();
					state.SpellID = reader.ReadByte();
					state.Charges = reader.ReadByte();
					state.NumRecharged = reader.ReadByte();
					state.MaxNumRecharged = reader.ReadByte();
					state.MaxCharges = reader.ReadByte();
					state.Count1 = reader.ReadByte();
					state.Count2 = reader.ReadByte();
					state.IconAnim = reader.ReadByte();
					state.Weight = reader.ReadInt16();
					state.Value = reader.ReadInt16()/10f;
					state.Icon = reader.ReadInt16();
					state.UsingClass = reader.ReadInt16();
					state.UsingRace = reader.ReadInt16();
					itemStates[i] = state;
				}
				ItemStates = itemStates;
			}
		}
		
		public static ItemState GetItemState(short type)
		{
			if(type == 0)return default(ItemState);
			if(ItemStates == null)
			{
				LoadItemStates();
			}
			return ItemStates[type-1];
		}
		
		public struct ItemState
		{
			public short Type{get; set;}
			public ItemClass Class{get; set;}
			public ItemSlot Slot{get; set;}
			public byte BreakRate{get; set;}
			public Gender Gender{get; set;}
			public byte FreeHands{get; set;}
			public byte LifePointsBonus{get; set;}
			public byte SpellPointsBonus{get; set;}
			public AttributeType AttributeType{get; set;}
			public byte AttributeBonus{get; set;}
			public SkillType SkillTypeBonus{get; set;}
			public byte SkillBonus{get; set;}
			public byte PhysicalDamageProtection{get; set;}
			public byte PhysicalDamageCaused{get; set;}
			public byte AmmunitionType{get; set;}
			public SkillType SkillType1Tax{get; set;}
			public SkillType SkillType2Tax{get; set;}
			public sbyte Skill1Tax{get; set;}
			public sbyte Skill2Tax{get; set;}
			public byte TorchIntensity{get; set;}
			public ItemActivates Activates{
				get{return (ItemActivates)TorchIntensity;}
				set{TorchIntensity = (byte)value;}
			}
			public byte AmmoAnimation{get; set;}
			public ItemSpellType Spell{get; set;}
			public byte SpellID{get; set;}
			public byte Charges{get; set;}
			public byte NumRecharged{get; set;}
			public byte MaxNumRecharged{get; set;}
			public byte MaxCharges{get; set;}
			public byte Count1{get; set;}
			public byte Count2{get; set;}
			public byte IconAnim{get; set;}
			public short Weight{get; set;}
			public float Value{get; set;}
			public short Icon{get; set;}
			public short UsingClass{get; set;}
			public short UsingRace{get; set;}
			
			public string TypeName{
				get{return Texts.GetItemName(Type);}
			}
		}
		
		public enum ItemClass : byte
		{
			Unknown = 0, Armour = 1, Helmet = 2, Shoes = 3, Shield = 4, Melee = 5,
			Ranged = 6, Ammo = 7, Document = 8, Spell = 9, Drink = 10, Amulet = 11,
			Ring = 13, Jewel = 14, Tool = 15, Key = 16, Normal = 17, Magical = 18,
			Special = 19, Lockpick = 21, Staff = 22
		}
		
		public enum ItemSlot : byte
		{
			Inventory = 0, Neck = 1, Head = 2, Tail = 3, RightHand = 4, Chest = 5,
			LeftHand = 6, RightFinger = 7, Feet = 8, LeftFinger = 9, RightHandOrTail = 10
		}
		
		[Flags]
		public enum Gender : byte
		{
			None = 0, Male = 1, Female = 2, Any = 3
		}
		
		public enum AttributeType : byte
		{
			Strength = 0, Intelligence = 1, Dexterity = 2, Speed = 3,
			Stamina = 4, Luck = 5, MagicResistance = 6, MagicTalent = 7
		}
		
		public enum SkillType : byte
		{
			CloseRangeCombat = 0, LongRangeCombat = 1, CriticalHit = 2,
			Lockpicking = 3
		}
		
		public enum ItemActivates : byte
		{
			Compass = 0, MonsterEye = 1, Clock = 3
		}
		
		public enum ItemSpellType : byte
		{
			DjiKas = 0, EnlightenedOne = 1, Druid = 2, OquloKamulos = 3
		}
		
		[Flags]
		public enum Class : byte
		{
			
		}
	}
	
	public static class Texts
	{
		private static string[][] ItemNames;
		
		public static Language DefaultLanguage{
			get;set;
		}
		
		static Texts()
		{
			DefaultLanguage = Language.English;
		}
		
		public static bool ItemNamesLoaded{
			get{return ItemNames != null;}
		}
		
		public static void LoadItemNames()
		{
			using(FileStream stream = new FileStream(Paths.ItemName, FileMode.Open))
			{
				BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
				int count = (int)(stream.Length/60);
				string[][] itemNames = new string[count][];
				for(int i = 0; i < count; i++)
				{
					string[] names = new string[3];
					names[0] = new string(reader.ReadChars(20)).TrimEnd('\0');
					names[1] = new string(reader.ReadChars(20)).TrimEnd('\0');
					names[2] = new string(reader.ReadChars(20)).TrimEnd('\0');
					itemNames[i] = names;
				}
				ItemNames = itemNames;
			}
		}
		
		public static string GetItemName(short type)
		{
			return GetItemName(type, DefaultLanguage);
		}
		
		public static string GetItemName(short type, Language language)
		{
			if(ItemNames == null)
			{
				LoadItemNames();
			}
			return ItemNames[type-1][(int)language];
		}
		
		public enum Language
		{
			German = 0, English = 1, French = 2
		}
	}
	
	public static class Imaging
	{
		private static readonly Color[][] Palettes = new Color[short.MaxValue][];
		private static Color[] GlobalPalette = null;
		
		public static int TransparentIndex = -1;
		
		public static void LoadPalettes()
		{
			using(FileStream stream = new FileStream(Paths.Palette0, FileMode.Open))
			{
				XLD.ForEachSubfile(stream, OnPaletteEnumerate);
			}
			using(FileStream stream = new FileStream(Paths.GlobalPalette, FileMode.Open))
			{
				ReadGlobalPalette((int)stream.Length, new BinaryReader(stream, Encoding.ASCII));
			}
		}
		
		private static void LoadGlobalPalette()
		{
			using(FileStream stream = new FileStream(Paths.GlobalPalette, FileMode.Open))
			{
				ReadGlobalPalette((int)stream.Length, new BinaryReader(stream, Encoding.ASCII));
			}
		}
		
		private static void LoadPalette(short index)
		{
			using(FileStream stream = new FileStream(Paths.Palette0, FileMode.Open))
			{
				int length;
				BinaryReader reader = XLD.ReadToIndex(stream, index, out length);
				ReadPalette(length, reader);
			}
		}
		
		private static int OnPaletteEnumerate(short index, int length, BinaryReader reader)
		{
			Palettes[index] = ReadPalette(length, reader);
			return length;
		}
		
		public static Color[] ReadPalette(int length, BinaryReader reader)
		{
			if(length%3!=0)
			{
				throw new Exception("Palette has not appropriate length.");
			}
			Color[] cols = new Color[length/3];
			for(int i = 0; i < length/3; i++)
			{
				byte R = reader.ReadByte();
				byte G = reader.ReadByte();
				byte B = reader.ReadByte();
				cols[i] = Color.FromArgb(R, G, B);
			}
			return cols;
		}
		
		private static void ReadGlobalPalette(int length, BinaryReader reader)
		{
			if(length != 192)
			{
				throw new Exception("Global palette has not appropriate length.");
			}
			GlobalPalette = new Color[64];
			for(int i = 0; i < length/3; i++)
			{
				byte R = reader.ReadByte();
				byte G = reader.ReadByte();
				byte B = reader.ReadByte();
				GlobalPalette[i] = Color.FromArgb(R, G, B);
			}
		}
		
		public static Color[] GetGlobalPalette()
		{
			if(GlobalPalette == null)
			{
				LoadGlobalPalette();
			}
			return GlobalPalette;
		}
		
		public static Color[] GetPalette(short index)
		{
			if(Palettes[index] == null)
			{
				LoadPalette(index);
			}
			return Palettes[index];
		}
		
		public static Bitmap DrawBitmap(byte[] data, int width, int height, short palette)
		{
			return DrawBitmap(data, width, height, GetPalette(palette), GetGlobalPalette());
		}
		
		public static Bitmap DrawBitmap(byte[] data, int width, int height, Color[] palette1, Color[] palette2)
		{
			Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
			ColorPalette pal = bmp.Palette;
			palette1.CopyTo(pal.Entries, 0);
			palette2.CopyTo(pal.Entries, 192);
			if(TransparentIndex >= 0)pal.Entries[TransparentIndex] = Color.Transparent;
			bmp.Palette = pal;
			BitmapData bmpdata = bmp.LockBits(new Rectangle(0,0,width,height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
			if(width%4 == 0)
			{
				Marshal.Copy(data, 0, bmpdata.Scan0, data.Length);
			}else{
				for(int y = 0; y < height; y++)
				{
					Marshal.Copy(data, width*y, bmpdata.Scan0+bmpdata.Stride*y, width);
				}
			}
			bmp.UnlockBits(bmpdata);
			return bmp;
		}
		
		public static Bitmap DrawBitmap(byte[] data, int width, int height, Color[] palette)
		{
			Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
			ColorPalette pal = bmp.Palette;
			palette.CopyTo(pal.Entries, 0);
			if(TransparentIndex >= 0)pal.Entries[TransparentIndex] = Color.Transparent;
			bmp.Palette = pal;
			BitmapData bmpdata = bmp.LockBits(new Rectangle(0,0,width,height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
			if(width%4 == 0)
			{
				Marshal.Copy(data, 0, bmpdata.Scan0, data.Length);
			}else{
				for(int y = 0; y < height; y++)
				{
					Marshal.Copy(data, width*y, bmpdata.Scan0+bmpdata.Stride*y, width);
				}
			}
			bmp.UnlockBits(bmpdata);
			return bmp;
		}
		
		public static Bitmap DrawBitmap(byte[] data, int width, short palette)
		{
			int height = (data.Length+width-1)/width;
			return DrawBitmap(data, width, height, palette);
		}
		
		public abstract class ImageBase
		{
			public byte[] ImageData{get;protected set;}
			public abstract byte[] ToRawData();
			public abstract Bitmap DrawToBitmap(short palette);
		}
		
		public sealed class RawImage : ImageBase
		{
			public int Width{get; set;}
			public int Height{get; set;}
			
			public override byte[] ToRawData()
			{
				return ImageData;
			}
			
			public override Bitmap DrawToBitmap(short palette)
			{
				return DrawBitmap(ImageData, Width, Height, palette);
			}
			
			public RawImage(byte[] rawdata)
			{
				if(rawdata.Length==0)return;
				ImageData = rawdata;
			}
			
			public RawImage(Stream stream, int length)
			{
				ImageData = new BinaryReader(stream, Encoding.ASCII).ReadBytes(length);
			}
			
			public RawImage(short width, byte[] data)
			{
				Width = width;
				Height = (data.Length+width-1)/width;
				ImageData = data;
			}
			
			public RawImage(short width, short height, byte[] data)
			{
				Width = width;
				Height = height;
				ImageData = data;
			}
			
			public static RawImage FromRawData(byte[] data)
			{
				if(data.Length==0)return null;
				return new RawImage(data);
			}
			
			public static RawImage FromStream(Stream stream, int length)
			{
				return new RawImage(stream, length);
			}
		}
		
		public sealed class ILBMImage : ImageBase
		{
			public short Width{get;private set;}
			public short Height{get;private set;}
			public short PosX{get;private set;}
			public short PosY{get;private set;}
			public byte NumPlanes{get;private set;}
			public byte Mask{get;private set;}
			public byte Compression{get;private set;}
			public byte Padding{get;private set;}
			public short Transparent{get;private set;}
			public short AspectRatio{get;private set;}
			public short PageWidth{get;private set;}
			public short PageHeight{get;private set;}
			public Color[] Palette{get;private set;}
			public short HotspotX{get;private set;}
			public short HotspotY{get;private set;}
			
			public TinyImage Tiny{get;private set;}
			
			public ILBMImage(byte[] rawdata)
			{
				if(rawdata.Length==0)return;
				using(MemoryStream stream = new MemoryStream(rawdata))
				{
					BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
					ReadNext(reader);
				}
			}
			
			public ILBMImage(Stream stream)
			{
				BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
				ReadNext(reader);
			}
			
			private void ReadNext(BinaryReader reader)
			{
				while(Char.IsControl((char)reader.PeekChar()))
				{
					reader.ReadByte();
				}
				string sig = new string(reader.ReadChars(4));
				int size = (int)IFF.Switch(reader.ReadUInt32());
				switch(sig)
				{
					case "FORM":
						reader.ReadBytes(4);
					break;
					case "BMHD":
						Width = (short)IFF.Switch(reader.ReadUInt16());
						Height = (short)IFF.Switch(reader.ReadUInt16());
						PosX = (short)IFF.Switch(reader.ReadUInt16());
						PosY = (short)IFF.Switch(reader.ReadUInt16());
						NumPlanes = reader.ReadByte();
						Mask = reader.ReadByte();
						Compression = reader.ReadByte();
						Padding = reader.ReadByte();
						Transparent = (short)IFF.Switch(reader.ReadUInt16());
						AspectRatio = (short)IFF.Switch(reader.ReadUInt16());
						PageWidth = (short)IFF.Switch(reader.ReadUInt16());
						PageHeight = (short)IFF.Switch(reader.ReadUInt16());
						reader.ReadBytes(size-20);
					break;
					case "CMAP":
						Palette = new Color[size/3];
						for(int i = 0; i < size/3; i++)
						{
							byte R = reader.ReadByte();
							byte G = reader.ReadByte();
							byte B = reader.ReadByte();
							Palette[i] = Color.FromArgb(R, G, B);
						}
					break;
					case "GRAB":
						HotspotX = (short)IFF.Switch(reader.ReadUInt16());
						HotspotY = (short)IFF.Switch(reader.ReadUInt16());
					break;
					case "TINY":
						short width = (short)IFF.Switch(reader.ReadUInt16());
						short height = (short)IFF.Switch(reader.ReadUInt16());
						byte[] tiny = reader.ReadBytes(size-4);
						if(Compression == 1)
						{
							tiny = IFF.Decompress(tiny, width*height);
						}
						Tiny = new TinyImage(width, height, tiny);
					break;
					case "BODY":
						byte[] data = reader.ReadBytes(size);
						if(Compression == 1)
						{
							data = IFF.Decompress(data, Width*Height);
						}
						ImageData = data;
					return;
					default:
						reader.ReadBytes(size);
					break;
				}
				ReadNext(reader);
			}
			
			public override byte[] ToRawData()
			{
				throw new NotImplementedException();
			}
			
			public override Bitmap DrawToBitmap(short palette)
			{
				if(palette >= 0)
					return DrawBitmap(ImageData, Width, Height, palette);
				else return DrawToBitmap();
			}
			
			public Bitmap DrawToBitmap()
			{
				return DrawBitmap(ImageData, Width, Height, Palette);
			}
			
			public Bitmap DrawTiny(short palette)
			{
				return Tiny.DrawToBitmap(palette);
			}
			
			public Bitmap DrawTiny()
			{
				return DrawBitmap(Tiny.ImageData, Tiny.Width, Tiny.Height, Palette);
			}
			
			public static ILBMImage FromRawData(byte[] data)
			{
				if(data.Length==0)return null;
				return new ILBMImage(data);
			}
			
			public static ILBMImage FromStream(Stream stream)
			{
				return new ILBMImage(stream);
			}
		}
		
		public sealed class TinyImage : ImageBase
		{
			public short Width{get;private set;}
			public short Height{get;private set;}
			
			public override Bitmap DrawToBitmap(short palette)
			{
				return DrawBitmap(ImageData, Width, Height, palette);
			}
			
			public override byte[] ToRawData()
			{
				byte[] data = new byte[ImageData.Length+4];
				BitConverter.GetBytes(Width).CopyTo(data, 0);
				BitConverter.GetBytes(Height).CopyTo(data, 2);
				ImageData.CopyTo(data, 4);
				return data;
			}
			
			public TinyImage(byte[] rawdata)
			{
				if(rawdata.Length==0)return;
				Width = BitConverter.ToInt16(rawdata, 0);
				Height = BitConverter.ToInt16(rawdata, 2);
				ImageData = new byte[rawdata.Length-4];
				Array.Copy(rawdata, 4, ImageData, 0, ImageData.Length);
			}
			
			public TinyImage(Stream stream)
			{
				BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
				Width = reader.ReadInt16();
				Height = reader.ReadInt16();
				ImageData = reader.ReadBytes(Width*Height);
			}
			
			public TinyImage(short width, short height, byte[] data)
			{
				Width = width;
				Height = height;
				ImageData = data;
			}
			
			public static TinyImage FromRawData(byte[] data)
			{
				if(data.Length==0)return null;
				return new TinyImage(data);
			}
			
			public static TinyImage FromStream(Stream stream)
			{
				return new TinyImage(stream);
			}
		}
		
		public sealed class HeaderedImage : ImageBase
		{
			public short Width{get;private set;}
			public short Height{get;private set;}
			public byte FramesCount{get;private set;}
			
			public override Bitmap DrawToBitmap(short palette)
			{
				return DrawBitmap(ImageData, Width, Height, palette);
			}
			
			public override byte[] ToRawData()
			{
				byte[] data = new byte[ImageData.Length+6];
				BitConverter.GetBytes(Width).CopyTo(data, 0);
				BitConverter.GetBytes(Height).CopyTo(data, 2);
				data[5] = FramesCount;
				ImageData.CopyTo(data, 6);
				return data;
			}
			
			public HeaderedImage(byte[] rawdata)
			{
				if(rawdata.Length==0)return;
				Width = BitConverter.ToInt16(rawdata, 0);
				Height = BitConverter.ToInt16(rawdata, 2);
				FramesCount = rawdata[5];
				ImageData = new byte[rawdata.Length-6];
				Array.Copy(rawdata, 6, ImageData, 0, ImageData.Length);
			}
			
			public HeaderedImage(Stream stream)
			{
				BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
				Width = reader.ReadInt16();
				Height = reader.ReadInt16();
				reader.ReadByte();
				FramesCount = reader.ReadByte();
				ImageData = reader.ReadBytes(Width*Height);
			}
			
			public HeaderedImage(short width, short height, byte[] data)
			{
				Width = width;
				Height = height;
				FramesCount = 1;
				ImageData = data;
			}
			
			public static HeaderedImage FromRawData(byte[] data)
			{
				if(data.Length==0)return null;
				return new HeaderedImage(data);
			}
			
			public static HeaderedImage FromStream(Stream stream)
			{
				return new HeaderedImage(stream);
			}
		}
		
		public sealed class AnimatedHeaderedImage : ImageBase
		{
			public byte FramesCount{get;private set;}
			public HeaderedImage[] Frames{get;private set;}
			
			public Bitmap DrawToBitmap(byte index, short palette)
			{
				return DrawBitmap(Frames[index].ImageData, Frames[index].Width, Frames[index].Height, palette);
			}
			
			public override Bitmap DrawToBitmap(short palette)
			{
				throw new Exception("You must specify animation index.");
			}
			
			public override byte[] ToRawData()
			{
				throw new NotImplementedException();
			}
			
			public AnimatedHeaderedImage(byte[] rawdata)
			{
				if(rawdata.Length==0)return;
				short width = BitConverter.ToInt16(rawdata, 0);
				short height = BitConverter.ToInt16(rawdata, 2);
				FramesCount = rawdata[5];
				Frames = new HeaderedImage[FramesCount];
				byte[] data = new byte[width*height];
				Array.Copy(rawdata, 6, data, 0, width*height);
				Frames[0] = new HeaderedImage(width, height, data);
				int nextindex = data.Length+6;
				for(int i = 1; i < FramesCount; i++)
				{
					width = BitConverter.ToInt16(rawdata, nextindex);
					height = BitConverter.ToInt16(rawdata, nextindex+2);
					data = new byte[width*height];
					Array.Copy(rawdata, nextindex+6, data, 0, width*height);
					Frames[i] = new HeaderedImage(width, height, data);
					nextindex += data.Length+6;
				}
			}
			
			public AnimatedHeaderedImage(Stream stream)
			{
				BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
				short width = reader.ReadInt16();
				short height = reader.ReadInt16();
				reader.ReadByte();
				FramesCount = reader.ReadByte();
				Frames = new HeaderedImage[FramesCount];
				Frames[0] = new HeaderedImage(width, height, reader.ReadBytes(width*height));
				for(int i = 1; i < FramesCount; i++)
				{
					width = reader.ReadInt16();
					height = reader.ReadInt16();
					reader.ReadByte();
					FramesCount = reader.ReadByte();
					Frames[i] = new HeaderedImage(width, height, reader.ReadBytes(width*height));
				}
			}
			
			public static AnimatedHeaderedImage FromRawData(byte[] data)
			{
				if(data.Length==0)return null;
				return new AnimatedHeaderedImage(data);
			}
			
			public static AnimatedHeaderedImage FromStream(Stream stream)
			{
				return new AnimatedHeaderedImage(stream);
			}
		}
	}
	
	public static class Scripts
	{
		public static string GetScript(int index)
		{
			int subindex = index%100;
			int fileindex = index/100;
			using(FileStream stream = new FileStream(String.Format(Paths.ScriptsN, fileindex), FileMode.Open))
			{
				int length;
				BinaryReader reader = XLD.ReadToIndex(stream, subindex, out length);
				return new string(reader.ReadChars(length));
			}
		}
		
		public static int RunScript(string script, IScriptExecutor executor)
		{
			return executor.Execute(script);
		}
		
		public static int RunScript(int index, IScriptExecutor executor)
		{
			return executor.Execute(GetScript(index));
		}
		
		public static int RunScript(string script, ExecuteHandler handler)
		{
			return handler(script);
		}
		
		public static int RunScript(int index, ExecuteHandler handler)
		{
			return handler(GetScript(index));
		}
		
		public class DebugExecutor : ScriptExecutionMachine
		{
			public override void OnComment(string comment)
			{
				Console.WriteLine("//"+comment);
			}
			
			public override void OnFunction(string function, int[] args)
			{
				Console.WriteLine("{0}({1})", function, string.Join(", ", args));
			}
			
			public override int Execute(string script)
			{
				ScriptExecutionException exception;
				if(Execute(script, out exception) == 0)
				{
					throw exception;
				}else{
					return 1;
				}
			}
		}
		
		public abstract class ScriptExecutionMachine : IScriptExecutor
		{
			public static readonly DebugExecutor DebugExecutor = new DebugExecutor();
			
			public virtual void OnComment(string comment){}
			public abstract void OnFunction(string function, int[] args);
			
			public int Execute(string script, out ScriptExecutionException exception)
			{
				string line = null;
				int i = -1;
				try{
					string[] lines = script.Split('\n');
					for(i = 0; i < lines.Length; i++)
					{
						line = lines[i].Trim();
						if(line.Length == 0)
						{
							continue;
						}else if(line[0] == ';')
						{
							OnComment(line.Substring(1));
						}else{
							string[] parts = line.Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries);
							int[] args = new int[parts.Length-1];
							for(int j = 1; j < parts.Length; j++)
							{
								if(!int.TryParse(parts[j], out args[j-1]))
								{
									throw new ScriptExecutionException(i+1, line, string.Format("Cannot parse argument \"{0}\".", parts[j]), null);
								}
							}
							OnFunction(parts[0], args);
						}
					}
					exception = null;
					return 1;
				}catch(ScriptExecutionException e)
				{
					exception = e;
					return 0;
				}catch(Exception e)
				{
					exception = new ScriptExecutionException(i+1, line, e.Message, e);
					return 0;
				}
			}
			
			public virtual int Execute(string script)
			{
				ScriptExecutionException exception;
				return Execute(script, out exception);
			}
			
			public class ScriptExecutionException : Exception
			{
				public int LinePosition{get;private set;}
				public string CurrentLine{get;private set;}
				
				public ScriptExecutionException(int line, string errorline, string message, Exception innerException) : base(message, innerException)
				{
					LinePosition = line;
					CurrentLine = errorline;
				}
			}
		}
		
		public delegate int ExecuteHandler(string script);
		
		public interface IScriptExecutor
		{
			int Execute(string script);
		}
	}
	
	public static class Sound
	{
		public class XMidiMusic
		{
			public short NumTracks{get;private set;}
			
			public XMidiMusic(byte[] xmidi)
			{
				using(MemoryStream stream = new MemoryStream(xmidi))
				{
					ReadNext(new BinaryReader(stream, Encoding.ASCII));
				}
			}
			
			public XMidiMusic(Stream stream)
			{
				ReadNext(new BinaryReader(stream, Encoding.ASCII));
			}
			
			private void ReadNext(BinaryReader reader)
			{
				while(Char.IsControl((char)reader.PeekChar()))
				{
					reader.ReadByte();
				}
				string sig = new string(reader.ReadChars(4));
				int size = (int)IFF.Switch(reader.ReadUInt32());
				switch(sig)
				{
					case "INFO":
						NumTracks = (short)IFF.Switch(reader.ReadUInt16());
					break;
					case "CAT ":
						reader.ReadBytes(4);
					break;
					case "FORM":
						//string sig2 = new string(reader.ReadChars(4));
						reader.ReadBytes(4);
					break;
					case "EVNT":
						//File.WriteAllBytes("test.dat", /*ILBM.Decompress(*/reader.ReadBytes(size)/*)*/);
					return;
					default:
						reader.ReadBytes(size);
					break;
				}
				ReadNext(reader);
			}
		}
		
		public abstract class SoundBase
		{
			protected static readonly byte[] WaveHeader = {0x52, 0x49, 0x46, 0x46, 0x92, 0x3B, 0x00, 0x00, 0x57, 0x41, 0x56, 0x45, 0x66, 0x6D, 0x74, 0x20, 0x10, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x11, 0x2B, 0x00, 0x00, 0x88, 0x58, 0x01, 0x00, 0x08, 0x00, 0x08, 0x00, 0x64, 0x61, 0x74, 0x61, 0x6E, 0x3B, 0x00, 0x00};
		}
		
		public class RawPCMSound : SoundBase
		{
			public int Frequency{get; set;}
			public byte[] SoundData{get;private set;}
			
			private RawPCMSound()
			{
				Frequency = 11025;
			}
			
			public RawPCMSound(byte[] data) : this()
			{
				SoundData = data;
			}
			
			public RawPCMSound(Stream stream, int length) : this()
			{
				BinaryReader reader = new BinaryReader(stream);
				SoundData = reader.ReadBytes(length);
			}
			
			public byte[] ToWAVE()
			{
				byte[] wave = new byte[44+SoundData.Length];
				int chunkSize = SoundData.Length+36;
				int subchunkSize = SoundData.Length;
				WaveHeader.CopyTo(wave, 0);
				BitConverter.GetBytes(chunkSize).CopyTo(wave, 4);
				BitConverter.GetBytes(Frequency).CopyTo(wave, 24);
				BitConverter.GetBytes(subchunkSize).CopyTo(wave, 40);
				SoundData.CopyTo(wave, 44);
				return wave;
			}
			
			public MemoryStream ToWAVEStream()
			{
				MemoryStream stream = new MemoryStream(ToWAVE());
				return stream;
			}
		}
		
		public class HeaderedPCMSound : SoundBase
		{
			
		}
	}
	
	internal static class IFF
	{
		public static byte[] Decompress(byte[] src, int size)
		{
			byte[] decompressed = new byte[size];
			int pos = 0;
			for(int i = 0; i < src.Length; i++)
			{
				int n = src[i];
				if (n >= 128)
					n -= 256;
				
				if(n < 0)
				{
					if(n == -128)continue;
					n = -n + 1;
					for(int b = 0; b < n; b++)
					{
						decompressed[pos++] = src[i+1];
					}
					i+=1;
				}else{
					for(int b = 0; b < n+1; b++)
					{
						if(i+b+1 >= src.Length)break;
						decompressed[pos++] = src[i+b+1];
					}
					i += n+1;
				}
			}
			return decompressed;
		}
		public static byte[] Decompress(byte[] src)
		{
			List<byte> decompressed = new List<byte>();
			for(int i = 0; i < src.Length; i++)
			{
				int n = src[i];
				if (n >= 128)
					n -= 256;
				
				if(n < 0)
				{
					if(n == -128)continue;
					n = -n + 1;
					for(int b = 0; b < n; b++)
					{
						decompressed.Add(src[i+1]);
					}
					i+=1;
				}else{
					for(int b = 0; b < n+1; b++)
					{
						if(i+b+1 >= src.Length)break;
						decompressed.Add(src[i+b+1]);
					}
					i += n+1;
				}
			}
			return decompressed.ToArray();
		}
			
		public static UInt16 Switch(UInt16 value)
		{
			if(BitConverter.IsLittleEndian)
			return (UInt16)unchecked(((value&0xFF)<<8)|((value&0xFF00)>>8));
			return value;
		}
			
		public static Int16 Switch(Int16 value)
		{
			if(BitConverter.IsLittleEndian)
			return (Int16)unchecked(((value&0xFF)<<8)|((value&0xFF00)>>8));
			return value;
		}
		
		public static UInt32 Switch(UInt32 value)
		{
			if(BitConverter.IsLittleEndian)
			return (UInt32)unchecked(((value&0xFF)<<24)|((value&0xFF00)<<8)|((value&0xFF0000)>>8)|((value&0xFF000000)>>24));
			return value;
		}
		
		public static Int32 Switch(Int32 value)
		{
			return (Int32)Switch((UInt32)value);
		}
	}
}