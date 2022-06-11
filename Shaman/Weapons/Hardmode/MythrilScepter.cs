using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class MythrilScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 41;
			Item.width = 46;
			Item.height = 46;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.knockBack = 4.15f;
			Item.rare = 4;
			Item.value = Item.sellPrice(0, 2, 2, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = Mod.Find<ModProjectile>("MythrilScepterProj").Type;
			this.empowermentType = 4;
			this.energy = 12;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Mythril Scepter");
			Tooltip.SetDefault("Shoots a potent mythril bolt, hitting your enemy 3 times"
							  + "\nHitting the same target with all 3 shots will grant you a mythril orb"
							  + "\nIf you have 5 mythril orbs, your next hit will increase your defense by 8 for 30 seconds");
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MythrilBar, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
			recipe.AddRecipe();
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				this.NewShamanProjectile(position.X, position.Y, speedX, speedY, Mod.Find<ModProjectile>("MythrilScepterProj").Type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}
}
