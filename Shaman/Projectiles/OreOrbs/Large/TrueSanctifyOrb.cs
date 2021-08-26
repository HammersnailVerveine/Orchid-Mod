using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Large
{
	public class TrueSanctifyOrb : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;
		int orbsNumber = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Sanctify Orb");
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
			Main.projFrames[projectile.type] = 24;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
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
				projectile.frame = 8;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 105)
				projectile.frame = 9;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 110)
				projectile.frame = 10;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 115)
				projectile.frame = 11;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 0)
				projectile.frame = 12;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 5)
				projectile.frame = 13;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 10)
				projectile.frame = 14;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 15)
				projectile.frame = 15;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 20)
				projectile.frame = 16;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 25)
				projectile.frame = 17;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 30)
				projectile.frame = 18;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 35)
				projectile.frame = 19;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 40)
				projectile.frame = 20;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 45)
				projectile.frame = 21;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 50)
				projectile.frame = 22;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 55)
				projectile.frame = 23;
			if (player.GetModPlayer<OrchidModPlayer>().timer120 == 60)
				projectile.frame = 0;


			if (player.GetModPlayer<OrchidModPlayer>().orbCountLarge < 5 || player.GetModPlayer<OrchidModPlayer>().orbCountLarge > 35 || player.GetModPlayer<OrchidModPlayer>().shamanOrbLarge != ShamanOrbLarge.TRUESANCTIFY)
				projectile.Kill();
			else orbsNumber = player.GetModPlayer<OrchidModPlayer>().orbCountLarge;

			if (projectile.timeLeft == 12960000)
			{

				int nbOrb = player.GetModPlayer<OrchidModPlayer>().orbCountLarge;
				int offsetX = 7;

				if (nbOrb > 4)
				{
					startX = -43 - offsetX;
					startY = -38 - offsetX;
				}

				if (nbOrb > 9)
				{
					startX = -30 - offsetX;
					startY = -48 - offsetX;
				}

				if (nbOrb > 14)
				{
					startX = -15 - offsetX;
					startY = -53 - offsetX;
				}

				if (nbOrb > 19)
				{
					startX = -0 - offsetX;
					startY = -55 - offsetX;
				}

				if (nbOrb > 24)
				{
					startX = +15 - offsetX;
					startY = -53 - offsetX;
				}

				if (nbOrb > 29)
				{
					startX = +30 - offsetX;
					startY = -48 - offsetX;
				}

				if (nbOrb > 34)
				{
					startX = +43 - offsetX;
					startY = -38 - offsetX;
				}

				if (projectile.damage != 0)
				{
					projectile.damage = 0;
					startX = -43 - offsetX;
					startY = -38 - offsetX;
				}
			}

			projectile.velocity.X = player.velocity.X;
			projectile.position.X = player.position.X + player.width / 2 + startX;
			projectile.position.Y = player.position.Y + startY;

			if (Main.rand.Next(30) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 254);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 0.7f;
			}


		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 254);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
		}
	}
}
