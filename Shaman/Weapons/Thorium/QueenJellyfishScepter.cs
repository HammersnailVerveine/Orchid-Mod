using OrchidMod.Common.Interfaces;
using OrchidMod.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class QueenJellyfishScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

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
			Item.shoot = ModContent.ProjectileType<QueenJellyfishScepterProj>();
			this.empowermentType = 2;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Jellyfish Dynamizer");
			Tooltip.SetDefault("Launches a bouncy jellyfish, gaining in damage with each rebound"
							+ "\nDamage increase depends on your number of active shamanic bonds"
							+ "\n'Apparently waterproof'");
		}
	}
}

