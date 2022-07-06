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
	public class JungleCardProj : OrchidModGamblerProjectile
	{
		//private Texture2D texture;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore Bud");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 22;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 120;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Main.projFrames[Projectile.type] = 2;
		}
		
		public override void OnSpawn() {
			int dustType = 31;
			Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
			Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, dustType)].velocity *= 0.25f;
			Projectile.frame = Main.rand.Next(2);
		}

		public override void SafeAI()
		{
			this.checkMouseDrag();
			
			
			if (Projectile.ai[1] == 2f) {
				Projectile.rotation += Projectile.velocity.Length() / 30.5f * (Projectile.velocity.X > 0 ? 1f : -1f);
				Projectile.velocity *= 0.975f;
				Projectile.friendly = Projectile.velocity.Length() > 1f;
			}
		}
		
		public void checkMouseDrag() {
			Projectile proj = Main.projectile[(int)Projectile.ai[0]];
			
			if (proj.type != ProjectileType<Gambler.Projectiles.JungleCardBase>() || proj.active == false && Projectile.ai[1] != 2f) {
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
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidGambler modPlayer)
		{
			Vector2 vel = Projectile.Center - target.Center;
			vel.Normalize();
			vel *= (Projectile.velocity.Length() * 0.75f);
			Projectile.velocity = vel;
			SoundEngine.PlaySound(SoundID.Item50, Projectile.Center);
		}
		
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
			damage *= 2;
			damage += (int)(Projectile.velocity.Length() * 2);
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X / 2;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y / 2;
			SoundEngine.PlaySound(SoundID.Item50, Projectile.Center);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 163);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
			
			int projType = ProjectileType<Gambler.Projectiles.JungleCardProjAlt>();
			bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
			int rand = 5 + Main.rand.Next(3);
			for (int i = 0 ; i < rand ; i ++) {
				Vector2 vel = new Vector2((float)(Main.rand.Next(3) + 4), 0f);
				vel = vel.RotatedByRandom(MathHelper.ToRadians(180));
				DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vel.X, vel.Y, projType, Projectile.damage, 0f, Projectile.owner), dummy);
			}
			SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
		}
	}
}