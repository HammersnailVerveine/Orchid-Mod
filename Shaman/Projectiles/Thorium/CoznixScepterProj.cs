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
    public class CoznixScepterProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 55;
			projectile.scale = 1f;
			projectile.alpha = 128;
			aiType = ProjectileID.Bullet; 
            this.empowermentType = 1;
            this.empowermentLevel = 3;
            this.spiritPollLoad = 0;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Bolt");
        } 
		
        public override void AI()
        {
			int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 60, projectile.velocity.X * 0.75f, projectile.velocity.Y * 0.75f, 125, default(Color), 1.25f);
			Main.dust[DustID].scale *= 1.5f;
			Main.dust[DustID].noGravity = true;
			
			DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, projectile.velocity.X, projectile.velocity.Y * 0.75f, 125, default(Color), 1.25f);
			Main.dust[DustID].scale *= 1.5f;
			Main.dust[DustID].noGravity = true;
			
			DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 90, projectile.velocity.X * 1.5f, projectile.velocity.Y * 1.5f, 125, default(Color), 1.25f);
			Main.dust[DustID].scale *= 1f;
			Main.dust[DustID].noGravity = true;
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
            for (int l = 0; l < Main.projectile.Length; l++)
            {  
                Projectile proj = Main.projectile[l];
                if (proj.active && proj.type == mod.ProjectileType("CoznixScepterProjPortal") && proj.owner == player.whoAmI)
                {
                    proj.active = false;
                }
            }
			
			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) > 2) {
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y - 300, 0f, 0f, mod.ProjectileType("CoznixScepterProjPortal"), 0, 0.0f, projectile.owner, 0.0f, 0.0f);
			}
		}
    }
}