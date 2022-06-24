using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Unique
{

	public class SolarPebbleScepterOrb : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;
		int orbsNumber = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Orb");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.timeLeft = 12960000;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Main.projFrames[Projectile.type] = 9;
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

			if (player.GetModPlayer<OrchidShaman>().orbCountUnique < 2)
				Projectile.frame = 0;
			if (player.GetModPlayer<OrchidShaman>().orbCountUnique >= 2 && player.GetModPlayer<OrchidShaman>().orbCountUnique < 4)
				Projectile.frame = 1;
			if (player.GetModPlayer<OrchidShaman>().orbCountUnique >= 4 && player.GetModPlayer<OrchidShaman>().orbCountUnique < 6)
				Projectile.frame = 2;
			if (player.GetModPlayer<OrchidShaman>().orbCountUnique >= 6 && player.GetModPlayer<OrchidShaman>().orbCountUnique < 8)
				Projectile.frame = 3;
			if (player.GetModPlayer<OrchidShaman>().orbCountUnique >= 8 && player.GetModPlayer<OrchidShaman>().orbCountUnique < 10)
				Projectile.frame = 4;
			if (player.GetModPlayer<OrchidShaman>().orbCountUnique >= 10 && player.GetModPlayer<OrchidShaman>().orbCountUnique < 12)
				Projectile.frame = 5;
			if (player.GetModPlayer<OrchidShaman>().orbCountUnique >= 12 && player.GetModPlayer<OrchidShaman>().orbCountUnique < 14)
				Projectile.frame = 6;
			if (player.GetModPlayer<OrchidShaman>().orbCountUnique >= 14 && player.GetModPlayer<OrchidShaman>().orbCountUnique < 16)
				Projectile.frame = 7;
			if (player.GetModPlayer<OrchidShaman>().orbCountUnique >= 16)
				Projectile.frame = 8;

			if (player.GetModPlayer<OrchidShaman>().orbCountUnique == 0 || player.GetModPlayer<OrchidShaman>().orbCountUnique > 17 || player.GetModPlayer<OrchidShaman>().shamanOrbUnique != ShamanOrbUnique.ECLIPSE)
				Projectile.Kill();

			else orbsNumber = player.GetModPlayer<OrchidShaman>().orbCountUnique;

			if (Projectile.timeLeft == 12960000)
			{
				startX = Projectile.position.X - player.position.X;
				startY = Projectile.position.Y - player.position.Y;
			}
			Projectile.velocity.X = player.velocity.X;
			Projectile.position.X = player.position.X + startX;
			Projectile.position.Y = player.position.Y + startY;

			if (Main.rand.Next(5) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 1.4f;
			}
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];

			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;

			}
		}
	}
}
