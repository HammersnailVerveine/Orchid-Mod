using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Content.Guardian
{
	public abstract class GuardianRuneProjectile : OrchidModGuardianProjectile
	{
		public OrchidGuardian guardian => Owner.GetModPlayer<OrchidGuardian>();

		public virtual bool SafeAI() => true;
		public virtual void FirstFrame() { }
		public float Distance => Projectile.ai[0];
		public float Angle => Projectile.ai[1];

		public bool JustCreated = true;

		public void Spin(float val)
		{
			Projectile.ai[1] += val;
		}

		public void SetDistance(float val)
		{
			Projectile.ai[0] = val;
		}

		public sealed override void AltSetDefaults()
		{
			Projectile.timeLeft = 1500;
			Projectile.tileCollide = false;
			SafeSetDefaults();
		}

		public sealed override void AI()
		{
			if (JustCreated)
			{
				JustCreated = false;
				Projectile.netUpdate = true;
				FirstFrame();
			}

			if (IsLocalOwner)
			{
				if (Owner.dead)
				{
					Projectile.Kill();
				}
			}
			else
			{
				Projectile.timeLeft = 120;
			}

			guardian.RuneProjectiles.Add(Projectile);

			if (SafeAI())
			{
				Vector2 position = new Vector2(0f, Projectile.ai[0]);
				position = position.RotatedBy(MathHelper.ToRadians(Projectile.ai[1]));
				Projectile.position = Owner.Center + position - new Vector2(Projectile.width, Projectile.height) * 0.5f + Vector2.UnitY * Owner.gfxOffY;
			}
		}
	}
}