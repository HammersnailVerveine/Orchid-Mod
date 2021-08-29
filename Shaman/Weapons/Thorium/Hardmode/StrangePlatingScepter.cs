using Microsoft.Xna.Framework;
using OrchidMod.Interfaces;
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
			item.damage = 65;
			item.width = 50;
			item.height = 50;
			item.useTime = 18;
			item.useAnimation = 18;
			item.knockBack = 1.25f;
			item.rare = ItemRarityID.LightPurple;
			item.value = Item.sellPrice(0, 6, 0, 0);
			item.UseSound = SoundID.Item12;
			item.autoReuse = true;
			item.shootSpeed = 20f;
			item.shoot = mod.ProjectileType("StrangePlatingScepterProj");
			this.empowermentType = 1;
			this.energy = 10;
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

			if (Main.rand.Next(101) < 4 + OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) * 4)
			{
				this.NewShamanProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("StrangePlatingScepterProjAlt"), damage * 2, knockBack, player.whoAmI);
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
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(thoriumMod, "StrangePlating", 12);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}

