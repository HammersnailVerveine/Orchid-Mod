using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist.Projectiles.Nature;
using OrchidMod.Alchemist.Projectiles.Fire;
using OrchidMod.Alchemist.Projectiles.Water;
using OrchidMod.Alchemist.Projectiles.Air;
using OrchidMod.Alchemist.Projectiles.Reactive;
using OrchidMod.Alchemist.Projectiles.Reactive.ReactiveSpawn;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Water
{
    public class IceChestFlaskProj : OrchidModAlchemistProjectile
    {
		public static List<int> smallProjectiles = setSmallProjectiles();
		public static List<int> bigProjectiles = setBigProjectiles();
		
		public static List<int> setSmallProjectiles() {
			List<int> smallProjectiles = new List<int>();
			smallProjectiles.Add(ProjectileType<AlchemistSlime>());
			smallProjectiles.Add(ProjectileType<BloomingPetal>());
			smallProjectiles.Add(ProjectileType<AirSporeProj>());
			smallProjectiles.Add(ProjectileType<CrimsonFlaskProj>());
			smallProjectiles.Add(ProjectileType<SunplateFlaskProj>());
			smallProjectiles.Add(ProjectileType<EmberVialProj>());
			smallProjectiles.Add(ProjectileType<FireSporeProj>());
			smallProjectiles.Add(ProjectileType<LivingSapVialProj>());
			smallProjectiles.Add(ProjectileType<NatureSporeProj>());
			smallProjectiles.Add(ProjectileType<PoisonVialProj>());
			smallProjectiles.Add(ProjectileType<DungeonFlaskProj>());
			smallProjectiles.Add(ProjectileType<SeafoamVialProj>());
			smallProjectiles.Add(ProjectileType<WaterSporeProj>());
			return smallProjectiles;
		}
		
		public static List<int> setBigProjectiles() {
			List<int> smallProjectiles = new List<int>();
			smallProjectiles.Add(ProjectileType<LivingSapBubble>());
			smallProjectiles.Add(ProjectileType<OilBubble>());
			smallProjectiles.Add(ProjectileType<PoisonBubble>());
			smallProjectiles.Add(ProjectileType<SeafoamBubble>());
			smallProjectiles.Add(ProjectileType<SlimeBubble>());
			smallProjectiles.Add(ProjectileType<SpiritedBubble>());
			smallProjectiles.Add(ProjectileType<AlchemistHive>());
			smallProjectiles.Add(ProjectileType<BloomingReactiveAlt>());
			smallProjectiles.Add(ProjectileType<CorruptionFlaskProj>());
			smallProjectiles.Add(ProjectileType<SunflowerFlaskProj3>());
			return smallProjectiles;
		}
		
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
			for (int i = 0 ; i < 20 ; i ++) {
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 261);
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity.X /= 3;
				Main.dust[dust2].velocity.Y /= 3;
			}
			
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
					if (smallProjectiles.Contains(proj.type)) {
						int damage = projectile.damage;
						int projType = ProjectileType<IceChestFlaskProjSmall>();
						Projectile.NewProjectile(proj.Center.X, proj.Center.Y, 0f, 1f, projType, damage, 1f, projectile.owner);
						proj.active = false;
					}
					if (bigProjectiles.Contains(proj.type)) {
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