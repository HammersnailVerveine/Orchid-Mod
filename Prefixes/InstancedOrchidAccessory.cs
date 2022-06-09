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
			pShamanTimer = 0;
			pAlchemistPotency = 0;
			pGamblerChip = 0;
			return base.PreReforge(item);
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (!item.social && pShamanTimer > 0)
			{
				TooltipLine line = new TooltipLine(Mod, "pShamanTimer", "+" + pShamanTimer + "s shamanic bond duration")
				{
					IsModifier = true
				};
				tooltips.Add(line);
			}
			if (!item.social && pAlchemistPotency > 0)
			{
				TooltipLine line = new TooltipLine(Mod, "pAlchemistPotency", "+" + pAlchemistPotency + " potency")
				{
					IsModifier = true
				};
				tooltips.Add(line);
			}
			if (!item.social && pGamblerChip > 0)
			{
				TooltipLine line = new TooltipLine(Mod, "pGamblerChip", "+" + pGamblerChip + " maximum chips")
				{
					IsModifier = true
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
