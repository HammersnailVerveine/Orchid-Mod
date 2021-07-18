using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Interfaces;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class AbyssPrecinct : OrchidModShamanItem, IGlowingItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 120;
			item.width = 38;
			item.height = 38;
			item.useTime = 60;
			item.useAnimation = 60;
			item.knockBack = 6.15f;
			item.rare = ItemRarityID.Red;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.UseSound = SoundID.Item122;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<Projectiles.OreOrbs.Big.AbyssPrecinctProj>();
			item.shootSpeed = 10f;
			this.empowermentType = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyss Precinct");
			Tooltip.SetDefault("Shoots an abyssal vortex, pulsating with energy"
								+ "\nHitting an enemy grants you an abyss fragment"
								+ "\nIf you have 5 abyss fragments, your next hit will increase shamanic damage by 20% for 30 seconds");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
			position += muzzleOffset;
			}
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Projectile.NewProjectile(position.X - 4, position.Y - 4, speedX, speedY, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Misc.AbyssFragment>(), 18);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			OrchidHelper.DrawSimpleItemGlowmaskInWorld(item, spriteBatch, ModContent.GetTexture("OrchidMod/Glowmasks/AbyssPrecinct_Glowmask"), Color.White, rotation, scale);
		}

		public void DrawItemGlowmask(PlayerDrawInfo drawInfo)
		{
			OrchidHelper.DrawSimpleItemGlowmaskOnPlayer(drawInfo, ModContent.GetTexture("OrchidMod/Glowmasks/AbyssPrecinct_Glowmask"));
		}
	}
}
