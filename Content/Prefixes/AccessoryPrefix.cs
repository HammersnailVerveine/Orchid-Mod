using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace OrchidMod.Content.Prefixes
{
	public abstract class AccessoryPrefix : ModPrefix
	{
		private static readonly List<AccessoryPrefix> prefixes = new();
		public static IReadOnlyList<AccessoryPrefix> GetPrefixes => prefixes;

		// ...

		private readonly string displayName;
		private readonly byte shamanTimer;
		private readonly byte alchemistPotency;
		private readonly byte gamblerChip;

		public override void Unload()
			=> prefixes.Clear();

		public override float RollChance(Item item)
			=> 1f;

		public override bool CanRoll(Item item)
			=> true;

		public override PrefixCategory Category
			=> PrefixCategory.Accessory;

		public AccessoryPrefix(string displayName, byte shamanTimer, byte alchemistPotency, byte gamblerChip)
		{
			this.displayName = displayName;
			this.shamanTimer = shamanTimer;
			this.alchemistPotency = alchemistPotency;
			this.gamblerChip = gamblerChip;
		}

		public override void Load()
			=> prefixes.Add(this);

		/* public override void SetStaticDefaults()
			=> DisplayName.SetDefault(displayName); */

		public override void Apply(Item item)
			=> item.GetGlobalItem<AccessoryPrefixItem>().SetPrefixVariables(shamanTimer, alchemistPotency, gamblerChip);

		public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1f + (0.05f * 2 * shamanTimer) + (0.1f * 2 * alchemistPotency) + (0.1f * gamblerChip);
			valueMult *= multiplier;
		}
	}

	public class AccessoryPrefixItem : GlobalItem
	{
		private byte shamanTimer;
		private byte alchemistPotency;
		private byte gamblerChip;

		// ...

		public void SetPrefixVariables(byte shamanTimer, byte alchemistPotency, byte gamblerChip)
		{
			this.shamanTimer = shamanTimer;
			this.alchemistPotency = alchemistPotency;
			this.gamblerChip = gamblerChip;
		}

		// ...

		public AccessoryPrefixItem()
		{
			shamanTimer = 0;
			alchemistPotency = 0;
			gamblerChip = 0;
		}

		public override bool InstancePerEntity => true;

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			AccessoryPrefixItem myClone = (AccessoryPrefixItem)base.Clone(item, itemClone);
			myClone.shamanTimer = shamanTimer;
			myClone.alchemistPotency = alchemistPotency;
			myClone.gamblerChip = gamblerChip;
			return myClone;
		}

		public override int ChoosePrefix(Item item, UnifiedRandom rand)
		{
			if (item.accessory && rand.NextBool(20))
			{
				var prefixes = ShamanPrefix.GetPrefixes;
				return prefixes[Main.rand.Next(prefixes.Count)].Type;
			}

			return -1;
		}

		public override void PreReforge(Item item)/* tModPorter Note: Use CanReforge instead for logic determining if a reforge can happen. */
		{
			shamanTimer = 0;
			alchemistPotency = 0;
			gamblerChip = 0;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (item.social) return;

			var ttNames = new HashSet<string> { "Material", "Defense", "Vanity", "Equipable" };
			var index = tooltips.FindLastIndex(i => i.Mod.Equals("Terraria") && (ttNames.Contains(i.Name) || i.Name.StartsWith("Tooltip"))) + 1;

			if (index == -1) return;

			if (shamanTimer > 0)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "ShamanTimerPrefix", "+" + shamanTimer + "s shamanic bond duration")
				{
					IsModifier = true
				});
				return;
			}

			if (alchemistPotency > 0)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "AlchemistPotencyPrefix", "+" + alchemistPotency + " potency")
				{
					IsModifier = true
				});
				return;
			}

			if (gamblerChip > 0)
			{
				tooltips.Insert(index, new TooltipLine(Mod, "GamblerChipPrefix", "+" + gamblerChip + " maximum chips")
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
				modPlayer.modPlayerShaman.ShamanBondDuration += shamanTimer;
				modPlayer.modPlayerAlchemist.alchemistPotencyMax += alchemistPotency;
				modPlayer.modPlayerGambler.gamblerChipsMax += gamblerChip;
			}
		}

		public override void NetSend(Item item, BinaryWriter writer)
		{
			writer.Write(shamanTimer);
			writer.Write(alchemistPotency);
			writer.Write(gamblerChip);
		}

		public override void NetReceive(Item item, BinaryReader reader)
		{
			shamanTimer = reader.ReadByte();
			alchemistPotency = reader.ReadByte();
			gamblerChip = reader.ReadByte();
		}
	}
}
