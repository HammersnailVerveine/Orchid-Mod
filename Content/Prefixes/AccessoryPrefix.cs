using System;
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
		private static readonly Dictionary<string, AccessoryPrefix> prefixesByName = new();
		public static IReadOnlyDictionary<string, AccessoryPrefix> AllPrefixesByName => prefixesByName;

		// ...

		private readonly string name;
		private readonly byte shamanTimer;
		private readonly byte alchemistPotency;
		private readonly byte gamblerChip;

		public override void Unload()
			=> prefixesByName.Clear();

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
			/*
			 *  [SP]
			 *  Terraria.ModLoader.Exceptions.MultipleException: Multiple errors occured.
			 *  ---> System.NullReferenceException: Object reference not set to an instance of an object.
			 *  at OrchidMod.Content.Prefixes.AccessoryPrefix.IsLoadingEnabled(Mod mod) in OrchidMod\Content\Prefixes\AccessoryPrefixes.cs:line 61
			 *  at Terraria.ModLoader.Mod.AddContent(ILoadable instance) in tModLoader\Terraria\ModLoader\Mod.cs:line 132
			 *  at Terraria.ModLoader.Core.LoaderUtils.ForEachAndAggregateExceptions[T](IEnumerable`1 enumerable, Action`1 action) in tModLoader\Terraria\ModLoader\Core\LoaderUtils.cs:line 47
			 * 
			 * 
			 * 
			void AddPrefix(string name, byte shamanTimer, byte alchemistPotency, byte gamblerChip)
			{
				var prefix = new AccessoryPrefix(name, shamanTimer, alchemistPotency, gamblerChip);
				Mod.AddContent(prefix);
				prefixesByName.Add(name, prefix);
			}

			AddPrefix("Natural", 1, 0, 0);
			AddPrefix("Spiritual", 2, 0, 0);
			AddPrefix("Brewing", 0, 1, 0);
			AddPrefix("Crooked", 0, 0, 1);
			AddPrefix("Loaded", 0, 0, 2);
			*/
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
			/*
			if (item.accessory && rand.NextBool(15))
			{
				var prefixes = ShamanPrefix.AllPrefixesByName;
				return prefixes.ElementAt(Main.rand.Next(0, prefixes.Count)).Value.Type;
			}
			*/
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
