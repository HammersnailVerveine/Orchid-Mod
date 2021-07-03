using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class GoblinArmyCardProjAlt : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblin Bolt");
        } 
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
		public override void SafeSetDefaults()
		{
			projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 90;
			projectile.penetrate = 3;	
			projectile.alpha = 128;	
			this.gamblingChipChance = 5;
			this.projectileTrail = true;
		}
		
		public override void SafeAI()
		{
			projectile.rotation += 0.2f;
			
			if (Main.rand.Next(2) == 0) {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27);
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].noGravity = true;
			}
		}
		
		public override void Kill(int timeLeft) {
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 27, 5, 5, true, 1.3f, 1f, 3f, true, true, false, 0, 0, true);
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {
			if (modPlayer.gamblerElementalLens) {
				target.AddBuff(153, 60 * 2); // Shadowflame
			}
        }
	}
}