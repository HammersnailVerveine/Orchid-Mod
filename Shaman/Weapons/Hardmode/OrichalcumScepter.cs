using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class OrichalcumScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 43;
			item.width = 30;
			item.height = 30;
			item.useTime = 50;
			item.useAnimation = 50;
			item.knockBack = 4.15f;
			item.rare = 4;
			item.value = Item.sellPrice(0, 2, 41, 0);
			item.UseSound = SoundID.Item117;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("OrichalcumScepterProj");
			this.empowermentType = 4;
			this.energy = 10;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Orichalcum Scepter");
			Tooltip.SetDefault("Shoots a potent orichalcum bolt, hitting your enemy 3 times"
							  + "\nHitting the same target with all 3 shots will grant you an orichalcum orb"
							  + "\nIf you have 5 orichalcum orbs, your next hit will release a burst of damaging petals");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.OrichalcumBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				this.NewShamanProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("OrichalcumScepterProj"), damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}
}
