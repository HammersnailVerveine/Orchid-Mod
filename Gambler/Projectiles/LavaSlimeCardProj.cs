using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class LavaSlimeCardProj : OrchidModGamblerProjectile
	{
		private int baseDamage = 0;
		private int justHit = 0;
		private int velocityStuck = 0;
		private float oldPositionY = 0f;
		private int nbBounces = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Slime");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 22;
			projectile.height = 20;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.penetrate = -1;
			projectile.alpha = 16;
			ProjectileID.Sets.Homing[projectile.type] = true;
			Main.projFrames[projectile.type] = 2;
			this.gamblingChipChance = 10;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void SafeAI()
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			projectile.velocity.Y += (projectile.wet || projectile.lavaWet || projectile.honeyWet) ? projectile.velocity.Y > -5f ? -0.5f : 0f : projectile.velocity.Y < 5f ? 0.3f : 0f;
			projectile.frame = projectile.velocity.Y < 0f ? 1 : 0;
			this.justHit -= this.justHit > 0 ? 1 : 0;

			if (Main.rand.Next(15) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
			}

			this.velocityStuck = projectile.Center.Y == oldPositionY ? this.velocityStuck + 1 : 0;
			this.oldPositionY = 0f + projectile.Center.Y;

			if (projectile.velocity.X > 4f)
			{
				projectile.velocity.X = 4f;
			}
			if (projectile.velocity.X < -4f)
			{
				projectile.velocity.X = -4f;
			}

			if (!this.initialized)
			{
				this.baseDamage = projectile.damage;
				this.initialized = true;
			}

			if (Main.myPlayer == projectile.owner)
			{
				if (velocityStuck >= 5)
				{
					projectile.velocity.Y = -5;
					this.velocityStuck = 0;
				}
				if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.LavaSlimeCard>() && modPlayer.GamblerDeckInHand)
				{
					Vector2 newMove = new Vector2(Main.screenPosition.X + (float)Main.mouseX, (float)projectile.Center.Y) - projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo > 5f)
					{
						if ((float)(Main.screenPosition.X + Main.mouseX) > projectile.Center.X)
						{
							projectile.velocity.X += projectile.velocity.X < 4f ? this.justHit > 0 ? 0.15f : 0.25f : 0f;
						}
						else
						{
							projectile.velocity.X -= projectile.velocity.X > -4f ? this.justHit > 0 ? 0.15f : 0.25f : 0f;
						}
					}
					else
					{
						if (projectile.velocity.Length() > 0.01f)
						{
							projectile.velocity.X *= 0.9f;
						}
					}

					int velocityXBy1000 = (int)(newMove.X * 1000f);
					int oldVelocityXBy1000 = (int)(projectile.velocity.X * 1000f);

					if (velocityXBy1000 != oldVelocityXBy1000)
					{
						projectile.netUpdate = true;
					}
				}
				else
				{
					projectile.Kill();
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.Y > 0f)
			{
				projectile.velocity.Y = -7;
				nbBounces = 0;
				if (this.baseDamage < projectile.damage)
				{
					projectile.damage = this.baseDamage;
					OrchidModProjectile.spawnDustCircle(projectile.Center, 184, 10, 10, true, 1.5f, 1f, 2f, true, true, false, 0, 0, false, true);
				}
			}
			else
			{
				projectile.velocity.Y = 1f;
			}
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
				projectile.velocity.Y = 0f;
			}

			return false;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (justHit == 0)
			{
				OrchidModProjectile.spawnDustCircle(projectile.Center, 6, 10, 10, true, 1.5f, 1f, 2f);
				projectile.damage += 6;

				nbBounces++;
				if (nbBounces > 2)
				{
					nbBounces = 0;
					OrchidModGamblerHelper.DummyProjectile(spawnGenericExplosion(projectile, projectile.damage, projectile.knockBack, 200, 3, true, 14), this.getDummy());
				}
			}

			projectile.velocity.Y = -7;
			projectile.velocity.X *= 0.5f;
			this.justHit = 30;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 7; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
			}
		}
	}
}