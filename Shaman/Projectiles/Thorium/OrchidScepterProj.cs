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
    public class OrchidScepterProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 28;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 30;
			projectile.scale = 1f;
			aiType = ProjectileID.Bullet; 
			projectile.penetrate = 2;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petal");
        } 
		
        public override void AI()
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			
			if (projectile.timeLeft == 30) {
				float rand = (float)(Main.rand.Next(4) - 2f);
				projectile.velocity.X += rand;
				projectile.velocity.Y += rand;
				
				if (nbBonds > 2) {
					projectile.penetrate = 2;
				}
			}
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustType<PollenDust>());
			Main.dust[dust].velocity *= 1.5f;
			Main.dust[dust].noGravity = true;
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<2; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustType<PollenDust>());
				Main.dust[dust].noGravity = false;
				Main.dust[dust].scale *= 1.2f;
				Main.dust[dust].velocity *= 2f;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null) {
				player.AddBuff((thoriumMod.BuffType("OverGrowth")), 3 * 60);
			}
		}
    }
}