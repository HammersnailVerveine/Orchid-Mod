using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
 
namespace OrchidMod.Shaman.Weapons.Thorium
{
    public class IronScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 23;
			item.width = 36;
			item.height = 38;
			item.useTime = 72;
			item.useAnimation = 72;
			item.knockBack = 4f;
			item.rare = 0;
			item.value = Item.sellPrice(0, 0, 10 , 0);
			item.UseSound = SoundID.Item45;
			item.shootSpeed = 7f;
			item.shoot = mod.ProjectileType("OpalScepterProj");
			this.empowermentType = 4;
			this.empowermentLevel = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Opal Scepter");
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod == null) {
				Tooltip.SetDefault("[c/FF0000:Thorium Mod is not loaded]"
								+ "\n[c/970000:This is a cross-content weapon]");
				return;
			}
			Tooltip.SetDefault("\nHitting an enemy will grant you an Opal orb"
							+"\nIf you have 3 opal orbs, your next hit will increase your shamanic critical strike damage for 15 seconds");
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
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				ModRecipe recipe = new ModRecipe(thoriumMod);
				recipe.AddTile(TileID.Anvils);		
				recipe.AddIngredient(null, "Opal", 8);
				recipe.AddIngredient(ItemID.IronBar, 10);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}
