using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Quarterstaves
{
	public class BambooQuarterstaff : OrchidModGuardianQuarterstaff
	{
		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 0, 3, 45);
			Item.rare = ItemRarityID.White;
			Item.useTime = 20;
			ParryDuration = 40;
			Item.knockBack = 4f;
			Item.damage = 21;
			GuardStacks = 1;
			SwingSpeed = 1.25f;
			CounterSpeed = 1.25f;
			JabSpeed = 1.25f;
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
