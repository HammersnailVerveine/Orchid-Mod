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
			Item.damage = 56;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.knockBack = 3.25f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 1f;
			Item.shoot = Mod.Find<ModProjectile>("MoltenResidueScepterProj").Type;
			this.empowermentType = 1;
			this.energy = 15;
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
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(Mod.Find<ModItem>("RitualScepter").Type, 1);
				recipe.AddIngredient(thoriumMod, "MoltenResidue", 8);
				recipe.AddIngredient(ItemID.SoulofNight, 7);
				recipe.Register();
				recipe.AddRecipe();
			}
		}
	}
}

