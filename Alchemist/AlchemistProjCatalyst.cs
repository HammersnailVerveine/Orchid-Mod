using OrchidMod.Common.Globals.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist
{
	public abstract class AlchemistProjCatalyst : ModProjectile
	{
		public virtual void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidAlchemist modPlayer) { }

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

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			OrchidGlobalNPC modTarget = target.GetGlobalNPC<OrchidGlobalNPC>();
			modTarget.AlchemistHit = true;
			SafeOnHitNPC(target, hit, damageDone, player, modPlayer);
		}
	}
}