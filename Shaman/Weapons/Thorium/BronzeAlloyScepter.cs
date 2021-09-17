using Microsoft.Xna.Framework;
using OrchidMod.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class BronzeAlloyScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 23;
			item.width = 30;
			item.height = 30;
			item.useTime = 18;
			item.useAnimation = 18;
			item.knockBack = 2.75f;
			item.rare = ItemRarityID.Orange;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shootSpeed = 14f;
			item.shoot = mod.ProjectileType("BronzeAlloyScepterProj");
			this.empowermentType = 4;
			this.energy = 5;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Basilisk's Fang");
			Tooltip.SetDefault("Fires out a poisonous basilisk fang"
							+ "\nThe number of active shamanic bonds will increase the poison duration"
							+ "\nThe weapon itself can critically strike, releasing a petrifying projectile"
							+ "\nThe more shamanic bonds you have, the higher the chances of critical strike");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);

			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3));
			if (Main.rand.Next(101) < 5 + nbBonds * 5)
			{
				this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X * 1.2f, perturbedSpeed.Y * 1.2f, mod.ProjectileType("BronzeAlloyScepterProjAlt"), damage * 2, knockBack, player.whoAmI);
			}
			else
			{
				this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
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
				recipe.AddIngredient(thoriumMod, "BronzeFragments", 10);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}

