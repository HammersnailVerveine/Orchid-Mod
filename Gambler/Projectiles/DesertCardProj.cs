using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class DesertCardProj : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pinecone");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 22;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 30;
			Projectile.penetrate = 2;
			this.gamblingChipChance = 10;
		}
		
		public override void OnSpawn() {
			int dustType = 31;
			Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
			Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, dustType)].velocity *= 0.25f;
		}

		public override void SafeAI()
		{
			this.checkMouseDrag();
			
			if (Projectile.ai[1] == 2f) {
				Projectile.rotation += Projectile.velocity.Length() / 30f * (Projectile.velocity.X > 0 ? 1f : -1f);
			}
			
			if (Projectile.timeLeft <= 21 || Projectile.penetrate < 2) {
				if (Projectile.timeLeft >= 21) {
					Projectile.velocity *= 0.0001f;
					Projectile.friendly = false;
					Projectile.alpha = 255;
					Projectile.timeLeft = 21;
					for (int i = 0 ; i < 2 ; i++) {
						Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 31)].velocity *= 0.25f;
					}
				}
				
				if (Projectile.timeLeft % 3 == 0) {
					Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 31)].velocity *= 0.25f;
					int projType = ProjectileType<Gambler.Projectiles.DesertCardProjAlt>();
					bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
					Vector2 vel = Projectile.velocity;
					vel.Normalize();
					vel *= 10f;
					vel = vel.RotatedByRandom(MathHelper.ToRadians(10));
					OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, vel.X, vel.Y, projType, Projectile.damage, Projectile.knockBack, Projectile.owner), dummy);
					SoundEngine.PlaySound(2, (int)Projectile.Center.X, (int)Projectile.Center.Y, 17);
				}
			}
		}
		
		public void checkMouseDrag() {
			Projectile proj = Main.projectile[(int)Projectile.ai[0]];
			
			if (proj.type != ProjectileType<Gambler.Projectiles.DesertCardBase>() || proj.active == false) {
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
						SoundEngine.PlaySound(2, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1);
					}
				}
			}
		}

		public override void Kill(int timeLeft)
		{
		}
	}
}