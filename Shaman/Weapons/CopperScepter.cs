using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
 
namespace OrchidMod.Shaman.Weapons
{
    public class CopperScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 18;
			item.width = 36;
			item.height = 38;
			item.useTime = 74;
			item.useAnimation = 74;
			item.knockBack = 3.25f;
			item.rare = 0;
			item.value = Item.sellPrice(0, 0, 4, 0);
			item.UseSound = SoundID.Item45;
			item.shootSpeed = 6f;
			item.shoot = mod.ProjectileType("CopperScepterProj");
			this.empowermentType = 4;
			this.empowermentLevel = 1;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Amethyst Scepter");
		  Tooltip.SetDefault("\nHitting an enemy will grant you an amethyst orb"
							+"\nIf you have 3 amethyst orbs, your next hit will empower your shamanic spirit bonds for 15 seconds");
		}
			
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 40f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
			position += muzzleOffset;
			}
			return true;
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);		
			recipe.AddIngredient(ItemID.Amethyst, 8);
			recipe.AddIngredient(ItemID.CopperBar, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}
