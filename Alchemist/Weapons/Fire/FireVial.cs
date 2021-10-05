using OrchidMod.Alchemist.Projectiles;
using Terraria;


namespace OrchidMod.Alchemist.Weapons.Fire
{
	public class FireVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 5;
			item.width = 24;
			item.height = 24;
			item.rare = 1;
			this.potencyCost = 1;
			this.element = AlchemistElement.FIRE;
			this.rightClickDust = 6;
			this.colorR = 253;
			this.colorG = 62;
			this.colorB = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fire Vial");
			Tooltip.SetDefault("\n[c/FF0000:Test Item]");
		}
	}
}
