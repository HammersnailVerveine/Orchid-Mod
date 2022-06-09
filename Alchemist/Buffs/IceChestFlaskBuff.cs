using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Buffs
{
	public class IceChestFlaskBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Permanent Freeze");
			Description.SetDefault("Constantly freezes various alchemical projectiles and slows enemies around you");
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			OrchidModProjectile.spawnDustCircle(player.Center, 261, 1, 1, true, 1.5f, 1f, 16f, true, true, false, 0, 0, true);
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (modPlayer.alchemistLastAttackDelay > 60)
			{
				int range = 100;
				int projType = ProjectileType<Alchemist.Projectiles.Water.IceChestFlaskProj>();
				int itemType = ItemType<Alchemist.Weapons.Water.IceChestFlask>();
				int damage = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, 2, true);
				int newProjectileInt = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, projType, damage, 0f, player.whoAmI);
				Projectile newProjectile = Main.projectile[newProjectileInt];
				newProjectile.width = range * 2;
				newProjectile.height = range * 2;
				newProjectile.position.X = player.Center.X - (newProjectile.width / 2);
				newProjectile.position.Y = player.Center.Y - (newProjectile.width / 2);
				newProjectile.netUpdate = true;
			}
		}
	}
}