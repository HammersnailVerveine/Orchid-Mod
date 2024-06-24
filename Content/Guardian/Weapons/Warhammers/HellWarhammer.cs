using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Warhammers
{
	public class HellWarhammer : OrchidModGuardianHammer
	{
		public override void SafeSetDefaults()
		{
			Item.width = 38;
			Item.height = 38;
			Item.value = Item.sellPrice(0, 0, 54, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.DD2_MonkStaffSwing;
			Item.knockBack = 10f;
			Item.shootSpeed = 10f;
			Item.damage = 72;
			range = 33;
			blockStacks = 2;
		}

		public override bool ThrowAI(Player player, OrchidGuardian guardian, Projectile projectile, bool weak)
		{
			if (Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Torch, Scale: Main.rand.NextFloat(1.5f, 2f));
				dust.velocity = dust.velocity * 0.25f + projectile.velocity * 0.2f;
				dust.noGravity = true;
			}
			return true;
		}


		public override void OnThrowHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak)
		{
			if (!Weak) OrchidModProjectile.spawnGenericExplosion(projectile, projectile.damage, 10f, 250, 0, true, true);
			target.AddBuff(BuffID.OnFire, 180);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.HellstoneBar, 20);
			recipe.Register();
		}
	}
}
