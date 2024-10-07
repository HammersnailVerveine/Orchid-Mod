using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Shields
{
	public class HorizonShieldProjBlue : OrchidModGuardianProjectile
	{
		private static Texture2D TextureMain;
		private static Texture2D TextureAlt;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public int TimeSpent;

		public override void AltSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 180;
			Projectile.scale = 1f;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureAlt ??= ModContent.Request<Texture2D>(Texture + "_Alt", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.timeLeft);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.timeLeft = reader.ReadInt32();
		}

		public override void AI()
		{
			TimeSpent++;
			if (Projectile.penetrate >= 1)
			{
				OldPosition.Add(Projectile.Center);
				OldRotation.Add(Projectile.rotation);

				if (Projectile.friendly)
				{ // Friedly, homes towards owner
					if (TimeSpent < 30)
					{
						Projectile.velocity *= 0.95f;
					}
					else
					{
						Vector2 newVelocity = Vector2.Normalize(Owner.Center - Projectile.Center) * 0.8f;
						Projectile.velocity = Projectile.velocity * 0.92f + newVelocity;

						if (Owner.Center.Distance(Projectile.Center) < 64f && Projectile.timeLeft > 30)
						{
							Projectile.timeLeft = 30;
							Projectile.netUpdate = true;
							Projectile.velocity *= 0.5f;
							SoundEngine.PlaySound(SoundID.Item131, Projectile.Center);
						}
					}
				}
				else
				{ // Hostile, homes towards closest enemy
					NPC closestTarget = null;
					float distanceClosest = 400f;
					foreach (NPC npc in Main.npc)
					{
						float distance = Projectile.Center.Distance(npc.Center);
						if (IsValidTarget(npc) && distance < distanceClosest)
						{
							closestTarget = npc;
							distanceClosest = distance;
						}
					}

					if (closestTarget != null)
					{
						Vector2 newVelocity = Vector2.Normalize(closestTarget.Center - Projectile.Center) * 0.8f;
						Projectile.velocity = Projectile.velocity * 0.92f + newVelocity;
					}
					else if (Projectile.timeLeft > 33)
					{
						Projectile.timeLeft -= 4;
						Projectile.velocity *= 0.95f;
					}
				}
			}

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 pos = OldPosition[i];
				pos.Y -= Main.rand.NextFloat(6.25f);
				pos.X += Main.rand.NextFloat(3f) - 1.5f;
				OldPosition[i] = pos;
			}

			if ((OldPosition.Count > 10 || Projectile.penetrate == -1) && OldPosition.Count > 0)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
		{
			if (Projectile.penetrate == 1)
			{
				Projectile.penetrate = -1;
				Projectile.velocity *= 0f;
				Projectile.timeLeft = 30;
				Projectile.netUpdate = true;
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 30) colorMult *= Projectile.timeLeft / 30f;
			Color color = new Color(59, 88, 204);

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = Vector2.Transform(OldPosition[i] - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.1f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.11f, SpriteEffects.None, 0f);
				spriteBatch.Draw(TextureAlt, drawPosition, null, new Color(221, 243, 255) * colorMult * 0.08f * (i + 1), Projectile.rotation, TextureAlt.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}