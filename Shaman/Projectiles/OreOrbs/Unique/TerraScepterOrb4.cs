using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
	public class TerraScepterOrb4 : OrchidModShamanProjectile
	{
		float startX = 0;
		float startY = 0;
		int orbsNumber = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Scepter Orb");
		}
		public override void SafeSetDefaults()
		{
			projectile.width = 58;
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

			if (player != Main.player[Main.myPlayer])
			{
				projectile.active = false;
			}

			if (player.GetModPlayer<OrchidModPlayer>().timer120 % 5 == 0)
				projectile.frame++;
			if (projectile.frame == 10)
				projectile.frame = 0;

			if (player.GetModPlayer<OrchidModPlayer>().orbCountUnique == 0 || player.GetModPlayer<OrchidModPlayer>().orbCountUnique > 24 || player.GetModPlayer<OrchidModPlayer>().shamanOrbUnique != ShamanOrbUnique.TERRA)
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

			if (Main.rand.Next(10) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 157);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 1.6f;
			}
			if (Main.rand.Next(10) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 269);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
				Main.dust[dust].scale *= 1.6f;
			}

		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 269);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
			Player player = Main.player[projectile.owner];
			int dmg = (int)((50 + (20 * (orbsNumber))) * player.GetModPlayer<OrchidModPlayer>().shamanDamage);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, (Main.rand.Next(20) - 10) * 1f, -5f, mod.ProjectileType("TerraScepterOrbHoming4"), dmg, 0f, projectile.owner, 0f, 0f);
		}
	}
}
