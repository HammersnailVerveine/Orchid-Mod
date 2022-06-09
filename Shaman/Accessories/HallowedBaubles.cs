using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	public class HallowedBaubles : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 1, 12, 75);
			Item.rare = 4;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Baubles");
			Tooltip.SetDefault("After completing an orb weapon cycle, you will be given a bonus orb on your next hit"
							  + "\nYou will also recover some health based on the number of orbs in the cycle");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod) > 0)
			{
				if (modPlayer.orbCountSmall == 0 && modPlayer.shamanOrbSmall != ShamanOrbSmall.NULL)
				{
					player.AddBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type, 10 * 60);
					modPlayer.shamanOrbSmall = ShamanOrbSmall.NULL;

					if (Main.myPlayer == player.whoAmI)
						player.HealEffect(5, true);
					player.statLife += 5;
				}

				if (modPlayer.orbCountBig == 0 && modPlayer.shamanOrbBig != ShamanOrbBig.NULL)
				{
					player.AddBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type, 10 * 60);
					modPlayer.shamanOrbBig = ShamanOrbBig.NULL;

					if (Main.myPlayer == player.whoAmI)
						player.HealEffect(10, true);
					player.statLife += 10;
				}

				if (modPlayer.orbCountLarge == 0 && modPlayer.shamanOrbLarge != ShamanOrbLarge.NULL)
				{
					player.AddBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type, 10 * 60);
					modPlayer.shamanOrbLarge = ShamanOrbLarge.NULL;

					if (Main.myPlayer == player.whoAmI)
						player.HealEffect(20, true);
					player.statLife += 20;
				}

				if (modPlayer.orbCountUnique == 0 && modPlayer.shamanOrbUnique != ShamanOrbUnique.NULL)
				{
					player.AddBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type, 10 * 60);
					modPlayer.shamanOrbUnique = ShamanOrbUnique.NULL;

					if (Main.myPlayer == player.whoAmI)
						player.HealEffect(20, true);
					player.statLife += 20;
				}
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(Mod);
			recipe.AddIngredient(null, "TreasuredBaubles", 1);
			recipe.AddIngredient(ItemID.PixieDust, 10);
			recipe.AddIngredient(ItemID.UnicornHorn, 2);
			recipe.AddIngredient(ItemID.CrystalShard, 5);
			recipe.AddTile(114);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
