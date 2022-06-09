using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Alchemist.Weapons.Light
{
	public class LightVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 5;
			Item.width = 24;
			Item.height = 24;
			Item.rare = 1;
			this.potencyCost = 1;
			this.element = AlchemistElement.LIGHT;
			this.rightClickDust = 57;
			this.colorR = 253;
			this.colorG = 194;
			this.colorB = 18;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Light Vial");
			Tooltip.SetDefault("[c/FF0000:Test Item]");
		}
	}
}
