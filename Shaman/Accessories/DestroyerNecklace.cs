using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Neck)]
	public class DestroyerNecklace : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(0, 4, 0, 0);
			item.rare = 5;
			item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Destroyer Necklace");
			Tooltip.SetDefault("Enter a frenzied state by dealing 5 critical strikes in a short period of time under the effect of a shamanic water bond"
							+ "\nWhile frenzied, shamanic damage and critical strike damage is increased by 15%");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDestroyer = true;

			if (modPlayer.shamanTimerDestroyer > 0)
			{
				modPlayer.shamanTimerDestroyer--;
				if (modPlayer.shamanTimerDestroyer == 0)
				{
					modPlayer.shamanDestroyerCount = 0;
				}

				if (modPlayer.shamanDestroyerCount == 5)
				{
					Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 33);
					player.AddBuff((mod.BuffType("DestroyerFrenzy")), 60 * 10);

					for (int i = 0; i < 15; i++)
					{
						int dust = Dust.NewDust(player.position, player.width, player.height, 60);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale *= 1.5f;
					}
				}
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(1225, 5); // Hallowed Bar
			recipe.AddIngredient(548, 20); // Sould of Might
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
