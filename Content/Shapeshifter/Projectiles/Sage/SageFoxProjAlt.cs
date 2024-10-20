using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shapeshifter.Weapons.Sage;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Sage
{
	public class SageFoxProjAlt : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public int TimeSpent;

		public override void AltSetDefaults()
		{
			Projectile.width = 5;
			Projectile.height = 5;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void AI()
		{
			Player owner = Owner;
			OrchidShapeshifter shapeshifter = owner.GetModPlayer<OrchidShapeshifter>();

			if (shapeshifter.Shapeshift != null && shapeshifter.Shapeshift is SageFox sageFoxItem)
			{
				TimeSpent ++;
				if (TimeSpent % 120 == 0)
				{
					Vector2 offsetnew = Vector2.UnitY.RotatedByRandom(3.14f) * Main.rand.NextFloat(24f, 32f);
					Projectile.ai[0] = offsetnew.X;
					Projectile.ai[1] = offsetnew.Y;
				}

				Vector2 offset = new Vector2(Projectile.ai[0], Projectile.ai[1]);
				if (owner.Distance(Projectile.Center + offset) > 32f)
				{
					Vector2 newVelocity = (owner.Center + offset - Projectile.Center) * 0.01f;
					Projectile.velocity = Projectile.velocity * 0.8f + newVelocity;
				}
				else
				{
					Projectile.velocity *= 0.8f;
				}
			}
			else
			{
				Projectile.Kill();
			}


			Projectile.timeLeft++;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 pos = OldPosition[i];
				pos.Y -= 3f;
				OldPosition[i] = pos;
			}

			OldPosition.Add(Projectile.Center + new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-3f, 3f)));
			OldRotation.Add(Projectile.rotation + Main.rand.NextFloat(MathHelper.Pi));

			if (OldPosition.Count > 5)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Main.rand.NextBool(10))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(0.8f, 1.2f));
				dust.noGravity = true;
				dust.noLight = true;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i ++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(0.8f, 1.2f));
				dust.noGravity = true;
				dust.noLight = true;
				dust.velocity *= 3f;
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 15) colorMult *= Projectile.timeLeft / 15f;
			Color color = new Color(180, 234, 237); // to 54 150 248

			for (int i = 0; i < OldPosition.Count; i++)
			{
				color.R -= 18;
				color.G -= 12;
				color.B -= 2;
				Vector2 drawPosition = Vector2.Transform(OldPosition[i] - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.18f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.12f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}