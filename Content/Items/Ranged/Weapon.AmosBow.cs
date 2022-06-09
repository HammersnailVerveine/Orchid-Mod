using Microsoft.Xna.Framework;
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
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 14;
			Item.useTime = 14;
			Item.width = 18;
			Item.height = 46;
			Item.shoot = ModContent.ProjectileType<AmosBowProjectile>();
			Item.useAmmo = AmmoID.Arrow;
			Item.UseSound = SoundID.Item5;
			Item.damage = 20;
			Item.knockBack = 5f;
			Item.shootSpeed = 6f;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.ranged = true;
			Item.noUseGraphic = true;
			Item.channel = true;
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
		//private int _charge = 0;

		// ...

		public override string Texture => "OrchidMod/Assets/Textures/Items/AmosBow";

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;

			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			// ...
		}
	}
}
