using OrchidMod.Alchemist.Projectiles;
using Terraria;


namespace OrchidMod.Alchemist.Weapons.Water
{
	public class WaterVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 5;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Blue;
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
