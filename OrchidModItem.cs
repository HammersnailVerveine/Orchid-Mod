using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace OrchidMod
{
	public abstract class OrchidModItem : ModItem
	{
		public virtual void AltSetStaticDefaults() {}

		public sealed override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			this.AltSetStaticDefaults();
		}

		public bool alreadyInInventory(Player player, bool addStack = false)
		{
			for (int i = 0; i < Main.InventorySlotsTotal; i++)
			{
				Item invItem = player.inventory[i];
				if (invItem.type == Item.type)
				{
					if (addStack) invItem.stack = invItem.stack + Item.stack <= invItem.maxStack ? invItem.stack + Item.stack : invItem.stack + Item.stack;
					return true;
				}
			}

			for (int i = 0; i < 20; i++)
			{
				if (player.armor[i].type == Item.type)
				{
					return true;
				}
			}

			for (int i = 0; i < 10; i++)
			{
				if (player.dye[i].type == Item.type)
				{
					return true;
				}
			}

			for (int i = 0; i < 5; i++)
			{
				if (player.miscEquips[i].type == Item.type)
				{
					return true;
				}
				if (player.miscDyes[i].type == Item.type)
				{
					return true;
				}
			}

			return false;
		}
	}
}
