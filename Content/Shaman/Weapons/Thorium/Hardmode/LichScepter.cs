using OrchidMod.Common.Attributes;
using OrchidMod.Content.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Thorium.Hardmode
{
	[CrossmodContent("ThoriumMod")]
	public class LichScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 75;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 42;
			Item.useAnimation = 42;
			Item.knockBack = 2.75f;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 1f;
			//Item.shoot = ModContent.ProjectileType<LichScepterProj>();
			this.Element = ShamanElement.AIR;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Reliquary Candle");
			/* Tooltip.SetDefault("Fires out a bolt of spiritual fire, dividing upon hitting a foe"
							+ "\nIf you have 4 or more active shamanic bonds, the bonus projectiles will home at nearby enemies"); */
		}
	}
}

