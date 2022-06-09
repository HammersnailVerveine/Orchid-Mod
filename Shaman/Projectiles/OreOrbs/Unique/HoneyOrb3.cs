using Terraria;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{

	public class HoneyOrb3 : OrchidModShamanProjectile
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
			Projectile.width = 26;
			Projectile.height = 26;
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

			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique == 0 || player.GetModPlayer<OrchidModPlayer>().orbCountUnique > 19 || player.GetModPlayer<OrchidModPlayer>().shamanOrbUnique != ShamanOrbUnique.HONEY)
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

			if (Main.rand.Next(13) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 153);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 1.4f;
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

			Player player = Main.player[Projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int dmg = (int)(10 * modPlayer.shamanDamage);
			for (int i = 0; i < 2 + (int)(modPlayer.orbCountUnique / 2); i++)
			{
				if (Main.player[Projectile.owner].strongBees && Main.rand.Next(2) == 0)
					Projectile.NewProjectile(Projectile.position.X, Projectile.position.Y, 3 - Main.rand.Next(6), 3 - Main.rand.Next(6), 566, (int)(dmg * 1.15f), 0f, Projectile.owner, 0f, 0f);
				else
					Projectile.NewProjectile(Projectile.position.X, Projectile.position.Y, 3 - Main.rand.Next(6), 3 - Main.rand.Next(6), 181, dmg, 0f, Projectile.owner, 0f, 0f);
			}
			modPlayer.orbCountUnique = 0;
		}
	}
}
