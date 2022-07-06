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
			Projectile.width = 22;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.alpha = 16;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 2;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void SafeAI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			int cardType = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			Projectile.velocity.Y += (Projectile.wet || Projectile.lavaWet || Projectile.honeyWet) ? Projectile.velocity.Y > -5f ? -0.5f : 0f : Projectile.velocity.Y < 5f ? 0.3f : 0f;
			Projectile.frame = Projectile.velocity.Y < 0f ? 1 : 0;
			this.justHit -= this.justHit > 0 ? 1 : 0;

			if (Main.rand.NextBool(15))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
			}

			this.velocityStuck = Projectile.Center.Y == oldPositionY ? this.velocityStuck + 1 : 0;
			this.oldPositionY = 0f + Projectile.Center.Y;

			if (Projectile.velocity.X > 4f)
			{
				Projectile.velocity.X = 4f;
			}
			if (Projectile.velocity.X < -4f)
			{
				Projectile.velocity.X = -4f;
			}

			if (!this.initialized)
			{
				this.baseDamage = Projectile.damage;
				this.initialized = true;
			}

			if (Main.myPlayer == Projectile.owner)
			{
				if (velocityStuck >= 5)
				{
					Projectile.velocity.Y = -5;
					this.velocityStuck = 0;
				}
				if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.LavaSlimeCard>() && modPlayer.GamblerDeckInHand)
				{
					Vector2 newMove = new Vector2(Main.screenPosition.X + (float)Main.mouseX, (float)Projectile.Center.Y) - Projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo > 5f)
					{
						if ((float)(Main.screenPosition.X + Main.mouseX) > Projectile.Center.X)
						{
							Projectile.velocity.X += Projectile.velocity.X < 6f ? this.justHit > 0 ? 0.15f : 0.25f : 0f;
						}
						else
						{
							Projectile.velocity.X -= Projectile.velocity.X > -6f ? this.justHit > 0 ? 0.15f : 0.25f : 0f;
						}
					}
					else
					{
						if (Projectile.velocity.Length() > 0.01f)
						{
							Projectile.velocity.X *= 0.9f;
						}
					}

					bool fallThrough = Main.screenPosition.Y + Main.mouseY > Projectile.Center.Y;
					if (Projectile.ai[1] == 0f && fallThrough) {
						Projectile.ai[1] = 1f;
						Projectile.netUpdate = true;
					} else if (Projectile.ai[1] == 1f && !fallThrough) {
						Projectile.ai[1] = 0f;
						Projectile.netUpdate = true;
					}

					int velocityXBy1000 = (int)(newMove.X * 1000f);
					int oldVelocityXBy1000 = (int)(Projectile.velocity.X * 1000f);

					if (velocityXBy1000 != oldVelocityXBy1000)
					{
						Projectile.netUpdate = true;
					}
				}
				else
				{
					Projectile.Kill();
				}
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = Projectile.ai[1] == 1f;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.Y >= 0f)
			{
				Projectile.velocity.Y = -7;
				nbBounces = 0;
				if (this.baseDamage < Projectile.damage)
				{
					Projectile.damage = this.baseDamage;
					OrchidModProjectile.spawnDustCircle(Projectile.Center, 184, 10, 10, true, 1.5f, 1f, 2f, true, true, false, 0, 0, false, true);
				}
			}
			else
			{
				Projectile.velocity.Y = 1f;
			}
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
				Projectile.velocity.Y = 0f;
			}

			return false;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidGambler modPlayer)
		{
			if (justHit == 0)
			{
				OrchidModProjectile.spawnDustCircle(Projectile.Center, 6, 10, 10, true, 1.5f, 1f, 2f);
				Projectile.damage += 6;

				nbBounces++;
				if (nbBounces > 2)
				{
					nbBounces = 0;
					DummyProjectile(spawnGenericExplosion(Projectile, Projectile.damage, Projectile.knockBack, 200, 3, true, 14), this.getDummy());
				}
			}

			Projectile.velocity.Y = -7;
			Projectile.velocity.X *= 0.5f;
			this.justHit = 30;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 7; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
			}
		}
	}
}