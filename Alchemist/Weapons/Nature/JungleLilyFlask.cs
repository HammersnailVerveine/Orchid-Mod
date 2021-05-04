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
	public class JungleLilyFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 8;
			item.width = 30;
			item.height = 30;
			item.rare = 2;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 3;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = DustType<Dusts.BloomingDust>();
			this.colorR = 177;
			this.colorG = 46;
			this.colorB = 77;
			this.secondaryDamage = 10;
			this.secondaryScaling = 5f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Lily Extract");
		    Tooltip.SetDefault("Enemies and spores within impact radius will bloom"
							+  "\nBloomed spores will duplicate a maximum of once"
							+  "\nBloomed enemies will spawn spores for each of their coatings"
							+  "\nDirect hits will apply a nature coating");
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);		
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(null, "JungleLilyItem", 2);
			recipe.AddIngredient(209, 5); // Stinger
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);		
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(null, "JungleLilyItemBloomed", 1);
			recipe.AddIngredient(209, 5); // Stinger
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
		
		public override void KillFirst(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			int range = 100 * alchProj.nbElements;
			int nb = 20 * alchProj.nbElements;
			OrchidModProjectile.spawnDustCircle(projectile.Center, this.rightClickDust, (int)(range * 0.75), nb, true, 1.5f, 1f, 8f);
			OrchidModProjectile.spawnDustCircle(projectile.Center, this.rightClickDust, (int)(range * 0.5), (int)(nb / 3), true, 1.5f, 1f, 16f, true, true, false, 0, 0, true);
				
			int projType = ProjectileType<Alchemist.Projectiles.Nature.JungleLilyFlaskProj>();
			int damage = getSecondaryDamage(modPlayer, alchProj.nbElements);
			int newProjectileInt = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, projType, damage, 0f, projectile.owner);
			Projectile newProjectile = Main.projectile[newProjectileInt];
			newProjectile.width = range * 2;
			newProjectile.height = range * 2;
			newProjectile.position.X = projectile.Center.X - (newProjectile.width / 2);
			newProjectile.position.Y = projectile.Center.Y - (newProjectile.width / 2);
			newProjectile.netUpdate = true;
		}
		
		public override void OnHitNPCFirst(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, 
		OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			modTarget.alchemistNature = 60 * 10;
		}
	}
}
