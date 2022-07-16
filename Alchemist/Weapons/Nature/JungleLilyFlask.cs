using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using OrchidMod.Common.Globals.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class JungleLilyFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 8;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 3;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = DustType<Content.Dusts.BloomingDust>();
			this.colorR = 177;
			this.colorG = 46;
			this.colorB = 77;
			this.secondaryDamage = 10;
			this.secondaryScaling = 5f;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Lily Extract");
			Tooltip.SetDefault("Enemies and spores within impact radius will bloom"
							+ "\nBloomed spores will duplicate a maximum of once"
							+ "\nBloomed enemies will spawn spores for each of their coatings"
							+ "\nDirect hits will apply a nature coating");
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(ModContent.ItemType<Alchemist.Misc.EmptyFlask>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Content.Items.Materials.JungleLily>(), 2);
			recipe.AddIngredient(ItemID.Stinger, 5);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(ModContent.ItemType<Alchemist.Misc.EmptyFlask>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Content.Items.Materials.JungleLilyBloomed>(), 1);
			recipe.AddIngredient(ItemID.Stinger, 5);
			recipe.Register();
		}

		public override void KillFirst(int timeLeft, Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int range = 100 * alchProj.nbElements;
			int nb = 20 * alchProj.nbElements;
			OrchidModProjectile.spawnDustCircle(projectile.Center, this.rightClickDust, (int)(range * 0.75), nb, true, 1.5f, 1f, 8f);
			OrchidModProjectile.spawnDustCircle(projectile.Center, this.rightClickDust, (int)(range * 0.5), (int)(nb / 3), true, 1.5f, 1f, 16f, true, true, false, 0, 0, true);

			int projType = ProjectileType<Alchemist.Projectiles.Nature.JungleLilyFlaskProj>();
			int damage = GetSecondaryDamage(player, alchProj.nbElements);
			int newProjectileInt = SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, Vector2.Zero, projType, damage, 0f, projectile.owner);
			Projectile newProjectile = Main.projectile[newProjectileInt];
			newProjectile.width = range * 2;
			newProjectile.height = range * 2;
			newProjectile.position.X = projectile.Center.X - (newProjectile.width / 2);
			newProjectile.position.Y = projectile.Center.Y - (newProjectile.width / 2);
			newProjectile.netUpdate = true;
		}

		public override void OnHitNPCFirst(NPC target, int damage, float knockback, bool crit, Player player, OrchidAlchemist modPlayer,
		OrchidModAlchemistNPC modTarget, OrchidGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			modTarget.alchemistNature = 60 * 10;
		}
	}
}
