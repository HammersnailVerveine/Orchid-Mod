using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class OrchidScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 28;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 30;
			Projectile.scale = 1f;
			AIType = ProjectileID.Bullet;
			Projectile.penetrate = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Petal");
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayerShaman modPlayer = player.GetModPlayer<OrchidModPlayerShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();

			if (Projectile.timeLeft == 30)
			{
				float rand = (float)(Main.rand.Next(4) - 2f);
				Projectile.velocity.X += rand;
				Projectile.velocity.Y += rand;

				if (nbBonds > 2)
				{
					Projectile.penetrate = 2;
				}
			}

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Content.Dusts.PollenDust>());
			Main.dust[dust].velocity *= 1.5f;
			Main.dust[dust].noGravity = true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Content.Dusts.PollenDust>());
				Main.dust[dust].noGravity = false;
				Main.dust[dust].scale *= 1.2f;
				Main.dust[dust].velocity *= 2f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerShaman modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				player.AddBuff((thoriumMod.Find<ModBuff>("OverGrowth").Type), 3 * 60);
			}
		}
	}
}