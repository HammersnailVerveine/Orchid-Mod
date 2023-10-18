using Terraria.ModLoader;

namespace OrchidMod.Content.NPCs.Town
{
	public class CroupierProjectile : ModProjectile
	{
		public override string Texture => OrchidAssets.ProjectilesPath + Name;

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
			// DisplayName.SetDefault("Chip");
		}
	}
}