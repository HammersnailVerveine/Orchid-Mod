using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class AbyssPrecinct : OrchidModShamanItem, IGlowingItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 120;
			Item.width = 38;
			Item.height = 38;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.knockBack = 6.15f;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.UseSound = SoundID.Item122;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.OreOrbs.Big.AbyssPrecinctProj>();
			Item.shootSpeed = 10f;
			this.empowermentType = 2;
			this.energy = 10;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Abyss Precinct");
			Tooltip.SetDefault("Shoots an abyssal vortex, pulsating with energy"
								+ "\nHitting an enemy grants you an abyss fragment"
								+ "\nIf you have 5 abyss fragments, your next hit will increase shamanic damage by 20% for 30 seconds");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				this.NewShamanProjectile(position.X - 4, position.Y - 4, speedX, speedY, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(Mod);
			recipe.AddIngredient(ModContent.ItemType<Misc.AbyssFragment>(), 18);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			OrchidHelper.DrawSimpleItemGlowmaskInWorld(Item, spriteBatch, ModContent.GetTexture("OrchidMod/Glowmasks/AbyssPrecinct_Glowmask"), Color.White, rotation, scale);
		}

		public void DrawItemGlowmask(PlayerDrawInfo drawInfo)
		{
			OrchidHelper.DrawSimpleItemGlowmaskOnPlayer(drawInfo, ModContent.GetTexture("OrchidMod/Glowmasks/AbyssPrecinct_Glowmask"));
		}
	}
}
