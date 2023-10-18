using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Gambler.Projectiles
{
	public class OceanCardProj : OrchidModGamblerProjectile
	{
		public float rolling = 0;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Mushroom");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 180;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
		}
		
		public override void OnSpawn(IEntitySource source)
		{
			Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke)].velocity *= 0.25f;
		}

		public override void SafeAI()
		{
			this.checkMouseDrag();
			
			if (Projectile.ai[1] == 2f)
			{
				Projectile.velocity.Y += 0.3f;
				if (Projectile.timeLeft % 15 == 0) {
					Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
					Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, DustID.Smoke)].velocity *= 0.25f;
				}
				Projectile.rotation += Projectile.velocity.Length() / 30f * (Projectile.velocity.X > 0 ? 1f : -1f);
			}
		}
		
		public void checkMouseDrag() {
			Projectile proj = Main.projectile[(int)Projectile.ai[0]];
			
			if (proj.type != ProjectileType<Content.Gambler.Projectiles.OceanCardBase>() || proj.active == false && Projectile.ai[1] != 2f) {
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
						Projectile.alpha = 0;
						SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
					}
				}
			}
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if (Math.Abs(oldVelocity.Y) > 9f)
			{
				OrchidModProjectile.spawnGenericExplosion(Projectile, Projectile.damage, Projectile.knockBack, 150, 3, true);
				Projectile.Kill();
			}
			else if (Math.Abs(oldVelocity.Y) > 0.5f)
			{
				SoundEngine.PlaySound(SoundID.Item50, Projectile.Center);
			}
			else
			{
				Projectile.velocity.X *= 0.98f;
				if (Math.Abs(Projectile.velocity.X) < 0.1f)
				{
					Projectile.Kill();
				}
			}

			return false;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidGambler modPlayer)
		{
			if (Math.Abs(Projectile.velocity.Y) > 9f)
			{
				OrchidModProjectile.spawnGenericExplosion(Projectile, Projectile.damage, Projectile.knockBack, 150, 3, true);
				Projectile.Kill();
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0 ; i < 5 ; i ++) {
				Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke)].velocity *= 0.25f;
			}
		}
	}
}