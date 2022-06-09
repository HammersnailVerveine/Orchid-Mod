using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class CoznixScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			Item.damage = 35;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.knockBack = 3.25f;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = false;
			Item.shootSpeed = 10f;
			Item.shoot = Mod.Find<ModProjectile>("CoznixScepterProj").Type;
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

