using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class SunflowerFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 7;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 3, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 3;
			this.colorR = 245;
			this.colorG = 197;
			this.colorB = 1;
			this.secondaryDamage = 8;
			this.secondaryScaling = 2f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forest Samples");
			Tooltip.SetDefault("Using a water element increases damage and spawns a damaging sunflower"
							+ "\n'Handcrafted jars are unfit for precise alchemy'");
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(ItemID.Bottle, 1);
			recipe.AddIngredient(ItemID.Sunflower, 1);
			recipe.AddIngredient(ItemID.Mushroom, 3);
			recipe.Register();
		}

		public override void KillSecond(int timeLeft, Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int nb = 2 + Main.rand.Next(2);
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj4>();
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, spawnProj, 0, 0f, projectile.owner);
			}
			if (alchProj.waterFlask.type != 0)
			{
				int dmg = GetSecondaryDamage(player, alchProj.nbElements);
				Vector2 vel = (new Vector2(0f, -2f).RotatedByRandom(MathHelper.ToRadians(20)));
				int newType = ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj1>();
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, newType, dmg, 0f, projectile.owner);
			}
		}

		public override void AddVariousEffects(Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			if (alchProj.waterFlask.type != 0) projectile.damage += (int)player.GetDamage<AlchemistDamageClass>().ApplyTo(3);
		}
	}
}
