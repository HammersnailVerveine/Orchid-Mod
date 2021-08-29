using Microsoft.Xna.Framework;
using OrchidMod.Interfaces;
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
			item.damage = 14;
			item.width = 34;
			item.height = 34;
			item.useTime = 30;
			item.useAnimation = 30;
			item.knockBack = 4f;
			item.rare = ItemRarityID.Orange;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shootSpeed = 14f;
			item.shoot = mod.ProjectileType("SpiritDropletScepterProj");
			this.empowermentType = 5;
			this.energy = 6;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fibula");
			Tooltip.SetDefault("Conjures a volley of ethereal bones"
							+ "\nThe number of bones increase with active shamanic bonds");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);

			for (int i = 0; i < nbBonds + 1; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(nbBonds + 1));
				this.newShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddTile(TileID.Anvils);
				recipe.AddIngredient(thoriumMod, "SpiritDroplet", 8);
				recipe.AddIngredient(ItemID.Bone, 20);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}

