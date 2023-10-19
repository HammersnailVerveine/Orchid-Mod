using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons
{
	public class BloodCaller : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 30;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.knockBack = 5.5f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 9.5f;
			//Item.shoot = ModContent.ProjectileType<CrimsonScepterProj>();
			this.Element = ShamanElement.FIRE;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Blood Caller");
			/* Tooltip.SetDefault("\nHitting an enemy will grant you a crimson heart"
							  + "\nIf you have 3 crimson hearts, your next hit will recover some life"); */
		}
	}
}
