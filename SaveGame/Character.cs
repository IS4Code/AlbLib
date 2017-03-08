using System;
using System.IO;
using System.Text;
using AlbLib.Texts;

namespace AlbLib.SaveGame
{
	/// <summary>
	/// Stores information about game character.
	/// </summary>
	[Serializable]
	public class Character : NPC
	{
		/// <summary>
		/// Character equipment items.
		/// </summary>
		public Equipment Equipment{get; private set;}
		
		/// <summary>
		/// Character backpack items.
		/// </summary>
		public Inventory Backpack{get; private set;}
		
		/// <summary>
		/// Creates new character.
		/// </summary>
		public Character() : base()
		{
			Equipment = new Equipment();
			Backpack = new Inventory(24);
		}
		
		/// <summary>
		/// Loads character.
		/// </summary>
		/// <param name="data">
		/// Byte array containing character data.
		/// </param>
		public Character(byte[] data) : base(data)
		{
			Equipment = new Equipment(data, 742);
			Backpack = new Inventory(data, 796, 24);
		}
		
		public Character(Stream stream) : this(new BinaryReader(stream, TextCore.DefaultEncoding))
		{}
		
		public Character(BinaryReader reader) : base(reader)
		{
			Equipment = new Equipment(reader);
			Backpack = new Inventory(reader, 24);
		}
		
		/// <summary>
		/// Saves character to byte array.
		/// </summary>
		/// <returns>
		/// Saved character.
		/// </returns>
		/*public byte[] ToRawData()
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
			
			byte[] name = TextCore.DefaultEncoding.GetBytes(Name.ToCharArray());
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
		}*/
	}
}