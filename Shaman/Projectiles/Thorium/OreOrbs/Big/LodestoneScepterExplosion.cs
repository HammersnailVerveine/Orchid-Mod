using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big
{
    public class LodestoneScepterExplosion : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 160;
            projectile.height = 160;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = 200;
            this.empowermentType = 4;
            this.empowermentLevel = 3;
            this.spiritPollLoad = 0;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion");
        }
		
        public override void AI()
        {
			for (int i = 0 ; i < 15 ; i ++) {
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 38);
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity.X /= 3;
				Main.dust[dust2].velocity.Y /= 3;
			}
			for (int i = 0 ; i < 15 ; i ++) {
				int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 182);
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity.X /= 3;
				Main.dust[dust2].velocity.Y /= 3;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				target.AddBuff((thoriumMod.BuffType("Sunder")), 10 * 60);
			}
			
			if (!(target.boss || target.type == NPCID.TargetDummy) && target.knockBackResist > 0f) {
				target.AddBuff((mod.BuffType("LodestoneSlow")), 10 * 60);
			}
		}
    }
}