using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class LichScepterProjAlt : OrchidModShamanProjectile
	{
		public int projType;

		public override void SafeSetDefaults()
		{
			projectile.width = 10;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.aiStyle = 1;
			projectile.timeLeft = 500;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			aiType = ProjectileID.Bullet;
			Main.projFrames[projectile.type] = 2;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Necrotic Bolt");
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);

			if (this.projType == 0)
			{
				this.projType = Main.rand.Next(2) + 1;
				projectile.frame = this.projType - 1;
			}

			if (projectile.timeLeft > 480)
			{
				projectile.friendly = false;
			}
			else
			{
				projectile.friendly = true;

				if (nbBonds > 3)
				{
					projectile.extraUpdates = 1;
					ProjectileID.Sets.Homing[projectile.type] = true;

					if (projectile.localAI[0] == 0f)
					{
						AdjustMagnitude(ref projectile.velocity);
						projectile.localAI[0] = 1f;
					}

					Vector2 move = Vector2.Zero;
					float distance = 1500f;
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
						projectile.velocity = (20 * projectile.velocity + move);
						AdjustMagnitude(ref projectile.velocity);
					}
					else
					{
						projectile.timeLeft -= 5;
					}
				}
				else
				{
					projectile.timeLeft -= 15;
				}
			}

			if (Main.rand.Next(2) == 0)
			{
				int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, this.projType == 1 ? 15 : 127, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
				Main.dust[DustID].noGravity = true;
			}
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 3f)
			{
				vector *= 3f / magnitude;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.projType == 1 ? 15 : 127);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = projectile.velocity / 2;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) { }
	}
}