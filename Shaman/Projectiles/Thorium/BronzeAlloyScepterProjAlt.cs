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
    public class BronzeAlloyScepterProjAlt : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 18;
            projectile.friendly = true;
            projectile.aiStyle = 1;
			projectile.timeLeft = 32;
			projectile.scale = 1f;
			aiType = ProjectileID.Bullet; 
            this.empowermentType = 4;
            this.empowermentLevel = 2;
            this.spiritPollLoad = 0;
			this.projectileTrail = true;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Basilisk Tooth");
        } 
		
        public override void AI()
        {
			int DustID = Dust.NewDust(new Vector2(projectile.position.X + 4, projectile.position.Y + 4), projectile.width / 3, projectile.height / 3, 64, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
			Main.dust[DustID].noGravity = true;
			Main.dust[DustID].velocity *= 0f;
			Main.dust[DustID].scale *= 1.5f;
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<4; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = projectile.velocity / 2;
				Main.dust[dust].scale *= 1.5f;
            }
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				target.AddBuff((thoriumMod.BuffType("Petrify")), 3 * 60);
			}
		}
    }
}