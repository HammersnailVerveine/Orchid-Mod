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

				default:
					Logger.WarnFormat("OrchidMod: Unknown Message type: {0}", msgType);
					break;
			}
		}
	}
}
