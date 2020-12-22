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
    public class BersekerShardScepterProj : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 29;
			projectile.timeLeft = 60;
			projectile.scale = 1f;
			projectile.alpha = 196;
			projectile.penetrate = 2;
            this.empowermentType = 1;
            this.empowermentLevel = 4;
            this.spiritPollLoad = 0;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Berserker Bolt");
        } 
		
        public override void AI()
        {
			projectile.rotation += 0.2f;
			projectile.velocity *= 1.01f;
			
			int dust  = Main.rand.Next(2) == 0 ? 258 : 60;

			for (int i = 0 ; i < 2 ; i ++) {
				int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, dust, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
				Main.dust[DustID].noGravity = true;
				Main.dust[DustID].scale *= dust == 60 ? 1.5f : 1f;
				Main.dust[DustID].velocity = projectile.velocity / 3;
			}
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<4; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 60);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = projectile.velocity / 2;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (modPlayer.getNbShamanicBonds() > 4) {
				Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
				if (thoriumMod != null) {
					target.AddBuff((thoriumMod.BuffType("BerserkSoul")), 5 * 60);
				}
			}
		}
    }
}