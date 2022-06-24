using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Weapons.Dice
{
	public class GemstoneDie : OrchidModGamblerDie
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			this.diceID = 1;
			this.diceCost = 1;
			this.diceDuration = 15;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gemstone Die");
			Tooltip.SetDefault("Gives 4 - 24% chance not to consume chips");
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.Amethyst, 5);
			recipe.AddIngredient(ItemID.Topaz, 5);
			recipe.AddIngredient(ItemID.Sapphire, 5);
			recipe.AddIngredient(ItemID.Emerald, 5);
			recipe.AddIngredient(ItemID.Ruby, 5);
			recipe.AddIngredient(ItemID.Diamond, 5);
			recipe.Register();
		}
	}
}
