﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace OrchidMod.Content.Prefixes
{
	public class AccessoryPrefix : ModPrefix
	{
		private readonly string name;
		private readonly byte shamanTimer;
		private readonly byte alchemistPotency;
		private readonly byte gamblerChip;

		// ...

		public override float RollChance(Item item)
			=> 1f;

		public override bool CanRoll(Item item)
			=> true;

		public override PrefixCategory Category
			=> PrefixCategory.Accessory;

		public AccessoryPrefix() { }

		public AccessoryPrefix(string name, byte shamanTimer, byte alchemistPotency, byte gamblerChip)
		{
			this.name = name;
			this.shamanTimer = shamanTimer;
			this.alchemistPotency = alchemistPotency;
			this.gamblerChip = gamblerChip;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(name);
		}

		public override bool IsLoadingEnabled(Mod mod)
		{
			void AddPrefix(string name, byte shamanTimer, byte alchemistPotency, byte gamblerChip)
			{
				Mod.AddContent(new AccessoryPrefix(name, shamanTimer, alchemistPotency, gamblerChip));
			}

			AddPrefix("Natural", 1, 0, 0);
			AddPrefix("Spiritual", 2, 0, 0);
			AddPrefix("Brewing", 0, 1, 0);
			AddPrefix("Crooked", 0, 0, 1);
			AddPrefix("Loaded", 0, 0, 2);

			return false;
		}

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
			if (item.accessory && rand.NextBool(15))
			{
				switch (rand.Next(5))
				{
					case 0:
						return Mod.Find<ModPrefix>("Natural").Type;
					case 1:
						return Mod.Find<ModPrefix>("Spiritual").Type;
					case 2:
						return Mod.Find<ModPrefix>("Brewing").Type;
					case 3:
						return Mod.Find<ModPrefix>("Loaded").Type;
					case 4:
						return Mod.Find<ModPrefix>("Crooked").Type;
					default:
						break;
				}
			}
			return -1;
		}

		public override bool PreReforge(Item item)
		{
			shamanTimer = 0;
			alchemistPotency = 0;
			gamblerChip = 0;

			return base.PreReforge(item);
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (item.social) return;

			var ttNames = new HashSet<string> { "Material", "Defense", "Vanity", "Equipable" };
			var index = tooltips.FindLastIndex(i => i.Name.Equals("Terraria") && ttNames.Contains(i.Text)) + 1;

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
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				modPlayer.shamanBuffTimer += shamanTimer;
				modPlayer.alchemistPotencyMax += alchemistPotency;
				modPlayer.gamblerChipsMax += gamblerChip;
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
