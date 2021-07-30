using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class SkyCardProjAlt : OrchidModGamblerProjectile
	{
		private bool animDirection = false;
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Banana");
        } 
		
		public override void SafeSetDefaults()
		{
			projectile.width = 18;
            projectile.height = 22;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.tileCollide = false;
			projectile.timeLeft = 600;	
			Main.projFrames[projectile.type] = 5;
			this.bonusTrigger = true;
		}
		
		public override void Kill(int timeLeft) {
			for (int i = 0 ; i < 10 ; i ++) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1.5f;
			}
		}
		
		public override void SafeAI()
		{
			projectile.velocity *= 0.95f;
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (modPlayer.timer120 % 10 == 0) {
				projectile.frame += animDirection ? -1 : 1;
				animDirection = projectile.frame == 4 ? true : projectile.frame == 0 ? false : animDirection;
			}
		}
		
		public override void BonusProjectiles(Player player, OrchidModPlayer modPlayer, Projectile projectile, OrchidModGlobalProjectile modProjectile, bool dummy) {
			if (modProjectile.gamblerInternalCooldown == 0) {
				modProjectile.gamblerInternalCooldown = 30;
				int projType = ProjectileType<Gambler.Projectiles.SkyCardProj>();
				Vector2 target = Main.MouseWorld;
				Vector2 heading = target - projectile.position;
				heading.Normalize();
				heading *= new Vector2(0f, 15f).Length();
				Vector2 vel = new Vector2(0f, 8f).RotatedByRandom(MathHelper.ToRadians(20));
				int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.Center.X, player.position.Y - Main.screenHeight / 2 - 20, vel.X, vel.Y, projType, projectile.damage, projectile.knockBack, projectile.owner), dummy);
				Main.projectile[newProjectile].ai[1] = 1f;
				Main.projectile[newProjectile].ai[0] = Main.screenPosition.Y + (float)Main.mouseY - 10;
				Main.projectile[newProjectile].netUpdate = true;
				OrchidModProjectile.spawnDustCircle(projectile.Center, 64, 10, 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
			}
		}
	}
}