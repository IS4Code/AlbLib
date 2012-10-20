using System;
using System.Collections.Generic;

namespace AlbLib.SaveGame
{
	/// <summary>
	/// Character equipment items.
	/// </summary>
	[Serializable]
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
					default: throw new ArgumentOutOfRangeException("index");
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
					default: throw new ArgumentOutOfRangeException("index");
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
}