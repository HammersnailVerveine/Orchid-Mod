using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class HellOil : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 15;
			item.width = 30;
			item.height = 30;
			item.rare = 3;
			item.value = Item.sellPrice(0, 3, 20, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 184;
			this.colorR = 117;
			this.colorG = 48;
			this.colorB = 48;
			this.secondaryDamage = 60;
			this.secondaryScaling = 20f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellfire Oil");
			Tooltip.SetDefault("Using a fire element in the same attack will drastically increase damage"
							+ "\nThis will also damage and spread alchemical fire to all nearby water coated enemies"
							+ "\nHas a chance to release a catalytic oil bubble, coating nearby enemies in water on reaction");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(null, "GoblinArmyFlask", 1);
			recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.AddIngredient(null, "AlchemicStabilizer", 1);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer,
		OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			if (alchProj.fireFlask.type != 0)
			{
				int dmg = getSecondaryDamage(player, modPlayer, alchProj.nbElements);
				modTarget.spreadOilFire(target.Center, dmg, Main.player[projectile.owner]);
				Main.PlaySound(2, (int)target.position.X, (int)target.position.Y, 45);
			}

			int rand = alchProj.nbElements;
			rand += alchProj.hasCloud() ? 2 : 0;
			if (Main.rand.Next(6) < rand && !alchProj.noCatalyticSpawn)
			{
				int dmg = 0;
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.OilBubble>();
				Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0f, projectile.owner);
			}
		}

		public override void AddVariousEffects(Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			if (alchProj.fireFlask.type != 0)
			{
				projectile.damage += (int)(50 * modPlayer.alchemistDamage);
			}
		}
	}
}
