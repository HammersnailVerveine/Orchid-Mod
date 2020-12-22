using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod;

namespace OrchidMod.Shaman.Projectiles.Bonds
{
    public class FireBondExplosion5 : OrchidModShamanProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 150;
            projectile.height = 150;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = 200;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion");
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			OrchidModGlobalNPC modTarget = target.GetGlobalNPC<OrchidModGlobalNPC>();
			
			target.AddBuff(BuffID.OnFire, 60 * 5);
			modTarget.shamanBomb = 300;
		}
    }
}