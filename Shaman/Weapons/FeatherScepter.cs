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
    public class FeatherScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 13;
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
			item.width = 40;
			item.height = 40;
			item.useTime = 33;
			item.useAnimation = 33;
			item.knockBack = 0f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.UseSound = SoundID.Item45;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("FeatherScepterProj");
			this.empowermentType = 3;
			this.empowermentLevel = 2;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Feather Scepter");
		  Tooltip.SetDefault("Shoots dangerous spinning feathers"
						    + "\nThe projectiles gain in damage after a while"
							+ "\nHaving 3 or more active shamanic bonds will result in more projectiles shot");
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			
			if (player.GetModPlayer<OrchidModPlayer>().getNbShamanicBonds() > 2) {
				Vector2 perturbedSpeed = new Vector2(speedX/2, speedY/2).RotatedByRandom(MathHelper.ToRadians(15));
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return true;
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);	
			recipe.AddIngredient(null, "HarpyTalon", 2);
			recipe.AddIngredient(ItemID.Feather, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}
