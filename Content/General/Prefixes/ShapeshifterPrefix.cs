using System.Collections.Generic;
using System.IO;
using OrchidMod.Common.Global.Items;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace OrchidMod.Content.General.Prefixes
{
	public abstract class ShapeshifterPrefix : ModPrefix
	{
		private static readonly List<ShapeshifterPrefix> prefixes = new();
		public static IReadOnlyList<ShapeshifterPrefix> GetPrefixes => prefixes;

		// ...

		private readonly float damage;
		private readonly float knockback;
		private readonly float attackSpeed;
		private readonly int crit;
		private readonly float moveSpeed;

		public override void Unload()
			=> prefixes.Clear();

		public override float RollChance(Item item)
			=> 500f;

		public override bool CanRoll(Item item)
			=> true;

		public override PrefixCategory Category
			=> PrefixCategory.Custom;

		public ShapeshifterPrefix(float damage, float knockback, float attackSpeed, int crit, float moveSpeed)
		{
			this.damage = damage;
			this.knockback = knockback;
			this.attackSpeed = attackSpeed;
			this.crit = crit;
			this.moveSpeed = moveSpeed;
		}

		public override void Load()
			=> prefixes.Add(this);

		public override void Apply(Item item)
			=> item.GetGlobalItem<ShapeshifterPrefixItem>().SetPrefixVariables(damage, knockback, attackSpeed, crit, moveSpeed);

		public override void ModifyValue(ref float valueMult)
		{
			float multiplier = 1f + (damage - 1f) * 0.05f + (knockback - 1f) * 0.05f + attackSpeed * 0.05f + moveSpeed * 0.05f + crit * 0.0015f;
			valueMult *= multiplier;
		}

		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult,
		ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{
			damageMult = damage;
			critBonus = crit;
			knockbackMult = knockback;
		}
	}

	public class ShapeshifterPrefixItem : GlobalItem
	{
		private float damage;
		private float knockback;
		private float attackSpeed;
		private int crit;
		private float moveSpeed;

		// ...

		public void SetPrefixVariables(float damage, float knockback, float attackSpeed, int crit, float moveSpeed)
		{
			this.damage = damage;
			this.knockback = knockback;
			this.attackSpeed = attackSpeed;
			this.crit = crit;
			this.moveSpeed = moveSpeed;
		}

		// ...

		//public float GetSlamDistance() => slamDistance != 0f ? slamDistance : 1f;
		public float GetMoveSpeed() => 1f + moveSpeed;
		public float GetAttackSpeed() => 1f + attackSpeed;

		public ShapeshifterPrefixItem()
		{
			damage = 0;
			knockback = 0;
			attackSpeed = 0;
			crit = 0;
			moveSpeed = 0;
		}

		public override bool InstancePerEntity
			=> true;

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			ShapeshifterPrefixItem myClone = (ShapeshifterPrefixItem)base.Clone(item, itemClone);
			myClone.damage = damage;
			myClone.knockback = knockback;
			myClone.attackSpeed = attackSpeed;
			myClone.crit = crit;
			myClone.moveSpeed = moveSpeed;
			return myClone;
		}

		public override void PreReforge(Item item)
		{
			damage = 0;
			knockback = 0;
			attackSpeed = 0;
			crit = 0;
			moveSpeed = 0;
		}

		public override void HoldItem(Item item, Player player)
		{
			OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
			if (attackSpeed != 0f) modPlayer.modPlayerShapeshifter.ShapeshifterMeleeSpeedBonus += attackSpeed;
			if (moveSpeed != 0f) modPlayer.modPlayerShapeshifter.ShapeshifterMoveSpeedBonus += moveSpeed;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (moveSpeed != 0f || attackSpeed != 0f)
			{
				// I have no clue how to do this in a clean way
				int index = -1;
				for (int i = 5; i >= 0; i--)
				{
					index = tooltips.FindIndex(ttip => ttip.Name.Equals("Tooltip" + i));
					if (index != -1)
					{
						index++;
						break;
					}
				}

				if (index == -1)
				{
					index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("PrefixDamage"));
					if (index == -1) index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("PrefixCritChance"));
					if (index == -1) index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("PrefixKnockback"));
					if (index == -1) index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Material"));
					if (index == -1) index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
					if (index == -1) return;
					index += 3;
				}


				if (attackSpeed != 0f)
				{
					tooltips.Insert(index, new TooltipLine(Mod, "ShapeshifterAttackSpeedPrefix", (attackSpeed > 0 ? "+" : "") + string.Format("{0:0}", attackSpeed * 100f) + "% attack speed")
					{
						IsModifier = true,
						IsModifierBad = attackSpeed < 0
					});
				}

				if (moveSpeed != 0f)
				{
					tooltips.Insert(index, new TooltipLine(Mod, "ShapeshifterMoveSpeedPrefix", (moveSpeed > 0 ? "+" : "") + string.Format("{0:0}", moveSpeed * 100f) + "% movement speed")
					{
						IsModifier = true,
						IsModifierBad = moveSpeed < 0
					});
				}
			}
		}

		public override int ChoosePrefix(Item item, UnifiedRandom rand)
		{
			if (item.ModItem is OrchidModShapeshifterShapeshift)
			{
				List<int> UniversalPrefixesIDs = [36, 37, 38, 39, 40, 41, 53, 54, 55, 56, 57, 59, 60, 61]; // Has to be hardcoded
				var prefixes = ShapeshifterPrefix.GetPrefixes;

				foreach (var prefix in prefixes) UniversalPrefixesIDs.Add(prefix.Type);
				return UniversalPrefixesIDs[Main.rand.Next(UniversalPrefixesIDs.Count)];
			}

			return -1;
		}

		public override void NetSend(Item item, BinaryWriter writer)
		{
			writer.Write(damage);
			writer.Write(knockback);
			writer.Write(attackSpeed);
			writer.Write(crit);
			writer.Write(moveSpeed);
		}

		public override void NetReceive(Item item, BinaryReader reader)
		{
			damage = reader.ReadSingle();
			knockback = reader.ReadSingle();
			attackSpeed = reader.ReadSingle();
			crit = reader.ReadInt32();
			moveSpeed = reader.ReadSingle();
		}
	}
}