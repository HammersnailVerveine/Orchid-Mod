using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
    public class IceFlakeConeProj : OrchidModShamanProjectile
    {
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Flake");
        } 
        public override void SafeSetDefaults()
        {
            projectile.width = 26;
            projectile.height = 26;
            projectile.friendly = true;
            projectile.aiStyle = 3;
			projectile.penetrate = 20;
			projectile.timeLeft = 300;
            projectile.extraUpdates = 1;
        }
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {    
		    for (int index1 = 0; index1 < 1; ++index1)
            {
				projectile.rotation += 0.6f;	
				int index = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 67, projectile.velocity.X, projectile.velocity.Y, 100, new Color(), 1f);
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity *= (float) (0.100000001490116 + (double) Main.rand.Next(4) * 0.100000001490116);
                Main.dust[index].scale *= (float) (1.0 + (double) Main.rand.Next(5) * 0.100000001490116);
            }
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (Main.rand.Next(5) == 0){
				target.AddBuff(44, 360);	
			}
		}
    }
}