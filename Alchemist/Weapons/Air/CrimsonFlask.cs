using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Air
{
	public class CrimsonFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 12;
			Item.width = 28;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 15, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.AIR;
			this.rightClickDust = 16;
			this.colorR = 238;
			this.colorG = 97;
			this.colorB = 94;
			this.secondaryDamage = 10;
			this.secondaryScaling = 5f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Visceral Mycelium");
			Tooltip.SetDefault("Releases floating mushrooms, exploding after a while or when being catalyzed"
							+ "\nThe mushrooms will absorb the properties of nearby spores, creating more of them"
							+ "\nOnly one set of mushrooms can exist at once");
		}

		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int projType = ProjectileType<Alchemist.Projectiles.Air.CrimsonFlaskProj>();
			int projType2 = ProjectileType<Alchemist.Projectiles.Air.CrimsonFlaskProjAlt>();
			bool spawnedMushroom = false;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active == true && proj.type == projType && proj.owner == projectile.owner)
				{
					spawnedMushroom = true;
					break;
				}
			}

			Vector2 pos = new Vector2(projectile.Center.X, projectile.Center.Y - 5);
			if (!spawnedMushroom)
			{
				int dmg = getSecondaryDamage(player, modPlayer, alchProj.nbElements);
				int nb = (alchProj.nbElements * 3) + Main.rand.Next(alchProj.nbElements * 2);
				for (int i = 0; i < nb; i++)
				{
					float speed = (5f / (nb + 1)) * (i + 1);
					Vector2 vel = (new Vector2(0f, speed).RotatedByRandom(MathHelper.ToRadians(180)));
					Projectile newProj = Main.projectile[Projectile.NewProjectile(player.GetSource_Misc("Alchemist Attack"), pos, vel, projType, dmg, 0.1f, projectile.owner)];
					newProj.timeLeft = 70 + (((20 - nb) > 10 ? (20 - nb) : 10) * i);
					newProj.netUpdate = true;
				}
				Projectile.NewProjectile(player.GetSource_Misc("Alchemist Attack"), pos, Vector2.Zero, projType2, 0, 0f, projectile.owner);
			}
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.Vertebrae, 5);
			recipe.AddIngredient(ItemID.ViciousMushroom, 5);
			recipe.Register();
		}
	}
}
