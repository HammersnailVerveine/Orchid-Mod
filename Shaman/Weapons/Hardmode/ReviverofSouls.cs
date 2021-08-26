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
    public class ReviverofSouls : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 61;
			item.channel = true;
			item.width = 30;
			item.height = 30;
			item.useTime = 45;
			item.useAnimation = 45;
			item.knockBack = 4.15f;
			item.rare = 8;
			item.value = Item.sellPrice(0, 6, 0, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("ReviverofSoulsProj");
			this.empowermentType = 3;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Reviver of Souls");
		  Tooltip.SetDefault("Summons vengeful souls, damaging your enemies 3 times"
							+"\nHitting will grant you and empower floating spirit flames"
							+"\nWeapon damage is increased with the number of spirit flames"
							+"\nUpon reaching a certain number of flames, they will empower you"
							+"\nWhile empowered, all your attacks with this weapon will reset every single active shamanic empowerment,"
							+"\n                                                            and the weapon damage will increase more per shot");
		}

		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().shamanDamage;
			add += (((OrchidModPlayer)player.GetModPlayer(mod, "OrchidModPlayer")).shamanOrbCircle == ShamanOrbCircle.REVIVER) ? ((OrchidModPlayer)player.GetModPlayer(mod, "OrchidModPlayer")).orbCountCircle * 0.035f : 0;
		}
		
		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				this.newShamanProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("ReviverofSoulsProj"), damage, knockBack, player.whoAmI);
			}
			return false;
		}
		
		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.MythrilAnvil);		
			recipe.AddIngredient(3261, 20);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}
