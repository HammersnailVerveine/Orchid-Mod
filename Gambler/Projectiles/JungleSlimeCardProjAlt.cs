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
			Projectile.width = 6;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 300;
			this.gamblingChipChance = 5;
		}

		public override void SafeAI()
		{
			Projectile.velocity.Y += 0.01f;
			Projectile.velocity.X *= 0.99f;
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int dustType = 31;
			Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType)].velocity *= 0.25f;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerGambler modPlayer)
		{
			target.AddBuff(20, 60 * 3);
		}
	}
}