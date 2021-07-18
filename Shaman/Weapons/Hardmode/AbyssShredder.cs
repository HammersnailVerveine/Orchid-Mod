using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod;
using OrchidMod.Shaman.Weapons.Hardmode;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Interfaces;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class AbyssShredder : OrchidModShamanItem, IGlowingItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 110;
			item.noUseGraphic = false;
			item.magic = true;
			item.width = 42;
			item.height = 42;
			item.useTime = 18;
			item.useAnimation = 18;
			item.knockBack = 1.15f;
			item.rare = ItemRarityID.Red;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.UseSound = SoundID.Item122;
			item.autoReuse = true;
			item.shootSpeed = 10f;
			item.shoot = ModContent.ProjectileType<Projectiles.AbyssShardS>();
			this.empowermentType = 1;

			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoUsetimeReforge = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyss Stormcaller");
			Tooltip.SetDefault("Shoots abyss energy thunderbolts"
								+ "\nIncreases weapon speed for each active shamanic bond");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			
			for (int i = 0; i < 1; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("AbyssShard"), damage, knockBack, player.whoAmI);
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("AbyssShardS"), damage, knockBack, player.whoAmI);
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("AbyssShardD"), damage, knockBack, player.whoAmI);
			}
			return false;
		}
		
		public override void UpdateInventory(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod);

			item.useTime = 18 - (2 * nbBonds);
			item.useAnimation = 18 - (2 * nbBonds);
		}
		
		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.Blue.ToVector3() * 0.55f * Main.essScale);
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
			OrchidHelper.DrawSimpleItemGlowmaskInWorld(item, spriteBatch, ModContent.GetTexture("OrchidMod/Glowmasks/AbyssShredder_Glowmask"), Color.White, rotation, scale);
		}

		public void DrawItemGlowmask(PlayerDrawInfo drawInfo)
		{
			OrchidHelper.DrawSimpleItemGlowmaskOnPlayer(drawInfo, ModContent.GetTexture("OrchidMod/Glowmasks/AbyssShredder_Glowmask"));
		}
	}
}
