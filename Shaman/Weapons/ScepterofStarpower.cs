using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
 
namespace OrchidMod.Shaman.Weapons
{
    public class ScepterofStarpower : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 10;
			item.width = 38;
			item.height = 38;
			item.useTime = 30;
			item.useAnimation = 30;
			item.knockBack = 5.5f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 15, 0);
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shootSpeed = 9.5f;
			item.shoot = mod.ProjectileType("StarpowerScepterProj");
			this.empowermentType = 3;
			this.empowermentLevel = 1;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Scepter of Starpower");
		  Tooltip.SetDefault("Critical strike chance increases with the number of active shamanic bonds");
		}
		
		public override void UpdateInventory(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			item.crit = 4 + 10 * OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) + modPlayer.shamanCrit;
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
			recipe.AddTile(TileID.WorkBenches);		
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddIngredient(ItemID.Wood, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}
