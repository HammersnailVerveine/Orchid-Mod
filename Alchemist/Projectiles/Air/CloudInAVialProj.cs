using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Projectiles.Air
{
    public class CloudInAVialProj : OrchidModAlchemistProjectile
    {
        public override void SafeSetDefaults()
        {
            projectile.width = 100;
            projectile.height = 100;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 3;
			projectile.tileCollide = false;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Airxplosion");
        }
		
		public override void AI() {
			for (int l = 0; l < Main.npc.Length; l++) {  
				NPC target = Main.npc[l];
				if (projectile.Hitbox.Intersects(target.Hitbox) && !(target.boss || target.type == NPCID.TargetDummy) && target.knockBackResist > 0f)  {
					target.velocity.Y = -(projectile.ai[1] * 4);
				}
			}
		}
    }
}