using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;

namespace OrchidMod.Common.PlayerDrawLayers
{
	public sealed class HeldItemLayer : PlayerDrawLayer
	{
		private static readonly Dictionary<int, DrawPlayerDelegate> drawMethodsByItemType = new();

		public static void RegisterDrawMethod(int itemType, DrawPlayerDelegate drawMethod)
		{
			if (drawMethod == null) return;

			drawMethodsByItemType.Add(itemType, drawMethod);
		}

		// ...

		public override void Unload()
			=> drawMethodsByItemType.Clear();

		public override Position GetDefaultPosition()
			=> new AfterParent(Terraria.DataStructures.PlayerDrawLayers.HeldItem);

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			var player = drawInfo.drawPlayer;
			var item = drawInfo.heldItem;
			var usingItem = player.itemAnimation > 0 && item.useStyle != ItemUseStyleID.None;
			var holdingSuitableItem = item.holdStyle != 0 && !player.pulley;

			if (!player.CanVisuallyHoldItem(item))
			{
				holdingSuitableItem = false;
			}

			var flags = false;
			flags |= drawInfo.shadow != 0f;
			flags |= player.JustDroppedAnItem;
			flags |= player.frozen;
			flags |= !(usingItem || holdingSuitableItem);
			flags |= item.type <= ItemID.None;
			flags |= player.dead;
			flags |= item.noUseGraphic;
			flags |= player.wet && item.noWet;
			flags |= player.happyFunTorchTime;
			flags |= player.happyFunTorchTime && player.HeldItem.createTile == TileID.Torches && player.itemAnimation == 0;

			return !flags;
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			var player = drawInfo.drawPlayer;
			var item = player.HeldItem;
			var itemType = item.type;

			if (!drawMethodsByItemType.ContainsKey(item.type)) return;

			drawMethodsByItemType[itemType].Invoke(ref drawInfo);
		}
	}
}