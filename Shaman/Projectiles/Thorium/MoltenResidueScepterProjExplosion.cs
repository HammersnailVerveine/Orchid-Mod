using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class MoltenResidueScepterProjExplosion : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = 200;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosion");
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			int size = 30 + 30 * OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);
			Projectile.width = size;
			Projectile.height = size;

			Projectile.position.X -= size / 2;
			Projectile.position.Y -= size / 2;

			// for (int i = 0 ; i < (int)(projectile.width / 2) ; i ++) {
			// int index1 = Dust.NewDust(projectile.position, size, 1, 6, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
			// Main.dust[index1].velocity *= 0.2f;
			// Main.dust[index1].fadeIn = 1f;
			// Main.dust[index1].scale = 1.5f;
			// Main.dust[index1].noGravity = true;
			// }

		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) { }
	}
}