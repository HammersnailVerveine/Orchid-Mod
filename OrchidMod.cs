using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist;
using OrchidMod.Alchemist.UI;
using OrchidMod.Common;
using OrchidMod.Common.Hooks;
using OrchidMod.Gambler.UI;
using OrchidMod.Shaman;
using OrchidMod.Shaman.UI;
using OrchidMod.Guardian;
using OrchidMod.Guardian.UI;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;

namespace OrchidMod
{
	public partial class OrchidMod : Mod
	{
		public static OrchidMod Instance { get; private set; }
		public static Mod ThoriumMod { get; private set; }

		internal CroupierUI croupierUI;

		public static List<AlchemistHiddenReactionRecipe> alchemistReactionRecipes;
		public static ModKeybind AlchemistReactionHotKey;
		public static ModKeybind AlchemistCatalystHotKey;

		public OrchidMod() => Instance = this;

		public override void Load()
		{
			// ThoriumMod = ModLoader.GetMod("ThoriumMod"); [SP] is this correct ?
			if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod))
				ThoriumMod = thoriumMod;

			// ...

			AlchemistReactionHotKey = KeybindLoader.RegisterKeybind(this, "Alchemist Hidden Reaction", "Mouse3");
			AlchemistCatalystHotKey = KeybindLoader.RegisterKeybind(this, "Alchemist Catalyst Tool Shortcut", Keys.Z);

			alchemistReactionRecipes = AlchemistHiddenReactionHelper.ListReactions();
		}

		public override void Unload()
		{
			AlchemistReactionHotKey = null;
			AlchemistCatalystHotKey = null;
			alchemistReactionRecipes = null;

			// ...

			croupierUI = null;

			ThoriumMod = null;
			Instance = null;
		}

		public override void PostSetupContent()
		{
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
					OrchidModPlayer modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();

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

					//Gambler Card in Deck
					bool cardInDeck = reader.ReadBoolean();
					modPlayer.gamblerHasCardInDeck = cardInDeck;
					break;
				case OrchidModMessageType.SHAMANORBTYPECHANGEDSMALL:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
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
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
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
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
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
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
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
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
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
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
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
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
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
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
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
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
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
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
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
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
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
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
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
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
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
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
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
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
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
					modPlayer = Main.player[playernumber].GetModPlayer<OrchidModPlayer>();
					cardInDeck = reader.ReadBoolean();
					modPlayer.gamblerHasCardInDeck = cardInDeck;
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)OrchidModMessageType.GAMBLERCARDINDECKCHANGED);
						packet.Write(playernumber);
						packet.Write(modPlayer.gamblerHasCardInDeck);
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
