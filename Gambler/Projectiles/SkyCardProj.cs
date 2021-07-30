using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class SkyCardProj : OrchidModGamblerProjectile
	{
		private float rotation = 0f;
	
		public override void SetStaticDefaults() {
            DisplayName.SetDefault("Banana Star");
        } 
		
		public override void SafeSetDefaults() {
			projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 300;
			this.gamblingChipChance = 15;
		}
		
        public override Color? GetAlpha(Color lightColor) {
            return Color.White;
        }
		
		public override void SafeAI() {
			if (!this.initialized) {
				this.rotation = 0.3f * (Main.rand.Next(2) == 0 ? -1 : 1);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 64, 5, 5, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
				this.initialized = true;
			}
			
			projectile.rotation += this.rotation;
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64);
			Main.dust[dust].scale *= 1.5f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = true;
			
			if (projectile.ai[0] != -1f && Main.myPlayer == projectile.owner) {
				if (projectile.position.Y >= projectile.ai[0]) {
					OrchidModProjectile.spawnDustCircle(projectile.Center, 64, 5, 5, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
					Vector2 mousePos = Main.MouseWorld;
					projectile.velocity.X = (mousePos.X > projectile.Center.X ? projectile.velocity.Y : - projectile.velocity.Y);
					projectile.velocity.Y = 0f;
					projectile.ai[0] = -1f;
					projectile.netUpdate = true;
				}
			}
		}
		
		public override void Kill(int timeLeft) {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 9);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 64, 5, 5, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);

			bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
			int dustType = 64;
			Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
			int rand = 40 - (modPlayer.gamblerLuckySprout ? 8 : 0);
			if (Main.rand.Next(rand) == 0 && projectile.ai[1] != 1f && projectile.owner == Main.myPlayer) {
				Vector2 vel = (new Vector2(0f, -3f).RotatedBy(MathHelper.ToRadians(10)));
				int projType = ProjectileType<Gambler.Projectiles.SkyCardProjAlt>();
				OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.position.X, projectile.position.Y, vel.X, vel.Y, projType, projectile.damage, projectile.knockBack, projectile.owner), dummy);
				for (int i = 0 ; i < 5 ; i ++) {
					Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
				}
			}
			for (int i = 0 ; i < 3 ; i ++) {
				Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].noGravity = true;
			}
		}
	}
}