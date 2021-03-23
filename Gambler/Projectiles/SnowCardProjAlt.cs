using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class SnowCardProjAlt : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pine Cone");
        } 
		
		public override void SafeSetDefaults()
		{
			projectile.width = 20;
            projectile.height = 24;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.tileCollide = false;
			projectile.timeLeft = 600;	
			projectile.alpha = 64;
			Main.projFrames[projectile.type] = 4;
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
				projectile.frame =  projectile.frame + 1 == 4 ? 0 : projectile.frame + 1;
			}
		}
		
		public override void BonusProjectiles(Player player, OrchidModPlayer modPlayer, Projectile projectile, OrchidModGlobalProjectile modProjectile, bool dummy) {
			if (modProjectile.gamblerInternalCooldown == 0) {
				modProjectile.gamblerInternalCooldown = 40;
				int projType = ProjectileType<Gambler.Projectiles.SnowCardProj>();
				Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
				Vector2 vel = new Vector2(0f, 0f);
				float absX = (float)Math.Sqrt((projectile.Center.X - target.X) * (projectile.Center.X - target.X));
				float absY = (float)Math.Sqrt((projectile.Center.Y - target.Y) * (projectile.Center.Y - target.Y));
				if (absX > absY) {
					vel.X = target.X < projectile.Center.X ? 1f : -1f;
				} else {
					vel.Y = target.Y < projectile.Center.Y ? 1f : -1f;
				}
				vel.Normalize();
				vel *= new Vector2(0f, 5f).Length();
				int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, projType, projectile.damage, projectile.knockBack, player.whoAmI), dummy);
				Main.projectile[newProjectile].ai[1] = 1f;
				Main.projectile[newProjectile].netUpdate = true;
				OrchidModProjectile.spawnDustCircle(projectile.Center, 31, 25, 10, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
			}
		}
	}
}