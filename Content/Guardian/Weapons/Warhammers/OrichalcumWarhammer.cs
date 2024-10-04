using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Warhammers
{
	public class OrichalcumWarhammer : OrchidModGuardianHammer
	{
		public override void SafeSetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.value = Item.sellPrice(0, 1, 65, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.DD2_MonkStaffSwing;
			Item.knockBack = 12f;
			Item.shootSpeed = 10f;
			Item.damage = 252;
			Item.useTime = 35;
			Range = 35;
			BlockStacks = 2;
			SlamStacks = 1;
			ReturnSpeed = 1.25f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddIngredient(ItemID.OrichalcumBar, 10);
			recipe.Register();
		}
	}
}
