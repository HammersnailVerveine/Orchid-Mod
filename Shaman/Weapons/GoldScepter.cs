using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
 
namespace OrchidMod.Shaman.Weapons
{
    public class GoldScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 32;
			item.width = 36;
			item.height = 38;
			item.useTime = 50;
			item.useAnimation = 50;
			item.knockBack = 4.75f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.UseSound = SoundID.Item45;
			item.autoReuse = true;
			item.shootSpeed = 9f;
			item.shoot = mod.ProjectileType("GoldScepterProj");
			this.empowermentType = 4;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Ruby Scepter");
		  Tooltip.SetDefault("\nHitting an enemy will grant you a ruby orb"
							+"\nIf you have 3 ruby orbs, your next hit will increase your life regeneration for 30 seconds");
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
			recipe.AddIngredient(ItemID.Ruby, 8);
			recipe.AddIngredient(ItemID.GoldBar, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}
