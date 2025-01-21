using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shaman.Misc;
using OrchidMod.Content.Shaman.Projectiles;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Projectiles.Air
{
	public class FeatherScepterProjectile : OrchidModShamanProjectile
	{
		private bool rapidFade = false;

		public override void SafeSetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 200;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			projectileTrail = true;
			Projectile.alpha = 255;
		}

		public override void SafeAI()
		{
			Projectile.velocity = Projectile.velocity * 0.95f;

			if (TimeSpent < 10) Projectile.alpha -= 25;

			if (Projectile.timeLeft < 85)
				Projectile.alpha += 3;

			if (Projectile.timeLeft == 130)
			{
				Projectile.damage += 5;
				int type = ModContent.ProjectileType<FeatherScepterProjectileSplash>();
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, type, 0, 0f, Projectile.owner);
			}

			if (Projectile.timeLeft < 150)
			{
				Projectile.rotation += Projectile.ai[0];
				if (Math.Abs(Projectile.ai[0]) < 0.8f) Projectile.ai[0] += 0.01f;
				if (Projectile.timeLeft > 130) Projectile.height++;
			}
			else
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
			}

			if (rapidFade)
				Projectile.alpha += 3;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity.X = 0f;
			Projectile.velocity.Y = 0f;
			Projectile.timeLeft /= 2;
			rapidFade = true;
			return false;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}

	public class FeatherScepterProjectileSplash : OrchidModShamanProjectile
	{
		private static Texture2D TextureMain;

		public override void SafeSetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 13;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		}

		public override void SafeAI()
		{
			if (TimeSpent == 0) Projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
		}


		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here
			float colorMult = 1f;
			if (Projectile.timeLeft < 5) colorMult *= Projectile.timeLeft / 5f;
			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPosition, null, Color.White * colorMult * 0.65f, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale * TimeSpent * 0.22f, SpriteEffects.None, 0f);

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}
