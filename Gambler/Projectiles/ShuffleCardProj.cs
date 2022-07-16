using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class ShuffleCardProj : OrchidModGamblerProjectile
	{
		private Vector2 initialHeartsVelocity = new Vector2(0f, 0f);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shuffle");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 900;
			Projectile.penetrate = -1;
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SafeAI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			int cardType = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			Projectile.frame = (int)(Projectile.ai[0]);

			if (Projectile.ai[1] != 6f)
			{
				Vector2 pos = new Vector2(0f, 0f);
				switch (Projectile.ai[1])
				{
					case 1:
						pos.X = player.Center.X;
						pos.Y = player.Center.Y - 75;
						break;
					case 2:
						pos.X = player.Center.X - 20;
						pos.Y = player.Center.Y - 60;
						break;
					case 3:
						pos.X = player.Center.X + 20;
						pos.Y = player.Center.Y - 60;
						break;
					case 4:
						pos.X = player.Center.X - 12;
						pos.Y = player.Center.Y - 38;
						break;
					case 5:
						pos.X = player.Center.X + 12;
						pos.Y = player.Center.Y - 38;
						break;
					default:
						break;
				}
				Projectile.position = pos - (new Vector2(Projectile.width, Projectile.height) / 2);
				if (Main.myPlayer == Projectile.owner)
				{
					Vector2 target = Main.MouseWorld;
					Vector2 heading = target - Projectile.Center;
					heading.Normalize();
					heading *= 15f;
					Projectile.velocity = heading;
					Projectile.rotation = Projectile.velocity.RotatedBy(MathHelper.ToRadians(90f)).ToRotation();
					Projectile.direction = Projectile.spriteDirection;
					if (!(Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.ShuffleCard>()))
					{
						int count = 0;
						if (Projectile.frame > 0)
						{
							for (int l = 0; l < Main.projectile.Length; l++)
							{
								Projectile proj = Main.projectile[l];
								if (player.whoAmI == proj.owner && proj.active && proj.ai[1] != 6f && proj.type == Projectile.type)
								{
									count++;
								}
							}

							float spreadMult = 1f + (0.5f * (count - 1));
							float spread = Projectile.frame == 3 ? 10f : 5f;
							Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(spread * (Projectile.ai[1] - spreadMult)));
						}
						else
						{
							Projectile.velocity *= 0.01f;
						}
						Projectile.knockBack = Projectile.frame == 1 ? 0.5f * count : 0f;
						Projectile.penetrate = Projectile.frame % 3 == 0 ? 1 : Projectile.frame == 2 ? 3 : -1;
						Projectile.friendly = true;
						Projectile.damage = Projectile.frame == 2 ? (int)(Projectile.damage * 2f) : Projectile.damage;
						Projectile.ai[1] = 6f;

						switch (Projectile.frame)
						{
							case 0:
								Projectile.timeLeft = 75;
								break;
							case 1:
								Projectile.timeLeft = 100 + (20 * count) - (int)(Projectile.ai[1] * 10);
								break;
							case 2:
								Projectile.timeLeft = 105;
								break;
							case 3:
								Projectile.timeLeft = 30;
								break;
							default:
								Projectile.timeLeft = 0;
								break;
						}

						Projectile.netUpdate = true;
					}
					else
					{
						Projectile.velocity *= 0f;
					}
				}
			}
			else
			{
				if (Projectile.timeLeft > 895)
				{
					Projectile.active = false;
					Projectile.netUpdate = true;
				}

				if (Projectile.frame == 1)
				{
					Projectile.rotation += 0.25f;
					Projectile.velocity *= 0.925f;
				}
				else
				{
					Projectile.rotation = Projectile.velocity.RotatedBy(MathHelper.ToRadians(90f)).ToRotation();
					Projectile.direction = Projectile.spriteDirection;
				}

				if (Projectile.frame == 0 && Projectile.timeLeft > 20)
				{
					Projectile.velocity *= 1.1f;
				}

				if (Projectile.frame == 2)
				{
					if (initialHeartsVelocity == Vector2.Zero)
					{
						initialHeartsVelocity = Projectile.velocity;
					}
					Projectile.velocity -= initialHeartsVelocity * 0.02f;
				}

				if (Main.rand.Next(5) == 0)
				{
					int dustType = Projectile.frame < 2 ? 63 : 60;
					Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
					int index = Dust.NewDust(pos, Projectile.width, Projectile.height, dustType);
					Main.dust[index].velocity *= 0.25f;
					Main.dust[index].scale *= 1.5f;
					Main.dust[index].noGravity = true;
				}
			}


			if (!this.initialized)
			{
				this.initialized = true;
				int dustType = Projectile.frame < 2 ? 63 : 60;
				OrchidModProjectile.spawnDustCircle(Projectile.Center, dustType, 5, 5, true, 1.5f, 1f, 3f, true, true, false, 0, 0, true);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.frame == 1)
			{
				Projectile.velocity *= 0f;
			}
			else
			{
				Projectile.Kill();
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 3; i++)
			{
				int dustType = Projectile.frame < 2 ? 63 : 60;
				Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
				int index = Dust.NewDust(pos, Projectile.width, Projectile.height, dustType);
				Main.dust[index].velocity *= 0.25f;
				Main.dust[index].scale *= 1.5f;
				Main.dust[index].noGravity = true;
			}

			if (Projectile.frame == 3 || Projectile.frame == 1)
			{
				bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
				int dustType = Projectile.frame < 2 ? 63 : 60;
				OrchidModProjectile.spawnDustCircle(Projectile.Center, dustType, 10, 15, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
				DummyProjectile(spawnGenericExplosion(Projectile, Projectile.damage, Projectile.knockBack, 80, 3, false, true), dummy);
			}
			else
			{
				SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
			}
		}
	}
}