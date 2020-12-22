using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Projectiles
{
    public class DepthBatonProjAlt : OrchidModShamanProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Depth Beam");
        } 
        public override void SafeSetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.aiStyle = 29;
			projectile.timeLeft = 45;
            projectile.extraUpdates = 10;
			projectile.alpha = 255;
			projectile.ignoreWater = true; 
            this.empowermentType = 5;
            this.empowermentLevel = 2;
            this.spiritPollLoad = 0;
        }

        public override void AI()
        {    
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 70);
			Main.dust[dust].velocity /= 3f;
			Main.dust[dust].scale = 1.3f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = true;
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			projectile.Kill();
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			return false;
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<7; i++)
            {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 70);
				Main.dust[dust].scale = 1.3f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}