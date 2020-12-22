using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class AbyssPrecinct : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 120;
			item.width = 38;
			item.height = 38;
			item.useTime = 60;
			item.useAnimation = 60;
			item.knockBack = 6.15f;
			item.rare = 10;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.UseSound = SoundID.Item122;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("AbyssPrecinctProj");
			item.shootSpeed = 10f;
			this.empowermentType = 2;
			this.empowermentLevel = 5;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Abyss Precinct");
		  Tooltip.SetDefault("Shoots an abyssal vortex, pulsating with energy"
							+"\nHitting an enemy grants you an abyss fragment"
							+"\nIf you have 5 abyss fragments, your next hit will make your shamanic bonds more effective for 15 seconds");
		}
		
		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
			position += muzzleOffset;
			}
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Projectile.NewProjectile(position.X - 4, position.Y - 4, speedX, speedY, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AbyssFragment", 18);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
