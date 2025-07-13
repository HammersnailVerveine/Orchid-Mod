using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Projectiles.Warden;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Misc
{
	public class ShapeshifterAshwoodFlame : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		private static Texture2D TextureAlt;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = false;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.scale = 0.6f;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureAlt ??= ModContent.Request<Texture2D>(Texture + "_Alt", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>(); 
		}

		public override void AI()
		{
			if (Projectile.ai[2] == 0f)
			{ // random rotation & scale (init)
				Projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
				Projectile.localAI[1] = Main.rand.NextFloat(0.5f, 1.5f);
				Projectile.localAI[2] = Main.rand.NextFloat(0.9f, 1.1f);
				SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, Projectile.Center);
			}

			Projectile.ai[2]++; // time spent alive

			if (OldPosition.Count > 6)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}
			else
			{
				OldPosition.Add(new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-2, 0f)));
				OldRotation.Add(0f);
			}

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 pos = OldPosition[i];
				pos.Y += Main.rand.NextFloat(2.5f);
				pos.X += Main.rand.NextFloat(-0.2f, 0.2f) ;
				OldPosition[i] = pos;
				OldRotation[i] += Owner.velocity.Length() / 200f * Math.Sign(- Owner.velocity.X);
			}

			if (Main.rand.NextBool(20))
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(6, 8), 8, 4, DustID.Smoke);
				dust.scale *= Main.rand.NextFloat(0.4f, 0.6f);
				dust.velocity.Y = Main.rand.NextFloat(-1f, -0.5f);
				dust.velocity.X *= 0.1f;
			}

			if (Main.rand.NextBool(40))
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 2), 4, 2, DustID.Smoke);
				dust.scale *= Main.rand.NextFloat(0.4f, 0.6f);
				dust.velocity *= 0.1f;
			}

			Lighting.AddLight(Projectile.Center, 0.3f, 0.1f, 0.1f);

			// Movement relative to player

			int count = 0; // nb of more recent projectiles
			int countTotal = 0; // nb of other projectiles
			float highestTimespent = 0;
			foreach (Projectile projectile in Main.projectile)
			{
				if (projectile.active && projectile.type == Type && Projectile.owner == projectile.owner)
				{
					countTotal++;

					if (projectile.ai[2] < Projectile.ai[2]) 
					{
						count++; 
					}

					if (projectile.ai[2] > highestTimespent)
					{
						highestTimespent = projectile.ai[2];
					}
				} 
			}

			Player owner = Owner;
			OrchidShapeshifter shapeshifter = owner.GetModPlayer<OrchidShapeshifter>();
			if (owner.active && !owner.dead && shapeshifter.ShapeshifterSetPyre && Projectile.timeLeft > 10 && Projectile.ai[0] == 0)
			{
				Projectile.timeLeft++;
				Vector2 targetPosition = owner.Center - Vector2.UnitY.RotatedBy(highestTimespent * 0.02f + (MathHelper.TwoPi / countTotal) * count) * (16f + Math.Max(owner.width, owner.height));
				Projectile.velocity = (targetPosition - Projectile.Center) * 0.1f + owner.velocity;

				if (Projectile.ai[1] > 0f)
				{ // detonation
					Projectile.ai[1]--;

					if (Projectile.ai[1] <= 0f)
					{
						float closestDistance = 240f;
						NPC closestTarget = null;
						foreach (NPC npc in Main.npc)
						{
							if (OrchidModProjectile.IsValidTarget(npc))
							{
								float distance = owner.Center.Distance(npc.Center);
								if (distance < closestDistance)
								{
									closestTarget = npc;
									closestDistance = distance;
								}
							}
						}

						if (closestTarget != null)
						{
							SoundStyle soundStyle = SoundID.DD2_ExplosiveTrapExplode;
							soundStyle.Volume = 0.8f;
							SoundEngine.PlaySound(soundStyle, closestTarget.Center);

							for (int i = 0; i < 8; i++)
							{
								Dust dust = Dust.NewDustDirect(closestTarget.position, Projectile.width, Projectile.height, DustID.Smoke);
								dust.scale = Main.rand.NextFloat(0.6f, 0.8f);
								dust.velocity *= Main.rand.NextFloat(0.25f, 0.75f);
							}

							for (int i = 0; i < 15; i++)
							{
								Dust dust = Dust.NewDustDirect(closestTarget.Center, 0, 0, DustID.Torch);
								dust.noGravity = true;
								dust.noLight = true;
								dust.scale = Main.rand.NextFloat(0.8f, 1.2f);
								dust.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(3f, 8f);
							}

							if (IsLocalOwner)
							{
								int projType = ModContent.ProjectileType<ShapeshifterAshwoodProj>();
								ShapeshifterNewProjectile(closestTarget.Center, Vector2.Zero, projType, Projectile.damage, Projectile.CritChance, 0f, Projectile.owner);
							}
						}
						else
						{
							SoundEngine.PlaySound(SoundID.LiquidsWaterLava, Projectile.Center);
						}

						Projectile.timeLeft = 10;
					}
				}
			}
			else if (Projectile.timeLeft > 10)
			{
				Projectile.timeLeft = 10;
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float scaleMult = 0f;
			scaleMult += Projectile.ai[2] * 0.05f;
			if (scaleMult > 1f) scaleMult = 1f;

			float colorMult = 1f;
			if (Projectile.timeLeft < 10) colorMult *= Projectile.timeLeft / 10f;
			float colorFlicker = scaleMult * Projectile.localAI[2] * (0.5f + (float)(Math.Sin(Projectile.ai[2] * 0.04851f) * 0.1f) + (float)(Math.Sin(Projectile.ai[2] * 0.123f) * 0.05f) + (float)(Math.Sin(Projectile.ai[2] * 0.31461f) * 0.02f) + (float)(Math.Sin(Projectile.ai[2] * 0.07124f) * 0.03f));
			spriteBatch.Draw(TextureAlt, Projectile.Center - Main.screenPosition, null, new Color(243, 185, 147) * colorMult, Projectile.rotation, TextureAlt.Size() * 0.5f, Projectile.scale * 2f * colorFlicker, SpriteEffects.None, 0f);
			spriteBatch.Draw(TextureAlt, Projectile.Center - Main.screenPosition, null, new Color(243, 185, 147) * colorMult, Projectile.rotation, TextureAlt.Size() * 0.5f, Projectile.scale * 1.5f * colorFlicker, SpriteEffects.None, 0f);
			colorFlicker += (float)(Math.Sin(Projectile.ai[2] * 0.24851f) * 0.1f);
			spriteBatch.Draw(TextureAlt, Projectile.Center - Main.screenPosition, null, new Color(193, 65, 47) * colorMult, Projectile.rotation + Projectile.localAI[1], TextureAlt.Size() * 0.5f, Projectile.scale * 2.6f * colorFlicker, SpriteEffects.None, 0f);

			for (int i = 0; i < OldPosition.Count - 1; i++)
			{
				Color color = new Color(193, 65, 47);
				for (int j = 0; j < i; j++)
				{
					if (j < 4)
					{
						color.R += 7;
						color.G += 10;
						color.B += 7;
					}
				}

				Vector2 drawPositionGlow = Projectile.Center - (Vector2.UnitY + OldPosition[i]).RotatedBy(OldRotation[i]) - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPositionGlow, null, color * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.15f * scaleMult, SpriteEffects.None, 0f);
				spriteBatch.Draw(TextureMain, drawPositionGlow, null, color * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.1f * scaleMult, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}