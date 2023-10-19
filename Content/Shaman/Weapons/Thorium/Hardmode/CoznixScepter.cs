using OrchidMod.Common.Attributes;
using OrchidMod.Content.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Thorium.Hardmode
{
	[CrossmodContent("ThoriumMod")]
	public class CoznixScepter : OrchidModShamanItem
	{
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
			//Item.shoot = ModContent.ProjectileType<CoznixScepterProj>();
			this.Element = ShamanElement.FIRE;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Gate to the Fallen");
			/* Tooltip.SetDefault("Fires out a void bolt"
							+ "\nIf you have 3 or more active shamanic bonds, the bolt will summon a void portal"); */
		}
	}
}

