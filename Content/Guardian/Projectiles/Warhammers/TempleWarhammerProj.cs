using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Utilities;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Warhammers
{
	public class TempleWarhammerProj : OrchidModGuardianProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void AltSetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 240;
			Projectile.scale = 1f;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
			Projectile.timeLeft -= Main.rand.Next(40);
			Projectile.ai[0] = Main.rand.NextFloat(-0.05f, 0.05f);
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
			Projectile.friendly = Projectile.penetrate == 1 && Projectile.timeLeft < 180;
			Projectile.rotation += Projectile.ai[0];

			if (Projectile.penetrate >= 1)
			{
				OldPosition.Add(Projectile.Center);
				OldRotation.Add(Projectile.rotation);

				if (Main.rand.NextBool(10))
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SpectreStaff, Scale: Main.rand.NextFloat(1f, 1.4f));
					dust.velocity = dust.velocity * 0.25f + Projectile.velocity * 0.2f;
					dust.noGravity = true;
				}
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

			// Draw code here

			float colorMult = 1f;
			if (Projectile.timeLeft < 30) colorMult *= Projectile.timeLeft / 30f;
			Color color = new Color(255, 232, 102);

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = Vector2.Transform(OldPosition[i] - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.1f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.11f, SpriteEffects.None, 0f);
			}

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}