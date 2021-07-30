using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace OrchidMod.Gambler.Projectiles
{
    public class SapCardProj: OrchidModGamblerProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sap Bubble");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 26;
            projectile.height = 26;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.alpha = 126;
			projectile.timeLeft = 1200;
			projectile.penetrate = -1;
			ProjectileID.Sets.Homing[projectile.type] = true;
			this.gamblingChipChance = 5;
        }
		
		public override void SafeAI()
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			projectile.rotation += 0.05f;
			
			if (projectile.timeLeft > 120) {
				projectile.velocity.Y += 0.01f;
				projectile.velocity.X *= 0.95f;
			}
			
			if (Main.rand.Next(15) == 0) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 102);
				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 1f;
            }
			if (Main.myPlayer == projectile.owner) {
				if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.SapCard>() && modPlayer.GamblerDeckInHand) {
					Vector2 newMove = Main.MouseWorld - projectile.Center;
					AdjustMagnitude(ref newMove);
					projectile.velocity = (5 * projectile.velocity + newMove);
					AdjustMagnitude(ref projectile.velocity);
					
					int velocityXBy1000 = (int)(newMove.X * 1000f);
					int oldVelocityXBy1000 = (int)(projectile.velocity.X * 1000f);
					int velocityYBy1000 = (int)(newMove.Y * 1000f);
					int oldVelocityYBy1000 = (int)(projectile.velocity.Y * 1000f);

					if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000) {
						projectile.netUpdate = true;
					}
				} else {
					projectile.Kill();
				}
			}
        }
		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 2f)
			{
				vector *= 2f / magnitude;
			}
		}
		
		public override void Kill(int timeLeft)
        {
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 85);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 102, 100, 20, false, 1.5f, 1f, 5f);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 102, 150, 20, false, 1.5f, 1f, 5f);
			int dmg = projectile.damage + (int)((1200 - timeLeft) / 10);
			int projType = ProjectileType<Gambler.Projectiles.SapCardProjExplosion>();
			bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, projType, dmg, 3f, projectile.owner, 0.0f, 0.0f), dummy);
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {
			if (modPlayer.gamblerElementalLens) {
				target.AddBuff(20, 60 * 5); // Poisoned
			}
        }
    }
}