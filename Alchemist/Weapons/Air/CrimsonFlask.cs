using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Air
{
	public class CrimsonFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 12;
			item.width = 28;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 15, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.AIR;
			this.rightClickDust = 16;
			this.colorR = 238;
			this.colorG = 97;
			this.colorB = 94;
			this.secondaryDamage = 5;
			this.secondaryScaling = 10f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corruption Flask");
		    Tooltip.SetDefault("Releases floating mushrooms, exploding after a while or when being catalyzed"
							+  "\nThe mushrooms will absorb the properties of nearby spores, creating more of them"
							+  "\nOnly one set of mushrooms can exist at once");
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.Vertebrae, 5);
			recipe.AddIngredient(2887, 5); // Viscious mushroom
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
		
		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			int projType = ProjectileType<Alchemist.Projectiles.Air.CrimsonFlaskProj>();
			int projType2 = ProjectileType<Alchemist.Projectiles.Air.CrimsonFlaskProjAlt>();
			bool spawnedMushroom = false;
			for (int l = 0; l < Main.projectile.Length; l++) {  
				Projectile proj = Main.projectile[l];
				if (proj.active == true && proj.type == projType && proj.owner == projectile.owner) {
					spawnedMushroom = true;
					break;
				}
			}
			
			if (!spawnedMushroom) {
				int dmg = getSecondaryDamage(modPlayer, alchProj.nbElements);
				int nb = (alchProj.nbElements * 3) + Main.rand.Next(alchProj.nbElements * 2);
				for (int i = 0 ; i < nb ; i ++) {
					float speed = (5f / (nb + 1)) * (i + 1); 
					Vector2 vel = (new Vector2(0f, speed).RotatedByRandom(MathHelper.ToRadians(180)));
					Projectile newProj = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 5, vel.X, vel.Y, projType, dmg, 0f, projectile.owner)];
					newProj.timeLeft = 70 + 15 * i;
					newProj.netUpdate = true;
				}
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 5, 0f, 0f, projType2, 0, 0f, projectile.owner);
			}
		}
	}
}
