using OrchidMod.Alchemist.Projectiles;
using Terraria;


namespace OrchidMod.Alchemist.Weapons.Water
{
	public class WaterVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 5;
			item.width = 24;
			item.height = 24;
			item.rare = 1;
			this.potencyCost = 1;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 29;
			this.colorR = 1;
			this.colorG = 139;
			this.colorB = 252;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Water Vial");
			Tooltip.SetDefault("\n[c/FF0000:Test Item]");
		}
	}
}
