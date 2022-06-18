using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class MushroomCardProj : OrchidModGamblerProjectile
	{
		bool bounced = false;
		bool exploded = false;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mushroom");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 60;
			this.gamblingChipChance = 10;
		}
		
		public override void OnSpawn() {
			Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
			Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, 56)].velocity *= 0.25f;
		}

		public override void SafeAI()
		{
			this.checkMouseDrag();
			
			if (Projectile.ai[1] == 2f) {
				if (bounced) {
					Projectile.velocity *= 0.985f;
					if (Projectile.timeLeft % 2 == 0) {
						Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
						Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, 56)].velocity *= 0.25f;
					}
				} else {
					if (Projectile.timeLeft % 5 == 0) {
						Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
						Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, 56)].velocity *= 0.25f;
					}
				}
				Projectile.rotation += Projectile.velocity.Length() / 30f * (Projectile.velocity.X > 0 ? 1f : -1f);
				if (Projectile.timeLeft == 2) {
					if (bounced) {
						this.Explode();
					} else {
						Projectile.Kill();
					}
				}
			}
		}
		
		public void checkMouseDrag() {
			Projectile proj = Main.projectile[(int)Projectile.ai[0]];
			
			if (proj.type != ProjectileType<Gambler.Projectiles.MushroomCardBase>() || proj.active == false) {
				Projectile.Kill();
			}
			
			if (Projectile.ai[1] == 0f) {
				proj.ai[0] ++;
				Projectile.timeLeft ++;
				if (Projectile.velocity.X > 0f) {
					Projectile.localAI[1] = Projectile.velocity.X;
					Projectile.velocity.X = 0f;
				}
				
				Projectile.position = proj.Center - new Vector2(Projectile.width, Projectile.height - 20) * 0.5f;
				
				if (Main.mouseLeft && Main.mouseLeftRelease) {
					Vector2 newMove = Main.MouseWorld - Projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < 25f) {
						Projectile.ai[1] = 1f;
						Projectile.netUpdate = true;
						Projectile.localAI[0] = Main.myPlayer;
					}
				}
			}
			
			if (Projectile.ai[1] == 1f) {
				proj.ai[0] ++;
				Projectile.timeLeft ++;
				if ((int)Projectile.localAI[0] == Main.myPlayer) {
					if (Main.mouseLeft) {
						Vector2 newMove = Main.MouseWorld - proj.Center;
						float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
						float range = 40f;
						if (distanceTo > range) {
							newMove.Normalize();
							Projectile.position = proj.Center + newMove * range - new Vector2(Projectile.width, Projectile.height) * 0.5f;
						} else {
							Projectile.position = Main.MouseWorld - new Vector2(Projectile.width, Projectile.height) * 0.5f;
						}
					} else {
						Vector2 newMove = proj.Center - Projectile.Center;
						newMove.Normalize();
						newMove *= Projectile.localAI[1];
						Projectile.velocity = newMove;
						Projectile.ai[1] = 2f;
						Projectile.tileCollide = true;
						Projectile.friendly = true;
						Projectile.netUpdate = true;
						SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
					}
				}
			}
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if (bounced) {
				this.Explode();
			} 
			else
			{
				SoundEngine.PlaySound(SoundID.Item56, Projectile.Center);
				bounced = true;
				Projectile.damage = (int)(Projectile.damage * 2f);
				Projectile.timeLeft = 180;
				if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X * 1.5f;
				if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y * 1.5f;
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0 ; i < 5 ; i ++) {
				int dustType = 56;
				Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType)].velocity *= 0.25f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (bounced)
			{
				this.Explode();
			}
		}
		
		public void Explode() {
			if (!exploded) {
				exploded = true;
				SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
				int radius = 150;
				Projectile.position -= new Vector2((radius - Projectile.width) / 2, (radius - Projectile.height) / 2);
				Projectile.width = radius;
				Projectile.height = radius;
				Projectile.timeLeft = 2;
				Projectile.penetrate = -1;
				Projectile.alpha = 255;
				for (int i = 0 ; i < 25 ; i ++) {
					int dustType = 56;
					Dust dust = Main.dust[Dust.NewDust(Projectile.Center, 0, 0, dustType)];
					dust.velocity *= 2f;
				}
			}
		}
	}
}