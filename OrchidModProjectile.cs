using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using OrchidMod;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod
{
    public abstract class OrchidModProjectile : ModProjectile
    {
		public int baseCritChance = 4; // Projectile Crit chance without player modifiers. Must equal weapon crit for coherence
		public bool projectileTrail = false; // Will the projectile leave a trail of afterimages ?
		public float projectileTrailOffset = 0f; // Offcenters the afterimages a bit. useless without projectileTrail activated. Looks terrible on most projectiles.
		public bool initialized; // Used in various AI.
		public bool projOwner = false;
		
		protected bool spawned; // Required for OnSpawn()

		public sealed override bool PreAI()
		{
			if (!spawned)
			{
				spawned = true;
				OnSpawn();
			}

			return OrchidPreAI();
		}

		public virtual bool OrchidPreAI() { return true; }

		public virtual void OnSpawn() { } // Called when projectile is created

		public virtual void AltSetDefaults() {}
		
		public virtual void SafeSetDefaults() {}
			
		public virtual void SafePostAI() {}
	
		public sealed override void SetDefaults() {
			AltSetDefaults();
		}
		
		public sealed override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (projectileTrail) 
			{
				PreDrawTrail(spriteBatch, lightColor);
			}
			return OrchidPreDraw(spriteBatch, lightColor);
		}

		public virtual bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor) { return true; }
		
		public sealed override void PostAI() {
			SafePostAI();
			if (this.projectileTrail) {
				PostAITrail();
			}
		}
		
		public void Bounce(Vector2 oldVelocity, float speedMult = 1f, bool reducePenetrate = false) {
			if (reducePenetrate) {
				projectile.penetrate --;
				if (projectile.penetrate < 0) projectile.Kill();
			}
            if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X  * speedMult;
            if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y * speedMult;
		}
		
		public static void DrawProjectileGlowmask(Projectile projectile, SpriteBatch spriteBatch, Texture2D texture, Color color, float offsetX = 0, float offsetY = 0) {
			spriteBatch.Draw(texture, projectile.position - Main.screenPosition + projectile.Size * 0.5f + new Vector2(offsetX, offsetY), null, color, projectile.rotation, texture.Size() * 0.5f, projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
		}
		
		public void PreDrawTrail(SpriteBatch spriteBatch, Color lightColor)
		{
			float offSet = this.projectileTrailOffset + 0.5f;
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * offSet, projectile.height * offSet);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0.3f);
			}
		}
		
		public void PostAITrail() {
            for (int num46 = projectile.oldPos.Length - 1; num46 > 0; num46--)
            {
                projectile.oldPos[num46] = projectile.oldPos[num46 - 1];
            }
            projectile.oldPos[0] = projectile.position;
        }
		
		public static void resetIFrames(Projectile projectile) {
			for (int l = 0; l < Main.npc.Length; l++) {  
				NPC target = Main.npc[l];
				if (projectile.Hitbox.Intersects(target.Hitbox))  {
					target.immune[projectile.owner] = 0;
				}
			}
		}
		
		public static void spawnExplosionGore(Projectile projectile) {
			for (int g = 0; g < 2; g++) {
				int goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[goreIndex].scale = 1.5f;
				Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
				Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
				goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[goreIndex].scale = 1.5f;
				Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
				Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
				goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[goreIndex].scale = 1.5f;
				Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
				Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
				goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[goreIndex].scale = 1.5f;
				Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
				Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
			}
		}
		
		public static void spawnGenericExplosion(Projectile projectile, int damage, float kb, int dimensions = 250, int damageType = 0, bool explosionGore = false, int soundType = 14) {
			if (soundType != 0) Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			if (explosionGore) OrchidModProjectile.spawnExplosionGore(projectile);
			int projType = ProjectileType<General.Projectiles.GenericExplosion>();
			int newProjectileInt = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, projType, damage, kb, projectile.owner);
			Projectile newProjectile = Main.projectile[newProjectileInt];
			newProjectile.width = dimensions;
			newProjectile.height = dimensions;
			newProjectile.position.X = projectile.Center.X - (newProjectile.width / 2);
			newProjectile.position.Y = projectile.Center.Y - (newProjectile.width / 2);
			
			if (damageType != 0) {
				OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
				OrchidModGlobalProjectile modProjectileNew = newProjectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
				modProjectileNew.baseCritChance = modProjectile.baseCritChance;
				
				if (damageType == 1) modProjectileNew.shamanProjectile = true;
				if (damageType == 2) modProjectileNew.alchemistProjectile = true;
				if (damageType == 3) modProjectileNew.gamblerProjectile = true;
				if (damageType == 4) modProjectileNew.dancerProjectile = true;
			}
		}
		
		public static void spawnDustCircle(Vector2 position, int dustType, double distToCenter, int number, bool noGravity = true, float dustScale = 1f, float velocityMult = 1f, float expandingSpeed = 0f, bool expandingHorizontal = true, bool expandingVertical = true, bool inwards = false, int offsetX = 0, int offsetY = 0, bool randomness = false, bool noLight = false) {
			int angle = (int)(360 / number);
			for (int i = 0 ; i < number ; i ++ )
			{
				double dustDeg = i * angle;
				double dustRad = dustDeg * (Math.PI / 180);
				
				float posX = position.X - (int)(Math.Cos(dustRad) * distToCenter) - offsetX;
				float posY = position.Y - (int)(Math.Sin(dustRad) * distToCenter) - offsetY;

				Vector2 dustPosition = new Vector2(posX, posY);
				int dust = Dust.NewDust(dustPosition, 1, 1, dustType);
				Dust myDust = Main.dust[dust];
				
				if (expandingSpeed != 0f) {
					float distX = position.X - posX;
					float distY = position.Y - posY;
					float magnitude = (float)Math.Sqrt(distX * distX + distY * distY);
					Vector2 vector = new Vector2(expandingVertical ? distX : 0, expandingHorizontal ? distY : 0);
					vector *= expandingSpeed / magnitude;
					myDust.velocity *= 0f;
					myDust.velocity += vector;
					myDust.velocity *= inwards ? 1f : -1f;
					myDust.velocity = myDust.velocity.RotatedByRandom(randomness ? 10 : 0);
					myDust.noLight = noLight;
				}
				
				myDust.velocity *= velocityMult;
				myDust.scale = dustScale;
				myDust.noGravity = noGravity;
			}
		}
    }
}
