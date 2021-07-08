using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Chat;
using System;
 
 
namespace OrchidMod.Shaman.Weapons
{
    public class AncientFossilScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 15;
			item.width = 36;
			item.height = 38;
			item.useTime = 30;
			item.useAnimation = 30;
			item.knockBack = 5.5f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.UseSound = SoundID.Item45;
			item.autoReuse = true;
			item.shootSpeed = 9.5f;
			item.shoot = mod.ProjectileType("AncientFossilScepterProjFire");
			this.empowermentType = 5;
		}

		// public override void SetStaticDefaults()
		// {
		  // DisplayName.SetDefault("Ancient Fossil Scepter");
		  // Tooltip.SetDefault("\nFires an ash bolt at your foes"
							// +"\nHaving 2 or more active shamanic bonds will shoot a fire bolt instead");
		// }
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Fossil Scepter");
			Tooltip.SetDefault("[c/FF0000:UNOBTAINABLE]");
		}
			
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int BuffsCount = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);
			
			if (BuffsCount > 1) {
				Vector2 perturbedSpeed = new Vector2(speedX/2, speedY/2).RotatedByRandom(MathHelper.ToRadians(15));
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage + 2, knockBack, player.whoAmI);
			}
			else {
				Vector2 perturbedSpeed = new Vector2(speedX/2, speedY/2).RotatedByRandom(MathHelper.ToRadians(15));
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("AncientFossilScepterProjAsh"), damage, knockBack, player.whoAmI);
			}
			return false;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AncientFossil", 40);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}
