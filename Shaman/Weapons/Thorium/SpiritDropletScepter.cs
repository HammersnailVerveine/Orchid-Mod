using Microsoft.Xna.Framework;
using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class SpiritDropletScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			Item.damage = 14;
			Item.width = 34;
			Item.height = 34;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.knockBack = 4f;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shootSpeed = 14f;
			Item.shoot = Mod.Find<ModProjectile>("SpiritDropletScepterProj").Type;
			this.empowermentType = 5;
			this.energy = 6;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Fibula");
			Tooltip.SetDefault("Conjures a volley of ethereal bones"
							+ "\nThe number of bones increase with active shamanic bonds");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);

			for (int i = 0; i < nbBonds + 1; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(nbBonds + 1));
				this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.Anvils);
				recipe.AddIngredient(thoriumMod, "SpiritDroplet", 8);
				recipe.AddIngredient(ItemID.Bone, 20);
				recipe.Register();
				recipe.AddRecipe();
			}
		}
	}
}

