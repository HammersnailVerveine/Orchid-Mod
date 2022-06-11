using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class WaterleafFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 10;
			Item.width = 30;
			Item.height = 30;
			Item.rare = 1;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 29;
			this.colorR = 22;
			this.colorG = 121;
			this.colorB = 82;
			this.secondaryDamage = 12;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Waterleaf Extract");
			Tooltip.SetDefault("Releases water spores, the less other extracts used, the more"
							+ "\nOnly one set of spores can exist at once"
							+ "\nSpores deals 10% increased damage against fire-coated enemies");
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.Waterleaf, 3);
			recipe.AddIngredient(ItemID.Cactus, 10);
			recipe.Register();
			recipe.AddRecipe();
		}

		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int nb = 2 + Main.rand.Next(2);
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Water.WaterSporeProjAlt>();
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
			}
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active == true && proj.type == ProjectileType<Alchemist.Projectiles.Water.WaterSporeProj>() && proj.owner == projectile.owner && proj.localAI[1] != 1f)
				{
					proj.Kill();
				}
			}
			nb = alchProj.nbElements + alchProj.nbElementsNoExtract;
			nb += player.HasBuff(BuffType<Alchemist.Buffs.MushroomHeal>()) ? Main.rand.Next(3) : 0;
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
				int dmg = getSecondaryDamage(player, modPlayer, alchProj.nbElements);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Water.WaterSporeProj>(), dmg, 0f, projectile.owner);
			}
		}

		public override void AddVariousEffects(Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			alchProj.nbElementsNoExtract--;
		}
	}
}
