using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class ChlorophyteScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 56;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.knockBack = 4.25f;
			Item.rare = 7;
			Item.value = Item.sellPrice(0, 5, 45, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = Mod.Find<ModProjectile>("ChlorophyteScepterProj").Type;
			this.empowermentType = 4;
			this.energy = 12;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Chlorophyte Scepter");
			Tooltip.SetDefault("Shoots a potent chlorophyte bolt, hitting your enemy 3 times"
							  + "\nHitting the same target with all 3 shots will grant you a leaf crystal"
							  + "\nIf you have 5 leaf crystals, your next hit will release harmful clouds of gas at your opponents");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				this.NewShamanProjectile(position.X, position.Y, speedX, speedY, Mod.Find<ModProjectile>("ChlorophyteScepterProj").Type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}
