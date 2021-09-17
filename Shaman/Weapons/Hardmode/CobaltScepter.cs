using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class CobaltScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 31;
			item.width = 46;
			item.height = 46;
			item.useTime = 50;
			item.useAnimation = 50;
			item.knockBack = 4.15f;
			item.rare = 4;
			item.value = Item.sellPrice(0, 1, 35, 0);
			item.UseSound = SoundID.Item117;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("CobaltScepterProj");
			this.empowermentType = 4;
			this.energy = 12;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Scepter");
			Tooltip.SetDefault("Shoots a potent cobalt bolt, hitting your enemy 3 times"
							  + "\nHitting the same target with all 3 shots will grant you a cobalt orb"
							  + "\nIf you have 5 cobalt orbs, your next attack will partially fill all active shamanic bonds");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				this.NewShamanProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("CobaltScepterProj"), damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CobaltBar, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
