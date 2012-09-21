using System.Collections.Generic;
namespace AlbLib.SaveGame
{
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
}