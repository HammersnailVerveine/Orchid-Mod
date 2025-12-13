using OrchidMod.Content.Shapeshifter.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Misc
{
	public class ShapeshifterSurvivalPotion : ModItem
	{
		public override void SetDefaults()
		{
			Item.UseSound = SoundID.Item3;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useTurn = true;
			Item.useAnimation = 16;
			Item.useTime = 16;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.width = 20;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.buffType = ModContent.BuffType<ShapeshifterSurvivalPotionBuff>();
			Item.buffTime = 60 * 60 * 8;
		}
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 20;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Bottles);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(ItemID.Fireblossom, 1);
			recipe.AddIngredient(ItemID.Damselfish, 1);
			recipe.Register();
		}
	}
}
