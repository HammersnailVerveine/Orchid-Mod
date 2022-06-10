using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Interfaces;
using OrchidMod.Utilities;
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
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.width = 24;
			Item.height = 62;
			Item.shoot = ModContent.ProjectileType<SearingOnslaughtProjectile>();
			//item.useAmmo = AmmoID.Arrow;
			Item.UseSound = SoundID.Item5;
			Item.damage = 20;
			Item.knockBack = 5f;
			Item.shootSpeed = 6f;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.ranged = true;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.useTurn = true;
			Item.autoReuse = true;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			Lighting.AddLight(player.itemLocation, _lightColor.ToVector3() * 0.2f);
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, _lightColor.ToVector3() * 0.2f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.DrawSimpleItemGlowmaskInWorld(Item, ModContent.GetTexture(this.Texture + "_Glow"), Color.White, rotation, scale);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.RemoveAll(i => i.Name != "ItemName");
			tooltips.Add(new TooltipLine(Mod, "ExtraInfo", "'This item will be available later'"));
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
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.timeLeft = 2;

			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;
		}

		public override void AI()
		{
			if (!Owner.active || Owner.dead) Projectile.Kill();

			if (Owner.channel)
			{
				Owner.heldProj = Projectile.whoAmI;
				Owner.itemAnimation = 2;
				Owner.itemTime = 2;
				Projectile.timeLeft = 2;

				Projectile.Center = Owner.itemLocation;

				if (Projectile.Center.X > Owner.MountedCenter.X)
				{
					Owner.ChangeDir(1);
					Projectile.direction = 1;
				}
				else
				{
					Owner.ChangeDir(-1);
					Projectile.direction = -1;
				}
			}
		}

		public override bool? CanDamage()/* Suggestion: Return null instead of false */ => false;
		public override bool? CanCutTiles() => false;
	}

	/*public class SearingOnslaughtArrowProjectile : OrchidProjectile
	{

	}*/
}
