using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Projectiles.Standards
{
	public class DesertStandardProj : OrchidModGuardianProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 2;
			Projectile.scale = 1f;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 3;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (target.whoAmI == Projectile.ai[0]) return false;
			return base.CanHitNPC(target);
		}

		public override void AI()
		{
			NPC closestTarget = null;
			float distanceClosest = 240f;
			foreach (NPC npc in Main.npc)
			{
				float distance = Projectile.Center.Distance(npc.Center);
				if (IsValidTarget(npc) && distance < distanceClosest && npc.whoAmI != Projectile.ai[0]) 
				{
					closestTarget = npc;
					distanceClosest = distance;
				}
			}

			if (closestTarget != null && Projectile.timeLeft == 2)
			{
				Vector2 currPoint = Projectile.Center;
				Vector2 nextPoint;
				float angleToEnemy = Projectile.Center.AngleTo(closestTarget.Center);
				bool whichWay = Main.rand.NextBool();
				for (int i = 1; i < 8; i++)
				{
					nextPoint = (Projectile.Center * (7 - i) / 7) + (closestTarget.Center * i / 7) + new Vector2(3.5f - Math.Abs(i - 3.5f), 0).RotatedBy(angleToEnemy + ((whichWay = !whichWay) ? 1.57f : -1.57f)) * (1f + Main.rand.NextFloat() * 3f);
					for (int j = 0; j < 4; j++)
					{
						Dust zapDust = Dust.NewDustDirect((currPoint * (4 - j) / 4f) + (nextPoint * j / 4f), 0, 0, DustID.Electric, Scale: 0.2f + distanceClosest * 0.0035f);
						zapDust.noGravity = true;
						zapDust.velocity *= 0.05f;
					}
					currPoint = nextPoint;
				}

				Projectile.position = closestTarget.Center;
			}
			else
			{
				Projectile.Kill();
			}
		}
	}
}