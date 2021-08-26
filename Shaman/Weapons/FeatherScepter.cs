using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class FeatherScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 13;
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
			item.width = 40;
			item.height = 40;
			item.useTime = 33;
			item.useAnimation = 33;
			item.knockBack = 0f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.UseSound = SoundID.Item45;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("FeatherScepterProj");
			this.empowermentType = 3;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Feather Scepter");
			Tooltip.SetDefault("Shoots dangerous spinning feathers"
							  + "\nThe projectiles gain in damage after a while"
							  + "\nHaving 3 or more active shamanic bonds will result in more projectiles shot");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);

			if (nbBonds > 2)
			{
				Vector2 perturbedSpeed = new Vector2(speedX / 2, speedY / 2).RotatedByRandom(MathHelper.ToRadians(15));
				this.newShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(null, "HarpyTalon", 2);
			recipe.AddIngredient(ItemID.Feather, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
