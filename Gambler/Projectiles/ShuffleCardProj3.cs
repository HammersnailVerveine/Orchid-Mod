using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class ShuffleCardProj3 : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clover");
        } 
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
		public override void SafeSetDefaults()
		{
			projectile.width = 18;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 600;
			projectile.penetrate = -1;
			this.gamblingChipChance = 10;
			this.projectileTrail = true;
		}
		
		public override void SafeAI()
		{
			if (Main.rand.Next(12) == 0) {
				int dustType = 63;
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
				int index = Dust.NewDust(pos, projectile.width, projectile.height, dustType);
				Main.dust[index].velocity *= 0.25f;
				Main.dust[index].scale *= 1.5f;
				Main.dust[index].noGravity = true;
			}
			projectile.velocity *= 0.98f;
			projectile.rotation += 0.3f;
		}
		
		public override void Kill(int timeLeft) {
			for (int i = 0 ; i < 3 ; i ++) {
				int dustType = 63;
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
				int index = Dust.NewDust(pos, projectile.width, projectile.height, dustType);
				Main.dust[index].velocity *= 0.25f;
				Main.dust[index].scale *= 1.5f;
				Main.dust[index].noGravity = true;
			}
		}
	}
}