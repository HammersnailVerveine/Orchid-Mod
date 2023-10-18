using Microsoft.Xna.Framework;
using OrchidMod.Gambler.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Armors.Outlaw
{
	[AutoloadEquip(EquipType.Body)]
	public class OutlawVest : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 6, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 3;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Outlaw Vest");
			// Tooltip.SetDefault("4% increased gambling damage");
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage<GamblerDamageClass>() += 0.04f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}

		public override void AddRecipes()
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;

			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Silk, 7);
			recipe.AddIngredient(ItemID.GoldBar, 15);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.Find<ModItem>("BirdTalon").Type : ItemType<VultureTalon>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Silk, 7);
			recipe.AddIngredient(ItemID.PlatinumBar, 15);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.Find<ModItem>("BirdTalon").Type : ItemType<VultureTalon>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
