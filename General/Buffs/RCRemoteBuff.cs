using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.General.Buffs
{
	public class RCRemoteBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Conspicuous Helicopter");
			Description.SetDefault("'That whirring is getting repetitive...'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<OrchidModPlayer>().remoteCopterPet = true;
			int projType = ModContent.ProjectileType<General.Projectiles.Pets.RCRemotePet>();
			bool petProjectileNotSpawned = player.ownedProjectileCounts[projType] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, projType, 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}