using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
	public class StarpowerScepterProj : OrchidModShamanProjectile
	{
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star");
        } 
		
		public override void SafeSetDefaults()
		{
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 29;
			projectile.timeLeft = 120;
			this.empowermentType = 3;
			this.empowermentLevel = 1;
			this.spiritPollLoad = 0;
		}
		
        public override void AI()
        {			
			projectile.rotation += 0.5f;
            projectile.velocity = projectile.velocity * 0.95f;
			projectile.alpha += 2;
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
			Main.dust[dust].velocity /= 10f;
			Main.dust[dust].scale = 1f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = false;
			
			if (!this.initialized) {
				this.initialized = true;
				Player player = Main.player[projectile.owner];
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				int newCrit = 10 * modPlayer.getNbShamanicBonds() + modPlayer.shamanCrit + player.inventory[player.selectedItem].crit;
				OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
				modProjectile.baseCritChance = newCrit;
			}
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
            }
        }
	}
}