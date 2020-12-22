using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Projectiles
{
	public class OceanCardProjAlt : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fartling ball");
        } 
		
		public override void SafeSetDefaults()
		{
			projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.tileCollide = false;
			projectile.timeLeft = 600;	
			Main.projFrames[projectile.type] = 5;
		}
		
		public override void Kill(int timeLeft) {
			for (int i = 0 ; i < 10 ; i ++) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 31);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
		}
		
		public override void SafeAI()
		{
			projectile.velocity *= 0.95f;
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (modPlayer.timer120 == 0 || modPlayer.timer120 > 49) projectile.frame = 0;
			if (modPlayer.timer120 == 10) projectile.frame = 1;
			if (modPlayer.timer120 == 20) projectile.frame = 2;
			if (modPlayer.timer120 == 30) projectile.frame = 3;
			if (modPlayer.timer120 == 40) projectile.frame = 4;
		}
	}
}