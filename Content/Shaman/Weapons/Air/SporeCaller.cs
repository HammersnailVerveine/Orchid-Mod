using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shaman.Projectiles.Air;

namespace OrchidMod.Content.Shaman.Weapons.Air
{
	public class SporeCaller : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 14;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.knockBack = 3.15f;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 0, 54, 0);
			Item.UseSound = SoundID.Item65;
			Item.shootSpeed = 10.5f;
			Item.shoot = ModContent.ProjectileType<SporeCallerProjectile>();
			Element = ShamanElement.AIR;
			catalystType = ShamanCatalystType.ROTATE;
			CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman shaman = player.GetModPlayer<OrchidShaman>();
			NewShamanProjectile(player, source, position, velocity.RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(0.5f, 1f), type, damage, knockback);
			if (Main.rand.NextBool(2)) NewShamanProjectile(player, source, position, velocity.RotatedByRandom(MathHelper.ToRadians(180)) * Main.rand.NextFloat(0.25f, 0.5f), type, damage, knockback);
			return false;
		}

		public override void CatalystSummonAI(Projectile projectile, int timeSpent)
		{
			if (timeSpent % (Item.useTime * 3) == 0)
			{
				Vector2 target = OrchidModProjectile.GetNearestTargetPosition(projectile);
				if (target != Vector2.Zero)
				{
					Vector2 velocity = Vector2.Normalize(target - projectile.Center).RotatedByRandom(MathHelper.ToRadians(30)) * Item.shootSpeed;
					NewShamanProjectileFromProjectile(projectile, velocity, Item.shoot, projectile.damage, projectile.knockBack, ai1: 1f);
				}
			}
		}

		public override void OnReleaseShamanicBond(Player player, OrchidShaman shamanPlayer)
		{
			foreach (Projectile projectile in Main.projectile)
			{
				if (projectile.type == Item.shoot && projectile.owner == player.whoAmI && projectile.active)
				{
					projectile.ai[1] = 1f;
				}
			}
		}
		/*
		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.JungleSpores, 8)
			.AddIngredient(ItemID.Stinger, 5)
			.AddTile(TileID.Anvils)
			.Register();
		*/
	}
}
