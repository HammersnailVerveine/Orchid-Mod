using Microsoft.Xna.Framework;
using Terraria;
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
			projectile.width = 18;
			projectile.height = 24;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.penetrate = -1;
			projectile.timeLeft = 900;
			projectile.friendly = false;
			ProjectileID.Sets.Homing[projectile.type] = true;
			this.gamblingChipChance = 5;
			Main.projFrames[projectile.type] = 3;
		}

		public override void SafeAI()
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int cardType = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;
			projectile.rotation += projectile.velocity.X != 0f ? (projectile.velocity.X * 1.5f) / 20f : (projectile.velocity.Y * 1.5f) / 20f;
			projectile.velocity.Y += 0.15f;
			projectile.velocity.X *= 0.98f;

			if (!this.initialized)
			{
				this.initialized = true;
				projectile.frame = Main.rand.Next(3);
			}

			if (Main.rand.Next(7) == 0)
			{
				Vector2 rotVector = (projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2(); // rotation vector to use for dust velocity
				Vector2 dustPos = projectile.Center - new Vector2(2f, 2f);
				dustPos += rotVector * (projectile.height / 2);
				Dust dust = Dust.NewDustDirect(dustPos, 2, 2, 6);
				//int dust = Dust.NewDust(projectile.Center - ((new Vector2(0f, projectile.height / 2)) * rotVector), 3, 3, 6);
				dust.noGravity = true;
			}

			if (Main.myPlayer == projectile.owner)
			{
				if (!(Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.DetonatorCard>() && modPlayer.GamblerDeckInHand) && projectile.timeLeft < 840)
				{
					bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
					OrchidModProjectile.spawnDustCircle(projectile.Center, 6, 10, 15, true, 1.3f, 1f, 8f, true, true, false, 0, 0, true);
					OrchidModGamblerHelper.DummyProjectile(spawnGenericExplosion(projectile, projectile.damage, projectile.knockBack, 150, 3, true, 14), dummy);
					projectile.Kill();
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.Y > 1f)
			{
				if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X / 3;
				if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y / 3;
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			}
			return false;
		}
	}
}
