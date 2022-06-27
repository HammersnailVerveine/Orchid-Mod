using MonoMod.Cil;
using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Bag
{
	[ClassTag(ClassTags.Alchemist)]
	public partial class PotionBag : ModItem, IOnMouseHover
	{
		public override void Load()
		{
			if (Main.dedServ) return;

			IL.Terraria.Player.Update += IgnoreSelectedItemChange;
			On.Terraria.UI.ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color += PostDrawItemSlot;
		}

		public override void Unload()
		{
			if (Main.dedServ) return;

			On.Terraria.UI.ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color -= PostDrawItemSlot;
			IL.Terraria.Player.Update -= IgnoreSelectedItemChange;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Potion Bag");
			Tooltip.SetDefault("Can carry up to 8 alchemical potions\n" +
							   "Items in the bag will appear in alchemist mixing interfaces\n" +
							   "'A prototype by the chemist, meant to be upgraded later'");
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 28;

			Item.maxStack = 1;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Orange;
		}

		public override void UpdateInventory(Player player)
			=> player.GetModPlayer<PotionBagPlayer>().AddBagToList(Item.ModItem as PotionBag);

		public override bool IsCloneable
			=> true;

		public override bool CanStack(Item item2)
			=> false;

		public override bool CanRightClick()
			=> true;

		public override bool ConsumeItem(Player player)
			=> false;

		public override void RightClick(Player player)
		{
			if (Main.mouseItem.dye > 0)
			{
				if (Main.mouseItem.stack <= 1)
				{
					(dye, Main.mouseItem) = (Main.mouseItem, dye);
					dye.favorited = false;

					return;
				}

				if (dye.IsAir)
				{
					Main.mouseItem.stack--;
					dye = Main.mouseItem.Clone();
					dye.stack = 1;
					dye.favorited = false;

					return;
				}

				return;
			}

			if (Main.mouseItem.TryGetGlobalItem(out OrchidModGlobalItem orchidItem) && orchidItem.alchemistElement != AlchemistElement.NULL)
			{
				for (int i = 0; i < SLOTS_XY; i++)
				{
					if (inventory[i].IsAir)
					{
						(inventory[i], Main.mouseItem) = (Main.mouseItem, inventory[i]);
						inventory[i].favorited = false;

						return;
					}
				}

				return;
			}

			if (Main.mouseItem.IsAir)
			{
				IsActive = !IsActive;
				return;
			}
		}

		void IOnMouseHover.OnMouseHover(int context)
		{
			ref var triggers = ref PlayerInput.Triggers;
			var player = Main.LocalPlayer;

			void RemoveItemFromInventory(int index)
			{
				if (inventory[index].IsAir) return;

				player.QuickSpawnClonedItem(player.GetSource_ItemUse(Item, "PotionBag"), inventory[index]);
				inventory[index].TurnToAir();

				for (int i = 0, j = -1; i < SLOTS_XY; i++)
				{
					if (!inventory[i].IsAir)
					{
						j++;
						(inventory[i], inventory[j]) = (inventory[j], inventory[i]);
					}
				}
			}

			if (triggers.JustPressed.Hotbar1)
			{
				RemoveItemFromInventory(0);
				return;
			}

			if (triggers.JustPressed.Hotbar2)
			{
				RemoveItemFromInventory(1);
				return;
			}

			if (triggers.JustPressed.Hotbar3)
			{
				RemoveItemFromInventory(2);
				return;
			}

			if (triggers.JustPressed.Hotbar4)
			{
				RemoveItemFromInventory(3);
				return;
			}

			if (triggers.JustPressed.Hotbar5)
			{
				RemoveItemFromInventory(4);
				return;
			}

			if (triggers.JustPressed.Hotbar6)
			{
				RemoveItemFromInventory(5);
				return;
			}

			if (triggers.JustPressed.Hotbar7)
			{
				RemoveItemFromInventory(6);
				return;
			}

			if (triggers.JustPressed.Hotbar8)
			{
				RemoveItemFromInventory(7);
				return;
			}

			if (triggers.JustPressed.Hotbar9)
			{
				for (int k = 0; k < SLOTS_XY; k++)
				{
					if (!inventory[k].IsAir)
					{
						player.QuickSpawnClonedItem(player.GetSource_ItemUse(Item, "PotionBag"), inventory[k]);
						inventory[k].TurnToAir();
					}
				}
				return;
			}

			if (triggers.JustPressed.Hotbar10)
			{
				player.QuickSpawnClonedItem(player.GetSource_ItemUse(Item, "PotionBag"), dye);
				dye.TurnToAir();
				return;
			}
		}

		private static void IgnoreSelectedItemChange(ILContext il)
		{
			var c = new ILCursor(il);

			// if (this.itemAnimation == 0 && this.ItemTimeIsZero && this.reuseDelay == 0)
			// {
			//	  this.dropItemCheck();
			//	  int num6 = this.selectedItem;
			//	  bool flag7 = false;
			//
			//    if (!Main.drawingPlayerChat && this.selectedItem != 58 && !Main.editSign && !Main.editChest)

			if (!c.TryGotoNext(MoveType.Before,
				i => i.MatchLdarg(0),
				i => i.MatchCall(typeof(Player).GetMethod("dropItemCheck")),
				i => i.MatchLdarg(0),
				i => i.MatchLdfld<Player>("selectedItem"),
				i => i.MatchStloc(out _))) return;

			ILLabel label = null;

			if (!c.TryGotoNext(MoveType.Before,
				i => i.MatchLdarg(0),
				i => i.MatchLdfld<Player>("selectedItem"),
				i => i.MatchLdcI4(58),
				i => i.MatchBeq(out label))) return;

			c.EmitDelegate<Func<bool>>(() => Main.HoverItem?.ModItem is PotionBag);
			c.Emit(Mono.Cecil.Cil.OpCodes.Brtrue, label);
		}
	}
}