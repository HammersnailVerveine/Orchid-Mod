using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class BoreanStriderScepterProjAlt : OrchidModShamanProjectile
	{
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Borean icicle");
        } 
		
		public override void SafeSetDefaults()
		{
            projectile.width = 18;
            projectile.height = 18;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 120;
			projectile.penetrate = 3;
            this.empowermentType = 2;
		}

		public override void AI()
        {	
			projectile.friendly = projectile.timeLeft < 170;
            if (Main.rand.Next(3) == 0)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].velocity = projectile.velocity / 4;
            }
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<2; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].velocity = projectile.velocity / 4;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				target.AddBuff((thoriumMod.BuffType("Freezing")), 2 * 60);
			}
		}
	}
}