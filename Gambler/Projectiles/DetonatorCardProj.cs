using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class DetonatorCardProj : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeletron Might");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 900;
			Projectile.friendly = false;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			this.gamblingChipChance = 5;
			Main.projFrames[Projectile.type] = 3;
		}

		public override void SafeAI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			Projectile.rotation += Projectile.velocity.X != 0f ? (Projectile.velocity.X * 1.5f) / 20f : (Projectile.velocity.Y * 1.5f) / 20f;
			Projectile.velocity.Y += 0.15f;
			Projectile.velocity.X *= 0.98f;

			if (!this.initialized)
			{
				this.initialized = true;
				Projectile.frame = Main.rand.Next(3);
			}

			if (Main.rand.Next(7) == 0)
			{
				Vector2 rotVector = (Projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2(); // rotation vector to use for dust velocity
				Vector2 dustPos = Projectile.Center - new Vector2(2f, 2f);
				dustPos += rotVector * (Projectile.height / 2);
				Dust dust = Dust.NewDustDirect(dustPos, 2, 2, 6);
				//int dust = Dust.NewDust(projectile.Center - ((new Vector2(0f, projectile.height / 2)) * rotVector), 3, 3, 6);
				dust.noGravity = true;
			}

			if (Main.myPlayer == Projectile.owner)
			{
				if (!(Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.DetonatorCard>() && modPlayer.GamblerDeckInHand) && Projectile.timeLeft < 840)
				{
					bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
					OrchidModProjectile.spawnDustCircle(Projectile.Center, 6, 10, 15, true, 1.3f, 1f, 8f, true, true, false, 0, 0, true);
					OrchidModGamblerHelper.DummyProjectile(spawnGenericExplosion(Projectile, Projectile.damage, Projectile.knockBack, 150, 3, true, 14), dummy);
					Projectile.Kill();
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.Y > 1f)
			{
				if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X / 3;
				if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y / 3;
				SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			}
			return false;
		}
	}
}
