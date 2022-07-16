using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Downpour
{
	[AutoloadEquip(EquipType.Legs)]
	public class DownpourKilt : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 11;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Downpour Kilt");
			Tooltip.SetDefault("15% increased shamanic damage");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			player.GetDamage<ShamanDamageClass>() += 0.15f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AdamantiteBar, 16);
			recipe.AddIngredient(null, "DownpourCrystal", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TitaniumBar, 16);
			recipe.AddIngredient(null, "DownpourCrystal", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
