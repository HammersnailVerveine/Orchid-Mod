using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Guardian;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Sage
{
	public class SageBeeProj : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 90;
			Projectile.scale = 0.8f;
			Projectile.alpha = 96;
			Projectile.penetrate = 1;
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override bool? CanHitNPC(NPC target)
		{
			if ((int)Projectile.ai[0] == -1)
			{
				return null;

			}
			return false;
		}

		public override void AI()
		{
			if (!initialized)
			{
				initialized = true;
				Projectile.ai[0] = -1;
			}

			if (Projectile.ai[0] != -1)
			{
				Projectile.extraUpdates = 0;
				NPC npc = Main.npc[(int)Projectile.ai[0]];
				if (npc.active)
				{
					Projectile.velocity *= 0.2f;
					Projectile.ai[1] -= Projectile.velocity.X;
					Projectile.ai[2] -= Projectile.velocity.Y;

					Projectile.position.X = npc.position.X - Projectile.ai[1];
					Projectile.position.Y = npc.position.Y - Projectile.ai[2];
				}
				else 
				{
					Projectile.Kill();
				}

				if (OldPosition.Count > 0)
				{
					OldPosition.RemoveAt(0);
					OldRotation.RemoveAt(0);
				}

				if (Projectile.timeLeft % 60 == 0)
				{
					Main.player[Projectile.owner].ApplyDamageToNPC(npc, Projectile.damage, 0f, npc.direction, Main.rand.Next(100) < Projectile.CritChance, ModContent.GetInstance<ShapeshifterDamageClass>(), true);
				}
			}
			else
			{
				OldPosition.Add(Projectile.Center);
				OldRotation.Add(Projectile.rotation);
				Projectile.rotation = Projectile.velocity.ToRotation();

				ResetIFrames(Projectile); // Prevents darts from passing through enemies while bees are spawned

				if (OldPosition.Count > 10)
				{
					OldPosition.RemoveAt(0);
					OldRotation.RemoveAt(0);
				}

				if (Projectile.timeLeft < 20)
				{
					Projectile.extraUpdates = 0;
					Projectile.velocity *= 0.9f;
				}
			}
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			if (Projectile.ai[0] == -1)
			{
				Projectile.ai[0] = target.whoAmI;
				Projectile.ai[1] = target.position.X - Projectile.position.X;
				Projectile.ai[2] = target.position.Y - Projectile.position.Y;
				Projectile.tileCollide = false;
				Projectile.timeLeft = 1800; // 30 sec
				Projectile.damage = (int)(Projectile.damage * 0.4f);
				Projectile.penetrate = -1;
				Projectile.knockBack = 0f;
				Projectile.netUpdate = true;
				Projectile.friendly = false;

				// Clears existing darts. Up to 3 can exist on a given target, and up to 9 overall

				Projectile lowestTimeLeft = null;
				Projectile lowestTimeLeftOverall = null;
				int count = 0;
				int countOverall = 0;
				foreach (Projectile proj in Main.projectile)
				{
					if (proj.active && proj.type == Type && proj.owner == Projectile.owner)
					{
						countOverall++;
						if (lowestTimeLeftOverall == null)
						{
							lowestTimeLeftOverall = proj;
						}
						else if (lowestTimeLeftOverall.timeLeft > proj.timeLeft)
						{
							lowestTimeLeftOverall = proj;
						}

						if (Projectile.ai[0] == proj.ai[0])
						{
							count++;
							if (lowestTimeLeft == null)
							{
								lowestTimeLeft = proj;
							}
							else if (lowestTimeLeft.timeLeft > proj.timeLeft)
							{
								lowestTimeLeft = proj;
							}
						}
					} 
				}

				if (count > 3)
				{
					lowestTimeLeft.Kill();
				}
				else if (countOverall > 9)
				{
					lowestTimeLeftOverall.Kill();
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			return true;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 10) colorMult *= Projectile.timeLeft / 10f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition2 = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition2, null, lightColor * 0.05f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.065f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);


			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPosition, null, lightColor * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}