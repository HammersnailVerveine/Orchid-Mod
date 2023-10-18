using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Content.Gambler.Projectiles
{
	public class GoblinArmyCardProjAlt : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Goblin Bolt");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 90;
			Projectile.penetrate = 3;
			Projectile.alpha = 128;
			this.projectileTrail = true;
		}

		public override void SafeAI()
		{
			Projectile.rotation += 0.2f;

			if (Main.rand.Next(2) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27);
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 27, 5, 5, true, 1.3f, 1f, 3f, true, true, false, 0, 0, true);
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidGambler modPlayer)
		{
			if (modPlayer.gamblerElementalLens)
			{
				target.AddBuff(153, 60 * 2); // Shadowflame
			}
		}
	}
}