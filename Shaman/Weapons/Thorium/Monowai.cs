using Microsoft.Xna.Framework;
using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class Monowai : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			Item.damage = 30;
			Item.width = 44;
			Item.height = 44;
			Item.useTime = 38;
			Item.useAnimation = 38;
			Item.knockBack = 4.75f;
			Item.rare = 3;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.UseSound = SoundID.Item21;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = Mod.Find<ModProjectile>("MonowaiProj").Type;
			this.empowermentType = 4;
			this.energy = 8;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Monowai"); // Named after an undersea volcano
			Tooltip.SetDefault("Shoots elemental bolts, hitting your enemy 2 times"
							+ "\nHitting the same target twice will grant you a volcanic orb"
							+ "\nIf you have 5 orbs, your next hit will explode, throwing enemies in the air"
							+ "\nAttacks might singe the target, causing extra damage");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 2;
			for (int i = 0; i < numberProjectiles; i++)
			{
				this.NewShamanProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(Mod);
				recipe.AddTile(TileID.Anvils);
				recipe.AddIngredient(Mod.Find<ModItem>("MagmaScepter").Type, 1);
				recipe.AddIngredient(Mod.Find<ModItem>("AquaiteScepter").Type, 1);
				recipe.AddIngredient(thoriumMod, "aDarksteelAlloy", 10);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}
