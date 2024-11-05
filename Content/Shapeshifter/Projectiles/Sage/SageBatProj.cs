using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shapeshifter.Buffs.Debuffs;
using OrchidMod.Content.Shapeshifter.Weapons.Sage;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Sage
{
	public class SageBatProj : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<Vector2> OldPosition2;
		public List<NPC> HitNPCs;
		public Color DrawColor;
		Vector2 BaseVelocity = Vector2.Zero;

		public override void SafeSetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 180;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldPosition2 = new List<Vector2>();
			HitNPCs = new List<NPC>();
			DrawColor = new Color(109, 248, 186);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}

		public override void AI()
		{

			if (Projectile.ai[0] > 0)
			{
				Projectile.ai[0]--;
				Projectile.friendly = false;

				if (Projectile.ai[0] <= 0)
				{
					Projectile.Kill();
				}

				if (OldPosition2.Count > 0)
				{
					OldPosition2.RemoveAt(0);
				}

				if (OldPosition.Count > 0)
				{
					OldPosition.RemoveAt(0);
				}
			}
			else
			{
				if (Projectile.ai[1] > 0)
				{// Color shift from (109, 248, 186) to (255, 182, 0) over 20 steps
					if (Projectile.ai[1] <= 20)
					{
						Projectile.tileCollide = false;
						DrawColor.R += 7;
						DrawColor.G -= 3;
						DrawColor.B -= 9;
						Projectile.ai[1]++;
					}

					if (Projectile.Center.Distance(Owner.Center) < 24f)
					{
						foreach(NPC target in HitNPCs)
						{
							if (target.active)
							{
								target.AddBuff(ModContent.BuffType<SageBatDebuff>(), 600);
							}
						}

						Projectile.Kill();
					}
				}

				if (BaseVelocity == Vector2.Zero)
				{
					BaseVelocity = Projectile.velocity;
				}

				if (Projectile.timeLeft <= 120 && Projectile.timeLeft > 90)
				{
					Projectile.velocity -= BaseVelocity / 15f;
					Projectile.tileCollide = false;
				}

				Projectile.rotation = Projectile.velocity.ToRotation();

				OldPosition.Add(Projectile.Center);

				if (OldPosition.Count > 10)
				{
					OldPosition.RemoveAt(0);
				}

				if (Projectile.timeLeft % 5 == 0)
				{
					OldPosition2.Add(Projectile.Center);

					if (OldPosition2.Count > 5)
					{
						OldPosition2.RemoveAt(0);
					}
				}
			}
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			if (Projectile.ai[1] == 0)
			{
				Projectile.ai[1] = 1;
				Projectile.netUpdate = true;
			}

			if (!HitNPCs.Contains(target))
			{
				HitNPCs.Add(target);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.ai[0] = 30;
			Projectile.netUpdate = true;
			Projectile.velocity *= 0f;
			return false;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			float scaleMult = 1.25f + (float)Math.Sin(Projectile.timeLeft * 0.15f) * 0.25f;
			if (Projectile.timeLeft < 7) colorMult *= Projectile.timeLeft / 7f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition2 = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition2, null, DrawColor * 0.08f * (i + 1) * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale * scaleMult * ((i + 1) * 0.05f + 0.5f), SpriteEffects.None, 0f);
			}

			for (int i = 0; i < OldPosition2.Count; i++)
			{
				Vector2 drawPosition2 = OldPosition2[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition2, null, DrawColor * 0.175f * (i + 1) * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale * scaleMult * (i + 1) * 0.2f, SpriteEffects.None, 0f);
			}

			if (Projectile.ai[0] <= 0)
			{
				Vector2 drawPosition = Projectile.Center - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition, null, DrawColor * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale * scaleMult * 1.1f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);


			return false;
		}
	}
}