using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
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
			Projectile.width = 18;
			Projectile.height = 22;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 3;
			this.gamblingChipChance = 5;
		}
		
		public override void OnSpawn() {
			Projectile.frame = Projectile.ai[0] == 0f ? 0 : 2;
			Projectile proj = Main.projectile[(int) Projectile.ai[1]];
			if (proj.ai[0] != 0) {
				proj.frame = 1;
				proj.netUpdate = true;
			}
		}

		public override void SafeAI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			int projType = ProjectileType<Gambler.Projectiles.EaterCardProj2>();
			this.bounceDelay -= this.bounceDelay > 0 ? 1 : 0;
			
			Projectile.rotation = Projectile.velocity.RotatedBy(MathHelper.ToRadians(90)).ToRotation();
			Projectile.direction = Projectile.spriteDirection;

			if (Main.rand.Next(10) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 18);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
			}
			
			if (Projectile.ai[0] != 0f) {
				if (Projectile.ai[0] > 3f) {
					Vector2 move = Vector2.Zero;
					float distance = 500f;
					bool target = false;
					OrchidModProjectile.resetIFrames(Projectile);
					for (int k = 0; k < 200; k++)
					{
						if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
						{
							Vector2 newMove = Main.npc[k].Center - Projectile.Center;
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
					Projectile proj = Main.projectile[(int)Projectile.ai[1]];
					if (proj.active && proj.type == Projectile.type)
					{
						this.Move(proj.position - proj.velocity * 2f - Projectile.position, false);
					} else {
						Projectile.ai[0] = 4f;
						Projectile.frame = 0;
						Projectile.penetrate = 1;
						//projectile.damage *= 3;
						Projectile.ai[0] = 5f;
						Projectile.timeLeft = 10;
						Projectile.netUpdate = true;
					}
				}
			} else {
				if (Main.rand.Next(60) == 0)
				{
					Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(80)));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vel.X, vel.Y, projType, 0, 0, Projectile.owner);
				}
				
				for (int l = 0; l < Main.projectile.Length; l++)
				{
					Projectile proj = Main.projectile[l];
					if (proj.active && proj.type == projType)
					{
						if (proj.Hitbox.Intersects(Projectile.Hitbox) && proj.damage > 0)
						{
							Projectile.damage += 2;
							SoundEngine.PlaySound(SoundID.Item2, Projectile.Center);
							proj.Kill();
							
							bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
							int newProjectile = (OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 0f, 0f, Projectile.type, 0, Projectile.knockBack, Projectile.owner), dummy));
							Main.projectile[newProjectile].ai[0] = 1f;
							
							Projectile aiProj = Projectile;
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
								if (proj2.active && proj2.type == Projectile.type && proj2.owner == player.whoAmI)
								{
									proj2.damage = Projectile.damage;
								}
							}
						}
					}
				}
				
				if (Main.myPlayer == Projectile.owner)
				{
					if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.EaterCard>() && modPlayer.GamblerDeckInHand)
					{
						if (this.bounceDelay <= 0)
						{
							this.Move(Main.MouseWorld - Projectile.Center, true);
						}
					} else {
						Projectile.ai[0] = 4f;
						Projectile.frame = 0;
						Projectile.penetrate = 1;
						Projectile.damage *= 3;
						Projectile.ai[0] = 5f;
						Projectile.timeLeft = 300;
						Projectile.netUpdate = true;
					}
				}
			}
		}
		
		private void Move(Vector2 newMove, bool netUpdateCheck) {
			if (netUpdateCheck) {
				AdjustMagnitude(ref newMove);
				Projectile.velocity = (15 * Projectile.velocity + newMove) / 10f;
				AdjustMagnitude(ref Projectile.velocity);
				
				int velocityXBy1000 = (int)(newMove.X * 1000f);
				int oldVelocityXBy1000 = (int)(Projectile.velocity.X * 1000f);
				int velocityYBy1000 = (int)(newMove.Y * 1000f);
				int oldVelocityYBy1000 = (int)(Projectile.velocity.Y * 1000f);
				
				if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000)
				{
					Projectile.netUpdate = true;
				}
			} else {
				float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
				if (distanceTo > 5f)
				{
					newMove *= 5f / distanceTo;
					Projectile.velocity = newMove;
				} else {
					if (Projectile.velocity.Length() > 0f)
					{
						Projectile.velocity *= 0f;
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
				if (proj.active && proj.type == projType && proj.owner == Projectile.owner)
				{
					proj.Kill();
				}
			}
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 18);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
			projType = ProjectileType<Gambler.Projectiles.EaterCardProj3>();
			bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, projType, Projectile.damage * 2, 0, Projectile.owner), dummy);
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 18, 5, 3 + Main.rand.Next(5), false, 1.5f, 1f, 7f, true, true, false, 0, 0, true);
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 18, 10, 5 + Main.rand.Next(5), false, 1f, 1f, 5f, true, true, false, 0, 0, true);
			SoundEngine.PlaySound(SoundID.Item83, Projectile.Center);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			this.bounceDelay = 15;
			return false;
		}
	}
}