using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Shields
{
	public class HallowedShieldProj : OrchidModGuardianProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public int Timespent = 0;

		public override void AltSetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 300;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.ai[1] = Main.rand.NextBool() ? 0f : 1f;
		}

		public override void AI()
		{
			Timespent++;
			Projectile.velocity *= 0.975f;
			Projectile.rotation -= (Projectile.timeLeft + 180) / 1500f;

			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			if (OldPosition.Count > 5)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Main.rand.NextBool(2))
			{
				int type = Main.rand.Next(3);
				if (Projectile.ai[1] == 0) type = DustID.Enchanted_Gold;
				else type = DustID.Enchanted_Pink;
				int dust = Dust.NewDust(Projectile.position, 40, 40, type);
				Main.dust[dust].velocity *= Main.rand.NextFloat(1.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale *= Main.rand.NextFloat(0.5f) + 0.5f;
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here

			float colorMult = 1f;
			if (Projectile.timeLeft < 30f) colorMult *= Projectile.timeLeft / 30f;
			Color color = Projectile.ai[1] == 0f ? new Color(255, 255, 105) : new Color(255, 142, 236);

			for (int i = 0; i < OldPosition.Count; i++)
			{
				if (Projectile.ai[1] == 0f) color.B += 5;
				else color.G += 5;

				Vector2 drawPosition = Vector2.Transform(OldPosition[i] - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.15f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * 1.2f, SpriteEffects.None, 0f);
			}

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}