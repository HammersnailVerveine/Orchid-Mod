using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Warhammers
{
	public class MagnetosphereWarhammerProj : OrchidModGuardianProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		int TimeSpent = 0;

		public override void AltSetDefaults()
		{
			Projectile.width = 150;
			Projectile.height = 150;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 20;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
		}

		public override void AI()
        {
			TimeSpent++;
			if (Projectile.ai[0] == 1) Projectile.timeLeft--;
        }

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here
			float colorMult = 1f;
			if (Projectile.ai[0] == 1) colorMult = 0.6f;
			if (Projectile.timeLeft < 10) colorMult *= Projectile.timeLeft / 10f;
			Vector2 drawPosition = Vector2.Transform(Projectile.Center - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
			spriteBatch.Draw(TextureMain, drawPosition, null, new Color(174, 216, 112) * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale * TimeSpent * 0.22f, SpriteEffects.None, 0f);
			spriteBatch.Draw(TextureMain, drawPosition, null, new Color(78, 177, 144) * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale * TimeSpent * 0.18f, SpriteEffects.None, 0f);

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}