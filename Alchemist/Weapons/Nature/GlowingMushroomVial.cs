using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;


namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class GlowingMushroomVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 10;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 172;
			this.colorR = 44;
			this.colorG = 26;
			this.colorB = 233;
			this.secondaryScaling = 4f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowing Mushroom Extract");
		    Tooltip.SetDefault("Grows a mushroom, which aura increases the number of spores released by other alchemic extracts"
							+  "\nThe mushroom will absorb the properties of nearby spores, creating more of them"
							+  "\nOnly one mushroom can exist at once");
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);		
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.GlowingMushroom, 5);
			recipe.AddIngredient(ItemID.MudBlock, 15);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
		
		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			int nb = 2 + Main.rand.Next(2);
			for (int i = 0 ; i < nb ; i ++) {
				Vector2	vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.GlowingMushroomVialProjAlt2>();
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
			}
			bool spawnedMushroom = false;
			for (int l = 0; l < Main.projectile.Length; l++) {  
				Projectile proj = Main.projectile[l];
				int projType = ProjectileType<Alchemist.Projectiles.Nature.GlowingMushroomVialProj>();
				int projTypeAlt = ProjectileType<Alchemist.Projectiles.Nature.GlowingMushroomVialProjAlt>();
				if (proj.active == true && (proj.type == projType || proj.type == projTypeAlt) && proj.owner == projectile.owner) {
					spawnedMushroom = true;
					break;
				}
			}
			if (!spawnedMushroom) {
				int dmg = getSecondaryDamage(modPlayer, alchProj.nbElements);
				Vector2 vel = (new Vector2(0f, -2f).RotatedByRandom(MathHelper.ToRadians(20)));
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Nature.GlowingMushroomVialProj>(), dmg, 0f, projectile.owner);
			}
		}
	}
}
