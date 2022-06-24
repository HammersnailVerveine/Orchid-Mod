using OrchidMod.Common.Interfaces;
using OrchidMod.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class IceShardScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			Item.damage = 14;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.knockBack = 3f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 5, 30);
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
			Item.shootSpeed = 10f;
			Item.shoot = ModContent.ProjectileType<IceShardScepterProj>();
			this.empowermentType = 2;
			this.energy = 6;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Scepter");
			Tooltip.SetDefault("Shoots frostburn bolts, growing for an instant before being launched"
							+ "\nCritical strike chance increases with the number of active shamanic bonds");
		}

		public override void UpdateInventory(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			Item.crit = 4 + 10 * modPlayer.GetNbShamanicBonds() + modPlayer.shamanCrit;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.WorkBenches);
				recipe.AddIngredient(thoriumMod, "IcyShard", 7);
				recipe.Register();
			}
		}
	}
}
