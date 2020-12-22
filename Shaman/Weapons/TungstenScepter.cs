using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
 
namespace OrchidMod.Shaman.Weapons
{
    public class TungstenScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 28;
			item.width = 36;
			item.height = 38;
			item.useTime = 58;
			item.useAnimation = 58;
			item.knockBack = 4.25f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 30, 0);
			item.UseSound = SoundID.Item45;
			item.autoReuse = true;
			item.shootSpeed = 8f;
			item.shoot = mod.ProjectileType("TungstenScepterProj");
			this.empowermentType = 4;
			this.empowermentLevel = 1;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Emerald Scepter");
		  Tooltip.SetDefault("\nHitting an enemy will grant you an emerald orb"
							+"\nIf you have 3 emerald orbs, your next hit will empower your shamanic air bonds for 15 seconds");
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
			recipe.AddIngredient(ItemID.Emerald, 8);
			recipe.AddIngredient(ItemID.TungstenBar, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}
