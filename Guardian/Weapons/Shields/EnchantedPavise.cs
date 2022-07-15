using Terraria;
using Terraria.ID;

namespace OrchidMod.Guardian.Weapons.Shields
{
	public class EnchantedPavise : OrchidModGuardianShield
	{

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.width = 28;
			Item.height = 38;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.knockBack = 7f;
			Item.damage = 33;
			Item.rare = ItemRarityID.White;
			Item.useAnimation = 25;
			Item.useTime = 25;
			this.distance = 40f;
			this.bashDistance = 110f;
			this.blockDuration = 100;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Pavise");
		}
	}
}
