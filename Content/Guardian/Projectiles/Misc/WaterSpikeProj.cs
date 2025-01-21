using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Misc
{
	public class WaterSpikeProj : OrchidModGuardianProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 65;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void AI()
		{
			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			Projectile.rotation = Projectile.velocity.ToRotation();

			if (OldPosition.Count > 10)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here

			float colorMult = 1f;
			if (Projectile.timeLeft < 10) colorMult *= Projectile.timeLeft / 10f;
			Color color = new Color(21, 50, 249);

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.1f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.08f, SpriteEffects.None, 0f);
			}

			if (Main.rand.NextBool(5))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.DungeonWater, Scale: Main.rand.NextFloat(0.8f, 1.2f));
				dust.velocity = dust.velocity * 0.5f + Projectile.velocity * 0.5f;
			}
			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}