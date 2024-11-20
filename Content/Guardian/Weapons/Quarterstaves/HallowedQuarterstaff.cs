using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Projectiles.Quarterstaves;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Quarterstaves
{
	public class HallowedQuarterstaff : OrchidModGuardianQuarterstaff
	{
		public override void SafeSetDefaults()
		{
			Item.width = 48;
			Item.height = 48;
			Item.value = Item.sellPrice(0, 4, 55, 0);
			Item.rare = ItemRarityID.Pink;
			Item.useTime = 25;
			ParryDuration = 90;
			Item.knockBack = 7.5f;
			Item.damage = 172;
			GuardStacks = 2;
		}
		
		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.Register();
		}
	}
}
