using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;

namespace OrchidMod.Alchemist.Bag
{
	public partial class PotionBag : ModItem
	{
		// Constants 

		private const string INVISIBLE_LINE = "                          "; // Invisible character: Alt + 255
		private const int SLOTS_X = 8;
		private const int SLOTS_Y = 1;
		private const int SLOTS_XY = SLOTS_X * SLOTS_Y;

		// Properties

		public IEnumerable<Item> GetPotions { get => inventory.Where(i => !i.type.Equals(ItemID.None)); }
		public bool InShop { get; set; }
		public bool IsActive { get; set; }

		// Fields

		private Item dye;
		private Item[] inventory;

		// Constructors

		public PotionBag()
		{
			IsActive = true;

			dye = new Item();
			dye.TurnToAir();

			inventory = new Item[SLOTS_XY];

			for (int i = 0; i < SLOTS_XY; i++)
			{
				var newItem = new Item();
				newItem.TurnToAir();
				inventory[i] = newItem;
			}
		}

		// Overrides

		public override ModItem Clone(Item item)
		{
			var newBag = (PotionBag)base.Clone(item);
			newBag.IsActive = IsActive;

			// Dye
			{
				newBag.dye = new Item();
				newBag.dye = dye?.Clone();

				if (newBag.dye is null)
				{
					var newItem = new Item();
					newItem.TurnToAir();
					newBag.dye = newItem;
				}
			}

			// Inventory
			{
				newBag.inventory = new Item[SLOTS_XY];

				for (int i = 0; i < SLOTS_XY; i++)
				{
					if (inventory[i].type > ItemID.None)
					{
						newBag.inventory[i] = inventory[i]?.Clone();
					}
					else
					{
						var newItem = new Item();
						newItem.TurnToAir();
						newBag.inventory[i] = newItem;
					}
				}
			}

			return newBag;
		}

		public override void SaveData(TagCompound tag)
		{
			tag.Add("Dye", ItemIO.Save(dye));
			tag.Add("IsActive", IsActive);
			tag.Add("Inventory", inventory.Select(ItemIO.Save).ToList());
		}

		public override void LoadData(TagCompound tag)
		{
			if (tag.GetCompound("Dye") is TagCompound item) dye = ItemIO.Load(item);

			IsActive = tag.GetBool("IsActive");
			inventory = tag.GetList<TagCompound>("Inventory").Select(ItemIO.Load).ToArray();
		}

		public override void NetSend(BinaryWriter writer)
		{
			ItemIO.Send(dye, writer, false, false);

			writer.Write(IsActive);

			for (int i = 0; i < SLOTS_XY; i++)
			{
				ItemIO.Send(inventory[i], writer, false, false);
			}
		}

		public override void NetReceive(BinaryReader reader)
		{
			dye = ItemIO.Receive(reader, false, false);

			IsActive = reader.ReadBoolean();

			for (int i = 0; i < SLOTS_XY; i++)
			{
				inventory[i] = ItemIO.Receive(reader, false, false);
			}
		}
	}
}