using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Alchemist.Weapons.Nature
{
	public class MushroomFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 7;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = DustID.Pumpkin;
			this.colorR = 237;
			this.colorG = 160;
			this.colorB = 69;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Mushroom Spores Flask");
			// Tooltip.SetDefault("When mixed an extract, increases its spore damage by 5");
		}
	}
}
