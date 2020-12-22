using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Thorium
{
    public class AquaiteScepter : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 20;
			item.width = 46;
			item.height = 46;
			item.useTime = 40;
			item.useAnimation = 40;
			item.knockBack = 4.75f;
			item.rare = 2;
			item.value = Item.sellPrice(0, 0, 30, 0);
			item.UseSound = SoundID.Item21;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("AquaiteScepterProj");
			this.empowermentType = 4;
			this.empowermentLevel = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aquaite Scepter");
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod == null) {
				Tooltip.SetDefault("[c/FF0000:Thorium Mod is not loaded]"
								+ "\n[c/970000:This is a cross-content weapon]");
				return;
			}
			Tooltip.SetDefault("Shoots a water bolt, hitting your enemy 2 times"
							+"\nHitting the same target twice will grant you a water crystal"
							+"\nIf you have 5 crystals, your next hit will summon a powerful geyser");
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
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("AquaiteScepterProj"), damage, knockBack, player.whoAmI, 0f, 0f);
			}
			return false;
		}
		
		public override void AddRecipes()
		{
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
				ModRecipe recipe = new ModRecipe(thoriumMod);
				recipe.AddTile(TileID.Anvils);		
				recipe.AddIngredient(null, "AquaiteBar", 14);
				recipe.AddIngredient(null, "DepthScale", 6);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
        }
    }
}
