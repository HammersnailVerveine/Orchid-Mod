using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Sage
{
	public class SageCorruptionProjAlt : OrchidModShapeshifterProjectile
	{
		public int Timespent = 0;

		public override void SafeSetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 2;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override void AI()
		{
			if (!Initialized)
			{
				Initialized = true;

				int dustType1 = DustID.ToxicBubble;
				int dustType2 = DustID.CorruptGibs;
				if (Projectile.ai[2] != 0)
				{ // crimson version
					dustType1 = DustID.Crimson;
					dustType2 = DustID.CrimsonPlants;
				}

				for (int i = 0; i < 20; i ++)
				{
					Dust dust2 = Dust.NewDustDirect(Projectile.Center, 0, 0, dustType1);
					dust2.scale = Main.rand.NextFloat(1.2f, 1.5f);
					dust2.velocity *= Main.rand.NextFloat(1.5f, 2f);
					dust2.noGravity = true;
				}

				for (int i = 0; i < 6; i ++)
				{
					Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType2);
					dust2.scale = Main.rand.NextFloat(0.6f, 0.8f);
					dust2.velocity.Y -= 1.5f;
				}

				if (Projectile.ai[0] == 1)
				{ // corpse explosion

					for (int i = 0; i < 8; i++)
					{
						Dust dust2 = Dust.NewDustDirect(Projectile.Center, 0, 0, dustType1);
						dust2.scale = Main.rand.NextFloat(1.2f, 1.5f);
						dust2.velocity *= Main.rand.NextFloat(2f, 2.5f);
						dust2.noGravity = true;
					}

					for (int i = 0; i < 6; i++)
					{
						Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
						dust2.velocity *= Main.rand.NextFloat(1f, 2f);
						dust2.noGravity = true;
					}

					for (int i = 0; i < 3; i ++)
					{
						int rand = Main.rand.Next(3);
						switch (rand)
						{
							case 0:
								rand = GoreID.Smoke1;
								break;
							case 1:
								rand = GoreID.Smoke2;
								break;
							default:
								rand = GoreID.Smoke3;
								break;
						}

						Gore.NewGoreDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitX.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(0f, 0.5f), rand).scale *= Main.rand.NextFloat(0.7f, 1f);
						SoundStyle soundStyle = SoundID.Item95;
						soundStyle.Pitch *= Main.rand.NextFloat(0.4f, 0.7f);
						SoundEngine.PlaySound(soundStyle, Projectile.Center);

						Vector2 oldCenter = Projectile.Center;
						Projectile.width = 80;
						Projectile.height = 80;
						Projectile.Center = oldCenter;
					}
				}
				else
				{
					SoundStyle soundStyle = SoundID.Item54;
					soundStyle.Pitch *= Main.rand.NextFloat(0f, 0.5f);
					SoundEngine.PlaySound(soundStyle, Projectile.Center);
				}
			}
		}
	}
}