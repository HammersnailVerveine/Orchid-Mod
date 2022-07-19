using OrchidMod.Alchemist;
using OrchidMod.Shaman;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OrchidMod.Utilities;
using OrchidMod.Common;
using System;
using Terraria.ModLoader.Core;
using System.Reflection;
using OrchidMod.Common.Attributes;
using ReLogic.Utilities;

namespace OrchidMod
{
	public partial class OrchidMod : Mod
	{
		public static OrchidMod Instance { get; private set; }
		public static Mod ThoriumMod { get; private set; }

		public static List<AlchemistHiddenReactionRecipe> alchemistReactionRecipes;

		public OrchidMod()
		{
			Instance = this;
			ContentAutoloadingEnabled = false;
		}

		public void LoadContent()
		{
			LoaderUtils.ForEachAndAggregateExceptions((
				from t in AssemblyManager.GetLoadableTypes(Code)
				where !t.IsAbstract && !t.ContainsGenericParameters
				where t.IsAssignableTo(typeof(ILoadable))
				where t.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) != null
				where AutoloadAttribute.GetValue(t).NeedsAutoloading
				select t).OrderBy((Type type) => type.FullName, StringComparer.InvariantCulture), delegate (Type t)
				{
					var instance = (ILoadable)Activator.CreateInstance(t, true);

					if (ModContent.GetInstance<OrchidConfig>().LoadCrossmodContentWithoutRequiredMods)
					{
						AddContent(instance);
						return;
					}

					var atr = t.GetAttribute<CrossmodContentAttribute>();

					if (atr is null)
					{
						AddContent(instance);
						return;
					}

					var hasAllMods = true;

					foreach (var mod in atr.Mods)
					{
						hasAllMods &= ModLoader.HasMod(mod);
					}

					if (hasAllMods)
					{
						AddContent(instance);
						return;
					}
				}
			);
		}

		public override void Load()
		{
			LoadContent();

			ThoriumMod = OrchidUtils.GetModWithPossibleNull("ThoriumMod");

			alchemistReactionRecipes = AlchemistHiddenReactionHelper.ListReactions();
		}

		public override void Unload()
		{
			alchemistReactionRecipes = null;

			ThoriumMod = null;
			Instance = null;
		}

		public override void PostSetupContent()
		{
			foreach (var type in Code.GetTypes())
			{
				if (!type.GetInterfaces().Contains(typeof(IPostSetupContent))) continue;

				var instance = (IPostSetupContent)Activator.CreateInstance(type, null);
				instance.PostSetupContent(this);
			}

			BossChecklistCalls();
			CensusModCalls();
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			OrchidModMessageType msgType = (OrchidModMessageType)reader.ReadByte();
			switch (msgType)
			{
				case OrchidModMessageType.ORCHIDPLAYERSYNCPLAYER:
					byte playernumber = reader.ReadByte();
					OrchidShaman modPlayer = Main.player[playernumber].GetModPlayer<OrchidShaman>();

					// Orbs
					byte readSmallOrb = reader.ReadByte();
					modPlayer.shamanOrbSmall = (ShamanOrbSmall)readSmallOrb;

					byte readBigOrb = reader.ReadByte();
					modPlayer.shamanOrbBig = (ShamanOrbBig)readBigOrb;

					byte readLargeOrb = reader.ReadByte();
					modPlayer.shamanOrbLarge = (ShamanOrbLarge)readLargeOrb;

					byte readUniqueOrb = reader.ReadByte();
					modPlayer.shamanOrbUnique = (ShamanOrbUnique)readUniqueOrb;

					byte readCircleOrb = reader.ReadByte();
					modPlayer.shamanOrbCircle = (ShamanOrbCircle)readCircleOrb;

					//Counts
					int countSmall = reader.ReadInt32();
					modPlayer.orbCountSmall = countSmall;

					int countBig = reader.ReadInt32();
					modPlayer.orbCountBig = countBig;

					int countLarge = reader.ReadInt32();
					modPlayer.orbCountLarge = countLarge;

					int countUnique = reader.ReadInt32();
					modPlayer.orbCountUnique = countUnique;

					int countCircle = reader.ReadInt32();
					modPlayer.orbCountCircle = countCircle;

					// Buff Timers
					int attackTimer = reader.ReadInt32();
					modPlayer.shamanFireTimer = attackTimer;

					int armorTimer = reader.ReadInt32();
					modPlayer.shamanWaterTimer = armorTimer;

					int criticalTimer = reader.ReadInt32();
					modPlayer.shamanAirTimer = criticalTimer;

					int regenerationTimer = reader.ReadInt32();
					modPlayer.shamanEarthTimer = regenerationTimer;

					int speedTimer = reader.ReadInt32();
					modPlayer.shamanSpiritTimer = speedTimer;

					break;
				case OrchidModMessageType.ORCHIDPLAYERSYNCPLAYERGAMBLER:
					playernumber = reader.ReadByte();
					OrchidGambler modPlayerGambler = Main.player[playernumber].GetModPlayer<OrchidGambler>();
					bool cardInDeck = reader.ReadBoolean();
					modPlayerGambler.gamblerHasCardInDeck = cardInDeck;
					break;
				case OrchidModMessageType.SHAMANORBTYPECHANGEDSMALL:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidShaman>();
					readSmallOrb = reader.ReadByte();
					modPlayer.shamanOrbSmall = (ShamanOrbSmall)readSmallOrb;

					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDSMALL);
						packet.Write(playernumber);
						packet.Write((byte)modPlayer.shamanOrbSmall);
						packet.Send(-1, playernumber);
					}
					break;

