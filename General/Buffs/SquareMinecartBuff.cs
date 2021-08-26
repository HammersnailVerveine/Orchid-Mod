using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.General.Buffs
{
	public class SquareMinecartBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Minecart"); // Square Minecart (all vanilla minecarts have this name...)
			Description.SetDefault("Riding in a minecart");

			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(ModContent.MountType<Mounts.SquareMinecartMount>(), player);
			player.buffTime[buffIndex] = 10;
		}
	}
}
