using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Misc
{
	public class WaterSpikeProjAlt : OrchidModGuardianProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.friendly = false;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 1;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
		}

		public override void OnKill(int timeLeft)
		{
			int type = ModContent.ProjectileType<WaterSpikeProj>();
			Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, type, Projectile.damage, Projectile.knockBack, Projectile.owner);
			projectile.CritChance = projectile.CritChance;
		}
	}
}