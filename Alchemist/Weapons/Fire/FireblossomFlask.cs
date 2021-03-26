using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;


namespace OrchidMod.Alchemist.Weapons.Fire
{
	public class FireblossomFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 15;
			item.width = 30;
			item.height = 30;
			item.rare = 3;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 3;
			this.element = AlchemistElement.FIRE;
			this.rightClickDust = 6;
			this.colorR = 255;
			this.colorG = 84;
			this.colorB = 0;
			this.secondaryDamage = 30;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fireblossom Extract");
		    Tooltip.SetDefault("Releases fire spores, the less other extracts used, the more"
							+  "\nOnly one set of spores can exist at once"
							+  "\nSpores deals 10% increased damage against fire-coated enemies");
		}
		
		public override void AddRecipes()
		{
		    ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);		
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.Fireblossom, 3);
			recipe.AddIngredient(ItemID.Hellstone, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
		
		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			int nb = 2 + Main.rand.Next(2);
			for (int i = 0 ; i < nb ; i ++) {
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Fire.FireSporeProjAlt>();
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
			}
			for (int l = 0; l < Main.projectile.Length; l++) {  
				Projectile proj = Main.projectile[l];
				if (proj.active == true && proj.type == ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>() && proj.owner == projectile.owner && proj.localAI[1] != 1f) {
					proj.Kill();
				}
			}
						
			nb = alchProj.nbElements + alchProj.nbElementsNoExtract;
			nb += player.HasBuff(BuffType<Alchemist.Buffs.MushroomHeal>()) ? Main.rand.Next(3) : 0;
			for (int i = 0 ; i < nb ; i ++) {
				Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
				int dmg = OrchidModAlchemistHelper.getSecondaryDamage(modPlayer, alchProj.nbElements);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>(), dmg, 0f, projectile.owner);
			}
		}
		
		public override void AddVariousEffects(Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile proj, OrchidModGlobalItem globalItem) {
			alchProj.nbElementsNoExtract --;
		}
		
	}
}
