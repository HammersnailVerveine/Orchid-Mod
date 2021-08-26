using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Big
{
	public class CobaltOrb : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Orb");
		}
		public override void SafeSetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.timeLeft = 12960000;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			Main.projFrames[projectile.type] = 8;
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

			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 65)
				projectile.frame = 1;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 70)
				projectile.frame = 2;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 75)
				projectile.frame = 3;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 80)
				projectile.frame = 4;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 85)
				projectile.frame = 5;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 90)
				projectile.frame = 6;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 95)
				projectile.frame = 7;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 100)
				projectile.frame = 0;

			if (player.GetModPlayer<OrchidModPlayer>().shamanOrbBig != ShamanOrbBig.COBALT || player.GetModPlayer<OrchidModPlayer>().orbCountBig <= 0)
				projectile.Kill();

			if (projectile.timeLeft == 12960000)
			{
				int nbOrb = player.GetModPlayer<OrchidModPlayer>().orbCountBig;
				int offsetX = 7;

				if (nbOrb > 2)
				{
					startX = -30 - offsetX;
					startY = -30 - offsetX;
				}

				if (nbOrb > 5)
				{
					startX = -15 - offsetX;
					startY = -38 - offsetX;
				}

				if (nbOrb > 8)
				{
					startX = -0 - offsetX;
					startY = -40 - offsetX;
				}

				if (nbOrb > 11)
				{
					startX = +15 - offsetX;
					startY = -38 - offsetX;
				}

				if (nbOrb > 14)
				{
					startX = +30 - offsetX;
					startY = -30 - offsetX;
				}

				if (projectile.damage != 0)
				{
					projectile.damage = 0;
					startX = -30 - offsetX;
					startY = -30 - offsetX;
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
