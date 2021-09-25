using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Weapons.Dice
{
	public class GemstoneDie : OrchidModGamblerDie
	{
		public override void SafeSetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.Amethyst, 5);
			recipe.AddIngredient(ItemID.Topaz, 5);
			recipe.AddIngredient(ItemID.Sapphire, 5);
			recipe.AddIngredient(ItemID.Emerald, 5);
			recipe.AddIngredient(ItemID.Ruby, 5);
			recipe.AddIngredient(ItemID.Diamond, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
