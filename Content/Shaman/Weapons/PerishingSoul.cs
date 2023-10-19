using OrchidMod.Content.Shaman.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons
{
	public class PerishingSoul : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 65;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.knockBack = 3.15f;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 0, 47, 0);
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			//Item.shoot = ModContent.ProjectileType<PerishingSoulProj>();
			this.Element = ShamanElement.FIRE;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Perishing Soul");
			/* Tooltip.SetDefault("Shoots fireballs, growing for an instant before being launched"
							  + "\nProjectile will grow faster if you have 3 or more active shamanic bonds"); */
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.HellstoneBar, 18)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
