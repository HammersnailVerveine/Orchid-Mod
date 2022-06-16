using Terraria;
using Terraria.ID;

namespace OrchidMod.Guardian.Weapons.Shields
{
	public class LeadPavise : OrchidModGuardianShield
	{

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 0, 50);
			Item.width = 28;
			Item.height = 38;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.knockBack = 7f;
			Item.damage = 25;
			Item.rare = ItemRarityID.White;
			Item.useAnimation = 25;
			Item.useTime = 25;
			this.distance = 40f;
			this.bashDistance = 100f;
			this.blockDuration = 120;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lead Pavise");
			Tooltip.SetDefault("owo");
		}
	}
}
