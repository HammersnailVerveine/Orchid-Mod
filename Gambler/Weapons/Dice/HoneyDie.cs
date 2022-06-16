using Terraria;

namespace OrchidMod.Gambler.Weapons.Dice
{
	public class HoneyDie : OrchidModGamblerDie
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Green;
			this.diceID = 2;
			this.diceCost = 3;
			this.diceDuration = 20;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wax Die");
			Tooltip.SetDefault("Recovers 1 - 6 health on gambling critical strike");
		}
	}
}
