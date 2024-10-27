using Microsoft.Xna.Framework;
using OrchidMod.Content.Shaman.Projectiles.Water;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Water
{
	public class BeeSeeker : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 15;
			Item.width = 36;
			Item.height = 36;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.knockBack = 2.75f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 54, 0);
			Item.UseSound = SoundID.Item21;
			Item.autoReuse = true;
			Item.shootSpeed = 12f;
			Item.shoot = ModContent.ProjectileType<BeeSeekerProjectile>();
			this.Element = ShamanElement.WATER;
			this.CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}

		public override void CatalystSummonRelease(Player player, Projectile projectile)
		{

			int dmg = (int)player.GetDamage<ShamanDamageClass>().ApplyTo(10);
			EntitySource_ItemUse source = (EntitySource_ItemUse)player.GetSource_ItemUse(Item);
			for (int i = 0; i < 15; i ++)
			{
				if (player.strongBees && Main.rand.NextBool(2))
					Projectile.NewProjectile(source, projectile.Center, Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * 5f, ProjectileID.GiantBee, (int)(dmg * 1.15f), 0f, player.whoAmI);
				else
					Projectile.NewProjectile(source, projectile.Center, Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * 5f, ProjectileID.Bee, dmg, 0f, player.whoAmI);
			}

			SoundEngine.PlaySound(SoundID.Item97, player.Center);
		}
	}
}