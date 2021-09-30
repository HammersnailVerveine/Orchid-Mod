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
	public class SearingOnslaught : OrchidItem, IGlowingItem
	{
		private readonly Color _lightColor = new Color(255, 150, 0);

		// ...

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Searing Onslaught");
			Tooltip.SetDefault("...");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 20;
			item.useTime = 20;
			item.width = 24;
			item.height = 62;
			item.shoot = ModContent.ProjectileType<SearingOnslaughtProjectile>();
			//item.useAmmo = AmmoID.Arrow;
			item.UseSound = SoundID.Item5;
			item.damage = 20;
			item.knockBack = 5f;
			item.shootSpeed = 6f;
			item.noMelee = true;
			item.value = Item.sellPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.ranged = true;
			item.channel = true;
			item.noUseGraphic = true;
			item.useTurn = true;
			item.autoReuse = true;
		}

		public override void UseStyle(Player player)
		{
			Lighting.AddLight(player.itemLocation, _lightColor.ToVector3() * 0.2f);
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, _lightColor.ToVector3() * 0.2f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			OrchidHelper.DrawSimpleItemGlowmaskInWorld(item, spriteBatch, ModContent.GetTexture(this.Texture + "_Glow"), Color.White, rotation, scale);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.RemoveAll(i => i.Name != "ItemName");
			tooltips.Add(new TooltipLine(mod, "ExtraInfo", "'This item will be available later'"));
		}

		public override Vector2? HoldoutOffset() => new Vector2(2, 0);

		// ...

		void IGlowingItem.DrawItemGlowmask(PlayerDrawInfo drawInfo)
		{
			OrchidHelper.DrawSimpleItemGlowmaskOnPlayer(drawInfo, ModContent.GetTexture(this.Texture + "_Glow"));
		}
	}

	public class SearingOnslaughtProjectile : OrchidProjectile
	{
		//private int _charge = 0;

		// ...

		public override string Texture => "OrchidMod/Assets/Textures/Items/AmosBow";

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.timeLeft = 2;

			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.netImportant = true;
		}

		public override void AI()
		{
			if (!Owner.active || Owner.dead) projectile.Kill();

			if (Owner.channel)
			{
				Owner.heldProj = projectile.whoAmI;
				Owner.itemAnimation = 2;
				Owner.itemTime = 2;
				projectile.timeLeft = 2;

				projectile.Center = Owner.itemLocation;

				if (projectile.Center.X > Owner.MountedCenter.X)
				{
					Owner.ChangeDir(1);
					projectile.direction = 1;
				}
				else
				{
					Owner.ChangeDir(-1);
					projectile.direction = -1;
				}
			}
		}

		public override bool CanDamage() => false;
		public override bool? CanCutTiles() => false;
	}

	/*public class SearingOnslaughtArrowProjectile : OrchidProjectile
	{

	}*/
}
