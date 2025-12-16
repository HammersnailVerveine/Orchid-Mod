using OrchidMod.Common.Attributes;
using OrchidMod.Common;
using OrchidMod.Content.Guardian.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Misc
{
	[ClassTag(ClassTags.Guardian)]
	public class GuardianRegenPotion : ModItem
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
			Item.buffType = ModContent.BuffType<GuardianRegenPotionBuff>();
			Item.buffTime = 60 * 60 * 4;
		}
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 20;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddTile(TileID.Bottles);
			recipe.AddIngredient(ItemID.BottledWater);
			recipe.AddIngredient(ItemID.Diamond);
			recipe.AddIngredient(ItemID.WormTooth);
			recipe.AddIngredient(ItemID.Ebonkoi);
			recipe.Register();
			recipe = CreateRecipe();
			recipe.AddTile(TileID.Bottles);
			recipe.AddIngredient(ItemID.BottledWater);
			recipe.AddIngredient(ItemID.Diamond);
			recipe.AddIngredient(ItemID.Vertebrae, 2);
			recipe.AddIngredient(ItemID.CrimsonTigerfish);
			recipe.Register();
			recipe = CreateRecipe();
			recipe.AddTile(TileID.Bottles);
			recipe.AddIngredient(ItemID.BottledWater);
			recipe.AddIngredient(ItemID.Diamond);
			recipe.AddIngredient(ItemID.UnicornHorn);
			recipe.AddIngredient(ItemID.PrincessFish);
			recipe.Register();
		}
	}
}
