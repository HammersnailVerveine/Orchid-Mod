using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Alchemist.Weapons.Fire
{
	public class FireVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 5;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Blue;
			this.potencyCost = 1;
			this.element = AlchemistElement.FIRE;
			this.rightClickDust = 6;
			this.colorR = 253;
			this.colorG = 62;
			this.colorB = 3;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Fire Vial");
			Tooltip.SetDefault("\n[c/FF0000:Test Item]");
		}
	}
}
