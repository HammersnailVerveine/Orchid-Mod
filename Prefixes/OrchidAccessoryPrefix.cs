using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Prefixes
{
	public class OrchidAccessoryPrefix : ModPrefix
	{
		private byte pShamanTimer;
		private byte pAlchemistPotency;
		private byte pGamblerChip;

		public override float RollChance(Item item)
			=> 1f;

		public override bool CanRoll(Item item)
			=> true;

		public override PrefixCategory Category
			=> PrefixCategory.Accessory;

		public OrchidAccessoryPrefix()
		{
		}

		public OrchidAccessoryPrefix(byte pShamanTimer, byte pAlchemistPotency, byte pGamblerChip)
		{
			this.pShamanTimer = pShamanTimer;
			this.pAlchemistPotency = pAlchemistPotency;
			this.pGamblerChip = pGamblerChip;
		}

		public override bool Autoload(ref string name)
		{
			if (!base.Autoload(ref name))
			{
				return false;
			}

			mod.AddPrefix("Natural", new OrchidAccessoryPrefix(1, 0, 0));
			mod.AddPrefix("Spiritual", new OrchidAccessoryPrefix(2, 0, 0));
			mod.AddPrefix("Brewing", new OrchidAccessoryPrefix(0, 1, 0));
			mod.AddPrefix("Crooked", new OrchidAccessoryPrefix(0, 0, 1));
			mod.AddPrefix("Loaded", new OrchidAccessoryPrefix(0, 0, 2));

			return false;
		}

		public override void Apply(Item item)
		{
			item.GetGlobalItem<InstancedOrchidAccessory>().pShamanTimer = pShamanTimer;
			item.GetGlobalItem<InstancedOrchidAccessory>().pAlchemistPotency = pAlchemistPotency;
			item.GetGlobalItem<InstancedOrchidAccessory>().pGamblerChip = pGamblerChip;
		}

		public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1f + (0.05f * 2 * pShamanTimer) + (0.1f * 2 * pAlchemistPotency) + (0.1f * pGamblerChip);
			valueMult *= multiplier;
		}
	}
}