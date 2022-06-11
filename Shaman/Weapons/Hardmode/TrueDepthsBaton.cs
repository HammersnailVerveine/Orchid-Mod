using Microsoft.Xna.Framework;
using OrchidMod.Shaman.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class TrueDepthsBaton : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 62;
			Item.magic = true;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 42;
			Item.useAnimation = 42;
			Item.knockBack = 1.15f;
			Item.rare = 8;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = Mod.Find<ModProjectile>("TrueDepthProj").Type;
			this.empowermentType = 5;
			this.energy = 14;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("True Depths Baton");
			Tooltip.SetDefault("Shoots 3 bolts of dark energy"
							  + "\nThe number of projectiles shot scales with the number of active shamanic bonds"
							  + "\nHitting at maximum range deals increased damage");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);

			int projectilesNumber = 0;

			switch (nbBonds)
			{
				case 1:
					projectilesNumber = 2;
					break;
				case 2:
					projectilesNumber = 2;
					break;
				case 3:
					projectilesNumber = 3;
					break;
				case 4:
					projectilesNumber = 3;
					break;
				case 5:
					projectilesNumber = 4;
					break;
				default:
					projectilesNumber = 1;
					break;
			}

			for (int i = 0; i < projectilesNumber; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(4));
				this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

		// public override void ModifyTooltips(List<TooltipLine> list) // Useful because of damage range
		// {
		// int index = -1;
		// for (int m = 0; m < list.Count; m++)
		// {
		// if (list[m].Name.Equals("Damage")) { index = m; break;}
		// }
		// string oldTooltip = list[index].text;
		// string[] split = oldTooltip.Split(' ');
		// int dmg2 = 0;
		// Int32.TryParse(split[0], out dmg2);
		// dmg2 = (int)(dmg2 + 10);
		// list.RemoveAt(index);
		// list.Insert(index, new TooltipLine(mod, "Damage", split[0] + " - " + dmg2 + " shamanic damage"));

		// Mod thoriumMod = OrchidMod.ThoriumMod;
		// if (thoriumMod != null) {
		// list.Insert(1, new TooltipLine(mod, "Class", "[c/666DFF:-Shaman Class-]"));
		// }
		// }

		public override void AddRecipes()
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;

			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<DepthsBaton>(), 1);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.Find<ModItem>("BrokenHeroFragment").Type : ItemType<BrokenHeroScepter>(), (thoriumMod != null) ? 2 : 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}
