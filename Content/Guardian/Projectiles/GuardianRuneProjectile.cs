using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace OrchidMod.Content.Guardian.Projectiles
{
	public abstract class GuardianRuneProjectile : OrchidModProjectile
	{
		public Player owner;
		public OrchidGuardian guardian;

		public virtual bool SafeAI() => true;
		public virtual void SafeOnSpawn(IEntitySource source) {}
		public float Distance => Projectile.ai[0];
		public float Angle => Projectile.ai[1];

		public void Spin(float val) {
			Projectile.ai[1] += val;
		}

		public void SetDistance(float val) {
			Projectile.ai[0] = val;
		}

		public sealed override void AltSetDefaults()
		{
			Projectile.timeLeft = 1500;
			Projectile.tileCollide = false;
			SafeSetDefaults();
		}

		public override void OnSpawn(IEntitySource source)
		{
			owner = Main.player[Projectile.owner];
			guardian = owner.GetModPlayer<OrchidGuardian>();
			Projectile.netUpdate = true;
			Projectile.netUpdate2 = true;
			SafeOnSpawn(source);
		}

		public sealed override void AI()
		{
			if (owner.dead) Projectile.Kill();
			if (SafeAI())
			{
				Vector2 position = new Vector2(0f, Projectile.ai[0]);
				position = position.RotatedBy(MathHelper.ToRadians(Projectile.ai[1]));
				Projectile.position = owner.Center + position;
			}
		}
	}
}