using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shapeshifter.Buffs.Debuffs;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities.Terraria.Utilities;

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
			Projectile.timeLeft = 240;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldPosition2 = new List<Vector2>();
			HitNPCs = new List<NPC>();
			DrawColor = Color.White;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}

		public override void AI()
		{
			if (DrawColor == Color.White) 
			{
				if (Projectile.ai[2] == 0f)
				{ // normal bat
					DrawColor = new Color(109, 248, 186);
				}
				else
				{ // hell bat
					DrawColor = new Color(255, 102, 20);
				}
			}


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
				{
					if (Projectile.ai[1] <= 20)
					{
						Projectile.tileCollide = false;
						Projectile.ai[1]++;
						if (Projectile.ai[2] == 0f)
						{ // normal bat - Color shift from (109, 248, 186) to (255, 182, 0) over 20 steps
							DrawColor.R += 7;
							DrawColor.G -= 3;
							DrawColor.B -= 9;
						}
						else
						{ // hell bat - Color shift from (255, 102, 20) to (255, 182, 80) over 20 steps
							Projectile.tileCollide = false;
							DrawColor.G += 4;
							DrawColor.B += 3;
						}
					}

					Player owner = Owner;
					float distance = Projectile.Center.Distance(owner.Center);
					float homingDistance = Projectile.ai[2] == 0f ? 80f : 120f;
					if (distance < homingDistance && Projectile.timeLeft < 150)
					{ // Homes towards owner to help pick up
						Projectile.velocity += (Owner.Center - Projectile.Center) * 0.05f;
						Projectile.velocity = Vector2.Normalize(Projectile.velocity) * BaseVelocity.Length();

						if (Projectile.timeLeft < 10)
						{
							Projectile.timeLeft = 10;
						}
					}

					if (distance < 24f && Projectile.timeLeft < 180)
					{ // Kills the projectile if caught by the player, and applies the debuff to hit enemies
						foreach(NPC target in HitNPCs)
						{
							if (target.active)
							{
								target.AddBuff(ModContent.BuffType<SageBatDebuff>(), 600);

								for (int i = 0; i < 5; i++)
								{
									Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.SandSpray);
									dust.noGravity = true;
									dust.scale = Main.rand.NextFloat(1.25f, 1.75f);
								}
							}
						}

						Projectile.Kill();
						SoundEngine.PlaySound(SoundID.Item4, Projectile.Center);

						for (int i = 0; i < 10; i++)
						{
							Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SandSpray);
							dust.noGravity = true;
							dust.scale = Main.rand.NextFloat(1.25f, 1.75f);
						}
					}
				}
				else if (Projectile.timeLeft < 175 && Projectile.timeLeft > 7) 
				{ // Reduced timeleft if not target was hit - 175 and 7 are here to accomodate the Sin() visuals in predraw
					Projectile.timeLeft = 7;
					Projectile.friendly = false;
				}

				if (BaseVelocity == Vector2.Zero)
				{
					BaseVelocity = Projectile.velocity;
				}

				if (Projectile.timeLeft <= 180 && Projectile.timeLeft > 150)
				{
					if (Projectile.velocity.Length() < 1f)
					{ // rotates towards the player when turning back
						BaseVelocity = Vector2.Normalize(Projectile.Center - Owner.Center) * BaseVelocity.Length();
					}

					float returnSpeedMult = 1f;
					if (Projectile.ai[2] != 0f)
					{
						returnSpeedMult = 1.25f;
					}

					Projectile.velocity -= BaseVelocity / 15f * returnSpeedMult;
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

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			float scaleMult = 1.1f + (float)Math.Sin(Projectile.timeLeft * 0.15f) * 0.25f;
			if (Projectile.timeLeft < 10) colorMult *= Projectile.timeLeft / 10f;

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