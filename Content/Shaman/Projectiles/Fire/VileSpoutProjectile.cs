using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using OrchidMod.Content.Shaman.Projectiles;
using OrchidMod.Utilities;
using OrchidMod.Common.ModObjects;

namespace OrchidMod.Content.Shaman.Projectiles.Fire
{
	public class VileSpoutProjectile : OrchidModShamanProjectile
	{
		private static Texture2D TextureMain;

		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public List<int> HitTargets;
		public int Target => (int)Projectile.ai[0] - 1;

		public override void SafeSetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 60;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Projectile.alpha = 255;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			HitTargets = new List<int>();
		}

		public override void SafeAI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			if (TimeSpent == 0) Projectile.penetrate = (int)Projectile.ai[1];

			if (OldPosition.Count > 10)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Main.rand.NextBool(10))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CorruptTorch, Scale: Main.rand.NextFloat(1f, 1.4f));
				dust.velocity = dust.velocity * 0.25f + Projectile.velocity * 0.2f;
				dust.noGravity = true;
			}


			if (Target > -1)
			{
				NPC target = Main.npc[Target];

				if (!target.active) Projectile.Kill();
				Vector2 newVelocity = Vector2.Normalize(target.Center - Projectile.Center) * 0.8f;
				Projectile.velocity = Projectile.velocity * 0.95f + newVelocity;
				Projectile.rotation = newVelocity.ToRotation();
			}
			else
			{
				Projectile.rotation = Projectile.velocity.ToRotation();

				if (Projectile.penetrate != (int)Projectile.ai[1])
				{
					NPC closestTarget = null;
					float distanceClosest = 240f;
					foreach (NPC npc in Main.npc)
					{
						float distance = Projectile.Center.Distance(npc.Center);
						if (IsValidTarget(npc) && !HitTargets.Contains(npc.whoAmI) && distance < distanceClosest)
						{
							closestTarget = npc;
							distanceClosest = distance;
						}
					}

					if (closestTarget != null && IsLocalOwner)
					{
						Projectile.ai[0] = closestTarget.whoAmI + 1;
						Projectile.netUpdate = true;
						Projectile.timeLeft = 60;
					}
				}
			}
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (HitTargets.Contains(target.whoAmI)) return false;
			return base.CanHitNPC(target);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			HitTargets.Add(target.whoAmI);
			Projectile.ai[0] = -1;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here

			float colorMult = 1f;
			if (Projectile.timeLeft < 8) colorMult *= Projectile.timeLeft / 8f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = OldPosition[i] - Main.screenPosition;
				Color color = new Color(113, 48, 255);
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.1f * i * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * i * 0.175f, SpriteEffects.None, 0f); ;
				spriteBatch.Draw(TextureMain, drawPosition, null, color * 0.05f * i * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * i * 0.12f, SpriteEffects.None, 0f);
			}

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}
