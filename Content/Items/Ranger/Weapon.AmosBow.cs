using Microsoft.Xna.Framework;
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
	public class AmosBow : OrchidItem
	{
		public override bool Autoload(ref string name) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amos' Bow");
			Tooltip.SetDefault("...");
		}

		public override void SetDefaults()
		{
			item.autoReuse = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 14;
			item.useTime = 14;
			item.width = 18;
			item.height = 46;
			item.shoot = ModContent.ProjectileType<AmosBowProjectile>();
			item.useAmmo = AmmoID.Arrow;
			item.UseSound = SoundID.Item5;
			item.damage = 20;
			item.knockBack = 5f;
			item.shootSpeed = 6f;
			item.noMelee = true;
			item.value = Item.sellPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.ranged = true;
			item.noUseGraphic = true;
			item.channel = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.ownedProjectileCounts[type] > 0) return false;

			var proj = Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
			return false;
		}
	}

	public class AmosBowProjectile : OrchidProjectile
	{
		private int _charge = 0;

		// ...

		public override string Texture => "OrchidMod/Assets/Textures/Items/AmosBow";

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;

			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			// ...
		}
	}
}
