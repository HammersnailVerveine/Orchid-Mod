using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class SlimeRainCardProj1 : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Slime Cloud");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 54;
			Projectile.height = 28;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.alpha = 126;
			Projectile.penetrate = -1;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 3;
		}

		public override void SafeAI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			int cardType = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;

			if (modPlayer.modPlayer.timer120 % 20 == 0)
			{
				Projectile.frame += Projectile.frame + 1 == 3 ? -2 : 1;
			}

			if (modPlayer.modPlayer.timer120 % 30 == 0)
			{
				int projType = ProjectileType<Gambler.Projectiles.SlimeRainCardProj2>();
				bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
				DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + Main.rand.Next(Projectile.width - 10) + 5, Projectile.Center.Y, 0f, 5f, projType, Projectile.damage, Projectile.knockBack, Projectile.owner), dummy);
			}

			if (Main.rand.Next(15) == 0)
			{
				int Alpha = 175;
				Color newColor = new Color(0, 80, 255, 0);
				int dust = Dust.NewDust(Projectile.position + Vector2.One * 6f, Projectile.width, Projectile.height, 4, 0.0f, 0.0f, Alpha, newColor, 1.2f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1.7f;
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noLight = true;
			}

			if (Main.myPlayer == Projectile.owner && !this.initialized)
			{
				if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.SlimeRainCard>() && modPlayer.GamblerDeckInHand)
				{
					Vector2 newMove = Main.MouseWorld - Projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo > 5f)
					{
						newMove *= 8f / distanceTo;
						Projectile.velocity = newMove;
					}
					else
					{
						if (Projectile.velocity.Length() > 0f)
						{
							Projectile.velocity *= 0f;
						}
					}

					int velocityXBy1000 = (int)(newMove.X * 1000f);
					int oldVelocityXBy1000 = (int)(Projectile.velocity.X * 1000f);
					int velocityYBy1000 = (int)(newMove.Y * 1000f);
					int oldVelocityYBy1000 = (int)(Projectile.velocity.Y * 1000f);

					if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000)
					{
						Projectile.netUpdate = true;
					}
				}
				else
				{
					Projectile.velocity *= 0f;
					Projectile.timeLeft = this.initialized ? Projectile.timeLeft : 600;
					this.initialized = true;
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			return false;
		}
	}
}