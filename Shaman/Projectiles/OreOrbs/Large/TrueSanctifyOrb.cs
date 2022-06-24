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
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.timeLeft = 12960000;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Main.projFrames[Projectile.type] = 24;
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
			Player player = Main.player[Projectile.owner];

			if (player != Main.player[Main.myPlayer])
			{
				Projectile.active = false;
			}

			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 65)
				Projectile.frame = 1;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 70)
				Projectile.frame = 2;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 75)
				Projectile.frame = 3;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 80)
				Projectile.frame = 4;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 85)
				Projectile.frame = 5;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 90)
				Projectile.frame = 6;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 95)
				Projectile.frame = 7;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 100)
				Projectile.frame = 8;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 105)
				Projectile.frame = 9;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 110)
				Projectile.frame = 10;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 115)
				Projectile.frame = 11;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 0)
				Projectile.frame = 12;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 5)
				Projectile.frame = 13;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 10)
				Projectile.frame = 14;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 15)
				Projectile.frame = 15;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 20)
				Projectile.frame = 16;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 25)
				Projectile.frame = 17;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 30)
				Projectile.frame = 18;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 35)
				Projectile.frame = 19;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 40)
				Projectile.frame = 20;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 45)
				Projectile.frame = 21;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 50)
				Projectile.frame = 22;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 55)
				Projectile.frame = 23;
			if (player.GetModPlayer<OrchidModPlayerShaman>().modPlayer.timer120 == 60)
				Projectile.frame = 0;


			if (player.GetModPlayer<OrchidModPlayerShaman>().orbCountLarge < 5 || player.GetModPlayer<OrchidModPlayerShaman>().orbCountLarge > 35 || player.GetModPlayer<OrchidModPlayerShaman>().shamanOrbLarge != ShamanOrbLarge.TRUESANCTIFY)
				Projectile.Kill();
			else orbsNumber = player.GetModPlayer<OrchidModPlayerShaman>().orbCountLarge;

			if (Projectile.timeLeft == 12960000)
			{

				int nbOrb = player.GetModPlayer<OrchidModPlayerShaman>().orbCountLarge;
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

				if (Projectile.damage != 0)
				{
					Projectile.damage = 0;
					startX = -43 - offsetX;
					startY = -38 - offsetX;
				}
			}

			Projectile.velocity.X = player.velocity.X;
			Projectile.position.X = player.position.X + player.width / 2 + startX;
			Projectile.position.Y = player.position.Y + startY;

			if (Main.rand.Next(30) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 254);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 0.7f;
			}


		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 254);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
		}
	}
}
