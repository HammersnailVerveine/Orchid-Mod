using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	public abstract class GuardianRuneProjectile : OrchidModGuardianProjectile
	{
		public int baseDamage = 0;
		public int baseCrit = 0;
		public OrchidGuardian guardian => Owner.GetModPlayer<OrchidGuardian>();

		public virtual bool SafeAI() => true;
		public virtual void FirstFrame() { }
		public virtual void RuneSetDefaults() { }
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

		public sealed override void SafeSetDefaults()
		{
			Projectile.timeLeft = 1500;
			Projectile.tileCollide = false;
			RuneSetDefaults();
		}

		public sealed override void AI()
		{
			if (JustCreated)
			{
				JustCreated = false;
				Projectile.netUpdate = true;
				baseCrit = Projectile.CritChance;
				baseDamage = Projectile.damage;
				FirstFrame();
			}

			if (IsLocalOwner)
			{
				Player owner = Owner;
				OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();
				Projectile.CritChance = (int)(owner.GetCritChance<GuardianDamageClass>() + owner.GetCritChance<GenericDamageClass>() + baseCrit);
				Projectile.damage = guardian.GetGuardianDamage(baseDamage);

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