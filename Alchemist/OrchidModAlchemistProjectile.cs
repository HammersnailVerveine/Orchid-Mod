using Terraria;
using Terraria.ModLoader;
using OrchidMod;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist
{
    public abstract class OrchidModAlchemistProjectile : OrchidModProjectile
    {
		public bool catalytic = false;
		
		public virtual void SafeOnHitNPC(NPC target, OrchidModAlchemistNPC modTarget, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {}
		
		public sealed override void AltSetDefaults() {
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			SafeSetDefaults();
			modProjectile.alchemistProjectile = true;
			modProjectile.baseCritChance = this.baseCritChance;
			modProjectile.alchemistReactiveProjectile = this.catalytic;
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			OrchidModGlobalNPC modTarget = target.GetGlobalNPC<OrchidModGlobalNPC>();
			modTarget.alchemistHit = true;
			OrchidModAlchemistNPC modTargetAlch = target.GetGlobalNPC<OrchidModAlchemistNPC>();
			SafeOnHitNPC(target, modTargetAlch, damage, knockback, crit, player, modPlayer);
		}
    }
}