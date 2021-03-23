using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class OceanCardProj : OrchidModGamblerProjectile
	{
		private bool firstCollide = false;
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coconut");
        } 
		
		public override void SafeSetDefaults()
		{
			projectile.width = 26;
            projectile.height = 26;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 600;
			projectile.penetrate = 10;
			this.gamblingChipChance = 5;
		}
		
		public override void SafeAI()
		{
			projectile.velocity.Y += 0.1f;
			if (projectile.velocity.Y > 0.5f) {
				this.firstCollide = false;
			}
			if (projectile.velocity.X > 0f && projectile.velocity.X < 3f) {
				projectile.velocity.X = 3f;
			}
			if (projectile.velocity.X <= 0f && projectile.velocity.X > -3f) {
				projectile.velocity.X = -3f;
			}
			
			projectile.rotation += projectile.velocity.X > 0 ? 0.15f : -0.15f;
			if (projectile.timeLeft % 600 == 0) {
				int dustType = 31;
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
				Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
			}
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.penetrate < 0) projectile.Kill();
			projectile.velocity.Y = oldVelocity.Y < 0f ? -oldVelocity.Y : 0f;
			if (!firstCollide) {
				firstCollide = true;
			}
            if (projectile.velocity.X != oldVelocity.X) {
				projectile.Kill();
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			}
            return false;
        }
		
		public override void Kill(int timeLeft) {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int dustType = 31;
			Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
			int rand = 10 - (modPlayer.gamblerLuckySprout ? 2 : 0);
			if (Main.rand.Next(rand) == 0 && projectile.ai[1] != 1f && projectile.owner == Main.myPlayer) {
				Vector2 vel = (new Vector2(0f, -3f).RotatedBy(MathHelper.ToRadians(10)));
				int projType = ProjectileType<Gambler.Projectiles.OceanCardProjAlt>();
				bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
				OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.position.X, projectile.position.Y, vel.X, vel.Y, projType, projectile.damage, projectile.knockBack, projectile.owner), dummy);
				for (int i = 0 ; i < 5 ; i ++) {
					Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
				}
			}
			Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
		}
	}
}