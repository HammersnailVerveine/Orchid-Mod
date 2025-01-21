using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Shields
{
	public class NightShieldProj : OrchidModGuardianProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 60;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void AI()
		{
			Projectile.friendly = Projectile.timeLeft < 55 && Projectile.penetrate == 1;
			if (Projectile.penetrate == 1)
			{
				OldPosition.Add(Projectile.Center);
				OldRotation.Add(Projectile.rotation);

				if (Main.rand.NextBool(10))
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, Scale: Main.rand.NextFloat(1f, 1.4f));
					dust.velocity = dust.velocity * 0.25f + Projectile.velocity * 0.2f;
					dust.noGravity = true;
				}

				if (Projectile.timeLeft < 55)
				{
					NPC closestTarget = null;
					float distanceClosest = 240f;
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
				Projectile.rotation = Projectile.velocity.ToRotation();
			}

			if ((OldPosition.Count > 10 || Projectile.penetrate == -1) && OldPosition.Count > 0)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
		{
			Projectile.penetrate = -1;
			Projectile.velocity *= 0f;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here

			float colorMult = 1f;
			if (Projectile.timeLeft < 30) colorMult *= Projectile.timeLeft / 30f;
			Color color = new Color(150, 55, 190);

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.1f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.08f, SpriteEffects.None, 0f);
			}

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}

	public class NightShieldProjAlt : OrchidModProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void AltSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 70;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void AI()
		{
			Projectile.velocity *= 0.96f;
			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			if (Main.rand.NextBool(10))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, Scale: Main.rand.NextFloat(1f, 1.4f));
				dust.velocity = dust.velocity * 0.25f + Projectile.velocity * 0.2f;
				dust.noGravity = true;
			}

			Projectile.rotation = Projectile.velocity.ToRotation();

			if (OldPosition.Count > 10)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item73, Projectile.Center);
			for (int i = 0; i < 3 + Main.rand.Next(3); i ++)
			{
				Vector2 vel = Vector2.Normalize(Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(45f)));
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + vel * 3f, vel * 7f, ModContent.ProjectileType<NightShieldProj>(), (int)(Projectile.damage * 0.75f), 1f, Projectile.owner);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here

			float colorMult = 1f;
			if (Projectile.timeLeft < 10) colorMult *= Projectile.timeLeft / 10f;
			Color color = new Color(150, 55, 190);

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.1f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.11f, SpriteEffects.None, 0f);
			}

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}