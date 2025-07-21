using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shapeshifter.Weapons.Predator;
using OrchidMod.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Predator
{
	public class PredatorUndineProj : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;

		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 18;
			Projectile.scale = 0.8f;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			MeleeHit = true;
		}

		public override void AI()
		{
			if (!Initialized)
			{
				Initialized = true;
				Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
				SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);

				OrchidShapeshifter shapeshifter = Owner.GetModPlayer<OrchidShapeshifter>();
				if (shapeshifter.IsShapeshifted)
				{
					if (shapeshifter.Shapeshift is PredatorUndine)
					{
						shapeshifter.ShapeshiftAnchor.ai[0] = 15;
					}
				}
			}

			if (Projectile.timeLeft % 3 == 0 && Projectile.frame < 4)
			{
				Projectile.frame++;
			}

			Projectile.friendly = Projectile.timeLeft > 10;
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1f;
				if (shapeshifter.IsShapeshifted)
				{
					if (shapeshifter.Shapeshift is PredatorUndine undine && shapeshifter.ShapeshiftAnchor.Projectile.ai[0] < 10f)
					{
						shapeshifter.ShapeshiftAnchor.Projectile.ai[0] += 0.5f;
					}
				}
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 7) colorMult *= Projectile.timeLeft / 7f;

			Rectangle rectangle = TextureMain.Bounds;
			rectangle.Height /= 5;
			rectangle.Y += rectangle.Height * Projectile.frame;

			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPosition, rectangle, Color.White * colorMult, Projectile.rotation, rectangle.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}