using OrchidMod.Alchemist.Projectiles;
using Terraria;


namespace OrchidMod.Alchemist.Weapons.Dark
{
	public class DarkVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 5;
			item.width = 24;
			item.height = 24;
			item.rare = 1;
			this.potencyCost = 1;
			this.element = AlchemistElement.DARK;
			this.rightClickDust = 27;
			this.colorR = 182;
			this.colorG = 27;
			this.colorB = 248;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Vial");
			Tooltip.SetDefault("\n[c/FF0000:Test Item]");
		}
	}
}
