using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shaman.Projectiles.Water;

namespace OrchidMod.Content.Shaman.Weapons.Water
{
	public class AvalancheScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 10;
			Item.width = 36;
			Item.height = 36;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.knockBack = 0.5f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.UseSound = SoundID.Item28;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			Item.shoot = ModContent.ProjectileType<AvalancheProjectile>();
			Element = ShamanElement.WATER;
			CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}

		public override void CatalystSummonRelease(Player player, Projectile projectile)
		{
			Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center) * Item.shootSpeed * 1.5f;
			int type = ModContent.ProjectileType<AvalancheProjectileAlt>();
			NewShamanProjectile(player, (EntitySource_ItemUse)player.GetSource_ItemUse(Item), projectile.Center, velocity, type, Item.damage * 10, Item.knockBack);
			SoundEngine.PlaySound(SoundID.Item28, projectile.Center);
		}

		public override void CatalystSummonAI(Projectile projectile, int timeSpent)
		{
			if (timeSpent % (Item.useTime * 2) == 0)
			{
				Vector2 target = OrchidModProjectile.GetNearestTargetPosition(projectile);
				if (target != Vector2.Zero)
				{
					Player player = Main.player[projectile.owner];
					Vector2 velocity = Vector2.Normalize(target - projectile.Center) * Item.shootSpeed;
					Vector2 position = projectile.Center + Vector2.UnitY.RotateRandom(MathHelper.Pi) * Main.rand.NextFloat(32f);
					NewShamanProjectile(player, (EntitySource_ItemUse)player.GetSource_ItemUse(Item), position, velocity, Item.shoot, projectile.damage, projectile.knockBack);
				}
			}
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 newPosition = position + Vector2.UnitY.RotateRandom(MathHelper.Pi) * Main.rand.NextFloat(32f);
			NewShamanProjectile(player, source, newPosition, velocity, type, damage, knockback);
			return false;
		}
	}
}