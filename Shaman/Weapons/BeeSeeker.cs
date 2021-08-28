using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Weapons
{
	public class BeeSeeker : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 15;
			item.width = 36;
			item.height = 36;
			item.useTime = 20;
			item.useAnimation = 20;
			item.knockBack = 2.75f;
			item.rare = 2;
			item.value = Item.sellPrice(0, 0, 54, 0);
			item.UseSound = SoundID.Item21;
			item.autoReuse = true;
			item.shootSpeed = 6f;
			item.shoot = mod.ProjectileType("HoneyProj");
			this.empowermentType = 2;
			this.energy = 10;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("The Hive");
			Tooltip.SetDefault("Shoots sticky honey balls, containing harmful bees"
							  + "\nThe more active shamanic bonds, the more bees are released"
							  + "\nWeapon damage increased by 25% if you are covered in honey"
							  + "\nHitting will fill an honey orb"
							  + "\nFilling the orb will make it explode into a swarm of bees");
		}

		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			mult *= player.GetModPlayer<OrchidModPlayer>().shamanDamage;
			if (Main.LocalPlayer.FindBuffIndex(48) > -1) add += 0.25f;
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
			this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			return false;
		}
	}
}
