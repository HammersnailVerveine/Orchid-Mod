using OrchidMod.Common.Global.Projectiles;
using OrchidMod.Common.ModObjects;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Shaman
{
	public abstract class OrchidModShamanProjectile : OrchidModProjectile
	{
		public int TimeSpent = 0;

		public virtual void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
		public virtual void SafeAI() { }

		public sealed override void AltSetDefaults()
		{
			Player player = Main.LocalPlayer;
			OrchidGlobalProjectile modProjectile = Projectile.GetGlobalProjectile<OrchidGlobalProjectile>();
			SafeSetDefaults();
			modProjectile.shamanProjectile = true;
		}

		public sealed override void AI()
		{
			SafeAI();
			TimeSpent++;
		}

		public sealed override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();

			if (target.type != NPCID.TargetDummy && !target.CountsAsACritter)
			{
				OrchidGlobalProjectile modProjectile = Projectile.GetGlobalProjectile<OrchidGlobalProjectile>();
				modPlayer.AddShamanicEmpowerment(modProjectile.shamanEmpowermentType);
			}

			SafeOnHitNPC(target, hit.Damage, hit.Knockback, hit.Crit, player, modPlayer);
		}
	}
}