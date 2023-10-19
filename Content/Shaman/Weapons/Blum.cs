using OrchidMod.Content.Shaman.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons
{
	public class Blum : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 17;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.knockBack = 3.25f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 47, 0);
			Item.UseSound = SoundID.Item21;
			Item.autoReuse = true;
			Item.shootSpeed = 16f;
			//Item.shoot = ModContent.ProjectileType<BlumProj>();
			this.Element = ShamanElement.WATER;
			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoUsetimeReforge = true;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Blum");
			/* Tooltip.SetDefault("Rapidly shoots dangerous magical bolts"
							  + "\nThe weapon speed depends on the number of active shamanic bonds"); */
		}

		public override void UpdateInventory(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();
			Item.useTime = 18 - (nbBonds * 2);
			Item.useAnimation = 18 - (nbBonds * 2);
		}
	}
}

