using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class SapCardProj : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 8;

			DisplayName.SetDefault("Sap Bubble");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.alpha = 50;
			Projectile.timeLeft = 1200;
			Projectile.penetrate = -1;

			this.gamblingChipChance = 5;
		}

		public override void SafeAI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayerGambler modPlayer = player.GetModPlayer<OrchidModPlayerGambler>();
			int cardType = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;

			Projectile.rotation = (float)Math.Sin(Projectile.timeLeft * 0.045f) * 0.25f;
			Projectile.scale = 1 + (float)Math.Cos(Projectile.timeLeft * 0.05f) * 0.1f;

			if (Projectile.timeLeft > 120) // ???
			{
				Projectile.velocity.Y += 0.01f;
				Projectile.velocity.X *= 0.95f;
			}

			if (Main.rand.NextBool(12))
			{
				var dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 102)];
				dust.alpha = 75;
			}

			if (Main.myPlayer == Projectile.owner)
			{
				if (Main.player[Main.myPlayer].channel && cardType == ItemType<Gambler.Weapons.Cards.SapCard>() && modPlayer.GamblerDeckInHand)
				{
					Vector2 newMove = Main.MouseWorld - Projectile.Center;

					int oldVelocityYBy1000 = (int)(Projectile.velocity.Y * 1000f);
					int oldVelocityXBy1000 = (int)(Projectile.velocity.X * 1000f);

					if (newMove.Length() < 5f)
					{
						Projectile.velocity *= 0f;
						newMove *= 0f;
					}
					else
					{
						AdjustMagnitude(ref newMove);
						Projectile.velocity = (5 * Projectile.velocity + newMove);
						AdjustMagnitude(ref Projectile.velocity);
					}

					int velocityXBy1000 = (int)(newMove.X * 1000f);
					int velocityYBy1000 = (int)(newMove.Y * 1000f);

					if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000)
					{
						Projectile.netUpdate = true;
					}
				}
				else
				{
					Projectile.Kill();
				}
			}

			if (++Projectile.frameCounter >= 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 8)
				{
					Projectile.frame = 0;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item85, Projectile.Center);

			DustUtils.SpawnDustCircle(
				center: Projectile.Center,
				radius: 20,
				count: 10,
				type: (index) => 102,
				onSpawn: (dust, index, angleFromCenter) =>
				{
					dust.alpha = 100;
					dust.velocity = new Vector2(Main.rand.NextFloat(1, 2.5f), 0).RotatedBy(angleFromCenter);
				}
			);

			int dmg = Projectile.damage + (int)((1200 - timeLeft) / 10);
			int projType = ProjectileType<Gambler.Projectiles.SapCardProjExplosion>();
			bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;

			DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, projType, dmg, 3f, Projectile.owner, 0.0f, 0.0f), dummy);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerGambler modPlayer)
		{
			if (modPlayer.gamblerElementalLens)
			{
				target.AddBuff(BuffID.Poisoned, 60 * 5);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1) spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
			int startY = frameHeight * Projectile.frame;

			Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
			Vector2 origin = sourceRectangle.Size() / 2f;
			Color mainColor = Projectile.GetAlpha(lightColor);

			Main.spriteBatch.Draw(texture, position, sourceRectangle, mainColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0f);

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