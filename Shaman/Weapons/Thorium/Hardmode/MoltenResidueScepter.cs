using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class MoltenResidueScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 56;
			item.width = 54;
			item.height = 54;
			item.useTime = 45;
			item.useAnimation = 45;
			item.knockBack = 3.25f;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(0, 7, 50, 0);
			item.UseSound = SoundID.Item45;
			item.autoReuse = true;
			item.shootSpeed = 1f;
			item.shoot = mod.ProjectileType("MoltenResidueScepterProj");
			this.empowermentType = 1;
			this.energy = 10;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Molten Bomb");
			Tooltip.SetDefault("Fires out a magmatic bomb"
							+ "\nThe explosion size and damage depends on your number of active shamanic bonds");
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(mod.ItemType("RitualScepter"), 1);
				recipe.AddIngredient(thoriumMod, "MoltenResidue", 8);
				recipe.AddIngredient(ItemID.SoulofNight, 7);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}

