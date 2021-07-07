using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Projectiles.Chips
{
	public class RusalkaProjAlt : OrchidModGamblerProjectile
	{
		private Vector2 initialVelocity = new Vector2(0f, 0f);
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rusalka's Waters");
        } 
		
		public override void SafeSetDefaults()
		{
			projectile.width = 26;
            projectile.height = 26;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.timeLeft = 360;
			projectile.alpha = 255;
		}
		
        // public override Color? GetAlpha(Color lightColor)
        // {
            // return Color.White;
        // }
		
		public override void SafeAI() {
			bool moreDust = projectile.timeLeft > 300;
			if (Main.rand.Next(moreDust ? 5 : 10) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, Main.rand.Next(2) == 0 ? 29 : 59);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;	
				Main.dust[dust].velocity *= moreDust ? 2.5f : 1f;	
			}
			
			if (!this.initialized) {
				this.initialized = true;
				this.initialVelocity = new Vector2(projectile.velocity.X, projectile.velocity.Y);
				projectile.velocity *= 0f;
			}
			
			if (projectile.timeLeft == 300) {
				projectile.velocity = this.initialVelocity;
				this.projectileTrail = true;
				projectile.alpha = 0;
				projectile.friendly = true;
				projectile.aiStyle = 2;
			}
		}
		
		public override void Kill(int timeLeft) {
			for (int i = 0 ; i < 10 ; i ++) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, Main.rand.Next(2) == 0 ? 29 : 59);
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].noGravity = true;	
			}
		}
	}
}