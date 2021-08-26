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
			Main.projFrames[projectile.type] = 4;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.ZephyrFish);
			aiType = ProjectileID.ZephyrFish;
		}

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.zephyrfish = false;
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (player.dead)
			{
				modPlayer.remoteCopterPet = false;
			}
			if (modPlayer.remoteCopterPet)
			{
				projectile.timeLeft = 2;
			}
		}
	}
}