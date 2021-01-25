using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class Sanctify : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 45;
			item.width = 30;
			item.height = 30;
			item.useTime = 24;
			item.useAnimation = 24;
			item.knockBack = 1.15f;
			item.rare = 5;
			item.value = Item.sellPrice(0, 4, 55, 0);
			item.UseSound = SoundID.Item117;
			item.autoReuse = true;
			item.shootSpeed = 7f;
			item.shoot = mod.ProjectileType("SanctifyProj");
			this.empowermentType = 5;
			this.empowermentLevel = 3;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Sanctify");
		  Tooltip.SetDefault("Casts radiant projectiles to purge your foes"
							+"\nHitting enemies will gradually grant you hallowed orbs"
							+"\nWhen reaching 7 orbs, they will break free and home into your enemies"
							+"\nHaving 3 or more active shamanic bonds will release homing projectiles");
		}
				
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			
			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) > 2) {
				for (int i = 0; i < 2; i ++) {
					Vector2 projectileVelocity = ( new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(i == 0 ? -20 : 20)));
					Projectile.NewProjectile(position.X, position.Y, projectileVelocity.X, projectileVelocity.Y, mod.ProjectileType("SanctifyProjAlt"), (int)(item.damage*0.75), knockBack, item.owner);
				}
			}
			
            return true;
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}
