using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist
{
	public abstract class OrchidModAlchemistScroll : OrchidModItem
	{
		public int hintLevel = 0;

		public virtual void SafeSetDefaults() { }

		public sealed override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 32;
			Item.DamageType = ModContent.GetInstance<AlchemistDamageClass>();
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item64;
			Item.consumable = true;
			Item.autoReuse = false;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 5, 0);
			SafeSetDefaults();
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			AlchemistHiddenReactionHelper.addAlchemistHint(player, modPlayer, this.hintLevel);
			return true;
		}

		protected override bool CloneNewInstances => true;

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				tooltips.Insert(1, new TooltipLine(Mod, "ClassTag", "-Alchemist Class-")
				{
					OverrideColor = new Color(155, 255, 55)
				});
			}
		}
	}
}
