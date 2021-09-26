using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class SapCardProj : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.Homing[projectile.type] = true;
			Main.projFrames[projectile.type] = 8;

			DisplayName.SetDefault("Sap Bubble");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.alpha = 50;
			projectile.timeLeft = 1200;
			projectile.penetrate = -1;

			this.gamblingChipChance = 5;
		}

		public override void SafeAI()
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;

			projectile.rotation = (float)Math.Sin(projectile.timeLeft * 0.045f) * 0.25f;
			projectile.scale = 1 + (float)Math.Cos(projectile.timeLeft * 0.05f) * 0.1f;

			if (projectile.timeLeft > 120) // ???
			{
				projectile.velocity.Y += 0.01f;
				projectile.velocity.X *= 0.95f;
			}

			if (Main.rand.Next(12) == 0)
			{
				var dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 102)];
				dust.alpha = 75;
			}

			if (Main.myPlayer == projectile.owner)
			{
				if (Main.player[Main.myPlayer].channel && cardType == ItemType<Gambler.Weapons.Cards.SapCard>() && modPlayer.GamblerDeckInHand)
				{
					Vector2 newMove = Main.MouseWorld - projectile.Center;

					int oldVelocityYBy1000 = (int)(projectile.velocity.Y * 1000f);
					int oldVelocityXBy1000 = (int)(projectile.velocity.X * 1000f);

					if (newMove.Length() < 5f)
					{
						projectile.velocity *= 0f;
						newMove *= 0f;
					}
					else
					{
						AdjustMagnitude(ref newMove);
						projectile.velocity = (5 * projectile.velocity + newMove);
						AdjustMagnitude(ref projectile.velocity);
					}

					int velocityXBy1000 = (int)(newMove.X * 1000f);
					int velocityYBy1000 = (int)(newMove.Y * 1000f);

					if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000)
					{
						projectile.netUpdate = true;
					}
				}
				else
				{
					projectile.Kill();
				}
			}

			if (++projectile.frameCounter >= 5)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 8)
				{
					projectile.frame = 0;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 85);

			OrchidHelper.SpawnDustCircle(
				center: projectile.Center,
				radius: 20,
				count: 10,
				type: (index) => 102,
				onSpawn: (dust, index, angleFromCenter) =>
				{
					dust.alpha = 100;
					dust.velocity = new Vector2(Main.rand.NextFloat(1, 2.5f), 0).RotatedBy(angleFromCenter);
				}
			);

			int dmg = projectile.damage + (int)((1200 - timeLeft) / 10);
			int projType = ProjectileType<Gambler.Projectiles.SapCardProjExplosion>();
			bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;

			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, projType, dmg, 3f, projectile.owner, 0.0f, 0.0f), dummy);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.gamblerElementalLens)
			{
				target.AddBuff(BuffID.Poisoned, 60 * 5);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (projectile.spriteDirection == -1) spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture = Main.projectileTexture[projectile.type];
			int frameHeight = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
			int startY = frameHeight * projectile.frame;

			Vector2 position = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
			Vector2 origin = sourceRectangle.Size() / 2f;
			Color mainColor = projectile.GetAlpha(lightColor);

			Main.spriteBatch.Draw(texture, position, sourceRectangle, mainColor, projectile.rotation, origin, projectile.scale, spriteEffects, 0f);

			return false;
		}

		// ...

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 2f)
			{
				vector *= 2f / magnitude;
			}
		}
	}
}