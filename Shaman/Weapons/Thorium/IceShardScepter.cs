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
    public class IceShardScepter : OrchidModShamanItem, ICrossmodItem
    {
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 14;
			item.width = 40;
			item.height = 40;
			item.useTime = 35;
			item.useAnimation = 35;
			item.knockBack = 3f;
			item.rare = ItemRarityID.White;
			item.value = Item.sellPrice(0, 0, 5, 30);
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.shootSpeed = 10f;
			item.shoot = mod.ProjectileType("IceShardScepterProj");
			this.empowermentType = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Scepter");
			Tooltip.SetDefault("Shoots frostburn bolts, growing for an instant before being launched"
							+  "\nCritical strike chance increases with the number of active shamanic bonds");
		}
		
		public override void UpdateInventory(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			item.crit = 4 + 10 * OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) + modPlayer.shamanCrit;
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
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddTile(TileID.WorkBenches);		
				recipe.AddIngredient(thoriumMod, "IcyShard", 7);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}
