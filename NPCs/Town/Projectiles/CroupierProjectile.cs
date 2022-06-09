using Terraria.ModLoader;

namespace OrchidMod.NPCs.Town.Projectiles
{
	public class CroupierProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.friendly = true;
			Projectile.aiStyle = 2;
			Projectile.timeLeft = 300;
			Projectile.scale = 1f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chip");
		}
	}
}