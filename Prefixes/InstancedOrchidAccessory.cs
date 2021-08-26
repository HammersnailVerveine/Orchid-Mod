using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace OrchidMod.Prefixes
{
	public class InstancedOrchidAccessory : GlobalItem
	{
		public byte pShamanTimer;
		public byte pAlchemistPotency;
		public byte pGamblerChip;

		public InstancedOrchidAccessory()
		{
			pShamanTimer = 0;
			pAlchemistPotency = 0;
			pGamblerChip = 0;
		}

		public override bool InstancePerEntity => true;

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			InstancedOrchidAccessory myClone = (InstancedOrchidAccessory)base.Clone(item, itemClone);
			myClone.pShamanTimer = pShamanTimer;
			myClone.pAlchemistPotency = pAlchemistPotency;
			myClone.pGamblerChip = pGamblerChip;

			return myClone;
		}

		public override int ChoosePrefix(Item item, UnifiedRandom rand)
		{

			int randValue = rand.Next(4);
			if (item.accessory && rand.Next(15) == 0)
			{
				switch (randValue)
				{
					case 0:
						return mod.PrefixType("Natural");
					case 1:
						return mod.PrefixType("Spiritual");
					case 2:
						return mod.PrefixType("Brewing");
					case 3:
						return mod.PrefixType("Loaded");
					case 4:
						return mod.PrefixType("Crooked");
					default:
						break;
				}
			}
			return -1;
		}

		public override bool NewPreReforge(Item item)
		{
			pShamanTimer = 0;
			pAlchemistPotency = 0;
			pGamblerChip = 0;
			return base.NewPreReforge(item);
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (!item.social && pShamanTimer > 0)
			{
				TooltipLine line = new TooltipLine(mod, "pShamanTimer", "+" + pShamanTimer + "s shamanic bond duration")
				{
					isModifier = true
				};
				tooltips.Add(line);
			}
			if (!item.social && pAlchemistPotency > 0)
			{
				TooltipLine line = new TooltipLine(mod, "pAlchemistPotency", "+" + pAlchemistPotency + " potency")
				{
					isModifier = true
				};
				tooltips.Add(line);
			}
			if (!item.social && pGamblerChip > 0)
			{
				TooltipLine line = new TooltipLine(mod, "pGamblerChip", "+" + pGamblerChip + " maximum chips")
				{
					isModifier = true
				};
				tooltips.Add(line);
			}
		}

		public override void UpdateEquip(Item item, Player player)
		{
			if (item.prefix > 0)
			{
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				modPlayer.shamanBuffTimer += pShamanTimer;
				modPlayer.alchemistPotencyMax += pAlchemistPotency;
				modPlayer.gamblerChipsMax += pGamblerChip;
			}
		}

		public override void NetSend(Item item, BinaryWriter writer)
		{
			writer.Write(pShamanTimer);
			writer.Write(pAlchemistPotency);
			writer.Write(pGamblerChip);
		}

		public override void NetReceive(Item item, BinaryReader reader)
		{
			pShamanTimer = reader.ReadByte();
			pAlchemistPotency = reader.ReadByte();
			pGamblerChip = reader.ReadByte();
		}
	}
}
