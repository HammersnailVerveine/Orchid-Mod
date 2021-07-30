using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
using OrchidMod.Interfaces;

namespace OrchidMod.Shaman.Weapons.Thorium
{
    public class LeadScepter : OrchidModShamanItem, ICrossmodItem
    {
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 26;
			item.width = 36;
			item.height = 38;
			item.useTime = 62;
			item.useAnimation = 62;
			item.autoReuse = true;
			item.knockBack = 4f;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(0, 0, 25, 0);
			item.UseSound = SoundID.Item45;
			item.shootSpeed = 7.5f;
			item.shoot = mod.ProjectileType("OnyxScepterProj");
			this.empowermentType = 4;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Onyx Scepter");
			Tooltip.SetDefault("\nHitting an enemy will grant you an Onyx orb"
							+"\nIf you have 3 onyx orbs, your next hit will give you 3 armor penetration for 30 seconds");
		}
			
		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddTile(TileID.Anvils);		
				recipe.AddIngredient(thoriumMod, "Onyx", 8);
				recipe.AddIngredient(ItemID.LeadBar, 10);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}
