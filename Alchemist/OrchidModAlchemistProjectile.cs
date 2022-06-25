using OrchidMod.Common.Globals.NPCs;
using Terraria;

namespace OrchidMod.Alchemist
{
	public abstract class OrchidModAlchemistProjectile : OrchidModProjectile
	{
		public bool catalytic = false;

		public virtual void SafeOnHitNPC(NPC target, OrchidModAlchemistNPC modTarget, int damage, float knockback, bool crit, Player player, OrchidAlchemist modPlayer) { }

		public virtual void Catalyze(Player player, Projectile projectile, OrchidModGlobalProjectile modProjectile)
		{
			projectile.Kill();
		}

		public sealed override void AltSetDefaults()
		{
			OrchidModGlobalProjectile modProjectile = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			SafeSetDefaults();
			modProjectile.alchemistProjectile = true;
			modProjectile.alchemistReactiveProjectile = this.catalytic;
			modProjectile.alchemistCatalyticTriggerDelegate = Catalyze;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[Projectile.owner];
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			OrchidGlobalNPC modTarget = target.GetGlobalNPC<OrchidGlobalNPC>();
			modTarget.AlchemistHit = true;
			OrchidModAlchemistNPC modTargetAlch = target.GetGlobalNPC<OrchidModAlchemistNPC>();
			SafeOnHitNPC(target, modTargetAlch, damage, knockback, crit, player, modPlayer);
		}
	}
}