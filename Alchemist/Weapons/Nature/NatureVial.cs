using OrchidMod.Alchemist.Projectiles;
using Terraria;


namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class NatureVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 5;
			item.width = 24;
			item.height = 24;
			item.rare = 1;
			this.potencyCost = 1;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 3;
			this.colorR = 75;
			this.colorG = 117;
			this.colorB = 0;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nature Vial");
			Tooltip.SetDefault("\n[c/FF0000:Test Item]");
		}
	}
}
