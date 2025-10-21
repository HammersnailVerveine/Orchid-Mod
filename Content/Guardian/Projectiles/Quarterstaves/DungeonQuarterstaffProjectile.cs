using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Quarterstaves
{
	public class DungeonQuarterstaffProjectile : OrchidModGuardianProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 30;
			Projectile.scale = 1f;
			Projectile.penetrate = 5;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void AI()
		{
			if (Projectile.owner == Main.myPlayer)
			{
				Vector2 skew = Main.MouseWorld - Projectile.Center;
				float toMouse = MathHelper.WrapAngle(Projectile.velocity.ToRotation() - skew.ToRotation());
				if (Projectile.ai[0] != 0)
				{
					if (Math.Abs(toMouse + Projectile.ai[0]) > Math.Abs(toMouse) || Math.Abs(toMouse) > MathHelper.TwoPi / 3)
					{
						Projectile.ai[0] = 0;
						Projectile.netUpdate = true;
					}
				}
					
				if (Projectile.ai[0] == 0)
				{
					if (Math.Abs(toMouse) < MathHelper.PiOver2 && Math.Abs(toMouse) > 0.1f)
					{
						if (toMouse > 0)
							Projectile.ai[0] = -0.0314f;
						else Projectile.ai[0] = 0.0314f;
						Projectile.netUpdate = true;
					}
				}
			}

			Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0]) * (Projectile.timeLeft < 10 ? 0.8f : 1.05f);

			for (int i = 0; i < 2; i ++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, 6, 6, DustID.BlueTorch);
				if (Main.rand.Next(10) > 0)
				{
					dust.scale = Main.rand.NextFloat(0.8f, 1.2f);
					dust.velocity = dust.velocity * 0.5f + Projectile.velocity * 0.5f;
					dust.noGravity = true;
				}
			}
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
		{
			if (guardian.GuardianItemCharge > 0 && Projectile.ai[1] != 0)
			{ // charge on jab projectile hit
				Projectile.ai[1] = 0;
				guardian.GuardianItemCharge += 30f * player.GetTotalAttackSpeed(DamageClass.Melee);
			}
		}
	}
}