using Terraria;
using Terraria.ID;

namespace OrchidMod.Gambler.Weapons.Dice
{
	public class GamblingDie : OrchidModGamblerDie
	{
		public int hitCount;

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 34;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			this.diceCost = 3;
			this.diceDuration = 90;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Gambling Die");
			Tooltip.SetDefault("Every second gambler hit deals 1-6 increased damage");
		}

		public override void UpdateDie(Player player, OrchidGambler gambler)
		{
		}

		public override void ModifyHitNPCWithProjDie(Player player, OrchidGambler gambler, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (hitCount > 0)
			{
				hitCount = 0;
				damage += gambler.gamblerDieValue;
			}
			else hitCount++;
		}
	}
}
