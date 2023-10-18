using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Misc
{
	public class AttractiteShuriken : OrchidModAlchemistMisc
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 5;
			Item.useStyle = 1;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.shootSpeed = 9f;
			Item.knockBack = 0f;
			Item.width = 22;
			Item.height = 22;
			Item.scale = 1f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 0, 0);
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.maxStack = 5;
			Item.UseSound = SoundID.Item1;
			Item.consumable = true;
			Item.shoot = ProjectileType<Alchemist.Projectiles.Misc.AttractiteShurikenProj>();
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Attractite Shuriken");
			// Tooltip.SetDefault("Inflicts attractite to hit enemies");
		}

		public override bool OnPickup(Player player)
		{
			return !AlreadyInInventory(player, true);
		}

		private bool AlreadyInInventory(Player player, bool addStack = false)
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
