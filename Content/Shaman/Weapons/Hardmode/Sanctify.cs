using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using OrchidMod.Content.Shaman.Projectiles;
using OrchidMod.Common.ModObjects;

namespace OrchidMod.Content.Shaman.Weapons.Hardmode
{
	public class Sanctify : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 45;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.knockBack = 1.15f;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 4, 55, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 7f;
			//Item.shoot = ModContent.ProjectileType<SanctifyProj>();
			Element = ShamanElement.SPIRIT;
			CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			/*
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			if (modPlayer.GetNbShamanicBonds() > 2)
			{
				int typeAlt = ModContent.ProjectileType<SanctifyProjAlt>();
				int newDamage = (int)(Item.damage * 0.75);
				for (int i = -1; i < 3; i+= 2)
				{
					Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(20 * i));
					this.NewShamanProjectile(player, source, position, newVelocity, typeAlt, newDamage, knockback);
				}
			}
			*/
			return true;
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
					NewShamanProjectileFromProjectile(projectile, velocity, Item.shoot, projectile.damage, projectile.knockBack);
				}
			}
		}

		/*
		public override void OnReleaseShamanicBond(Player player, OrchidShaman shamanPlayer)
		{
			int dmg = (int)player.GetDamage<ShamanDamageClass>().ApplyTo(30);
			int type = ModContent.ProjectileType<SanctifyOrbHoming>();
			for (int i = 0; i < 7; i++)
			{
				Vector2 refVelocity = new Vector2(0, 48f).RotatedBy(MathHelper.ToRadians(-30 + i * 10));
				Vector2 velocity = Vector2.Normalize(refVelocity) * -3f;
				Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center - refVelocity, velocity, type, dmg, 0f, player.whoAmI, 0f, 0f);
			}
		}
		*/

		/*
		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.HallowedBar, 12)
			.AddTile(TileID.MythrilAnvil)
			.Register();
		*/
	}
}
