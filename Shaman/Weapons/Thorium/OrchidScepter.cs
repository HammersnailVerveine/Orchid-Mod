using Microsoft.Xna.Framework;
using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class OrchidScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			Item.damage = 18;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.knockBack = 4f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shootSpeed = 13f;
			Item.shoot = Mod.Find<ModProjectile>("OrchidScepterProj").Type;
			this.empowermentType = 4;
			this.energy = 4;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Orchid Scepter");
			Tooltip.SetDefault("Shoots a volley of piercing petals"
							+ "\nThe number of petals increase with active shamanic bonds"
							+ "\nHaving 3 or more bonds will allow the petals to pierce more enemies");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 1 + Main.rand.Next(2);
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);
			numberProjectiles += nbBonds > 1 ? nbBonds > 3 ? 2 : 1 : 0;

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(4));
				this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(Mod);
				recipe.AddTile(thoriumMod.Find<ModTile>("ArcaneArmorFabricator").Type);
				recipe.AddIngredient(thoriumMod, "Petal", 8);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}

