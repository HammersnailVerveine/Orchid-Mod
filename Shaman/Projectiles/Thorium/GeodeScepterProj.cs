using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class GeodeScepterProj : OrchidModShamanProjectile
	{
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Geode Cluster");
        } 
		
		public override void SafeSetDefaults()
		{
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.aiStyle = 2;
			projectile.timeLeft = 45;
            this.empowermentType = 4;
            this.empowermentLevel = 3;
            this.spiritPollLoad = 0;
			this.projectileTrail = true;
		}
		
		public override void Kill(int timeLeft)
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			
			for (int i = 0 ; i < 3 + modPlayer.getNbShamanicBonds() ; i ++) {	
				Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(40));
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("GeodeScepterProjAlt"), (int)(projectile.damage * 0.70), 0.0f, player.whoAmI, 0.0f, 0.0f);
			}
			
			for(int i=0; i<10; i++) {
				
				int dustType = 60;
				switch (Main.rand.Next(3)) {
					case 0:
						dustType = 60;
						break;
					case 1:
						dustType = 59;
						break;
					case 2:
						dustType = 62;
						break;
					default:
						break;
				}
				
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
				Main.dust[dust].scale *= 1.5f * ((Main.rand.Next(20) + 5 ) / 10);
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
	}
}