using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class DragonScaleScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 60;
			Projectile.penetrate = 10;
			Projectile.scale = 1f;
			AIType = ProjectileID.Bullet;
			Projectile.alpha = 196;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Draconic Fire Bolt");
		}

		public override void AI()
		{
			Projectile.rotation += 0.2f;

			if (Projectile.penetrate == 10)
			{
				Player player = Main.player[Projectile.owner];
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				Projectile.penetrate = 1 + OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);
			}


			int dust = 75;

			switch (Main.rand.Next(3))
			{
				case 0:
					dust = 75;
					break;
				case 1:
					dust = 74;
					break;
				case 2:
					dust = 61;
					break;
				default:
					break;
			}
			for (int i = 0; i < 2; i++)
			{
				int DustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dust, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
				Main.dust[DustID].noGravity = true;
				Main.dust[DustID].velocity = Projectile.velocity / 3;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 75);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Projectile.velocity / 2;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			target.AddBuff(39, 60 * 2); // Cursed Inferno
		}
	}
}