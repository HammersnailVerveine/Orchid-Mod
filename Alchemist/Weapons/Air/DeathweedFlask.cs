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
			Item.damage = 13;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 10, 0);
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

		public override void KillSecond(int timeLeft, Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int nb = 2 + Main.rand.Next(2);
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Air.AirSporeProjAlt>();
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, spawnProj, 0, 0f, projectile.owner);
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
				int dmg = GetSecondaryDamage(player, alchProj.nbElements);
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>(), dmg, 0f, projectile.owner);
			}
		}

		public override void AddVariousEffects(Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			alchProj.nbElementsNoExtract--;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.Deathweed, 3);
			recipe.AddIngredient(ItemID.ShadowScale, 5);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.Deathweed, 3);
			recipe.AddIngredient(ItemID.TissueSample, 5);
			recipe.Register();
		}
	}
}
