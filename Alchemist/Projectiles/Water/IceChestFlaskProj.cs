using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist.Weapons.Water;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Water
{
    public class IceChestFlaskProj : OrchidModAlchemistProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.tileCollide = false;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
        }
		
        public override void AI()
        {
			for (int l = 0; l < Main.npc.Length; l++) {  
				NPC target = Main.npc[l];
				if (projectile.Hitbox.Intersects(target.Hitbox))  {
					OrchidModAlchemistNPC modTarget = target.GetGlobalNPC<OrchidModAlchemistNPC>();
					target.AddBuff(BuffType<Alchemist.Buffs.Debuffs.FlashFreeze>(), modTarget.alchemistWater > 0 ? 60 * 30 : 60 * 3);
				}
			}
			
			for (int l = 0; l < Main.projectile.Length; l++) {  
				Projectile proj = Main.projectile[l];
				if (projectile.owner == proj.owner && proj.active && projectile.Hitbox.Intersects(proj.Hitbox))  {
					if (IceChestFlask.smallProjectiles.Contains(proj.type)) {
						int damage = projectile.damage;
						int projType = ProjectileType<IceChestFlaskProjSmall>();
						Projectile.NewProjectile(proj.Center.X, proj.Center.Y, 0f, 1f, projType, damage, 1f, projectile.owner);
						proj.active = false;
					}
					if (IceChestFlask.bigProjectiles.Contains(proj.type)) {
						int damage = projectile.damage * 5;
						int projType = ProjectileType<IceChestFlaskProjBig>();
						Projectile.NewProjectile(proj.Center.X, proj.Center.Y, 0f, 1f, projType, damage, 5f, projectile.owner);
						proj.active = false;
					}
				}
			}
		}
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flash Freeze Aura");
        }
    }
}