using OrchidMod.Gambler.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Armors.Outlaw
{
	[AutoloadEquip(EquipType.Legs)]
	public class OutlawPants : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 5, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Outlaw Pants");
			// Tooltip.SetDefault("4% increased gambling damage");
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage<GamblerDamageClass>() += 0.04f;
		}

		public override void AddRecipes()
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;

			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Silk, 6);
			recipe.AddIngredient(ItemID.GoldBar, 10);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.Find<ModItem>("BirdTalon").Type : ItemType<VultureTalon>(), 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Silk, 6);
			recipe.AddIngredient(ItemID.PlatinumBar, 10);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.Find<ModItem>("BirdTalon").Type : ItemType<VultureTalon>(), 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
