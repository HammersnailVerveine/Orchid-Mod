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
    public class PalladiumScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 33;
			item.width = 46;
			item.height = 46;
			item.useTime = 50;
			item.useAnimation = 50;
			item.knockBack = 4.15f;
			item.rare = 4;
			item.value = Item.sellPrice(0, 1, 76, 0);
			item.UseSound = SoundID.Item117;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("PalladiumScepterProj");
			this.empowermentType = 4;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Palladium Scepter");
		  Tooltip.SetDefault("Shoots a potent palladium bolt, hitting your enemy 3 times"
							+"\nHitting the same target with all 3 shots will grant you a palladium orb"
							+"\nIf you have 5 palladium orbs, your next attack will resplenish 25 life on hit");
		}
		
		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				this.newShamanProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("PalladiumScepterProj"), damage, knockBack, player.whoAmI);
			}
			return false;
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PalladiumBar, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}
