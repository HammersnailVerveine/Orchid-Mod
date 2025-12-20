using System;
using System.Collections.Generic;
using System.IO;
using OrchidMod.Common;
using OrchidMod.Common.ModObjects;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace OrchidMod.Content.General.Prefixes
{
	public abstract class AccessoryPrefix : ModPrefix
	{
		private static readonly List<AccessoryPrefix> prefixes = new();
		public static IReadOnlyList<AccessoryPrefix> GetPrefixes => prefixes;

		// ...

		private readonly byte alchemistPotency;
		private readonly byte gamblerChip;
		private readonly byte guardianBlock;

		public override void Unload()
			=> prefixes.Clear();

		public override float RollChance(Item item)
			=> 1f;

		public override bool CanRoll(Item item)
		{
			if (!ModContent.GetInstance<OrchidServerConfig>().EnableContentAlchemist && this is BrewingPrefix)
			{ // Disable Brewing prefix rolls if alchemist content is disabled
				return false;
			}

			if (!ModContent.GetInstance<OrchidServerConfig>().EnableContentGambler && this is LoadedPrefix)
			{ // Disable Loaded prefix rolls if gambler content is disabled
				return false;
			}

			return true;
		}

		public override PrefixCategory Category
			=> PrefixCategory.Accessory;

		public AccessoryPrefix(byte alchemistPotency, byte gamblerChip, byte guardianBlock)
		{
			this.alchemistPotency = alchemistPotency;
			this.gamblerChip = gamblerChip;
			this.guardianBlock = guardianBlock;
		}

		public override void Load() => prefixes.Add(this);

		public override void Apply(Item item)
			=> item.GetGlobalItem<AccessoryPrefixItem>().SetPrefixVariables(alchemistPotency, gamblerChip, guardianBlock);

		public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1f + 0.2f * alchemistPotency + 0.1f * gamblerChip + 0.2f * guardianBlock;
			valueMult *= multiplier;
		}
	}

	public class AccessoryPrefixItem : GlobalItem
	{
		private byte alchemistPotency;
		private byte gamblerChip;
		private byte guardianBlock;

		// ...

		public void SetPrefixVariables(byte alchemistPotency, byte gamblerChip, byte guardianBlock)
		{
			this.alchemistPotency = alchemistPotency;
			this.gamblerChip = gamblerChip;
			this.guardianBlock = guardianBlock;
		}

		// ...

		public AccessoryPrefixItem()
		{
			alchemistPotency = 0;
			gamblerChip = 0;
			guardianBlock = 0;
		}

		public override bool InstancePerEntity => true;

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			AccessoryPrefixItem myClone = (AccessoryPrefixItem)base.Clone(item, itemClone);
			myClone.alchemistPotency = alchemistPotency;
			myClone.gamblerChip = gamblerChip;
			myClone.guardianBlock = guardianBlock;
			return myClone;
		}

		public override int ChoosePrefix(Item item, UnifiedRandom rand)
		{
			/*
			if (item.accessory && rand.NextBool(20))
			{
				var prefixes = AccessoryPrefix.GetPrefixes;
				return prefixes[Main.rand.Next(prefixes.Count)].Type;
			}
			*/

			return -1;
		}

		public override void PreReforge(Item item)
		{
			alchemistPotency = 0;
			gamblerChip = 0;
			guardianBlock = 0;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (item.social) return;

			var ttNames = new HashSet<string> { "Material", "Defense", "Vanity", "Equipable" };
			var index = tooltips.FindLastIndex(i => i.Mod.Equals("Terraria") && (ttNames.Contains(i.Name) || i.Name.StartsWith("Tooltip"))) + 1;

			if (index == -1) return;

			if (alchemistPotency > 0)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "AlchemistPotencyPrefix", Language.GetTextValue("Mods.OrchidMod.Prefixes.AddPotency", alchemistPotency))
				{
					IsModifier = true
				});
				return;
			}

			if (gamblerChip > 0)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "GamblerChipPrefix", Language.GetTextValue("Mods.OrchidMod.Prefixes.Effects.AddMaximumChips", gamblerChip))
				{
					IsModifier = true
				});
				return;
			}

			if (guardianBlock > 0)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "GuardianBlockPrefix", Language.GetTextValue("Mods.OrchidMod.Prefixes.AddGuard", guardianBlock))
				{
					IsModifier = true
				});
				return;
			}
		}

		public override void UpdateEquip(Item item, Player player)
		{
			if (item.prefix > 0)
			{
				OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
				modPlayer.modPlayerAlchemist.alchemistPotencyMax += alchemistPotency;
				modPlayer.modPlayerGambler.gamblerChipsMax += gamblerChip;
				modPlayer.modPlayerGuardian.GuardianGuardMax += guardianBlock;
			}
		}

		public override void NetSend(Item item, BinaryWriter writer)
		{
			writer.Write(alchemistPotency);
			writer.Write(gamblerChip);
			writer.Write(guardianBlock);
		}

		public override void NetReceive(Item item, BinaryReader reader)
		{
			alchemistPotency = reader.ReadByte();
			gamblerChip = reader.ReadByte();
			guardianBlock = reader.ReadByte();
		}
	}
}
