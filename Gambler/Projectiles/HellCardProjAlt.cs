using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class HellCardProjAlt : OrchidModGamblerProjectile
	{
		private bool animDirection = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pepper");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 24;
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
			OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
			if (modPlayer.timer120 % 10 == 0)
			{
				Projectile.frame += animDirection ? -1 : 1;
				animDirection = Projectile.frame == 4 ? true : Projectile.frame == 0 ? false : animDirection;
			}
		}

		public override void BonusProjectiles(Player player, OrchidGambler modPlayer, Projectile projectile, OrchidModGlobalProjectile modProjectile, bool dummy)
		{
			if (modProjectile.gamblerInternalCooldown == 0)
			{
				modProjectile.gamblerInternalCooldown = 30;
				int projType = ProjectileType<Gambler.Projectiles.HellCardProj>();
				Vector2 target = Main.MouseWorld;
				Vector2 heading = target - projectile.position;
				heading.Normalize();
				heading *= new Vector2(0f, 15f).Length();
				int newProjectile = DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, heading.X, heading.Y, projType, projectile.damage, projectile.knockBack, player.whoAmI), dummy);
				Main.projectile[newProjectile].ai[1] = projectile.ai[1];
				Main.projectile[newProjectile].netUpdate = true;
				OrchidModProjectile.spawnDustCircle(projectile.Center, 6, 10, 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
			}
		}
	}
}