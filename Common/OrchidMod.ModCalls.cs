using OrchidMod.Common;
using OrchidMod.Content.General.NPCs.Town;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Weapons.Warhammers;
using OrchidMod.Content.Shapeshifter;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod
{
	public partial class OrchidMod
	{
		private static Action<Player> GuardianFocus = new (player =>
		{
			player.statDefense += 3;
			player.lifeRegen += 6;
			OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
			guardian.GuardianGuardMax += 3;
			guardian.GuardianSlamMax += 3;
		});

		private void ThoriumModCalls()
		{
			if (ThoriumMod == null || ThoriumMod.Version < new Version(1, 7, 2, 0)) return;
			ThoriumMod.Call("TerrariumArmorAddClassFocus", ModContent.GetInstance<GuardianDamageClass>(), GuardianFocus, OrchidColors.GuardianTag);
			ThoriumMod.Call("AddMartianItemID", ModContent.ItemType<MartianWarhammer>());
		}

		private void CensusModCalls()
		{
			if (!ModLoader.TryGetMod("Census", out Mod censusMod)) return;

			censusMod.Call
			(
				"TownNPCCondition",
				ModContent.NPCType<Croupier>(),
				ModContent.GetInstance<Croupier>().GetLocalization("Census.SpawnCondition")
			);

			censusMod.Call
			(
				"TownNPCCondition",
				ModContent.NPCType<Chemist>(),
				ModContent.GetInstance<Chemist>().GetLocalization("Census.SpawnCondition")
			);
		}

		private static void ColoredDamageTypeModCalls()
		{
			if (!Main.dedServ && ModLoader.TryGetMod("ColoredDamageTypes", out Mod coloreddamagetypes))
			{
				// Colors in order : Tooltip, Damage, Crit
				coloreddamagetypes.Call("AddDamageType", ModContent.GetInstance<GuardianDamageClass>(), (165, 130, 100), (198, 172, 146), (155, 109, 85));
				coloreddamagetypes.Call("AddDamageType", ModContent.GetInstance<ShapeshifterDamageClass>(), (100, 175, 150), (120, 195, 170), (43, 132, 101));
			}
		}
	}
}
