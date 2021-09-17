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
			item.damage = 56;
			item.width = 50;
			item.height = 50;
			item.useTime = 45;
			item.useAnimation = 45;
			item.knockBack = 4.25f;
			item.rare = 7;
			item.value = Item.sellPrice(0, 5, 45, 0);
			item.UseSound = SoundID.Item117;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("ChlorophyteScepterProj");
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
				this.NewShamanProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("ChlorophyteScepterProj"), damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
