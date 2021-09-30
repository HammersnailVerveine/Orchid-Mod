using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class DaybloomFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 8;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 55;
			this.colorR = 255;
			this.colorG = 198;
			this.colorB = 0;
			this.secondaryDamage = 12;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Daybloom Extract");
			Tooltip.SetDefault("Releases nature spores, the less other extracts used, the more"
							+ "\nOnly one set of spores can exist at once"
							+ "\nSpores deals 10% increased damage against fire-coated enemies");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.Daybloom, 3);
			recipe.AddIngredient(ItemID.Acorn, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int nb = 2 + Main.rand.Next(2);
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
			}
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active == true && proj.type == ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>() && proj.owner == projectile.owner && proj.localAI[1] != 1f)
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
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>(), dmg, 0f, projectile.owner);
			}
		}

		public override void AddVariousEffects(Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile proj, OrchidModGlobalItem globalItem)
		{
			alchProj.nbElementsNoExtract--;
		}
	}
}
