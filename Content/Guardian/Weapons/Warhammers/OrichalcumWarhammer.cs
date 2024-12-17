using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using OrchidMod.Content.Guardian.Projectiles.Warhammers;

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
			Item.shootSpeed = 8f;
			Item.damage = 252;
			Item.useTime = 38;
			Range = 60;
			GuardStacks = 1;
			SlamStacks = 2;
			Penetrate = true;
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
