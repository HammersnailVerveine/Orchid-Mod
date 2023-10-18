using Microsoft.Xna.Framework;
using OrchidMod.Content.Alchemist.Projectiles.Air;
using OrchidMod.Content.Alchemist.Projectiles.Fire;
using OrchidMod.Content.Alchemist.Projectiles.Water;
using System.Collections.Generic;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Projectiles.Nature
{
	public class JungleLilyFlaskProj : OrchidModAlchemistProjectile
	{
		public static List<int> sporeProjectiles = setSporeProjectiles();

		public static List<int> setSporeProjectiles()
		{
			List<int> sporeProjectiles = new List<int>();
			sporeProjectiles.Add(ProjectileType<WaterSporeProj>());
			sporeProjectiles.Add(ProjectileType<AirSporeProj>());
			sporeProjectiles.Add(ProjectileType<FireSporeProj>());
			sporeProjectiles.Add(ProjectileType<NatureSporeProj>());
			return sporeProjectiles;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Jungle Lily Aura");
		}

		public override void AI()
		{
			for (int l = 0; l < Main.npc.Length; l++)
			{
				NPC target = Main.npc[l];
				if (Projectile.Hitbox.Intersects(target.Hitbox))
				{
					OrchidModAlchemistNPC modTarget = target.GetGlobalNPC<OrchidModAlchemistNPC>();
					if (modTarget.alchemistWater > 0)
					{
						spawnSpores(ProjectileType<WaterSporeProj>(), target, Projectile);
					}
					if (modTarget.alchemistAir > 0)
					{
						spawnSpores(ProjectileType<AirSporeProj>(), target, Projectile);
					}
					if (modTarget.alchemistFire > 0)
					{
						spawnSpores(ProjectileType<FireSporeProj>(), target, Projectile);
					}
					if (modTarget.alchemistNature > 0)
					{
						spawnSpores(ProjectileType<NatureSporeProj>(), target, Projectile);
					}
				}
			}

			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (Projectile.owner == proj.owner && proj.active && Projectile.Hitbox.Intersects(proj.Hitbox) && sporeProjectiles.Contains(proj.type) && proj.localAI[0] != 1f && proj.timeLeft < 590)
				{
					for (int i = 0; i < 2; i++)
					{
						Vector2 vel = (new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
						int spawnProjInt = Projectile.NewProjectile(Projectile.GetSource_FromThis(), proj.Center, vel, proj.type, proj.damage, proj.knockBack, proj.owner);
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

		public static void spawnSpores(int type, NPC target, Projectile projectile)
		{
			int rand = Main.rand.Next(2) + 2;
			for (int i = 0; i < rand; i++)
			{
				Vector2 vel = (new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
				int spawnProj = Projectile.NewProjectile(projectile.GetSource_FromThis(), target.Center, vel, type, projectile.damage, 0f, projectile.owner);
				Main.projectile[spawnProj].localAI[1] = 1f;
				Main.projectile[spawnProj].localAI[0] = 1f;
			}
		}
	}
}