using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Projectiles.Warhammers;
using OrchidMod.Content.Guardian.Misc;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Warhammers
{
	public class MagnetosphereWarhammer : OrchidModGuardianHammer
	{
		public override void SafeSetDefaults()
		{
			Item.width = 52;
			Item.height = 52;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.DD2_MonkStaffSwing;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.knockBack = 10f;
			Item.shootSpeed = 14f;
			Item.damage = 267;
			range = 50;
			blockStacks = 3;
			slamStacks = 2;
		}

        public override bool ThrowAI(Player player, OrchidGuardian guardian, Projectile projectile, bool Weak)
        {
            if (projectile.timeLeft % 8 == 0)
			{
				int type = ModContent.ProjectileType<MagnetosphereWarhammerProj>();
				Projectile.NewProjectile(Item.GetSource_FromThis(), projectile.Center + Vector2.UnitY.RotatedBy(MathHelper.TwoPi) * Main.rand.NextFloat(0f, 5f), Vector2.Zero, type, (int)(projectile.damage * 0.5f), 0f, player.whoAmI, 1f);
			}
			return true;
		}

        public override void OnThrowHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak)
		{
			int type = ModContent.ProjectileType<MagnetosphereWarhammerProj>();
			Projectile.NewProjectile(Item.GetSource_FromThis(), projectile.Center, Vector2.Zero, type, (int)(projectile.damage), Item.knockBack, player.whoAmI);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<GuardianFragmentMaterial>(18);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
