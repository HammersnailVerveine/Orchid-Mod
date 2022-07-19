using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace OrchidMod.Shaman.Accessories
{
	public class SinisterPresent : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
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
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			if (!(Main.LocalPlayer.FindBuffIndex(Mod.Find<ModBuff>("BrokenPower").Type) > -1))
			{
				player.GetDamage<ShamanDamageClass>() += 0.30f;
			}

			modPlayer.shamanBuffTimer += 10;
			modPlayer.shamanSunBelt = true;
			modPlayer.shamanMourningTorch = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MourningTorch>(), 1);
			recipe.AddIngredient(ModContent.ItemType<FragilePresent>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}