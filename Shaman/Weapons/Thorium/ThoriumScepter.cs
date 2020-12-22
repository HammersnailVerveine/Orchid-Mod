using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class ThoriumScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 14;
			item.width = 30;
			item.height = 30;
			item.useTime = 25;
			item.useAnimation = 25;
			item.knockBack = 3.25f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 28, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = false;
			item.shootSpeed = 10f;
			item.shoot = mod.ProjectileType("ThoriumScepterProj");
			this.empowermentType = 1;
			this.empowermentLevel = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thorium Scepter");
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod == null) {
				Tooltip.SetDefault("[c/FF0000:Thorium Mod is not loaded]"
								+ "\n[c/970000:This is a cross-content weapon]");
				return;
			}
			Tooltip.SetDefault("Fires out a bolt of magic, dividing upon hitting a foe"
							+ "\nIf you have 3 or more active shamanic bonds, the bonus projectiles will home at nearby enemies");
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f; 
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
			
			return true;
		}
		
		public override void AddRecipes()
		{
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				ModRecipe recipe = new ModRecipe(thoriumMod);
				recipe.AddTile(thoriumMod.TileType("ThoriumAnvil"));		
				recipe.AddIngredient(null, "ThoriumBar", 8);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}

