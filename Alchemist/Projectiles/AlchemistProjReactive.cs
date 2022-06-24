using Terraria;

namespace OrchidMod.Alchemist.Projectiles
{
	public abstract class AlchemistProjReactive : OrchidModProjectile
	{
		public int spawnTimeLeft = 0;
		public int killTimeLeft = 0;

		public virtual void SafeKill(int timeLeft, Player player, OrchidModPlayerAlchemist modPlayer) { }

		public virtual void SafeAI() { }

		public virtual void Despawn() { }

		public virtual void Catalyze(Player player, Projectile projectile, OrchidModGlobalProjectile modProjectile)
		{
			projectile.Kill();
		}

		public sealed override void AltSetDefaults()
		{
			OrchidModGlobalProjectile modProjectile = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			SafeSetDefaults();
			modProjectile.alchemistProjectile = true;
			modProjectile.alchemistReactiveProjectile = true;
			modProjectile.baseCritChance = this.baseCritChance;
			modProjectile.alchemistCatalyticTriggerDelegate = Catalyze;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 1)
			{
				Despawn();
				Projectile.active = false;
			}
			this.killTimeLeft = Projectile.timeLeft;
			SafeAI();
		}

		public override void Kill(int timeLeft)
		{
			if (this.killTimeLeft < this.spawnTimeLeft)
			{
				Player player = Main.player[Projectile.owner];
				OrchidModPlayerAlchemist modPlayer = player.GetModPlayer<OrchidModPlayerAlchemist>();
				SafeKill(timeLeft, player, modPlayer);
				Despawn();
			}
			else
			{
				return;
			}
		}
	}
}