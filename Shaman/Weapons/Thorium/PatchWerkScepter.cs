using OrchidMod.Common.Attributes;
using OrchidMod.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	[CrossmodContent("ThoriumMod")]
	public class PatchWerkScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 12;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.knockBack = 5.25f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			//Item.shoot = ModContent.ProjectileType<PatchWerkScepterProj>();
			this.Element = 4;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Festering Fork");
			/* Tooltip.SetDefault("Fires out a bolt of festering magic"
							+ "\nIf you have 2 or more active shamanic bonds, hitting will summon maggots"); */
		}
	}
}