				case OrchidModMessageType.SHAMANORBTYPECHANGEDBIG:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidShaman>();
					readBigOrb = reader.ReadByte();
					modPlayer.shamanOrbBig = (ShamanOrbBig)readBigOrb;

					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDBIG);
						packet.Write(playernumber);
						packet.Write((byte)modPlayer.shamanOrbBig);
						packet.Send(-1, playernumber);
					}
					break;

				case OrchidModMessageType.SHAMANORBTYPECHANGEDLARGE:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidShaman>();
					readLargeOrb = reader.ReadByte();
					modPlayer.shamanOrbLarge = (ShamanOrbLarge)readLargeOrb;

					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDLARGE);
						packet.Write(playernumber);
						packet.Write((byte)modPlayer.shamanOrbLarge);
						packet.Send(-1, playernumber);
					}
					break;

				case OrchidModMessageType.SHAMANORBTYPECHANGEDUNIQUE:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidShaman>();
					readUniqueOrb = reader.ReadByte();
					modPlayer.shamanOrbUnique = (ShamanOrbUnique)readUniqueOrb;

					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDUNIQUE);
						packet.Write(playernumber);
						packet.Write((byte)modPlayer.shamanOrbUnique);
						packet.Send(-1, playernumber);
					}
					break;

				case OrchidModMessageType.SHAMANORBTYPECHANGEDCIRCLE:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidShaman>();
					readCircleOrb = reader.ReadByte();
					modPlayer.shamanOrbCircle = (ShamanOrbCircle)readCircleOrb;

					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDCIRCLE);
						packet.Write(playernumber);
						packet.Write((byte)modPlayer.shamanOrbCircle);
						packet.Send(-1, playernumber);
					}
					break;

				case OrchidModMessageType.SHAMANORBCOUNTCHANGEDSMALL:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidShaman>();
					countSmall = reader.ReadInt32();
					modPlayer.orbCountSmall = countSmall;
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDSMALL);
						packet.Write(playernumber);
						packet.Write(modPlayer.orbCountSmall);
						packet.Send(-1, playernumber);
					}
					break;

				case OrchidModMessageType.SHAMANORBCOUNTCHANGEDBIG:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidShaman>();
					countBig = reader.ReadInt32();
					modPlayer.orbCountBig = countBig;
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDBIG);
						packet.Write(playernumber);
						packet.Write(modPlayer.orbCountBig);
						packet.Send(-1, playernumber);
					}
					break;

				case OrchidModMessageType.SHAMANORBCOUNTCHANGEDLARGE:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidShaman>();
					countLarge = reader.ReadInt32();
					modPlayer.orbCountLarge = countLarge;
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDLARGE);
						packet.Write(playernumber);
						packet.Write(modPlayer.orbCountLarge);
						packet.Send(-1, playernumber);
					}
					break;

				case OrchidModMessageType.SHAMANORBCOUNTCHANGEDUNIQUE:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidShaman>();
					countUnique = reader.ReadInt32();
					modPlayer.orbCountUnique = countUnique;
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDUNIQUE);
						packet.Write(playernumber);
						packet.Write(modPlayer.orbCountUnique);
						packet.Send(-1, playernumber);
					}
					break;

				case OrchidModMessageType.SHAMANORBCOUNTCHANGEDCIRCLE:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidShaman>();
					countCircle = reader.ReadInt32();
					modPlayer.orbCountCircle = countCircle;
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDCIRCLE);
						packet.Write(playernumber);
						packet.Write(modPlayer.orbCountCircle);
						packet.Send(-1, playernumber);
					}
					break;

				case OrchidModMessageType.SHAMANBUFFTIMERCHANGEDATTACK:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidShaman>();
					attackTimer = reader.ReadInt32();
					modPlayer.shamanFireTimer = attackTimer;
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDATTACK);
						packet.Write(playernumber);
						packet.Write(modPlayer.shamanFireTimer);
						packet.Send(-1, playernumber);
					}
					break;

				case OrchidModMessageType.SHAMANBUFFTIMERCHANGEDARMOR:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidShaman>();
					armorTimer = reader.ReadInt32();
					modPlayer.shamanWaterTimer = armorTimer;
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDARMOR);
						packet.Write(playernumber);
						packet.Write(modPlayer.shamanWaterTimer);
						packet.Send(-1, playernumber);
					}
					break;

				case OrchidModMessageType.SHAMANBUFFTIMERCHANGEDCRITICAL:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidShaman>();
					criticalTimer = reader.ReadInt32();
					modPlayer.shamanAirTimer = criticalTimer;
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDCRITICAL);
						packet.Write(playernumber);
						packet.Write(modPlayer.shamanAirTimer);
						packet.Send(-1, playernumber);
					}
					break;

				case OrchidModMessageType.SHAMANBUFFTIMERCHANGEDREGENERATION:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidShaman>();
					regenerationTimer = reader.ReadInt32();
					modPlayer.shamanEarthTimer = regenerationTimer;
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDREGENERATION);
						packet.Write(playernumber);
						packet.Write(modPlayer.shamanEarthTimer);
						packet.Send(-1, playernumber);
					}
					break;

				case OrchidModMessageType.SHAMANBUFFTIMERCHANGEDSPEED:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidShaman>();
					speedTimer = reader.ReadInt32();
					modPlayer.shamanSpiritTimer = speedTimer;
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDSPEED);
						packet.Write(playernumber);
						packet.Write(modPlayer.shamanSpiritTimer);
						packet.Send(-1, playernumber);
					}
					break;

				case OrchidModMessageType.GAMBLERCARDINDECKCHANGED:
					playernumber = reader.ReadByte();
					modPlayerGambler = Main.player[playernumber].GetModPlayer<OrchidGambler>();
					cardInDeck = reader.ReadBoolean();
					modPlayerGambler.gamblerHasCardInDeck = cardInDeck;
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.GAMBLERCARDINDECKCHANGED);
						packet.Write(playernumber);
						packet.Write(modPlayerGambler.gamblerHasCardInDeck);
						packet.Send(-1, playernumber);
					}
					break;

				default:
					Logger.WarnFormat("OrchidMod: Unknown Message type: {0}", msgType);
					break;
			}
		}
	}
}
