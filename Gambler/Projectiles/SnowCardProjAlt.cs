using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Projectiles
{
	public class SnowCardProjAlt : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pine Cone");
        } 
		
		public override void SafeSetDefaults()
		{
			projectile.width = 20;
            projectile.height = 24;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.tileCollide = false;
			projectile.timeLeft = 600;	
			projectile.alpha = 64;
			Main.projFrames[projectile.type] = 4;
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
			if (modPlayer.timer120 % 10 == 0) {
				projectile.frame =  projectile.frame + 1 == 4 ? 0 : projectile.frame + 1;
			}
		}
	}
}