using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class HellCardProjAlt : OrchidModGamblerProjectile
	{
		private bool animDirection = false;
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Acorn");
        } 
		
		public override void SafeSetDefaults()
		{
			projectile.width = 20;
            projectile.height = 24;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.tileCollide = false;
			projectile.timeLeft = 600;	
			Main.projFrames[projectile.type] = 5;
			this.bonusTrigger = true;
		}
		
		public override void Kill(int timeLeft) {
			for (int i = 0 ; i < 10 ; i ++) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 31);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
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
				int projType = ProjectileType<Gambler.Projectiles.HellCardProj>();
				Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
				Vector2 heading = target - projectile.position;
				heading.Normalize();
				heading *= new Vector2(0f, 15f).Length();
				int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, heading.X, heading.Y, projType, projectile.damage, projectile.knockBack, player.whoAmI), dummy);
				Main.projectile[newProjectile].ai[1] = 1f;
				Main.projectile[newProjectile].netUpdate = true;
				OrchidModProjectile.spawnDustCircle(projectile.Center, 6, 10, 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
			}
		}
	}
}