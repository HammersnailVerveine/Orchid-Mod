using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Gambler.Weapons.Dice
{
	public class HoneyDie : OrchidModGamblerDie
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Green;
			this.diceCost = 3;
			this.diceDuration = 60;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Wax Die");
			// Tooltip.SetDefault("Recovers 1 - 6 health on gambling critical strike");
		}

		public override void UpdateDie(Player player, OrchidGambler gambler)
		{
		}

		public override void OnHitNPCWithProj(Player player, OrchidGambler gambler, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (hit.Crit && gambler.gamblerTimerHoney >= 60)
			{
				player.HealEffect(gambler.gamblerDieValue, true);
				player.statLife += gambler.gamblerDieValue;
				gambler.gamblerTimerHoney = 0;
			}
		}
	}
}
