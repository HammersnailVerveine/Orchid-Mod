using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Warhammers
{
	public class ChlorophyteWarhammer : OrchidModGuardianHammer
	{
		public override void SafeSetDefaults()
		{
			Item.width = 44;
			Item.height = 44;
			Item.value = Item.sellPrice(0, 5, 52, 0);
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.DD2_MonkStaffSwing;
			Item.knockBack = 10f;
			Item.shootSpeed = 13f;
			Item.damage = 228;
			Item.useTime = 25;
			range = 45;
			blockStacks = 2;
			slamStacks = 1;
		}

		public override void OnThrowHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak)
		{
			if (!Weak && IsLocalPlayer(player))
			{
				for (int i = 0; i < 6; i++)
				{
					Vector2 dir = Vector2.UnitY.RotatedBy(MathHelper.TwoPi / 6f * i).RotatedByRandom(MathHelper.ToRadians(15f)) * (2f + Main.rand.NextFloat(6f));
					Projectile.NewProjectile(Item.GetSource_FromThis(), projectile.Center, dir, ProjectileID.SporeCloud, projectile.damage, Item.knockBack, player.whoAmI);
				}
			}
		}

		public override void OnMeleeHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit)
		{
			if (IsLocalPlayer(player))
			{
				for (int i = 0; i < 3; i++)
				{
					Vector2 dir = Vector2.UnitY.RotatedBy(MathHelper.TwoPi / 3f * i).RotatedByRandom(MathHelper.ToRadians(15f)) * (1f + Main.rand.NextFloat(4f));
					Projectile.NewProjectile(Item.GetSource_FromThis(), projectile.Center, dir, ProjectileID.SporeCloud, (int)(projectile.damage * 0.75f), Item.knockBack, player.whoAmI);
				}
			}
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
			recipe.Register();
		}
	}
}
