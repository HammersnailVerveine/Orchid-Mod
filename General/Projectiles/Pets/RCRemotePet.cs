using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Projectiles.Pets
{
	public class RCRemotePet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("RC Copter");
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			AIType = ProjectileID.ZephyrFish;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			player.zephyrfish = false;
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
			if (player.dead)
			{
				modPlayer.remoteCopterPet = false;
			}
			if (modPlayer.remoteCopterPet)
			{
				Projectile.timeLeft = 2;
			}
		}
	}
}