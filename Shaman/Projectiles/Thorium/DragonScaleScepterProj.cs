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
    public class DragonScaleScepterProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 60;
			projectile.penetrate = 10;
			projectile.scale = 1f;
			aiType = ProjectileID.Bullet; 	
			projectile.alpha = 196;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Draconic Fire Bolt");
        } 
		
        public override void AI()
        {
			projectile.rotation += 0.2f;
			
			if (projectile.penetrate == 10) {
				Player player = Main.player[projectile.owner];
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();	
				projectile.penetrate = 1 + OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			}
			

			int dust  = 75;
			
			switch (Main.rand.Next(3)) {
				case 0:
					dust = 75;
					break;
				case 1:
					dust = 74;
					break;
				case 2:
					dust = 61;
					break;
				default:
					break;
			}
			for (int i = 0 ; i < 2 ; i ++) {
				int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, dust, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
				Main.dust[DustID].noGravity = true;
				Main.dust[DustID].velocity = projectile.velocity / 3;
			}
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<4; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 75);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = projectile.velocity / 2;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			target.AddBuff(39, 60 * 2); // Cursed Inferno
		}
    }
}