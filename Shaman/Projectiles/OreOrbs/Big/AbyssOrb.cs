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
			projectile.width = 24;
			projectile.height = 24;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.timeLeft = 12960000;
			projectile.scale = 1f;
			projectile.tileCollide = false;
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
			Player player = Main.player[projectile.owner];

			if (player != Main.player[Main.myPlayer])
			{
				projectile.active = false;
			}

			projectile.rotation += 0.1f;

			if (player.GetModPlayer<OrchidModPlayer>().shamanOrbBig != ShamanOrbBig.ABYSS || player.GetModPlayer<OrchidModPlayer>().orbCountBig <= 0)
				projectile.Kill();

			if (projectile.timeLeft == 12960000)
			{
				int nbOrb = player.GetModPlayer<OrchidModPlayer>().orbCountBig;
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

				if (projectile.damage != 0)
				{
					projectile.damage = 0;
					startX = -36 - offsetX;
					startY = -26 - offsetX;
				}
			}

			projectile.velocity.X = player.velocity.X;
			projectile.position.X = player.position.X + player.width / 2 + startX;
			projectile.position.Y = player.position.Y + startY;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 59);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
		}
	}
}
