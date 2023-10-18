using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class GlowingMushroomVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 10;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 10, 0);
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
			// DisplayName.SetDefault("Glowing Mushroom Extract");
			/* Tooltip.SetDefault("Grows a mushroom, which aura increases the number of spores released by other alchemic extracts"
							+ "\nThe mushroom will absorb the properties of nearby spores, creating more of them"
							+ "\nOnly one mushroom can exist at once"); */
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.GlowingMushroom, 5);
			recipe.AddIngredient(ItemID.MudBlock, 15);
			recipe.Register();
		}

		public override void KillSecond(int timeLeft, Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int nb = 2 + Main.rand.Next(2);
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.GlowingMushroomVialProjAlt2>();
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, spawnProj, 0, 0f, projectile.owner);
			}
			bool spawnedMushroom = false;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				int projType = ProjectileType<Alchemist.Projectiles.Nature.GlowingMushroomVialProj>();
				int projTypeAlt = ProjectileType<Alchemist.Projectiles.Nature.GlowingMushroomVialProjAlt>();
				if (proj.active == true && (proj.type == projType || proj.type == projTypeAlt) && proj.owner == projectile.owner)
				{
					spawnedMushroom = true;
					break;
				}
			}
			if (!spawnedMushroom)
			{
				int dmg = GetSecondaryDamage(player, alchProj.nbElements);
				Vector2 vel = (new Vector2(0f, -2f).RotatedByRandom(MathHelper.ToRadians(20)));
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, ProjectileType<Alchemist.Projectiles.Nature.GlowingMushroomVialProj>(), dmg, 0f, projectile.owner);
			}
		}
	}
}
