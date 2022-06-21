using Terraria;
using Terraria.ID;

namespace OrchidMod.Gambler.Weapons.Dice
{
	public class GamblingDie : OrchidModGamblerDie
	{
		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 34;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			this.diceID = 0;
			this.diceCost = 2;
			this.diceDuration = 15;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gambling Die");
			Tooltip.SetDefault("Increases gambling damage by 3 - 18%");
		}
	}
}
