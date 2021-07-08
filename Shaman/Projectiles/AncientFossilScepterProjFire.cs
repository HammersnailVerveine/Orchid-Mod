using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
    public class AncientFossilScepterProjFire : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 50;	
            this.empowermentType = 1;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fire Bolt");
        }
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
        public override void AI()
        {
			for (int index1 = 0; index1 < 2; ++index1)
			{
				int index2 = Dust.NewDust(new Vector2(projectile.position.X + projectile.velocity.X/3, projectile.position.Y + projectile.velocity.Y/3), projectile.width, projectile.height, 6, projectile.velocity.X, projectile.velocity.Y, 100, new Color(), 1f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= (float) (0.100000001490116 + (double) Main.rand.Next(4) * 0.100000001490116);
				Main.dust[index2].scale *= (float) (1.5 + (double) Main.rand.Next(5) * 0.100000001490116);
			}	
        }
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<5; i++)
            {
				int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, projectile.velocity.X, projectile.velocity.Y, 100, new Color(), 1f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 3f;
				Main.dust[index2].scale *= (float) (1.0 + (double) Main.rand.Next(5) * 0.100000001490116);
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			target.AddBuff(BuffID.OnFire, 60 * 3);
		}
    }
}