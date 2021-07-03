using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class GoblinArmyCardProj : OrchidModGamblerProjectile
	{
		private int fireTimer = 60; 
		private int fireTimerRef = 60; 
		private double dustVal = 0;
		private static float distance = 300f;
		private Vector2 baseVelocity = Vector2.Zero;
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblin Portal");
        } 
		
		public override void SafeSetDefaults()
		{
			projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = false;
            projectile.aiStyle = 0;
			projectile.penetrate = -1;	
			projectile.alpha = 100;	
			this.gamblingChipChance = 5;
		}
		
		public override void SafeAI() {
			if (!this.initialized) {
				this.initialized = true;
				this.baseVelocity = projectile.velocity;
			}
			
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			projectile.rotation += projectile.ai[1] * 0.05f;
			this.fireTimer --;
			this.dustVal --;
			projectile.velocity += this.baseVelocity / 100f;
			
			if (Main.rand.Next(20) == 0) {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27);
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].noGravity = true;
			}

			if (Main.myPlayer == projectile.owner) {
				Vector2 vectorDist = player.Center - projectile.Center;
				float distanceTo = (float)Math.Sqrt(vectorDist.X * vectorDist.X + vectorDist.Y * vectorDist.Y);
				if ((!(Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.GoblinArmyCard>() && modPlayer.GamblerDeckInHand) && projectile.timeLeft < 840) || distanceTo > distance) {
					bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
					OrchidModProjectile.spawnDustCircle(projectile.Center, 27, 5, 5, true, 1.3f, 1f, 5f, true, true, false, 0, 0, true);
					OrchidModProjectile.spawnDustCircle(projectile.Center, 27, 5, 5, true, 1.3f, 1f, 3f, true, true, false, 0, 0, true);
					projectile.Kill();
				}
			}
			
			if (fireTimer <= 0 && projectile.ai[1] == 1f) {
				Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
				Vector2 heading = target - projectile.Center;
				heading.Normalize();
				heading *= 15f;
				int projType = ProjectileType<Gambler.Projectiles.GoblinArmyCardProjAlt>();
				bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
				OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, heading.X, heading.Y, projType, projectile.damage, projectile.knockBack, projectile.owner), dummy);
					OrchidModProjectile.spawnDustCircle(projectile.Center, 27, 5, 5, true, 1.3f, 1f, 3f, true, true, false, 0, 0, true);
				fireTimerRef -= fireTimerRef > 15 ? 4 : 0;
				fireTimer = fireTimerRef;
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 8);
			}
			
			this.spawnDust(27, (int)distance);
		}
		
		public void spawnDust(int dustType, int distToCenter) {
			for (int i = 0 ; i < 3 ; i ++ ) {
				double deg = (2 * (42 + this.dustVal) + i * 120);
				double rad = deg * (Math.PI / 180);
						 
				float posX = projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) + projectile.velocity.X - 4;
				float posY = projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) + projectile.velocity.Y - 4;
						
				Vector2 dustPosition = new Vector2(posX, posY);
						
				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
						
				Main.dust[index2].velocity = projectile.velocity / 2;
				Main.dust[index2].scale = 2f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = true;
			}
		}
	}
}