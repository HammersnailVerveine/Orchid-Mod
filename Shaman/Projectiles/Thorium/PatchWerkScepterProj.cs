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
    public class PatchWerkScepterProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 120;
			projectile.scale = 1f;
			aiType = ProjectileID.Bullet; 	
			projectile.alpha = 196;
            this.empowermentType = 4;
            this.empowermentLevel = 2;
            this.spiritPollLoad = 0;
			this.projectileTrail = true;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Festering Bolt");
        } 
		
        public override void AI()
        {
			projectile.rotation += 0.2f;
			projectile.velocity.Y += 0.2f;
			projectile.velocity.X *= 0.99f;

			int dust  = 5;
			if (Main.rand.Next(4) == 0) {
				switch (Main.rand.Next(2)) {
					case 0:
						dust = 258;
						break;
					case 1:
						dust = 60;
						break;			
					default:
						break;
				}
			}

			int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, dust, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
			Main.dust[DustID].noGravity = true;
			Main.dust[DustID].velocity = -projectile.velocity / 2;
			Main.dust[DustID].scale *= 1.5f;
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<4; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 258);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = -projectile.velocity / 2;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) > 1) {
				Projectile.NewProjectile(player.Center.X, player.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("PatchWerkScepterProjAlt"), projectile.damage, 0f, 0, 0f, 0f);
			}
		}
    }
}