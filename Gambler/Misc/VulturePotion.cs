using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Misc
{
	public class VulturePotion : OrchidModItem
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
			Item.height = 30;
			Item.rare = 1;
			Item.buffType = BuffType<Gambler.Buffs.VulturePotionBuff>();
			Item.buffTime = 60 * 180;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scavenger Potion");
			Tooltip.SetDefault("20% increased gambler chip generation");
		}
		public override void AddRecipes()
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;

			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Bottles);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(ItemID.Blinkroot, 1);
			recipe.AddIngredient(ItemID.Cactus, 1);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.Find<ModItem>("BirdTalon").Type : ItemType<VultureTalon>(), 2);
			recipe.Register();
			recipe.Register();
		}
	}
}
