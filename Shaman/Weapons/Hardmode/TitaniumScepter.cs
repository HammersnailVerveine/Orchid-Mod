using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class TitaniumScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 53;
			Item.width = 46;
			Item.height = 46;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.knockBack = 4.15f;
			Item.rare = 4;
			Item.value = Item.sellPrice(0, 3, 20, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = Mod.Find<ModProjectile>("TitaniumScepterProj").Type;
			this.empowermentType = 4;
			this.energy = 12;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Titanium Scepter");
			Tooltip.SetDefault("Shoots a potent titanium bolt, hitting your enemy 3 times"
							  + "\nHitting the same target with all 3 shots will grant you a titanium orb"
							  + "\nIf you have 5 titanium orbs, your next hit will allow you to dodge one attack within 3 seconds");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				this.NewShamanProjectile(position.X, position.Y, speedX, speedY, Mod.Find<ModProjectile>("TitaniumScepterProj").Type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TitaniumBar, 13);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}
