using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman
{
	public abstract class OrchidModShamanProjectile : OrchidModProjectile
	{
		public virtual void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }

		public sealed override void AltSetDefaults()
		{
			Player player = Main.LocalPlayer;
			OrchidModGlobalProjectile modProjectile = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			SafeSetDefaults();
			modProjectile.shamanProjectile = true;
		}

		public sealed override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			//if (target.type != NPCID.TargetDummy)
			//{
				OrchidModGlobalProjectile modProjectile = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
				modPlayer.AddShamanicEmpowerment(modProjectile.shamanEmpowermentType);
			//}
			SafeOnHitNPC(target, hit.Damage, hit.Knockback, hit.Crit, player, modPlayer);
		}
	}
}