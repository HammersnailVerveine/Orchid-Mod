using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrchidMod.Interfaces;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium
{
    public class MagmaScepter : OrchidModShamanItem, ICrossmodItem
    {
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 15;
			item.width = 46;
			item.height = 46;
			item.useTime = 48;
			item.useAnimation = 48;
			item.knockBack = 4.35f;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(0, 0, 25, 0);
			item.UseSound = SoundID.Item21;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("MagmaScepterProj");
			this.empowermentType = 4;
			this.empowermentLevel = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magma Scepter");
			Tooltip.SetDefault("Shoots a magma bolt, hitting your enemy 2 times"
							+"\nHitting the same target twice will grant you a magma orb"
							+"\nIf you have 5 orbs, your next hit will explode"
							+"\nAttacks might singe the target, causing extra damage");
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
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
				recipe.AddIngredient(thoriumMod, "MagmaCore", 8);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}
