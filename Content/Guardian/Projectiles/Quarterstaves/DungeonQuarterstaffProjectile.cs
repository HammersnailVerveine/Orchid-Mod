using Terraria;
using Terraria.ID;

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
			if (Projectile.timeLeft < 15)
			{
				Projectile.velocity *= 0.85f;
			}

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
	}
}