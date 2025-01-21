using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Global.Projectiles;
using OrchidMod.Common.ModObjects;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Gambler.Projectiles
{
	public class KingSlimeCardProj : OrchidModGamblerProjectile
	{
		private int baseDamage = 0;
		private int justHit = 0;
		private int velocityStuck = 0;
		private float oldPositionY = 0f;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Slime");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 22;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.alpha = 64;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SafeAI()
		{

			if (!this.initialized)
			{
				this.baseDamage = Projectile.damage;
				this.initialized = true;
				Projectile.ai[1] = 1f;
			}

			Player player = Main.player[Projectile.owner];
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			int cardType = Projectile.GetGlobalProjectile<OrchidGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			if (Projectile.ai[1] == 2f && Projectile.timeLeft % 10 == 0 && Projectile.velocity.Y > 0f)
			{
				Projectile.damage++;
			}

			if (Projectile.ai[1] == 0f || Projectile.ai[1] == 2f)
			{
				Projectile.velocity.Y += (Projectile.wet || Projectile.lavaWet || Projectile.honeyWet) ? Projectile.velocity.Y > -7.5f ? -0.5f : 0f : Projectile.velocity.Y < 7.5f ? 0.4f : 0f;
			}

			Projectile.frame = Projectile.velocity.Y < 0f ? 1 : 0;
			this.justHit -= this.justHit > 0 ? 1 : 0;

			this.velocityStuck = Projectile.Center.Y == oldPositionY ? this.velocityStuck + 1 : 0;
			this.oldPositionY = 0f + Projectile.Center.Y;

			if (Projectile.velocity.X > 6f)
			{
				Projectile.velocity.X = 6f;
			}
			if (Projectile.velocity.X < -6f)
			{
				Projectile.velocity.X = -6f;
			}

			if (Main.myPlayer == Projectile.owner)
			{
				if (velocityStuck >= 5)
				{
					Projectile.velocity.Y = -5;
					this.velocityStuck = 0;
				}
				if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.KingSlimeCard>() && modPlayer.GamblerDeckInHand)
				{
					Vector2 newMove = new Vector2(Main.screenPosition.X + (float)Main.mouseX, Projectile.Center.Y) - Projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo > 5f)
					{
						if ((float)(Main.screenPosition.X + Main.mouseX) > Projectile.Center.X)
						{
							Projectile.velocity.X += Projectile.velocity.X < 8f ? this.justHit > 0 ? 0.25f : 0.35f : 0f;
						}
						else
						{
							Projectile.velocity.X -= Projectile.velocity.X > -8f ? this.justHit > 0 ? 0.25f : 0.35f : 0f;
						}
					}
					else
					{
						if (Projectile.velocity.Length() > 0.01f)
						{
							Projectile.velocity.X *= 0.8f;
						}
					}

					if (Projectile.ai[1] == 1f)
					{
						Projectile.velocity.Y = -10f;
						if (Projectile.Center.Y - 50f < (Main.screenPosition.Y + (float)Main.mouseY))
						{
							Projectile.ai[1] = 2f;
							Projectile.netUpdate = true;
						}
					}

					bool fallThrough = Main.screenPosition.Y + Main.mouseY > Projectile.Center.Y;
					if (Projectile.ai[0] == 0f && fallThrough) {
						Projectile.ai[0] = 1f;
						Projectile.netUpdate = true;
					} else if (Projectile.ai[0] == 1f && !fallThrough) {
						Projectile.ai[0] = 0f;
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
			fallThrough = Projectile.ai[0] == 1f;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.Y >= 0f)
			{
				Projectile.velocity.Y = -10;
				Projectile.ai[1] = 1f;
				if (this.baseDamage < Projectile.damage)
				{
					if (this.baseDamage < Projectile.damage - 6)
					{
						OrchidModProjectile.spawnDustCircle(Projectile.Center, 60, 10, 10, true, 1.5f, 1f, 2f, true, true, false, 0, 0, false, true);
					}
					Projectile.damage = this.baseDamage;
				}
			}
			else
			{
				Projectile.velocity.Y = 1f;
				Projectile.ai[1] = 0f;
			}
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
				Projectile.velocity.Y = 0f;
			}
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = ModContent.Request<Texture2D>("OrchidMod/Content/Gambler/Projectiles/KingSlimeCardProj_Glow").Value;
			OrchidModProjectile.DrawProjectileGlowmask(Projectile, Main.spriteBatch, texture, Color.White);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidGambler modPlayer)
		{
			if (justHit == 0)
			{
				Projectile.damage += 2;
				bool dummy = Projectile.GetGlobalProjectile<OrchidGlobalProjectile>().gamblerDummyProj;
				int projType = ProjectileType<Content.Gambler.Projectiles.KingSlimeCardProj2>();
				DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, projType, Projectile.damage, Projectile.knockBack, Projectile.owner), dummy);
				OrchidModProjectile.spawnDustCircle(Projectile.Center, 59, 10, 10, true, 1.5f, 1f, 2f, true, true, false, 0, 0, false, true);
			}

			Projectile.velocity.Y = -10f;
			Projectile.velocity.X *= 0.5f;
			this.justHit = 30;
			Projectile.ai[1] = 1f;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 7; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 4, 0.0f, 0.0f, 175, new Color(0, 80, 255, 0));
			}
		}

		public override void SafePostAI()
		{
			SlimePostAITrail();
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			SlimePreDrawTrail(spriteBatch, lightColor);
			return true;
		}

		public void SlimePreDrawTrail(SpriteBatch spriteBatch, Color lightColor)
		{
			float offSet = this.projectileTrailOffset + 0.5f;
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * offSet, Projectile.height * offSet);
			Texture2D texture = ModContent.Request<Texture2D>("OrchidMod/Content/Gambler/Projectiles/KingSlimeCardProj2").Value;
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0.3f);
			}
		}

		public void SlimePostAITrail()
		{
			for (int num46 = Projectile.oldPos.Length - 1; num46 > 0; num46--)
			{
				Projectile.oldPos[num46] = Projectile.oldPos[num46 - 1];
			}
			Projectile.oldPos[0] = Projectile.position;
		}
	}
}
