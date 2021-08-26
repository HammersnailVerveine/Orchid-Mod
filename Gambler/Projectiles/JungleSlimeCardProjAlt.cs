using Terraria;

namespace OrchidMod.Gambler.Projectiles
{
	public class JungleSlimeCardProjAlt : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slime Thorn");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 6;
			projectile.height = 12;
			projectile.friendly = true;
			projectile.aiStyle = 1;
			projectile.timeLeft = 300;
			this.gamblingChipChance = 5;
		}

		public override void SafeAI()
		{
			projectile.velocity.Y += 0.01f;
			projectile.velocity.X *= 0.99f;
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int dustType = 31;
			Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			target.AddBuff(20, 60 * 3);
		}
	}
}