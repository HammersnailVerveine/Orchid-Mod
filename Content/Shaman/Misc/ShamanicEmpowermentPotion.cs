using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Misc
{
	public class ShamanicEmpowermentPotion : ModItem
	{
		public override void SetDefaults()
		{
			Item.UseSound = SoundID.Item3;
			Item.useStyle = 2;
			Item.useTurn = true;
			Item.useAnimation = 16;
			Item.useTime = 16;
			Item.maxStack = 30;
			Item.consumable = true;
			Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.width = 20;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.buffType = Mod.Find<ModBuff>("ShamanicEmpowerment").Type;
			Item.buffTime = 60 * 420;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Shamanic Accurary Potion");
			// Tooltip.SetDefault("The source of your shamanic attacks will stay in position better");
		}
		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Bottles);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(ItemID.Fireblossom, 1);
			recipe.AddIngredient(ItemID.Shiverthorn, 1);
			recipe.AddIngredient(ItemID.Feather, 1);
			recipe.Register();
		}
	}
}
