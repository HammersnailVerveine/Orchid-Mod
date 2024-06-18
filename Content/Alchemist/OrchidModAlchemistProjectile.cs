using OrchidMod.Common.Global.Projectiles;
using OrchidMod.Common.Global.NPCs;
using OrchidMod.Common.ModObjects;
using Terraria;

namespace OrchidMod.Content.Alchemist
{
	public abstract class OrchidModAlchemistProjectile : OrchidModProjectile
	{
		public bool catalytic = false;

		public virtual void SafeOnHitNPC(NPC target, OrchidModAlchemistNPC modTarget, int damage, float knockback, bool crit, Player player, OrchidAlchemist modPlayer) { }

		public virtual void Catalyze(Player player, Projectile projectile, OrchidGlobalProjectile modProjectile)
		{
			projectile.Kill();
		}

		public sealed override void AltSetDefaults()
		{
			OrchidGlobalProjectile modProjectile = Projectile.GetGlobalProjectile<OrchidGlobalProjectile>();
			SafeSetDefaults();
			modProjectile.alchemistProjectile = true;
			modProjectile.alchemistReactiveProjectile = this.catalytic;
			modProjectile.alchemistCatalyticTriggerDelegate = Catalyze;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			OrchidGlobalNPC modTarget = target.GetGlobalNPC<OrchidGlobalNPC>();
			modTarget.AlchemistHit = true;
			OrchidModAlchemistNPC modTargetAlch = target.GetGlobalNPC<OrchidModAlchemistNPC>();
			SafeOnHitNPC(target, modTargetAlch, hit.Damage, hit.Knockback, hit.Crit, player, modPlayer);
		}
	}
}