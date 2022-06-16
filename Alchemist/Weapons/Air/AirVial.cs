using Terraria;
using Terraria.ID;

namespace OrchidMod.Alchemist.Weapons.Air
{
	public class AirVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 5;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Blue;
			this.potencyCost = 1;
			this.element = AlchemistElement.AIR;
			this.rightClickDust = 16;
			this.colorR = 121;
			this.colorG = 152;
			this.colorB = 239;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Air Vial");
			Tooltip.SetDefault("\n[c/FF0000:Test Item]");
		}
	}
}
