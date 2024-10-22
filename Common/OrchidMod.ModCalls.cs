using OrchidMod.Content.General.NPCs.Town;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Shapeshifter;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod
{
	public partial class OrchidMod
	{
		private void BossChecklistCalls()
		{
			if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist)) return;

			// TODO AddToBossLoot calls use new structure ("Terraria Pumpkin Moon" as a single string) and should be unnecessary once NPCLoot is properly implemented (need to doublecheck events though)
			// Bosses -- Vanilla

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"QueenBee",
				new List<int>
				{
						ModContent.ItemType<Content.Gambler.Weapons.Cards.QueenBeeCard>(),
						ModContent.ItemType<Content.Gambler.Weapons.Dice.HoneyDie>(),
						ModContent.ItemType<Content.Shaman.Weapons.BeeSeeker>(),
						ModContent.ItemType<Content.Shaman.Accessories.WaxyVial>(),
						ModContent.ItemType<Content.Alchemist.Weapons.Air.QueenBeeFlask>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"MoonLord",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Hardmode.Nirvana>(),
					ModContent.ItemType<Content.Shaman.Weapons.Hardmode.TheCore>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"WallofFlesh",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Accessories.ShamanEmblem>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"Plantera",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Hardmode.BulbScepter>(),
					ModContent.ItemType<Content.Shaman.Accessories.FloralStinger>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"Golem",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Hardmode.SunRay>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"KingSlime",
				new List<int>
				{
					ModContent.ItemType<Content.Alchemist.Weapons.Water.KingSlimeFlask>(),
					ModContent.ItemType<Content.Gambler.Weapons.Cards.KingSlimeCard>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"EaterofWorldsHead",
				new List<int>
				{
					ModContent.ItemType<Content.Alchemist.Accessories.PreservedCorruption>(),
					ModContent.ItemType<Content.Gambler.Weapons.Cards.EaterCard>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"BrainofCthulhu",
				new List<int>
				{
					ModContent.ItemType<Content.Alchemist.Accessories.PreservedCrimson>(),
					ModContent.ItemType<Content.Gambler.Weapons.Cards.BrainCard>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"EyeofCthulhu",
				new List<int>
				{
					ModContent.ItemType<Content.Gambler.Weapons.Cards.EyeCard>()
				});

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"SkeletronHead",
				new List<int>
				{
					ModContent.ItemType<Content.Gambler.Weapons.Cards.SkeletronCard>()
				}
			);

			// Minibosses and Events -- Vanilla

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"Goblin Army",
				new List<int>
				{
					ModContent.ItemType<Content.Alchemist.Weapons.Water.GoblinArmyFlask>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"Blood Moon",
				new List<int>
				{
					ModContent.ItemType<Content.Alchemist.Weapons.Water.BloodMoonFlask>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"Pirate Invasion",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Hardmode.PiratesGlory>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"PirateShip",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Hardmode.PiratesGlory>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"Frost Moon",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Accessories.FragilePresent>(),
					ModContent.ItemType<Content.Shaman.Weapons.Hardmode.IceFlakeCone>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"SantaNK1",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Accessories.FragilePresent>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"IceQueen",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Hardmode.IceFlakeCone>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"Pumpkin Moon",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Accessories.MourningTorch>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"MourningWood",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Accessories.MourningTorch>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"Martian Madness",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Hardmode.MartianBeamer>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"MartianSaucer",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Hardmode.MartianBeamer>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"Terraria",
				"CultistBoss",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Misc.AbyssFragment>()
				}
			);

			// Bosses -- Thorium

			bossChecklist.Call
			(
				"AddToBossLoot",
				"ThoriumMod",
				"The Grand Thunder Bird",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Thorium.ThunderScepter>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"ThoriumMod",
				"The Queen Jellyfish",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Thorium.QueenJellyfishScepter>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"ThoriumMod",
				"Granite Energy Storm",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Thorium.GraniteEnergyScepter>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"ThoriumMod",
				"Viscount",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Thorium.ViscountScepter>(),
					ModContent.ItemType<Content.Shaman.Misc.Thorium.ViscountMaterial>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"ThoriumMod",
				"Star Scouter",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Thorium.StarScouterScepter>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"ThoriumMod",
				"Coznix, the Fallen Beholder",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Thorium.Hardmode.CoznixScepter>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"ThoriumMod",
				"Borean Strider",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Thorium.Hardmode.BoreanStriderScepter>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"ThoriumMod",
				"The Lich",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Thorium.Hardmode.LichScepter>()
				}
			);

			bossChecklist.Call
			(
				"AddToBossLoot",
				"ThoriumMod",
				"Abyssion, the Forgotten One",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Thorium.Hardmode.AbyssionScepter>()
				}
			);

			// Minibosses and Events -- Thorium

			bossChecklist.Call
			(
				"AddToBossLoot",
				"ThoriumMod",
				"Patch Werk",
				new List<int>
				{
					ModContent.ItemType<Content.Shaman.Weapons.Thorium.PatchWerkScepter>()
				}
			);
		}

		private void CensusModCalls()
		{
			if (!ModLoader.TryGetMod("Census", out Mod censusMod)) return;

			censusMod.Call
				(
					"TownNPCCondition",
					ModContent.NPCType<Croupier>(),
					$"Have a gamber card ([i:{ModContent.ItemType<Content.Gambler.Weapons.Cards.SlimeCard>()}][i:{ModContent.ItemType<Content.Gambler.Weapons.Cards.EmbersCard>()}] etc.) in your deck"
				);

			censusMod.Call
			(
				"TownNPCCondition",
				ModContent.NPCType<Chemist>(),
				"Find in the main mineshaft, in the center of your world"
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