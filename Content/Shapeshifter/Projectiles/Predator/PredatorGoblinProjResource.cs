using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shapeshifter.Weapons.Predator;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Predator
{
	public class PredatorGoblinProjResource : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;

		public override void SafeSetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 300;
			Projectile.scale = 0.5f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 5;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override void AI()
		{
			Player target = Owner;
			if (Projectile.ai[1] > 0)
			{
				Projectile.scale = 0.75f;

				if (Main.rand.NextBool(3))
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame);
					dust.scale *= Main.rand.NextFloat(0.4f, 0.6f);
					dust.velocity *= Main.rand.NextFloat(0.2f, 0.3f);
				}
			}

			if (!Owner.dead)
			{
				Projectile.localAI[0]++;
				Projectile.velocity += (target.Center - Projectile.Center) * (0.003f + Projectile.localAI[0] * 0.0001f) * (Projectile.ai[1] > 0 ? 0.2f : 1f);
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * (Projectile.ai[1] > 0 ? 1.3f : 2.4f);

				if (Projectile.Distance(target.Center) < 16f)
				{
					Projectile.Kill();
				}
			}
			else
			{
				Projectile.Kill();
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			OldPosition.Add(Projectile.Center);
			if (OldPosition.Count > (Projectile.ai[1] > 0 ? 100 : 30))
			{
				OldPosition.RemoveAt(0);
			}
		}

		public override void OnKill(int timeLeft)
		{
			OrchidShapeshifter shapeshifter = Owner.GetModPlayer<OrchidShapeshifter>();
			if (shapeshifter.IsShapeshifted)
			{
				if (shapeshifter.Shapeshift is PredatorGoblin)
				{
					if (shapeshifter.ShapeshiftAnchor.Projectile.ai[2] < 100f)
					{
						shapeshifter.ShapeshiftAnchor.Projectile.ai[2] += Projectile.ai[0];

						SoundStyle soundStyle = SoundID.Item103;
						if (shapeshifter.ShapeshiftAnchor.Projectile.ai[2] > 100f)
						{
							soundStyle.Pitch -= 0.5f;
							shapeshifter.ShapeshiftAnchor.Projectile.ai[2] = 100f;

							for (int i = 0; i < 10; i++)
							{
								Dust dust = Dust.NewDustDirect(Owner.Center, 0, 0, DustID.Shadowflame);
								dust.scale *= Main.rand.NextFloat(1f, 1.5f);
								dust.velocity *= Main.rand.NextFloat(0.5f, 0.75f);
							}
						}
						else
						{
							soundStyle.Pitch += Main.rand.NextFloat(0.5f, 1f);
							soundStyle.Volume *= 0.25f;
						}

						for (int i = 0; i < 3; i++)
						{
							Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Shadowflame);
							dust.scale *= Main.rand.NextFloat(1f, 1.5f);
							dust.velocity *= Main.rand.NextFloat(0.25f, 0.5f);
						}

						SoundEngine.PlaySound(soundStyle, Owner.Center);
					}
				}
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 20) colorMult *= Projectile.timeLeft / 20f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition2 = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition2, null, Color.White * (Projectile.ai[1] > 0 ? 0.01f : 0.03f) * (i + 1) * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale * 0.8f, SpriteEffects.None, 0f);
			}

			Vector2 drawPosition = Projectile.Center - Main.screenPosition;

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);

			return false;
		}
	}
}