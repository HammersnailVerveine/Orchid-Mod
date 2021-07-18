using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrchidMod.Interfaces;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium
{
    public class Monowai : OrchidModShamanItem, ICrossmodItem
    {
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 30;
			item.width = 44;
			item.height = 44;
			item.useTime = 38;
			item.useAnimation = 38;
			item.knockBack = 4.75f;
			item.rare = 3;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.UseSound = SoundID.Item21;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("MonowaiProj");
			this.empowermentType = 4;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Monowai"); // Named after an undersea volcano
			Tooltip.SetDefault("Shoots elemental bolts, hitting your enemy 2 times"
							+"\nHitting the same target twice will grant you a volcanic orb"
							+"\nIf you have 5 orbs, your next hit will explode, throwing enemies in the air"
							+"\nAttacks might singe the target, causing extra damage");
		}
		
		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			
			int numberProjectiles = 2;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
			}
			return false;
		}
		
		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddTile(TileID.Anvils);
				recipe.AddIngredient(mod.ItemType("MagmaScepter"), 1);
				recipe.AddIngredient(mod.ItemType("AquaiteScepter"), 1);
				recipe.AddIngredient(thoriumMod, "aDarksteelAlloy", 10);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}
