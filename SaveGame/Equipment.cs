using System;
using System.Collections.Generic;
using System.IO;

namespace AlbLib.SaveGame
{
	/// <summary>
	/// Character equipment items.
	/// </summary>
	[Serializable]
	public class Equipment : Inventory
	{
		/// <summary>
		/// Creates empty inventory.
		/// </summary>
		public Equipment() : base(9)
		{}
		
		/// <summary>
		/// Loads inventory from byte array.
		/// </summary>
		/// <param name="data">
		/// Byte array containing inventory items.
		/// </param>
		/// <param name="offset">
		/// Position of inventory.
		/// </param>
		public Equipment(byte[] data, int offset) : base(data, offset, 9)
		{}
		
		public Equipment(Stream stream) : this(new BinaryReader(stream))
		{}
		
		public Equipment(BinaryReader reader) : base(reader, 9)
		{}
		
		/// <summary>
		/// Item on neck.
		/// </summary>
		public ItemStack Neck{
			get{
				return this[0];
			}
			set{
				this[0] = value;
			}
		}
		
		/// <summary>
		/// Item on head.
		/// </summary>
		public ItemStack Head{
			get{
				return this[1];
			}
			set{
				this[1] = value;
			}
		}
		
		/// <summary>
		/// Item in tail.
		/// </summary>
		public ItemStack Tail{
			get{
				return this[2];
			}
			set{
				this[2] = value;
			}
		}
		
		/// <summary>
		/// Item in right hand.
		/// </summary>
		public ItemStack RightHand{
			get{
				return this[3];
			}
			set{
				this[3] = value;
			}
		}
		
		/// <summary>
		/// Item on chest.
		/// </summary>
		public ItemStack Chest{
			get{
				return this[4];
			}
			set{
				this[4] = value;
			}
		}
		
		/// <summary>
		/// Item in left hand.
		/// </summary>
		public ItemStack LeftHand{
			get{
				return this[5];
			}
			set{
				this[5] = value;
			}
		}
		
		/// <summary>
		/// Item on right finger.
		/// </summary>
		public ItemStack RightFinger{
			get{
				return this[6];
			}
			set{
				this[6] = value;
			}
		}
		
		/// <summary>
		/// Item on feet.
		/// </summary>
		public ItemStack Feet{
			get{
				return this[7];
			}
			set{
				this[7] = value;
			}
		}
		
		/// <summary>
		/// Item on left finger.
		/// </summary>
		public ItemStack LeftFinger{
			get{
				return this[8];
			}
			set{
				this[8] = value;
			}
		}
	}
}