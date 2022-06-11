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
			Item.damage = 43;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.knockBack = 4.15f;
			Item.rare = 4;
			Item.value = Item.sellPrice(0, 2, 41, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = Mod.Find<ModProjectile>("OrichalcumScepterProj").Type;
			this.empowermentType = 4;
			this.energy = 12;
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
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.OrichalcumBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
			recipe.AddRecipe();
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				this.NewShamanProjectile(position.X, position.Y, speedX, speedY, Mod.Find<ModProjectile>("OrichalcumScepterProj").Type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}
}
