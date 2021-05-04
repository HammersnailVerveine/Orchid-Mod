using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist.Projectiles.Nature;
using OrchidMod.Alchemist.Projectiles.Fire;
using OrchidMod.Alchemist.Projectiles.Water;
using OrchidMod.Alchemist.Projectiles.Air;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Nature
{
    public class JungleLilyFlaskProj : OrchidModAlchemistProjectile
    {
		public static List<int> sporeProjectiles = setSporeProjectiles();
		
		public static List<int> setSporeProjectiles() {
			List<int> sporeProjectiles = new List<int>();
			sporeProjectiles.Add(ProjectileType<WaterSporeProj>());
			sporeProjectiles.Add(ProjectileType<AirSporeProj>());
			sporeProjectiles.Add(ProjectileType<FireSporeProj>());
			sporeProjectiles.Add(ProjectileType<NatureSporeProj>());
			return sporeProjectiles;
		}
		
        public override void SafeSetDefaults()
        {
            projectile.width = 100;
            projectile.height = 100;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.tileCollide = false;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jungle Lily Aura");
        }
		
		public override void AI() {
			for (int l = 0; l < Main.npc.Length; l++) {  
				NPC target = Main.npc[l];
				if (projectile.Hitbox.Intersects(target.Hitbox))  {
					OrchidModAlchemistNPC modTarget = target.GetGlobalNPC<OrchidModAlchemistNPC>();
					if (modTarget.alchemistWater > 0) {
						spawnSpores(ProjectileType<WaterSporeProj>(), target, projectile);
					}
					if (modTarget.alchemistAir > 0) {
						spawnSpores(ProjectileType<AirSporeProj>(), target, projectile);
					}
					if (modTarget.alchemistFire > 0) {
						spawnSpores(ProjectileType<FireSporeProj>(), target, projectile);
					}
					if (modTarget.alchemistNature > 0) {
						spawnSpores(ProjectileType<NatureSporeProj>(), target, projectile);
					}
				}
			}
			
			for (int l = 0; l < Main.projectile.Length; l++) {
				Projectile proj = Main.projectile[l];
				if (projectile.owner == proj.owner && proj.active && projectile.Hitbox.Intersects(proj.Hitbox) && sporeProjectiles.Contains(proj.type) && proj.localAI[0] != 1f && proj.timeLeft < 590)  {
					for (int i = 0 ; i < 2 ; i ++) {
						Vector2 vel = ( new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
						int spawnProjInt = Projectile.NewProjectile(proj.Center.X, proj.Center.Y, vel.X, vel.Y, proj.type, proj.damage, proj.knockBack, proj.owner);
						Projectile spawnProj = Main.projectile[spawnProjInt];
						spawnProj.localAI[1] = proj.localAI[1];
						spawnProj.localAI[0] = 1f;
						spawnProj.timeLeft = 580;
						spawnProj.netUpdate = true;
					}
					proj.active = false;
					proj.netUpdate = true;
				}
			}
		}
		
		public static void spawnSpores(int type, NPC target, Projectile projectile) {
			int rand = Main.rand.Next(2) + 2;
			for (int i = 0 ; i < rand ; i ++) {
				Vector2 vel = ( new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
				int spawnProj = Projectile.NewProjectile(target.Center.X, target.Center.Y, vel.X, vel.Y, type, projectile.damage, 0f, projectile.owner);
				Main.projectile[spawnProj].localAI[1] = 1f;
				Main.projectile[spawnProj].localAI[0] = 1f;
			}
		}
    }
}