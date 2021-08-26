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
			item.damage = 62;
			item.magic = true;
			item.width = 30;
			item.height = 30;
			item.useTime = 42;
			item.useAnimation = 42;
			item.knockBack = 1.15f;
			item.rare = 8;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.UseSound = SoundID.Item117;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("TrueDepthProj");
			this.empowermentType = 5;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Depths Baton");
			Tooltip.SetDefault("Shoots 3 bolts of dark energy"
							  + "\nThe number of projectiles shot scales with the number of active shamanic bonds"
							  + "\nHitting at maximum range deals increased damage");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);

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
				this.newShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
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

			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemType<DepthsBaton>(), 1);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.ItemType("BrokenHeroFragment") : ItemType<BrokenHeroScepter>(), (thoriumMod != null) ? 2 : 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
