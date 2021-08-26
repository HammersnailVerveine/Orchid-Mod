using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles.Chips
{
	public class HellChipProj : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellfire Chip");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			projectile.friendly = true;
			projectile.aiStyle = 2;
			projectile.timeLeft = 300;
		}

		public override void SafeAI()
		{
			if (Main.rand.Next(10) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = projectile.velocity * 0.7f;
			}
		}

		public override void Kill(int timeLeft)
		{
			int projType = ProjectileType<Gambler.Projectiles.Chips.HellChipProjAlt>();
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, projType, projectile.damage, 3f, projectile.owner);
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
		}
	}
}