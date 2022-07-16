using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Alchemist.Weapons.Dark
{
	public class DarkVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 5;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Blue;
			this.potencyCost = 1;
			this.element = AlchemistElement.DARK;
			this.rightClickDust = 27;
			this.colorR = 182;
			this.colorG = 27;
			this.colorB = 248;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Vial");
			Tooltip.SetDefault("\n[c/FF0000:Test Item]");
		}
	}
}
