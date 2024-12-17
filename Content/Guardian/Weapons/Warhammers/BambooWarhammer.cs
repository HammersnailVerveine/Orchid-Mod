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
			Item.value = Item.sellPrice(0, 0, 3, 50);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item65;
			Item.knockBack = 5f;
			Item.shootSpeed = 14f;
			Item.useTime = 20;
			Item.damage = 40;
			Range = 20;
			GuardStacks = 1;
			ReturnSpeed = 1.5f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Sawmill);
			recipe.AddIngredient(ItemID.BambooBlock, 20);
			recipe.Register();
		}
	}
}
