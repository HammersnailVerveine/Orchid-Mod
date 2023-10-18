using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class TungstenScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 24;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 58;
			Item.useAnimation = 58;
			Item.knockBack = 4.25f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 8f;
			//Item.shoot = ModContent.ProjectileType<TungstenScepterProj>();
			this.Element = 4;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Emerald Scepter");
			/* Tooltip.SetDefault("\nHitting an enemy will grant you an emerald orb"
							  + "\nIf you have 3 emerald orbs, your next hit will increase your movement speed for 30 seconds"); */
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Emerald, 8)
			.AddIngredient(ItemID.TungstenBar, 10)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
