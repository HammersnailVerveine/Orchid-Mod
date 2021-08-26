using System.Collections.Generic;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
 
namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class MythrilScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 41;
			item.width = 46;
			item.height = 46;
			item.useTime = 50;
			item.useAnimation = 50;
			item.knockBack = 4.15f;
			item.rare = 4;
			item.value = Item.sellPrice(0, 2, 2, 0);
			item.UseSound = SoundID.Item117;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("MythrilScepterProj");
			this.empowermentType = 4;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Mythril Scepter");
		  Tooltip.SetDefault("Shoots a potent mythril bolt, hitting your enemy 3 times"
							+"\nHitting the same target with all 3 shots will grant you a mythril orb"
							+"\nIf you have 5 mythril orbs, your next hit will increase your defense by 8 for 30 seconds");
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MythrilBar, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
		
		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				this.newShamanProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("MythrilScepterProj"), damage, knockBack, player.whoAmI);
			}
			return false;
		}
    }
}
