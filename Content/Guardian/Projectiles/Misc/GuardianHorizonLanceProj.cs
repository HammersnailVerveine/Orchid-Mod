using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Misc
{
	public class GuardianHorizonLanceProj : OrchidModGuardianProjectile
	{
		public int TimeSpent = 0;
		public int HitCount = 0;
		private static Texture2D TextureMain;
		private static Texture2D TextureBlur;
		public List<Vector2> Positions;
		public List<int> HitNPCs;

		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 900;
			Projectile.scale = 1f;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureBlur ??= ModContent.Request<Texture2D>(Texture + "_Blur", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Positions = new List<Vector2>();
			HitNPCs = new List<int>();
			Initialized = false;
			Strong = true;
		}

		public override void AI()
		{
			if (Projectile.velocity.Length() > 0)
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.velocity *= 0f;
				Initialized = true;
			}

			if (Initialized)
			{
				TimeSpent++;

				if (TimeSpent <= 30)
				{
					Positions.Add(Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * 14f * Positions.Count);
					Positions.Add(Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * 14f * Positions.Count);
				}

				if (TimeSpent % 30 == 0 && HitNPCs.Count > 0)
				{
					if (HitCount < 5)
					{
						HitCount++;
					}
					HitNPCs.Clear();
				}
			}

			if (Projectile.ai[1] != 0 && Projectile.timeLeft > 120)
			{ // reduces projectile lifespan if another one is spawned
				Projectile.timeLeft = 30;
				Projectile.netUpdate = true;
			}

			if (IsLocalOwner)
			{
				foreach (NPC npc in Main.npc)
				{
					if (IsValidTarget(npc) && !HitNPCs.Contains(npc.whoAmI))
					{
						foreach (Vector2 pos in Positions)
						{
							if (npc.Hitbox.Contains(new Point((int)pos.X, (int)pos.Y)))
							{
								HitNPCs.Add(npc.whoAmI);
								Owner.ApplyDamageToNPC(npc, (int)(Projectile.damage * (1f - HitCount * 0.1f)), 0f, 1, Main.rand.Next(100) < Projectile.CritChance, ModContent.GetInstance<GuardianDamageClass>());
								break;
							}
						}
					}
				}
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			//spriteBatch.Begin(spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });
			//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			//GameShaders.Misc["OrchidMod:HorizonGlow"].Apply();

			if (!Initialized) return false;

			float colorMult = 1f;
			if (Projectile.timeLeft < 20) colorMult *= Projectile.timeLeft / 20f;
			Color color = new Color(216, 61, 5); // to (112, 152, 255);
			for (int i = 0; i < Positions.Count; i++)
			{
				if (i > 5 && i < 55)
				{
					color.R -= 2;
					color.G += 2;
					color.B += 5;
				}

				Rectangle rectangle = TextureMain.Bounds;
				Vector2 drawPosition = Positions[i] - Main.screenPosition;
				Color drawcolor = (Color.White * ((float)Math.Sin((TimeSpent + i) * 0.33f) * 0.1f + 0.9f)).MultiplyRGB(color) * colorMult;

				if (i < 9)
				{
					rectangle.Width -= 2 * (9 - i);
					rectangle.X += (9 - i);
					drawPosition += Vector2.UnitX.RotatedBy(Projectile.rotation) * (9 - i);
				}
				else if (i > Positions.Count - 9)
				{
					rectangle.Width += 2 * (Positions.Count - (i + 1) - 8);
					rectangle.X -= (Positions.Count - (i + 1) - 8);
					drawPosition -= Vector2.UnitX.RotatedBy(Projectile.rotation) * (Positions.Count - (i + 1) - 8);
				}

				drawPosition -= Vector2.UnitX.RotatedBy(Projectile.rotation) * (float)Math.Sin((TimeSpent + i * 5) * 0.1) * 2.5f;

				if (i > 10 && i < 50)
				{
					float blurscale = (float)Math.Sin(MathHelper.Pi / 60f * i) * 1.5f;
					spriteBatch.Draw(TextureBlur, drawPosition, null, drawcolor * 0.8f, Projectile.rotation, TextureBlur.Size() * 0.5f, Projectile.scale * blurscale, SpriteEffects.None, 0f);
				}

				spriteBatch.Draw(TextureMain, drawPosition, rectangle, Color.DarkGray.MultiplyRGB(drawcolor), Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);

				rectangle.Width -= 4;
				rectangle.X -= 2;
				drawPosition += Vector2.UnitX.RotatedBy(Projectile.rotation) * 2;

				spriteBatch.Draw(TextureMain, drawPosition, rectangle, drawcolor, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}