using Microsoft.Xna.Framework;
using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	public class StrangePlatingScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			Item.damage = 65;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.knockBack = 1.25f;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = Item.sellPrice(0, 6, 0, 0);
			Item.UseSound = SoundID.Item12;
			Item.autoReuse = true;
			Item.shootSpeed = 20f;
			Item.shoot = Mod.Find<ModProjectile>("StrangePlatingScepterProj").Type;
			this.empowermentType = 1;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Prime's Laser");
			Tooltip.SetDefault("The weapon itself can critically strike, releasing a powerful blast"
							+ "\nThe more shamanic bonds you have, the higher the chances of critical strike");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			if (Main.rand.Next(101) < 4 + OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod) * 4)
			{
				this.NewShamanProjectile(position.X, position.Y, speedX, speedY, Mod.Find<ModProjectile>("StrangePlatingScepterProjAlt").Type, damage * 2, knockBack, player.whoAmI);
			}
			else
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
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(thoriumMod, "StrangePlating", 12);
				recipe.Register();
				recipe.AddRecipe();
			}
		}
	}
}

