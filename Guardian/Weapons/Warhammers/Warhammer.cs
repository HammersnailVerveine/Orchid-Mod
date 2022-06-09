using Terraria;
using Terraria.ID;

namespace OrchidMod.Guardian.Weapons.Warhammers
{
	public class Warhammer : OrchidModGuardianHammer
	{

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 0, 50);
			Item.width = 36;
			Item.height = 36;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.knockBack = 5f;
			Item.damage = 10;
			Item.rare = 0;
			Item.useAnimation = 35;
			Item.useTime = 35;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Warhammer");
			Tooltip.SetDefault("uwu");
		}	
	}
}
