using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class EaterCardProj1 : OrchidModGamblerProjectile
	{
		private int bounceDelay = 0;
		private int count = 1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eater Eye");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 18;
			projectile.height = 22;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.penetrate = -1;
			ProjectileID.Sets.Homing[projectile.type] = true;
			Main.projFrames[projectile.type] = 3;
			this.gamblingChipChance = 5;
		}
		
		public override void OnSpawn() {
			projectile.frame = projectile.ai[0] == 0f ? 0 : 2;
			Projectile proj = Main.projectile[(int) projectile.ai[1]];
			if (proj.ai[0] != 0) {
				proj.frame = 1;
				proj.netUpdate = true;
			}
		}

		public override void SafeAI()
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			int projType = ProjectileType<Gambler.Projectiles.EaterCardProj2>();
			this.bounceDelay -= this.bounceDelay > 0 ? 1 : 0;
			
			projectile.rotation = projectile.velocity.RotatedBy(MathHelper.ToRadians(90)).ToRotation();
			projectile.direction = projectile.spriteDirection;

			if (Main.rand.Next(10) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 18);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
			}
			
			if (projectile.ai[0] != 0f) {
				if (projectile.ai[0] > 3f) {
					Vector2 move = Vector2.Zero;
					float distance = 500f;
					bool target = false;
					OrchidModProjectile.resetIFrames(projectile);
					for (int k = 0; k < 200; k++)
					{
						if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
						{
							Vector2 newMove = Main.npc[k].Center - projectile.Center;
							float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
							if (distanceTo < distance)
							{
								move = newMove;
								distance = distanceTo;
								target = true;
							}
						}
					} if (target) {
						this.Move(move, true);
					}
				} else {
					Projectile proj = Main.projectile[(int)projectile.ai[1]];
					if (proj.active && proj.type == projectile.type)
					{
						this.Move(proj.position - proj.velocity * 2f - projectile.position, false);
					} else {
						projectile.ai[0] = 4f;
						projectile.frame = 0;
						projectile.penetrate = 1;
						projectile.damage *= 3;
						projectile.ai[0] = 5f;
						projectile.timeLeft = 10;
						projectile.netUpdate = true;
					}
				}
			} else {
				if (Main.rand.Next(60) == 0)
				{
					Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(80)));
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, projType, 0, 0, projectile.owner);
				}
				
				for (int l = 0; l < Main.projectile.Length; l++)
				{
					Projectile proj = Main.projectile[l];
					if (proj.active && proj.type == projType)
					{
						if (proj.Hitbox.Intersects(projectile.Hitbox) && proj.damage > 0)
						{
							projectile.damage += 2;
							Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 2);
							proj.Kill();
							
							bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
							int newProjectile = (OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 0f, projectile.type, 0, projectile.knockBack, projectile.owner), dummy));
							Main.projectile[newProjectile].ai[0] = 1f;
							
							Projectile aiProj = projectile;
							for (int i = this.count; i > 0 ; i --) {
								aiProj = Main.projectile[(int) aiProj.localAI[0]];
							}
							aiProj.localAI[0] = newProjectile;
							
							Main.projectile[newProjectile].ai[1] = aiProj.whoAmI;
							Main.projectile[newProjectile].position = aiProj.position - aiProj.velocity * 3f;
							Main.projectile[newProjectile].netUpdate = true;
							
							this.count ++;
							
							for (int k = 0; k < Main.projectile.Length; k++)
							{
								Projectile proj2 = Main.projectile[k];
								if (proj2.active && proj2.type == projectile.type && proj2.owner == player.whoAmI)
								{
									proj2.damage = projectile.damage;
								}
							}
						}
					}
				}
				
				if (Main.myPlayer == projectile.owner)
				{
					if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.EaterCard>() && modPlayer.GamblerDeckInHand)
					{
						if (this.bounceDelay <= 0)
						{
							this.Move(Main.MouseWorld - projectile.Center, true);
						}
					} else {
						projectile.ai[0] = 4f;
						projectile.frame = 0;
						projectile.penetrate = 1;
						projectile.damage *= 3;
						projectile.ai[0] = 5f;
						projectile.timeLeft = 300;
						projectile.netUpdate = true;
					}
				}
			}
		}
		
		private void Move(Vector2 newMove, bool netUpdateCheck) {
			if (netUpdateCheck) {
				AdjustMagnitude(ref newMove);
				projectile.velocity = (15 * projectile.velocity + newMove) / 10f;
				AdjustMagnitude(ref projectile.velocity);
				
				int velocityXBy1000 = (int)(newMove.X * 1000f);
				int oldVelocityXBy1000 = (int)(projectile.velocity.X * 1000f);
				int velocityYBy1000 = (int)(newMove.Y * 1000f);
				int oldVelocityYBy1000 = (int)(projectile.velocity.Y * 1000f);
				
				if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000)
				{
					projectile.netUpdate = true;
				}
			} else {
				float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
				if (distanceTo > 5f)
				{
					newMove *= 5f / distanceTo;
					projectile.velocity = newMove;
				} else {
					if (projectile.velocity.Length() > 0f)
					{
						projectile.velocity *= 0f;
					}
				}
			}
		}
		
		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 5f)
			{
				vector *= 5f / magnitude;
			}
		}

		public override void Kill(int timeLeft)
		{
			int projType = ProjectileType<Gambler.Projectiles.EaterCardProj2>();
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType && proj.owner == projectile.owner)
				{
					proj.Kill();
				}
			}
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 18);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
			projType = ProjectileType<Gambler.Projectiles.EaterCardProj3>();
			bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, projType, projectile.damage * 2, 0, projectile.owner), dummy);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 18, 5, 3 + Main.rand.Next(5), false, 1.5f, 1f, 7f, true, true, false, 0, 0, true);
			OrchidModProjectile.spawnDustCircle(projectile.Center, 18, 10, 5 + Main.rand.Next(5), false, 1f, 1f, 5f, true, true, false, 0, 0, true);
			Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 83);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
			if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			this.bounceDelay = 15;
			return false;
		}
	}
}