using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Projectiles
{
    public class SlimeRainCardProj2: OrchidModGamblerProjectile
	{
		private bool greenSlime = false;
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gambler Slime");
        } 
		
		public override void SafeSetDefaults()
		{
			projectile.width = 16;
            projectile.height = 10;
            projectile.aiStyle = 63;
            projectile.friendly = true;
            projectile.timeLeft = 180;
            projectile.penetrate = 10;
			projectile.scale = 1f;
			projectile.alpha = 64;
			Main.projFrames[projectile.type] = 4;
			this.gamblingChipChance = 10;
		}
		
        public override void AI() {
			if (projectile.ai[1] == 1f) {
				this.initialized = true;
			} 
			
			if (projectile.timeLeft == 180) {
				this.greenSlime = Main.rand.Next(2) == 0;
			}
			
			projectile.aiStyle = this.initialized ? 63 : 0;
			
			if (projectile.timeLeft % 10 == 0 && !this.initialized) {
				projectile.damage ++;
			}
			
			if (projectile.velocity.Y > 8) projectile.velocity.Y = 8;
			if (projectile.velocity.X > 4) projectile.velocity.X = 4;
			if (projectile.velocity.X < -4) projectile.velocity.X = -4;
			projectile.frame = projectile.velocity.Y < 0f ? 1 + (this.greenSlime ? 2 : 0) : 0 + (this.greenSlime ? 2 : 0);
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			projectile.timeLeft = this.initialized ? projectile.timeLeft : 90;
			this.initialized = true;
            projectile.velocity.Y = -3;
            return false;
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			this.gamblingChipChance = 3;
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<5; i++)
            {
				Color dustColor = this.greenSlime ? new Color(0, 255, 70, 0) : new Color(0, 80, 255, 0);
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 4, 0.0f, 0.0f, 175, dustColor);
            }
        }
    }
}
