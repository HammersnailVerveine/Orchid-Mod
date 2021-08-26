using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
    public class SpineScepterProj : OrchidModShamanProjectile
    {
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spine Beam");
        } 
        public override void SafeSetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.penetrate = 5;
			projectile.timeLeft = 30;
            projectile.extraUpdates = 10;		
			projectile.ignoreWater = true; 	
        }
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {    
		    for(int i=0; i<2; i++)
			{
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 258);
				Main.dust[dust].velocity /= 3f;
				Main.dust[dust].scale = 1.3f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
            }
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
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 258);
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
    }
}