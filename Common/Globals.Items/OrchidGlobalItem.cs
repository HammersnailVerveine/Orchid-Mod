using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Common.Globals.Items
{
	public class OrchidGlobalItem : GlobalItem, IPostSetupContent
	{
		private static readonly Dictionary<int, ClassTags> tagsByItemType = new();

		// ...

		void IPostSetupContent.PostSetupContent(Mod mod)
		{
			foreach (var modItem in mod.GetContent<ModItem>())
			{
				if (tagsByItemType.ContainsKey(modItem.Type)) continue;

				var atr = modItem.GetType().GetCustomAttribute<ClassTagAttribute>();

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
			var config = ModContent.GetInstance<OrchidConfig>();

			if (config.ShowClassTags && tagsByItemType.ContainsKey(item.type))
			{
				AddClassTagToTooltips(item, tooltips);
			}

			if (config.LoadCrossmodContentWithoutRequiredMods)
			{
				var atr = item.ModItem?.GetType().GetCustomAttribute<CrossmodContentAttribute>();

				if (atr is not null)
				{
					AddCrossmodInfoToTooltips(item, tooltips, atr.Mods);
				}
			}
		}

		// ...

		private void AddClassTagToTooltips(Item item, List<TooltipLine> tooltips)
		{
			var index = tooltips.FindIndex(i => i.Mod.Equals("Terraria") && i.Name.Equals("ItemName"));

			if (index < 0) return;

			var tagType = tagsByItemType[item.type];

			tooltips.Insert(index + 1, new TooltipLine(Mod, "ClassTag", GetClassTagText(tagType))
			{
				OverrideColor = OrchidColors.GetClassTagColor(tagType)
			});
		}

		private void AddCrossmodInfoToTooltips(Item item, List<TooltipLine> tooltips, string[] mods)
		{
			var text = "This is a crossmod item: " + String.Join(", ", mods);

			tooltips.Add(new TooltipLine(Mod, "CrossmodInfo", text)
			{
				OverrideColor = OrchidColors.CrossmodContentWarning
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