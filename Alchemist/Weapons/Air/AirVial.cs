using OrchidMod.Alchemist.Projectiles;
using Terraria;


namespace OrchidMod.Alchemist.Weapons.Air
{
	public class AirVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 5;
			item.width = 24;
			item.height = 24;
			item.rare = 1;
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
