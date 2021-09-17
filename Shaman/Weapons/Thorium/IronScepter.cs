using OrchidMod.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class IronScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 19;
			item.width = 36;
			item.height = 38;
			item.useTime = 72;
			item.useAnimation = 72;
			item.knockBack = 4f;
			item.rare = ItemRarityID.White;
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.UseSound = SoundID.Item45;
			item.shootSpeed = 7f;
			item.shoot = mod.ProjectileType("OpalScepterProj");
			this.empowermentType = 4;
			this.energy = 9;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Opal Scepter");
			Tooltip.SetDefault("\nHitting an enemy will grant you an Opal orb"
							+ "\nIf you have 3 opal orbs, your next hit will increase your shamanic critical strike damage for 30 seconds");
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddTile(TileID.Anvils);
				recipe.AddIngredient(thoriumMod, "Opal", 8);
				recipe.AddIngredient(ItemID.IronBar, 10);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}
