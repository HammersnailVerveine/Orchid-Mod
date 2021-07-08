using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
using System;
 
namespace OrchidMod.Shaman.Weapons
{
    public class DepthsBaton : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 57;
			item.width = 40;
			item.height = 40;
			item.useTime = 35;
			item.useAnimation = 35;
			item.knockBack = 3.15f;
			item.rare = 3;
			item.value = Item.sellPrice(0, 1, 6, 0);
			item.UseSound = SoundID.Item72;
			item.autoReuse = true;
			item.shootSpeed = 12f;
			item.shoot = mod.ProjectileType("DepthsBatonProj");
			this.empowermentType = 5;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Depths Baton");
		  Tooltip.SetDefault("Shoots bolts of dark energy"
							+ "\nHitting at maximum range deals increased damage" 
							+ "\nHaving 3 or more active shamanic bonds will allow the weapon to shoot a straight beam");
		} 

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int BuffsCount = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			if (BuffsCount > 2) {
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("DepthBatonProjAlt"), damage - 10, knockBack, player.whoAmI);
			}
			return true;
		}
		

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Blum", 1);
			recipe.AddIngredient(null, "PerishingSoul", 1);
			recipe.AddIngredient(null, "SporeCaller", 1);
			recipe.AddIngredient(null, "VileSpout", 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Blum", 1);
			recipe.AddIngredient(null, "PerishingSoul", 1);
			recipe.AddIngredient(null, "SporeCaller", 1);
			recipe.AddIngredient(null, "SpineScepter", 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}
