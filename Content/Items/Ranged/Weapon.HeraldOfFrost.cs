using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Items.Ranged
{
	public class HeraldOfFrost : ModItem
	{
		public override string Texture => OrchidAssets.ItemsPath + Name;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Herald of Frost");
			Tooltip.SetDefault("...");
		}

		public override void SetDefaults()
		{
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 14;
			Item.useTime = 14;
			Item.width = 24;
			Item.height = 58;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.useAmmo = AmmoID.Arrow;
			Item.UseSound = SoundID.Item5;
			Item.damage = 20;
			Item.knockBack = 5f;
			Item.shootSpeed = 6f;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.DamageType = DamageClass.Ranged;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.RemoveAll(i => i.Name != "ItemName");
			tooltips.Add(new TooltipLine(Mod, "ExtraInfo", "'This item will be available later'"));
		}

		public override Vector2? HoldoutOffset()
			=> new Vector2(2, 0);
	}
}
