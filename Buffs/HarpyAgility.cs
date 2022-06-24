using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Buffs
{
	public class HarpyAgility : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Harpy Agility");
			Description.SetDefault("Ability to double jump, your fist bonus jump will release damaging feathers around you");
		}
		public override void Update(Player player, ref int buffIndex)
		{
			Main.player[Main.myPlayer].GetModPlayer<OrchidShaman>().doubleJumpHarpy = true;
		}
	}
}