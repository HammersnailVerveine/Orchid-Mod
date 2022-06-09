using Terraria;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{

	public class HoneyOrb2 : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;
		int orbsNumber = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Honey Orb");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.timeLeft = 12960000;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Main.projFrames[Projectile.type] = 8;
		}

		// public override Color? GetAlpha(Color lightColor)
		// {
		// return Color.White;
		// }

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (player != Main.player[Main.myPlayer])
			{
				Projectile.active = false;
			}

			if (player.GetModPlayer<OrchidModPlayer>().timer120 % 10 == 0)
				Projectile.frame++;
			if (Projectile.frame == 8)
				Projectile.frame = 0;

			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique == 0 || player.GetModPlayer<OrchidModPlayer>().orbCountUnique > 14 || player.GetModPlayer<OrchidModPlayer>().shamanOrbUnique != ShamanOrbUnique.HONEY)
				Projectile.Kill();
			else orbsNumber = player.GetModPlayer<OrchidModPlayer>().orbCountUnique;

			if (Projectile.timeLeft == 12960000)
			{
				startX = Projectile.position.X - player.position.X;
				startY = Projectile.position.Y - player.position.Y;
			}
			Projectile.velocity.X = player.velocity.X;
			Projectile.position.X = player.position.X + startX;
			Projectile.position.Y = player.position.Y + startY;

			if (Main.rand.Next(16) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 153);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 1.2f;

			}

		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 153);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 5f;
			}
		}
	}
}
