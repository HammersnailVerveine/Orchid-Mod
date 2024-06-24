using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Warhammers
{
	public class BambooWarhammer : OrchidModGuardianHammer
	{

		public override void SafeSetDefaults()
		{
			Item.width = 38;
			Item.height = 36;
			Item.value = Item.sellPrice(0, 0, 0, 20);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item1;
			Item.knockBack = 5f;
			Item.shootSpeed = 8f;
			Item.damage = 25;
			range = 18;
			blockStacks = 1;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(ItemID.BambooBlock, 20);
			recipe.Register();
		}
	}
}
