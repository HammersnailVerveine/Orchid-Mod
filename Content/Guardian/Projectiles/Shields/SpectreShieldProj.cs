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
	public class SpectreShieldProj : OrchidModGuardianProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public int Cooldown = 20;

		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 180;
			Projectile.scale = 1f;
			Projectile.penetrate = 3;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
		}

		public override void AI()
		{
			Cooldown++;
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

				if (Cooldown > 30)
				{
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
			Cooldown = Main.rand.Next(30);
			if (Projectile.penetrate == 1)
			{
				Projectile.penetrate = -1;
				Projectile.friendly = false;
				Projectile.velocity *= 0f;
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here

			float colorMult = 1f;
			if (Projectile.timeLeft < 30) colorMult *= Projectile.timeLeft / 30f;
			Color color = new Color(140, 238, 255);

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