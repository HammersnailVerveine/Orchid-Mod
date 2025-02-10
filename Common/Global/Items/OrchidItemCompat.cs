using Mono.Cecil;
using OrchidMod.Common.Attributes;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Weapons.Gauntlets;
using OrchidMod.Content.Guardian.Weapons.Warhammers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Common.Global.Items
{
	public partial class OrchidItemCompat : GlobalItem
	{
		public override void RightClick(Item item, Player player)
		{
			if (OrchidMod.ThoriumMod != null)
			{
				if (item.type == OrchidMod.ThoriumMod.Find<ModItem>("MeleeThorHammer").Type)
				{
					item.TurnToAir();
					Item.NewItem(item.GetSource_FromThis(), (int)player.Center.X, (int)player.Center.Y, 0, 0, ModContent.ItemType<ThoriumThorsHammerWarhammer>(), pfix: 0, noGrabDelay: true);
				}

				if (item.type == ModContent.ItemType<ThoriumThorsHammerWarhammer>())
				{
					item.TurnToAir();
					Item.NewItem(item.GetSource_FromThis(), (int)player.Center.X, (int)player.Center.Y, 0, 0, OrchidMod.ThoriumMod.Find<ModItem>("MeleeThorHammer").Type, pfix: 0, noGrabDelay: true);
				}
			}
		}
	}
}