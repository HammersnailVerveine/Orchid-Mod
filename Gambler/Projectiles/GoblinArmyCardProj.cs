using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class GoblinArmyCardProj : OrchidModGamblerProjectile
	{
		private int fireTimer = 60;
		private int fireTimerRef = 60;
		private double dustVal = 0;
		private static float distance = 300f;
		private Vector2 baseVelocity = Vector2.Zero;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goblin Portal");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.alpha = 100;
		}

		public override void SafeAI()
		{
			if (!this.initialized)
			{
				this.initialized = true;
				this.baseVelocity = Projectile.velocity;
			}

			Player player = Main.player[Projectile.owner];
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			int cardType = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			Projectile.rotation += Projectile.ai[1] * 0.05f;
			this.fireTimer--;
			this.dustVal--;
			Projectile.velocity += this.baseVelocity / 100f;

			if (Main.rand.NextBool(20))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27);
				Main.dust[dust].scale *= 1.5f;
				Main.dust[dust].noGravity = true;
			}

			if (Main.myPlayer == Projectile.owner)
			{
				Vector2 vectorDist = player.Center - Projectile.Center;
				float distanceTo = (float)Math.Sqrt(vectorDist.X * vectorDist.X + vectorDist.Y * vectorDist.Y);
				if ((!(Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.GoblinArmyCard>() && modPlayer.GamblerDeckInHand) && Projectile.timeLeft < 840) || distanceTo > distance)
				{
					bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
					OrchidModProjectile.spawnDustCircle(Projectile.Center, 27, 5, 5, true, 1.3f, 1f, 5f, true, true, false, 0, 0, true);
					OrchidModProjectile.spawnDustCircle(Projectile.Center, 27, 5, 5, true, 1.3f, 1f, 3f, true, true, false, 0, 0, true);
					Projectile.Kill();
				}
			}

			if (fireTimer <= 0 && Projectile.ai[1] == 1f)
			{
				Vector2 target = Main.MouseWorld;
				Vector2 heading = target - Projectile.Center;
				heading.Normalize();
				heading *= 15f;
				int projType = ProjectileType<Gambler.Projectiles.GoblinArmyCardProjAlt>();
				bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
				DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, heading.X, heading.Y, projType, Projectile.damage, Projectile.knockBack, Projectile.owner), dummy);
				OrchidModProjectile.spawnDustCircle(Projectile.Center, 27, 5, 5, true, 1.3f, 1f, 3f, true, true, false, 0, 0, true);
				fireTimerRef -= fireTimerRef > 15 ? 4 : 0;
				fireTimer = fireTimerRef;
				SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
			}

			this.spawnDust(27, (int)distance);
		}

		public void spawnDust(int dustType, int distToCenter)
		{
			for (int i = 0; i < 3; i++)
			{
				double deg = (2 * (42 + this.dustVal) + i * 120);
				double rad = deg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) + Projectile.velocity.X - 4;
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) + Projectile.velocity.Y - 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity = Projectile.velocity / 2;
				Main.dust[index2].scale = 2f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = true;
			}
		}
	}
}