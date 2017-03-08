using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace AlbLib.SaveGame
{
	/// <summary>
	/// Inventory holding items.
	/// </summary>
	[Serializable]
	public class Inventory : IList<ItemStack>
	{
		private readonly ItemStack[] items;
		
		/// <summary>
		/// Accesses item using item <paramref name="index"/>.
		/// </summary>
		/// <param name="index">
		/// Item index from 0 to <see cref="Capacity"/>-1.
		/// </param>
		public ItemStack this[int index]{
			get{
				return items[index];
			}
			set{
				items[index] = value;
			}
		}
		
		public Inventory(int size)
		{
			items = new ItemStack[size];
		}
		
		public Inventory(IList<ItemStack> items)
		{
			this.items = new ItemStack[items.Count];
			items.CopyTo(this.items, 0);
		}
		
		public Inventory(byte[] data, int offset, int size) : this(size)
		{
			for(int i = 0; i < size; i++)
			{
				this[i] = new ItemStack(data, offset+i*6);
			}
		}
		
		public Inventory(Stream stream, int size) : this(new BinaryReader(stream), size)
		{}
		
		public Inventory(BinaryReader reader, int size) : this(size)
		{
			for(int i = 0; i < size; i++)
			{
				this[i] = new ItemStack(reader);
			}
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return items.GetEnumerator();
		}
		
		/// <summary>
		/// Enumerates through all items in inventory.
		/// </summary>
		/// <returns>
		/// Item enumerator.
		/// </returns>
		public virtual IEnumerator<ItemStack> GetEnumerator()
		{
			foreach(ItemStack item in items)
			{
				yield return item;
			}
		}
		
		bool ICollection<ItemStack>.IsReadOnly{
			get{return false;}
		}
		
		/// <summary>
		/// Count of item slots.
		/// </summary>
		public int Capacity{
			get{
				return items.Length;
			}
		}
		
		/// <summary>
		/// Counts all used item slots.
		/// </summary>
		int ICollection<ItemStack>.Count{
			get{
				return items.Length;
			}
		}
		
		public virtual int Count()
		{
			int count = 0;
			foreach(ItemStack item in this)
			{
				count += 1;
			}
			return count;
		}
		
		/// <summary>
		/// Checks if inventory is full.
		/// </summary>
		public bool IsFull{
			get{
				return Count() == Capacity;
			}
		}
		
		/// <summary>
		/// Checks if inventory is empty.
		/// </summary>
		public bool IsEmpty{
			get{
				return Count() == 0;
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
			items.CopyTo(array, arrayIndex);
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
			for(int i = 0; i < items.Length; i++)
			{
				if(this[i].IsEmpty)
				{
					this[i] = item;
					return;
				}
			}
			throw new Exception("Can't add item to stack.");
		}
		
		/// <summary>
		/// Removes item stack at specified <paramref name="index"/>.
		/// </summary>
		/// <param name="index">
		/// Index of item slot.
		/// </param>
		public virtual void RemoveAt(int index)
		{
			this[index] = default(ItemStack);
		}
		
		/// <summary>
		/// Decreases item count of stack at specified <paramref name="index"/>.
		/// </summary>
		/// <param name="index">
		/// Index of item slot.
		/// </param>
		public virtual void RemoveOne(int index)
		{
			ItemStack item = this[index];
			if(item.Count > 1)
			{
				item.Count -= 1;
				this[index] = item;
			}
			else this[index] = default(ItemStack);
		}
		
		void IList<ItemStack>.Insert(int index, ItemStack item)
		{
			throw new NotSupportedException();
		}
		
		int IList<ItemStack>.IndexOf(ItemStack item)
		{
			for(int i = 0; i < items.Length; i++)
			{
				if(this[i] == item)return i;
			}
			return -1;
		}
		
		/// <summary>
		/// Removes all items from inventory.
		/// </summary>
		public void Clear()
		{
			for(int i = 0; i < items.Length; i++)
			{
				items[i] = default(ItemStack);
			}
		}
		
		/// <summary>
		/// Converts items to byte array.
		/// </summary>
		/// <returns>
		/// Array containg items.
		/// </returns>
		public byte[] ToRawData()
		{
			byte[] buffer = new byte[items.Length*6];
			for(int i = 0; i < items.Length; i++)
			{
				items[i].ToRawData().CopyTo(buffer, i*6);
			}
			return buffer;
		}
	}
}