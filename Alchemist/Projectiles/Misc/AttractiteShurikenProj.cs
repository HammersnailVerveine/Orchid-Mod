using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Alchemist.Projectiles.Misc
{
    public class AttractiteShurikenProj : OrchidModAlchemistProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Attractite Shuriken");
        } 
		
        public override void SafeSetDefaults() {
            projectile.width = 22;
            projectile.height = 22;
            projectile.aiStyle = 2;
			projectile.timeLeft = 600;
			projectile.friendly = true;
			projectile.penetrate = 3;
        }
		
		public override void SafeOnHitNPC(NPC target, OrchidModAlchemistNPC modTarget, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {
			target.AddBuff(BuffType<Alchemist.Buffs.Debuffs.Attraction>(), 60 * 5);
		}
    }
}