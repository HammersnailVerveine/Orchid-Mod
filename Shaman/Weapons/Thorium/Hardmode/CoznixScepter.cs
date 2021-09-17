using OrchidMod.Interfaces;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class CoznixScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 35;
			item.width = 54;
			item.height = 54;
			item.useTime = 45;
			item.useAnimation = 45;
			item.knockBack = 3.25f;
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.UseSound = SoundID.Item45;
			item.autoReuse = false;
			item.shootSpeed = 10f;
			item.shoot = mod.ProjectileType("CoznixScepterProj");
			this.empowermentType = 1;
			this.energy = 25;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Gate to the Fallen");
			Tooltip.SetDefault("Fires out a void bolt"
							+ "\nIf you have 3 or more active shamanic bonds, the bolt will summon a void portal");
		}
	}
}

