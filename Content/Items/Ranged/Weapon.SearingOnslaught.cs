using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.Interfaces;
using OrchidMod.Common.PlayerDrawLayers;
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
	public class SearingOnslaught : ModItem
	{
		private static readonly Color lightColor = new(255, 150, 0);

		// ...

		public override string Texture => OrchidAssets.ItemsPath + Name;

		public override void SetStaticDefaults()
		{
			HeldItemLayer.RegisterDrawMethod(Type, DrawUtils.DrawSimpleItemGlowmaskOnPlayer);

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
			Item.DamageType = DamageClass.Ranged;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.useTurn = true;
			Item.autoReuse = true;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			Lighting.AddLight(player.itemLocation, lightColor.ToVector3() * 0.2f);
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, lightColor.ToVector3() * 0.2f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.DrawSimpleItemGlowmaskInWorld(Item, Color.White, rotation, scale);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.RemoveAll(i => i.Name != "ItemName");
			tooltips.Add(new TooltipLine(Mod, "ExtraInfo", "'This item will be available later'"));
		}

		public override Vector2? HoldoutOffset() => new Vector2(2, 0);
	}

	public class SearingOnslaughtProjectile : ModProjectile
	{
		public override string Texture => OrchidAssets.InvisiblePath;

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
			var owner = Main.player[Projectile.owner];

			if (!owner.active || owner.dead) Projectile.Kill();

			if (owner.channel)
			{
				owner.heldProj = Projectile.whoAmI;
				owner.itemAnimation = 2;
				owner.itemTime = 2;
				Projectile.timeLeft = 2;

				Projectile.Center = owner.itemLocation;

				if (Projectile.Center.X > owner.MountedCenter.X)
				{
					owner.ChangeDir(1);
					Projectile.direction = 1;
				}
				else
				{
					owner.ChangeDir(-1);
					Projectile.direction = -1;
				}
			}
		}

		public override bool? CanDamage() => false;
		public override bool? CanCutTiles() => false;
	}
}
