using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shaman.Projectiles.Fire;

namespace OrchidMod.Content.Shaman.Weapons.Fire
{
	public class SpineScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 20;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.knockBack = 3.15f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 27, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 7f;
			Item.shoot = ModContent.ProjectileType<SpineScepterProjectile>();
			Element = ShamanElement.FIRE;
			CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman shaman = player.GetModPlayer<OrchidShaman>();
			int bounces = 1;
			if (shaman.CountShamanicBonds() > 0) bounces += 2;
			if (shaman.CountShamanicBonds() > 1) bounces += 2;
			NewShamanProjectile(player, source, position, velocity, type, damage, knockback, ai1 : bounces);
			return false;
		}

		public override void CatalystSummonAI(Projectile projectile, int timeSpent)
		{
			if (timeSpent % (Item.useTime * 3) == 0)
			{
				Vector2 target = OrchidModProjectile.GetNearestTargetPosition(projectile);
				if (target != Vector2.Zero)
				{
					Vector2 velocity = target - projectile.Center;
					velocity.Normalize();
					velocity *= Item.shootSpeed;
					OrchidShaman shaman = Main.player[projectile.owner].GetModPlayer<OrchidShaman>();
					int bounces = 1;
					if (shaman.CountShamanicBonds() > 0) bounces += 2;
					if (shaman.CountShamanicBonds() > 1) bounces += 2;
					NewShamanProjectileFromProjectile(projectile, velocity, Item.shoot, projectile.damage, projectile.knockBack, ai1: bounces);
				}
			}
		}
		/*
		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.CrimtaneBar, 10)
			.AddTile(TileID.Anvils)
			.Register();
		*/
	}
}