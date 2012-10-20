using System;
using System.Collections.Generic;
using System.Collections;

namespace AlbLib.SaveGame
{
	/// <summary>
	/// Inventory holding items.
	/// </summary>
	[Serializable]
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
}