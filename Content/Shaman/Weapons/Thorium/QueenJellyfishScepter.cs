using OrchidMod.Common.Attributes;
using OrchidMod.Content.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Thorium
{
	[CrossmodContent("ThoriumMod")]
	public class QueenJellyfishScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 20;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 42;
			Item.useAnimation = 42;
			Item.knockBack = 5.5f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 5f;
			//Item.shoot = ModContent.ProjectileType<QueenJellyfishScepterProj>();
			this.Element = ShamanElement.WATER;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Jellyfish Dynamizer");
			/* Tooltip.SetDefault("Launches a bouncy jellyfish, gaining in damage with each rebound"
							+ "\nDamage increase depends on your number of active shamanic bonds"
							+ "\n'Apparently waterproof'"); */
		}
	}
}

