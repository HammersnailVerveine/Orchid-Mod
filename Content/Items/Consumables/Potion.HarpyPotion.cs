using OrchidMod.Content.Shaman.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Items.Consumables
{
	public class HarpyPotion : ModItem
	{
		public override string Texture => OrchidAssets.ItemsPath + Name;

		public override void SetDefaults()
		{
			Item.UseSound = SoundID.Item3;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTurn = true;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.maxStack = 30;
			Item.consumable = true;
			Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.width = 20;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.buffType = ModContent.BuffType<HarpyPotionBuff>();
			Item.buffTime = 60 * 180;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cloud Burst Potion");
			/* Tooltip.SetDefault("Your first bonus jump will release a burst of damaging feathers"
						  + "\nAllows you to double jump, if you cannot already"); */
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Bottles);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(ItemID.Deathweed, 1);
			recipe.AddIngredient(ModContent.ItemType<HarpyTalon>(), 1);
			recipe.AddIngredient(ItemID.Feather, 1);
			recipe.Register();
		}
	}

	public class HarpyPotionBuff : ModBuff
	{
		public override string Texture => OrchidAssets.BuffsPath + Name;

		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;

			// DisplayName.SetDefault("Harpy Agility");
			// Description.SetDefault("Ability to double jump, your fist bonus jump will release damaging feathers around you");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			Main.player[Main.myPlayer].GetModPlayer<OrchidShaman>().doubleJumpHarpy = true;
		}
	}
}