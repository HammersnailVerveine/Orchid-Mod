using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Items.Ranger
{
	public class HeraldOfFrost : OrchidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Herald of Frost");
			Tooltip.SetDefault("...");
		}

		public override void SetDefaults()
		{
			item.autoReuse = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 14;
			item.useTime = 14;
			item.width = 24;
			item.height = 58;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.useAmmo = AmmoID.Arrow;
			item.UseSound = SoundID.Item5;
			item.damage = 20;
			item.knockBack = 5f;
			item.shootSpeed = 6f;
			item.noMelee = true;
			item.value = Item.sellPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.ranged = true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.RemoveAll(i => i.Name != "ItemName");
			tooltips.Add(new TooltipLine(mod, "ExtraInfo", "'This item will be available later'"));
		}

		public override Vector2? HoldoutOffset() => new Vector2(2, 0);
	}
}
