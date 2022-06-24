using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Unique
{
	public class ThunderScepterTornado : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 44;
			Projectile.height = 44;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 100;
			Projectile.scale = 1f;
			Main.projFrames[Projectile.type] = 6;
			Projectile.alpha = 128;
			Projectile.tileCollide = false;
			Projectile.penetrate = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tornado");
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 % 5 == 0)
				Projectile.frame++;
			if (Projectile.frame == 6)
				Projectile.frame = 0;

			if (Main.rand.Next(3) == 0)
			{
				Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
				int dust = Dust.NewDust(pos, Projectile.width, Projectile.height, 16, 0f, 0f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1f;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 16);
				Main.dust[dust].velocity = Projectile.velocity / 2;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}