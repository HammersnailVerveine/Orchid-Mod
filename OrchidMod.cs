using OrchidMod.Content.Alchemist;
using OrchidMod.Content.Shaman;
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
using OrchidMod.Common.Global.NPCs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using OrchidMod.Content.Guardian;

namespace OrchidMod
{
	public partial class OrchidMod : Mod
	{
		public static OrchidMod Instance { get; private set; }
		public static Mod ThoriumMod { get; private set; }

		public static List<AlchemistHiddenReactionRecipe> AlchemistReactionRecipes;

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

					if (ModContent.GetInstance<OrchidServerConfig>().LoadCrossmodContentWithoutRequiredMods)
					{
						AddContent(instance);
						return;
					}

					var atr = t.GetCustomAttribute<CrossmodContentAttribute>();

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

		public void LoadShaders()
		{
			GameShaders.Misc["OrchidMod:HorizonGlow"] = new MiscShaderData(Assets.Request<Effect>("Assets/Effects/HorizonGlow"), "HorizonShaderPass");
		}

		public override void Load()
		{
			LoadContent();
			LoadShaders();

			ThoriumMod = OrchidUtils.GetModWithPossibleNull("ThoriumMod");

			AlchemistReactionRecipes = AlchemistHiddenReactionHelper.ListReactions();
		}

		public override void Unload()
		{
			AlchemistReactionRecipes = null;

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

			ThoriumModCalls();
			//BossChecklistCalls();
			CensusModCalls();
			ColoredDamageTypeModCalls();
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			OrchidModMessageType msgType = (OrchidModMessageType)reader.ReadByte();
			byte playernumber;
			switch (msgType)
			{
				case OrchidModMessageType.ORCHIDPLAYERSYNCPLAYERGAMBLER:
					playernumber = reader.ReadByte();
					OrchidGambler modPlayerGambler = Main.player[playernumber].GetModPlayer<OrchidGambler>();
					bool cardInDeck = reader.ReadBoolean();
					modPlayerGambler.gamblerHasCardInDeck = cardInDeck;
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

				case OrchidModMessageType.NPCHITBYCLASS: // Received by the server when a player damages a NPC for the first time with a orchid damage class
					OrchidGlobalNPC globalNPC = Main.npc[reader.ReadByte()].GetGlobalNPC<OrchidGlobalNPC>();
					switch (reader.ReadByte())
					{
						default:
							globalNPC.AlchemistHit = true;
							break;
						case 1:
							globalNPC.GamblerHit = true;
							break;
						case 2:
							globalNPC.GuardianHit = true;
							break;
						case 3:
							globalNPC.ShamanHit = true;
							break;
						case 4:
							globalNPC.ShapeshifterHit = true;
							break;
					}
					break;

				default:
					Logger.WarnFormat("OrchidMod: Unknown Message type: {0}", msgType);
					break;
			}
		}
	}
}
