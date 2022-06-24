using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Big
{
	public class AbyssOrb : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyss Orb");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.timeLeft = 12960000;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

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

			Projectile.rotation += 0.1f;

			if (player.GetModPlayer<OrchidShaman>().shamanOrbBig != ShamanOrbBig.ABYSS || player.GetModPlayer<OrchidShaman>().orbCountBig <= 0)
				Projectile.Kill();

			if (Projectile.timeLeft == 12960000)
			{
				int nbOrb = player.GetModPlayer<OrchidShaman>().orbCountBig;
				int offsetX = 11;

				if (nbOrb > 2)
				{
					startX = -36 - offsetX;
					startY = -26 - offsetX;
				}

				if (nbOrb > 5)
				{
					startX = -20 - offsetX;
					startY = -36 - offsetX;
				}

				if (nbOrb > 8)
				{
					startX = -2 - offsetX;
					startY = -40 - offsetX;
				}

				if (nbOrb > 11)
				{
					startX = +16 - offsetX;
					startY = -36 - offsetX;
				}

				if (nbOrb > 14)
				{
					startX = +32 - offsetX;
					startY = -26 - offsetX;
				}

				if (Projectile.damage != 0)
				{
					Projectile.damage = 0;
					startX = -36 - offsetX;
					startY = -26 - offsetX;
				}
			}

			Projectile.velocity.X = player.velocity.X;
			Projectile.position.X = player.position.X + player.width / 2 + startX;
			Projectile.position.Y = player.position.Y + startY;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 59);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
		}
	}
}
