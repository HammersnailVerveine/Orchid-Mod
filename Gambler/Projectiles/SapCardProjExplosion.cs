using Terraria;

namespace OrchidMod.Gambler.Projectiles
{
	public class SapCardProjExplosion : OrchidModGamblerProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 150;
			projectile.height = 150;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			this.gamblingChipChance = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosion");
		}

		public override void AI()
		{
			OrchidModProjectile.resetIFrames(projectile);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.gamblerElementalLens)
			{
				target.AddBuff(20, 60 * 5); // Poisoned
			}
		}
	}
}