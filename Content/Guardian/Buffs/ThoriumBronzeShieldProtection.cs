using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using OrchidMod.Content.Guardian.Weapons.Shields;

namespace OrchidMod.Content.Guardian.Buffs
{
	public class ThoriumBronzeShieldProtection : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<OrchidGuardian>().GuardianBronzeShieldProtection = true;
			if (player.whoAmI == Main.myPlayer)
			{
				if (player.buffTime[buffIndex] > 58) Main.buffNoTimeDisplay[Type] = true;
				else Main.buffNoTimeDisplay[Type] = false;
			}
		}
	}
}