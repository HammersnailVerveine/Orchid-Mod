using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Air
{
	public class DeathweedFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 13;
			item.width = 30;
			item.height = 30;
			item.rare = 2;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.AIR;
			this.rightClickDust = 27;
			this.colorR = 165;
			this.colorG = 0;
			this.colorB = 236;
			this.secondaryDamage = 18;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deathweed Extract");
			Tooltip.SetDefault("Releases air spores, the less other extracts used, the more"
							+ "\nOnly one set of spores can exist at once"
							+ "\nSpores deals 10% increased damage against fire-coated enemies");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.Deathweed, 3);
			recipe.AddIngredient(ItemID.ShadowScale, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.Deathweed, 3);
			recipe.AddIngredient(ItemID.TissueSample, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			mult *= player.GetModPlayer<OrchidModPlayer>().alchemistDamage;
			if (player.ZoneCrimson || player.ZoneCorrupt) mult *= 1.2f;
		}

		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int nb = 2 + Main.rand.Next(2);
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Air.AirSporeProjAlt>();
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
			}
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active == true && proj.type == ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>() && proj.owner == projectile.owner && proj.localAI[1] != 1f)
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
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>(), dmg, 0f, projectile.owner);
			}
		}

		public override void AddVariousEffects(Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			alchProj.nbElementsNoExtract--;
		}
	}
}
