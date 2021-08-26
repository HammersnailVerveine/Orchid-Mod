using Terraria;
using Terraria.ModLoader;


namespace OrchidMod.Shaman.Accessories
{
	public class SinisterPresent : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 7, 50, 0);
			item.rare = 8;
			item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sinister Present");
			Tooltip.SetDefault("30% increased shamanic damage"
							 + "\nYour shamanic bonds will last 10 seconds longer"
							 + "\nTaking direct damage will reduce their current duration by 5 seconds"
							 + "\nIt will also nullify the damage increase for 15 seconds");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (!(Main.LocalPlayer.FindBuffIndex(mod.BuffType("BrokenPower")) > -1))
			{
				modPlayer.shamanDamage += 0.30f;
			}

			modPlayer.shamanBuffTimer += 10;
			modPlayer.shamanSunBelt = true;
			modPlayer.shamanMourningTorch = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MourningTorch", 1);
			recipe.AddIngredient(null, "FragilePresent", 1);
			recipe.AddTile(114);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}