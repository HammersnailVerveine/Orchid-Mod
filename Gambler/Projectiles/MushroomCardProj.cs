using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class MushroomCardProj : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosive Mushroom");
        } 
		
		public override void SafeSetDefaults()
		{
			projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = false;
            projectile.aiStyle = 2;
			projectile.timeLeft = 300;
			projectile.penetrate = 2;
			this.gamblingChipChance = 5;
		}
		
		public override void SafeAI()
		{
			projectile.friendly = projectile.penetrate < 2;
			
			if (projectile.timeLeft == 180) {
				int dustType = 172;
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
				Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
			}
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			projectile.penetrate--;
			projectile.timeLeft = 60;
            if (projectile.penetrate < 0) projectile.Kill();
            if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
            if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
            return false;
        }
		
		public override void Kill(int timeLeft) {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			OrchidModProjectile.spawnDustCircle(projectile.Center, 172, 25, 10, true, 1.5f, 1f, 5f);
			int projType = ProjectileType<Gambler.Projectiles.MushroomCardProjExplosion>();
			bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
			GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, projType, (int)(projectile.damage * 0.8), 3f, projectile.owner, 0.0f, 0.0f), dummy);
			int dustType = 172;
			Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
			int rand = 15 - (modPlayer.gamblerLuckySprout ? 3 : 0);
			if (Main.rand.Next(rand) == 0 && projectile.ai[1] != 1f && projectile.owner == Main.myPlayer) {
				Vector2 vel = (new Vector2(0f, -3f).RotatedBy(MathHelper.ToRadians(10)));
				projType = ProjectileType<Gambler.Projectiles.MushroomCardProjAlt>();
				GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(projectile.position.X, projectile.position.Y, vel.X, vel.Y, projType, projectile.damage, projectile.knockBack, projectile.owner), dummy);
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