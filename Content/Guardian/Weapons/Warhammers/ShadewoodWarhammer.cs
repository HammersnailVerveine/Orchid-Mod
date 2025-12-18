using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Warhammers
{
	public class ShadewoodWarhammer : OrchidModGuardianHammer
	{

		public override void SafeSetDefaults()
		{
			Item.width = 36;
			Item.height = 36;
			Item.value = Item.sellPrice(0, 0, 0, 20);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item1;
			Item.knockBack = 5f;
			Item.shootSpeed = 8f;
			Item.damage = 64;
			Item.useTime = 28;
			Range = 26;
			GuardStacks = 1;
			SwingSpeed = 1.5f;
			ReturnSpeed = 0.8f;
			BlockDuration = 180;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(ItemID.Shadewood, 10);
			recipe.Register();
		}
	}
}
