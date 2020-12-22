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
    public class LichScepterProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 10;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 70;
			projectile.scale = 1f;
			aiType = ProjectileID.Bullet; 	
            this.empowermentType = 3;
            this.empowermentLevel = 4;
            this.spiritPollLoad = 0;
			this.projectileTrail = true;
        }
		
		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Necrotic Bolt");
        } 
		
        public override void AI()
        {
			if (projectile.timeLeft > 42) projectile.velocity *= 1.1f;
			
			if (Main.rand.Next(2) == 0)
			{
				int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 15, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
				Main.dust[DustID].noGravity = true;
			}
			
			if (Main.rand.Next(2) == 0)
			{
				int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 127, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
				Main.dust[DustID].noGravity = true;
			}
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<4; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 15);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = projectile.velocity / 2;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			for (int i = 0 ; i < 4 ; i ++ ) {
				Vector2 projectileVelocity = (new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(-40 + (25 * i))));
				Projectile.NewProjectile(projectile.position.X + projectileVelocity.X, projectile.position.Y + projectile.velocity.Y, projectileVelocity.X / 2, projectileVelocity.Y / 2, mod.ProjectileType("LichScepterProjAlt"), (int)(projectile.damage * 0.75), 0.0f, projectile.owner, 0.0f, 0.0f);
			}
		}
    }
}