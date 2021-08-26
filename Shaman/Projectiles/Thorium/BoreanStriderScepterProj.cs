using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class BoreanStriderScepterProj : OrchidModShamanProjectile
	{
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Borean egg");
        } 
		
		public override void SafeSetDefaults()
		{
            projectile.width = 18;
            projectile.height = 18;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 45;
            this.projectileTrail = true;
		}
		
        public override void AI()
        {	
		    if (Main.rand.Next(2) == 0)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].velocity = projectile.velocity;
            }
		}
		
		public override void Kill(int timeLeft)
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			
			for (int i = 0 ; i < nbBonds ; i ++) {	
				Vector2 perturbedSpeed = new Vector2(projectile.velocity.X / (Main.rand.Next(3) + 2), -3f).RotatedByRandom(MathHelper.ToRadians(30));
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("BoreanStriderScepterProjAlt"), (int)(projectile.damage * 0.70), 0.0f, player.whoAmI, 0.0f, 0.0f);
			}
			
			for(int i=0; i<5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].velocity = projectile.velocity / 3;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null) {
				target.AddBuff((thoriumMod.BuffType("Freezing")), 2 * 60);
			}
		}
	}
}