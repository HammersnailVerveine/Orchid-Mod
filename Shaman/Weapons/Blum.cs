using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Weapons
{
	public class Blum : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 17;
			item.width = 30;
			item.height = 30;
			item.useTime = 18;
			item.useAnimation = 18;
			item.knockBack = 3.25f;
			item.rare = 2;
			item.value = Item.sellPrice(0, 0, 47, 0);
			item.UseSound = SoundID.Item21;
			item.autoReuse = true;
			item.shootSpeed = 16f;
			item.shoot = mod.ProjectileType("BlumProj");
			this.empowermentType = 2;
			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoUsetimeReforge = true;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blum");
			Tooltip.SetDefault("Rapidly shoots dangerous magical bolts"
							  + "\nThe weapon speed depends on the number of active shamanic bonds");
		}

		public override void UpdateInventory(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			item.useTime = 18 - (nbBonds * 2);
			item.useAnimation = 18 - (nbBonds * 2);
		}
	}
}

