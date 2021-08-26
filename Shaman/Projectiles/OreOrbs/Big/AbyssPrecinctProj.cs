using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Big
{
	public class AbyssPrecinctProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 36;
			projectile.height = 36;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 100;
			projectile.tileCollide = false;
			projectile.alpha = 192;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyss Bolt");
		}

		public override void AI()
		{
			projectile.rotation += 0.1f;

			if (projectile.timeLeft % 30 == 0)
			{
				spawnDustCircle(172, 50);
				spawnDustCircle(172, 100);
				spawnDustCircle(29, 75);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, mod.ProjectileType("AbyssPrecinctProjExplosion"), projectile.damage * 2, 0.0f, projectile.owner, 0.0f, 0.0f);
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			}

			int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
			Main.dust[dust2].velocity /= 1f;
			Main.dust[dust2].scale = 1.7f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].noLight = true;

			if (projectile.localAI[0] == 0f)
			{
				AdjustMagnitude(ref projectile.velocity);
				projectile.localAI[0] = 1f;
			}

			Vector2 move = Vector2.Zero;
			float distance = 500f;
			bool target = false;
			for (int k = 0; k < 200; k++)
			{
				if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
				{
					Vector2 newMove = Main.npc[k].Center - projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < distance)
					{
						move = newMove;
						distance = distanceTo;
						target = true;
					}
				}
			}
			if (target)
			{
				AdjustMagnitude(ref move);
				projectile.velocity = (10 * projectile.velocity + move) / 3f;
				AdjustMagnitude(ref projectile.velocity);
			}
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 20; i++)
			{
				double deg = (i * (36 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);

				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - projectile.width / 2 + projectile.velocity.X + 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - projectile.height / 2 + projectile.velocity.Y + 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity = distToCenter == 50 ? projectile.velocity : distToCenter == 75 ? projectile.velocity * 0.75f : projectile.velocity / 2;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
				Main.dust[dust].scale = 1.5f;
			}
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f)
			{
				vector *= 6f / magnitude;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			projectile.velocity *= 0.2f;
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("AbyssPrecinctProjAlt"), projectile.damage, 0.0f, projectile.owner, 0.0f, 0.0f);

			if (modPlayer.shamanOrbBig != ShamanOrbBig.ABYSS)
			{
				modPlayer.shamanOrbBig = ShamanOrbBig.ABYSS;
				modPlayer.orbCountBig = 0;
			}
			modPlayer.orbCountBig++;

			if (modPlayer.orbCountBig == 3)
			{
				Projectile.NewProjectile(player.Center.X - 30, player.position.Y - 30, 0f, 0f, mod.ProjectileType("AbyssOrb"), 0, 0, projectile.owner, 0f, 0f);

				if (player.FindBuffIndex(mod.BuffType("ShamanicBaubles")) > -1)
				{
					modPlayer.orbCountBig += 3;
					Projectile.NewProjectile(player.Center.X - 15, player.position.Y - 38, 0f, 0f, mod.ProjectileType("AbyssOrb"), 1, 0, projectile.owner, 0f, 0f);
					player.ClearBuff(mod.BuffType("ShamanicBaubles"));
				}
			}
			if (modPlayer.orbCountBig == 6)
				Projectile.NewProjectile(player.Center.X - 23, player.position.Y - 34, 0f, 0f, mod.ProjectileType("AbyssOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 9)
				Projectile.NewProjectile(player.Center.X - 2, player.position.Y - 40, 0f, 0f, mod.ProjectileType("AbyssOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 12)
				Projectile.NewProjectile(player.Center.X + 19, player.position.Y - 34, 0f, 0f, mod.ProjectileType("AbyssOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 15)
				Projectile.NewProjectile(player.Center.X + 38, player.position.Y - 18, 0f, 0f, mod.ProjectileType("AbyssOrb"), 0, 0, projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig > 15)
			{
				player.AddBuff(mod.BuffType("AbyssEmpowerment"), 60 * 30);
				modPlayer.orbCountBig = -3;
			}
		}
	}
}