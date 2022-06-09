using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist
{
	public abstract class AlchemistProjCatalyst : ModProjectile
	{
		public virtual void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) { }

		public virtual void SafeAI() { }

		public virtual void CatalystInteractionEffect(Player player) { }

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && Projectile.Hitbox.Intersects(proj.Hitbox))
				{
					OrchidModGlobalProjectile modProjectile = proj.GetGlobalProjectile<OrchidModGlobalProjectile>();
					if (modProjectile.alchemistReactiveProjectile)
					{
						modProjectile.alchemistCatalyticTriggerDelegate(player, proj, modProjectile);
						CatalystInteractionEffect(player);
					}
				}
			}
			SafeAI();
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			OrchidModGlobalNPC modTarget = target.GetGlobalNPC<OrchidModGlobalNPC>();
			modTarget.alchemistHit = true;
			SafeOnHitNPC(target, damage, knockback, crit, player, modPlayer);
		}
	}
}