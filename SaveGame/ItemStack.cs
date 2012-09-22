using System;
using AlbLib.Items;
using AlbLib.Texts;

namespace AlbLib.SaveGame
{
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
		
		/// <summary>
		/// Returns localized item name.
		/// </summary>
		public string TypeName{
			get{return TextCore.GetItemName(Type);}
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
		
		/// <summary>
		/// Converts item stack to string.
		/// </summary>
		public override string ToString()
		{
			if(Count == 0 || Type == 0)
			{
				return "[ItemStack None]";
			}else{
				return string.Format("[ItemStack Count={0}, Type={1}, Flags={2}]", Count, TypeName+" ("+Type+")", Flags);
			}
		}
		
		/// <summary>
		/// Compares two item stacks.
		/// </summary>
		public override bool Equals(object obj)
		{
			if(obj is ItemStack)
			{
				return this.Equals((ItemStack)obj);
			}else{
				return false;
			}
		}
		
		/// <summary>
		/// Compares two item stacks.
		/// </summary>
		public bool Equals(ItemStack stack)
		{
			return this.Count == stack.Count &&
				   this.Charges == stack.Charges &&
				   this.NumRecharged == stack.NumRecharged &&
				   this.Flags == stack.Flags &&
				   this.Type == stack.Type;
		}
		
		/// <summary>
		/// Compares two item stacks.
		/// </summary>
		public static bool operator ==(ItemStack a, ItemStack b)
		{
			return a.Equals(b);
		}
		
		/// <summary>
		/// Compares two item stacks.
		/// </summary>
		public static bool operator !=(ItemStack a, ItemStack b)
		{
			return !a.Equals(b);
		}
		
		/// <summary>
		/// Computes unique hash code for this item stack.
		/// </summary>
		/// <returns>
		/// A hash code for this item stack.
		/// </returns>
		public override int GetHashCode()
		{
			return (Count|(Charges<<8)|(NumRecharged<<16)|((byte)Flags<<32))^Type<<16;
		}
	}
}