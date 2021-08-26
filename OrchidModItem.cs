using Terraria;
using Terraria.ModLoader;

namespace OrchidMod
{
	public abstract class OrchidModItem : ModItem
	{
		// Hehe 'Te Nandayo?!
		public bool alreadyInInventory(Player player, bool addStack = false)
		{
			for (int i = 0; i < Main.maxInventory; i++)
			{
				Item invItem = player.inventory[i];
				if (invItem.type == item.type)
				{
					if (addStack) invItem.stack = invItem.stack + item.stack <= invItem.maxStack ? invItem.stack + item.stack : invItem.stack + item.stack;
					return true;
				}
			}

			for (int i = 0; i < 20; i++)
			{
				if (player.armor[i].type == item.type)
				{
					return true;
				}
			}

			for (int i = 0; i < 10; i++)
			{
				if (player.dye[i].type == item.type)
				{
					return true;
				}
			}

			for (int i = 0; i < 5; i++)
			{
				if (player.miscEquips[i].type == item.type)
				{
					return true;
				}
				if (player.miscDyes[i].type == item.type)
				{
					return true;
				}
			}

			return false;
		}
	}
}
