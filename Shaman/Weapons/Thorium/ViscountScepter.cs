using OrchidMod.Common.Attributes;
using OrchidMod.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	[CrossmodContent("ThoriumMod")]
	public class ViscountScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 28;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.knockBack = 3.75f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 8f;
			Item.shoot = ModContent.ProjectileType<ViscountScepterProj>();
			this.empowermentType = 3;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Chiroptera");
			Tooltip.SetDefault("Fires out a bolt of blood magic"
							+ "\nHas a chance to spawn a homing bat on hit"
							+ "\nThe chance for a bat to spawn increases with the number of active shamanic bonds");
		}
	}
}

