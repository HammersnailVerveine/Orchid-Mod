using OrchidMod.Content.Alchemist.Projectiles;
using OrchidMod.Common.Globals.NPCs;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Alchemist.Weapons.Fire
{
	public class StardustSamples : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 8;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 3, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.FIRE;
			this.rightClickDust = DustID.YellowStarDust;
			this.colorR = 251;
			this.colorG = 189;
			this.colorB = 56;
			this.secondaryDamage = 8;
			this.secondaryScaling = 1f;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stardust Samples");
			/* Tooltip.SetDefault("Mixing with a nature element confuses hit enemies"
							+ "\n'Handcrafted jars are unfit for precise alchemy'"); */
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(ItemID.Bottle, 1);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.Register();
		}

		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidAlchemist modPlayer, OrchidModAlchemistNPC modTarget, OrchidGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			if (alchProj.natureFlask.type != ItemID.None)
			{
				target.AddBuff(BuffID.Confused, (60 * (alchProj.nbElements + 3)));
			}
		}
	}
}
