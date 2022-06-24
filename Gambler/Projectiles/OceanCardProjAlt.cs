using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class OceanCardProjAlt : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fartling ball");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 600;
			Main.projFrames[Projectile.type] = 5;
			this.bonusTrigger = true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 31);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
		}

		public override void SafeAI()
		{
			Projectile.velocity *= 0.95f;
			Player player = Main.player[Projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (modPlayer.timer120 == 0 || modPlayer.timer120 > 49) Projectile.frame = 0;
			if (modPlayer.timer120 == 10) Projectile.frame = 1;
			if (modPlayer.timer120 == 20) Projectile.frame = 2;
			if (modPlayer.timer120 == 30) Projectile.frame = 3;
			if (modPlayer.timer120 == 40) Projectile.frame = 4;
		}

		public override void BonusProjectiles(Player player, OrchidModPlayerGambler modPlayer, Projectile projectile, OrchidModGlobalProjectile modProjectile, bool dummy)
		{
			if (modProjectile.gamblerInternalCooldown == 0)
			{
				modProjectile.gamblerInternalCooldown = 50;
				int projType = ProjectileType<Gambler.Projectiles.OceanCardProj>();
				Vector2 target = Main.MouseWorld;
				Vector2 heading = target - projectile.position;
				heading.Normalize();
				heading *= new Vector2(0f, 5f).Length();
				int newProjectile = DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, heading.X, heading.Y, projType, projectile.damage, projectile.knockBack, player.whoAmI), dummy);
				Main.projectile[newProjectile].ai[1] = projectile.ai[1];
				Main.projectile[newProjectile].netUpdate = true;
				OrchidModProjectile.spawnDustCircle(projectile.Center, 31, 10, 10, true, 1.5f, 1f, 3f, true, true, false, 0, 0, true);
			}
		}
	}
}