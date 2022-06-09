using Terraria;
using Terraria.ID;

namespace OrchidMod.Gambler.Projectiles
{
	public class BrainCardProj2 : OrchidModGamblerProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 75;
			Projectile.height = 75;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			target.AddBuff(BuffID.Confused, 60 * 3);
		}

		public override void SafeAI()
		{
			for (int i = 0; i < 10; i++)
			{
				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5);
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity.X /= 3;
				Main.dust[dust2].velocity.Y /= 3;
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brainxplosion");
		}
	}
}