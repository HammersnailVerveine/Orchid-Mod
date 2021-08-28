using OrchidMod.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class UnfathomableFleshScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 50;
			item.width = 50;
			item.height = 50;
			item.useTime = 35;
			item.useAnimation = 35;
			item.knockBack = 4.25f;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shootSpeed = 8f;
			item.shoot = mod.ProjectileType("UnfathomableFleshScepterProj");
			this.empowermentType = 5;
			this.energy = 10;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Symbiosis Catalyst");
			Tooltip.SetDefault("Fires out a bolt of flesh magic"
							+ "\nIf you have 5 active shamanic bonds, your attack will steal life"
							+ "\nAfter stealing life, your regeneration will be nullified for a moment");
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(mod.ItemType("RitualScepter"), 1);
				recipe.AddIngredient(thoriumMod, "UnfathomableFlesh", 9);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}

