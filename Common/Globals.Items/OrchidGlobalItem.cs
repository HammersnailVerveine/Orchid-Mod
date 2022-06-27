using OrchidMod.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Common.Globals.Items
{
	public class OrchidGlobalItem : GlobalItem, IPostSetupContent
	{
		private static readonly Dictionary<int, ClassTags> tagsByItemType = new();

		// ...

		void IPostSetupContent.PostSetupContent(Mod mod)
		{
			foreach (var modItem in OrchidMod.Instance.GetContent<ModItem>())
			{
				if (tagsByItemType.ContainsKey(modItem.Type)) continue;

				var atr = modItem.GetType().GetCustomAttributes(typeof(ClassTagAttribute), true).FirstOrDefault() as ClassTagAttribute;

				if (atr is null) continue;

				tagsByItemType.Add(modItem.Type, atr.Tag);
			}
		}

		public override void Unload()
		{
			tagsByItemType.Clear();
		}

		// ...

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (ModContent.GetInstance<OrchidConfig>().ShowClassTags && tagsByItemType.ContainsKey(item.type))
			{
				AddClassTagInTooltips(item, tooltips);
			}
		}

		// ...

		private void AddClassTagInTooltips(Item item, List<TooltipLine> tooltips)
		{
			var index = tooltips.FindIndex(i => i.Mod.Equals("Terraria") && i.Name.Equals("ItemName"));

			if (index < 0) return;

			var tagType = tagsByItemType[item.type];

			tooltips.Insert(index + 1, new TooltipLine(Mod, "ClassTag", GetClassTagText(tagType))
			{
				OverrideColor = OrchidColors.GetClassTagColor(tagType)
			});
		}

		// ...

		private static string GetClassTagText(ClassTags tag)
		{
			return tag switch
			{
				ClassTags.Alchemist => "-Alchemist Class-",
				_ => throw new NotImplementedException(),
			};
		}
	}
}