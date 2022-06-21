using Terraria;
using Terraria.Audio;
using Terraria.ID;
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
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.friendly = true;
			Projectile.aiStyle = 2;
			Projectile.timeLeft = 300;
		}

		public override void SafeAI()
		{
			if (Main.rand.Next(10) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Projectile.velocity * 0.7f;
			}
		}

		public override void Kill(int timeLeft)
		{
			int projType = ProjectileType<Gambler.Projectiles.Chips.HellChipProjAlt>();
			Projectile.NewProjectile(Projectile.position.X, Projectile.position.Y, 0f, 0f, projType, Projectile.damage, 3f, Projectile.owner);
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
		}
	}
}