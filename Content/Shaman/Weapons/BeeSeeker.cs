using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons
{
	public class BeeSeeker : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 15;
			Item.width = 36;
			Item.height = 36;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.knockBack = 2.75f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 54, 0);
			Item.UseSound = SoundID.Item21;
			Item.autoReuse = true;
			Item.shootSpeed = 6f;
			// //Item.shoot = ModContent.ProjectileType<HoneyProj>();
			this.Element = 2;
			this.energy = 5;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("The Hive");
			/* Tooltip.SetDefault("Shoots sticky honey balls, containing harmful bees"
							  + "\nThe more active shamanic bonds, the more bees are released"
							  + "\nWeapon damage increased by 25% if you are covered in honey"
							  + "\nHitting will fill an honey orb"
							  + "\nFilling the orb will make it explode into a swarm of bees"); */
		}

		public override void SafeModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			if (Main.LocalPlayer.FindBuffIndex(48) > -1) damage += 0.25f;
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
			this.NewShamanProjectile(player, source, position, newVelocity, type, damage, knockback);
			return false;
		}
	}
}
