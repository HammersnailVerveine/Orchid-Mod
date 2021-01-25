using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
using System;
 
namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class SightScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 25;
			item.width = 48;
			item.height = 48;
			item.useTime = 4;
			item.useAnimation = 32;
			item.knockBack = 0f;
			item.rare = 5;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.autoReuse = true;
			item.shootSpeed = 5.25f;
			item.shoot = mod.ProjectileType("SightScepterProj");
			item.UseSound = SoundID.Item15;
			this.empowermentType = 3;
			this.empowermentLevel = 3;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Light Concentrator");
		  Tooltip.SetDefault("Channels a beam of prismatic energy"
							+"\nHaving 4 or more active shamanic bonds will drastically increase the weapon damage and range");
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			
			mult *= modPlayer.shamanDamage;
			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) > 3) mult *= 1.5f;
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
			position += muzzleOffset;
			}
			return true;
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddIngredient(549, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}

