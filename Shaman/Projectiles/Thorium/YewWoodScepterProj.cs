using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod.Dusts.Thorium;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
    public class YewWoodScepterProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 8;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 55;
			projectile.scale = 1f;
			projectile.alpha = 128;
			aiType = ProjectileID.Bullet; 
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadowflame Bolt");
        } 
		
        public override void AI()
        {
			if (projectile.timeLeft == 55) {
				projectile.ai[0] = (((float)Main.rand.Next(10) / 10f) - 0.5f);
			}
			
			projectile.velocity *= 1.03f;
			
			Vector2 projectileVelocity = ( new Vector2(projectile.velocity.X, projectile.velocity.Y ).RotatedBy(MathHelper.ToRadians(projectile.ai[0])));
			projectile.velocity = projectileVelocity;
			projectile.netUpdate = true;
			
			int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 27, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
			Main.dust[DustID].noGravity = true;
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<4; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = projectile.velocity / 2;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			float randX = target.position.X + (target.width / 2) + Main.rand.Next(700) - 350;
			float randY = target.position.Y + (target.height / 2) - Main.rand.Next(300) - 100;
			
			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) > 2 && Main.rand.Next(7) == 0) {
				Projectile.NewProjectile(randX, randY, 0f, 0f, mod.ProjectileType("YewWoodScepterPortal"), 0, 0.0f, projectile.owner, 0.0f, 0.0f);
			}
		}
    }
}