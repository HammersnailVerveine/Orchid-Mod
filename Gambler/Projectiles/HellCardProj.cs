using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class HellCardProj : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pepper Mortar");
        } 
		
		public override void SafeSetDefaults()
		{
			projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.aiStyle = 2;
			projectile.timeLeft = 300;
			this.gamblingChipChance = 5;
		}
		
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
		public override void SafeAI()
		{
			if (!this.initialized) {
				projectile.velocity.Y = projectile.velocity.Y > -10f ? -10f : projectile.velocity.Y;
				this.initialized = true;
			}
			if (projectile.timeLeft == 180) {
				int dustType = 6;
				Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
				Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, dustType)].velocity *= 0.25f;
			}
			if (Main.rand.Next(3) == 0) {
				int index1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
				Main.dust[index1].velocity = projectile.velocity * 0.5f;
				Main.dust[index1].fadeIn = 1f;
				Main.dust[index1].scale *= 1.5f;
				Main.dust[index1].noGravity = true;
			}
		}
		
		public override void Kill(int timeLeft) {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 6, 25, 10, true, 1.5f, 1f, 5f);
			int projType = ProjectileType<Gambler.Projectiles.MushroomCardProjExplosion>();
			bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
			GamblerAttackHelper.DummyProjectile(Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, projType, projectile.damage, 3f, projectile.owner, 0.0f, 0.0f), dummy);
			int dustType = 6;
			Vector2 pos = new Vector2(projectile.position.X, projectile.position.Y);
			int rand = 15 - (modPlayer.gamblerLuckySprout ? 3 : 0);
			if (Main.rand.Next(rand) == 0 && projectile.ai[1] != 1f && projectile.owner == Main.myPlayer) {
				Vector2 vel = (new Vector2(0f, -3f).RotatedBy(MathHelper.ToRadians(10)));
				projType = ProjectileType<Gambler.Projectiles.HellCardProjAlt>();
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