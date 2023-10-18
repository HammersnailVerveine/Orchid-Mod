using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Balloon)]
	public class MourningTorch : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Mourning Torch");
			/* Tooltip.SetDefault("10% increased shamanic damage"
							 + "\nYour shamanic bonds will last 10 seconds longer"
							 + "\nHowever, taking direct damage will reduce their current duration by 5 seconds"); */
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			player.GetDamage<ShamanDamageClass>() += 0.1f;
			modPlayer.shamanMourningTorch = true;
			modPlayer.shamanBuffTimer += 10;
		}
	}
}