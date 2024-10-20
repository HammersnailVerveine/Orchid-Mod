using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Sage
{
	public class SageFoxProj : OrchidModShapeshifterProjectile
	{
		public int TimeSpent = 0;
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public NPC target;

		public override void AltSetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
		}

		public override void AI()
		{
			TimeSpent++;

			if (target == null)
			{
				Projectile.ai[0] += 3f;
				Vector2 position = Vector2.UnitY.RotatedBy(MathHelper.ToRadians(Projectile.ai[0] + Projectile.ai[1] * 120f)) * 64f;
				Projectile.position = Owner.Center + position - new Vector2(Projectile.width, Projectile.height) * 0.5f + Vector2.UnitY * Owner.gfxOffY;

				NPC closestTarget = null;
				float distanceClosest = 200f;
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
					target = closestTarget;
					SoundEngine.PlaySound(SoundID.Item7, Projectile.Center);
				}
			}
			else if (target.active)
			{
				Vector2 newVelocity = Vector2.Normalize(target.Center - Projectile.Center) * 0.75f;
				Projectile.velocity = Projectile.velocity * 0.9f + newVelocity;
			}
			else
			{
				Projectile.velocity *= 0.8f;
				if (Projectile.timeLeft > 60)
				{
					Projectile.timeLeft = 60;
				}
			}

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 pos = OldPosition[i];
				pos.Y -= 3f;
				OldPosition[i] = pos;
			}

			OldPosition.Add(Projectile.Center + new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-3f, 3f)));
			OldRotation.Add(Projectile.rotation + Main.rand.NextFloat(MathHelper.Pi));

			if (OldPosition.Count > 7)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Main.rand.NextBool(5))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, Scale: Main.rand.NextFloat(0.8f, 1.2f));
				dust.noGravity = true;
			}
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			target.AddBuff(BuffID.Frostburn, 180);
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 30) colorMult *= Projectile.timeLeft / 30f;
			Color color = new Color(180, 234, 237); // to 54 150 248

			for (int i = 0; i < OldPosition.Count; i++)
			{
				color.R -= 18;
				color.G -= 12;
				color.B -= 2;
				Vector2 drawPosition = Vector2.Transform(OldPosition[i] - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.14f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.12f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}