using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
	public class IchorOrb : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;
		int orbsNumber = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Orb");
		}
		public override void SafeSetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.timeLeft = 12960000;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			Main.projFrames[projectile.type] = 10;
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

			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 2)
				projectile.frame = 0;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 2 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 4)
				projectile.frame = 1;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 4 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 6)
				projectile.frame = 2;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 6 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 8)
				projectile.frame = 3;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 8 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 10)
				projectile.frame = 4;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 10 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 12)
				projectile.frame = 5;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 12 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 14)
				projectile.frame = 6;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 14 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 16)
				projectile.frame = 7;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 16 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 18)
				projectile.frame = 8;
			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique >= 18 && player.GetModPlayer<OrchidModPlayer>().orbCountUnique < 20)
				projectile.frame = 9;

			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique == 0 || player.GetModPlayer<OrchidModPlayer>().orbCountUnique > 20 || player.GetModPlayer<OrchidModPlayer>().shamanOrbUnique != ShamanOrbUnique.ICHOR)
				projectile.Kill();
			else orbsNumber = player.GetModPlayer<OrchidModPlayer>().orbCountUnique;

			if (projectile.timeLeft == 12960000)
			{
				startX = projectile.position.X - player.position.X;
				startY = projectile.position.Y - player.position.Y;
			}
			projectile.velocity.X = player.velocity.X;
			projectile.position.X = player.position.X + startX;
			projectile.position.Y = player.position.Y + startY;

			if (Main.rand.Next(13) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 162);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 1.4f;
			}
			if (Main.rand.Next(13) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 169);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 1.4f;
			}

		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];

			for (int i = 0; i < 10; i++)
			{
				int dust;
				switch (Main.rand.Next(3))
				{
					case 0:
						dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 169);
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale = 1.5f;
						Main.dust[dust].noGravity = true;
						break;
					case 1:
						dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 162);
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale = 2f;
						Main.dust[dust].noGravity = true;
						break;
					case 2:
						dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 228);
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].scale = 1.75f;
						Main.dust[dust].noGravity = true;
						break;
				}
			}

			int dmg = (int)((30 + (5 * (orbsNumber))) * player.GetModPlayer<OrchidModPlayer>().shamanDamage);
			int rainCount = (int)(orbsNumber / 2);
			for (int i = 0; i < rainCount; i++)
			{
				Vector2 perturbedSpeed = new Vector2(projectile.velocity.X - Main.rand.Next(2) + 1, -((float)(Main.rand.Next(3) + 3))).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(player.Center.X + projectile.velocity.X, player.Center.Y - 125 + projectile.velocity.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("IchorOrbRain"), dmg, 0.0f, player.whoAmI, 0.0f, 0.0f);
			}
			Main.player[projectile.owner].GetModPlayer<OrchidModPlayer>().orbCountUnique = 0;
		}
	}
}
