using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class StarScouterScepterProj : OrchidModShamanProjectile
	{
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orbital Mine");
        } 
		
		public override void SafeSetDefaults()
		{
            projectile.width = 18;
            projectile.height = 18;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.timeLeft = 120;
			Main.projFrames[projectile.type] = 2;
            this.empowermentType = 3;
            this.empowermentLevel = 2;
            this.spiritPollLoad = 0;
		}
		
        public override void AI()
        {	
			projectile.frame = (projectile.timeLeft > 80) ? 0 : 1;
			projectile.friendly = (projectile.timeLeft > 80) ? false : true;
			projectile.velocity *= (projectile.timeLeft > 80) ? 1f : 0f;
			
			if (projectile.timeLeft == 80) spawnDustCircle(62, 20);
		}
		
		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float offSet = this.projectileTrailOffset + 0.5f;
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * offSet, projectile.height * offSet);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[ProjectileType<Shaman.Projectiles.Thorium.StarScouterScepterProjAltExplosion>()], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0.3f);
			}
			return true;
		}
		
		public override void SafePostAI() {
            for (int num46 = projectile.oldPos.Length - 1; num46 > 0; num46--)
            {
                projectile.oldPos[num46] = projectile.oldPos[num46 - 1];
            }
            projectile.oldPos[0] = projectile.position;
        }
		
		public void spawnDustCircle(int dustType, int distToCenter) {
			for (int i = 0 ; i < 20 ; i ++ )
			{
				double dustDeg = (double) projectile.ai[1] * (i * (18));
				double dustRad = dustDeg * (Math.PI / 180);
				
				float posX = projectile.Center.X - (int)(Math.Cos(dustRad) * distToCenter) - projectile.width/4;
				float posY = projectile.Center.Y - (int)(Math.Sin(dustRad) * distToCenter) - projectile.height/4;
				
				Vector2 dustPosition = new Vector2(posX, posY);
				
				int index1 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);
				
				Main.dust[index1].velocity *= 0.05f;
				Main.dust[index1].fadeIn = 1f;
				Main.dust[index1].scale = 0.8f;
				Main.dust[index1].noGravity = true;
			}
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if (projectile.timeLeft > 80) {
				if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
				if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			}
            return false;
        }
		
		public override void Kill(int timeLeft)
        {
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			
			for (int i = 1 ; i < 6 ; i ++) {
				spawnDustCircle(62, i * 10);
			}
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 91);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, mod.ProjectileType("StarScouterScepterProjExplosion"), projectile.damage, 0.0f, player.whoAmI, 0.0f, 0.0f);
			
			if (nbBonds > 2) {
				for (int i = 0 ; i < 3 ; i ++) {
					
					Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(30));
					Projectile.NewProjectile(projectile.position.X, projectile.position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("StarScouterScepterProjAlt"), (int)(projectile.damage * 0.70), 0.0f, player.whoAmI, 0.0f, 0.0f);
				}
			}
        }
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
	}
}
