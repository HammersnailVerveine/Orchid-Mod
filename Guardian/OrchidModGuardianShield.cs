using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace OrchidMod.Guardian
{
	public abstract class OrchidModGuardianShield : OrchidModGuardianItem
	{
		public override bool CloneNewInstances => true;

		public sealed override void SetStaticDefaults()
		{
			this.SafeSetStaticDefaults();
		}

		public sealed override void SetDefaults()
		{
			item.crit = 4;
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
			item.noMelee = true;
			item.maxStack = 1;
			item.useStyle = ItemUseStyleID.Stabbing;

			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.guardianWeapon = true;

			this.SafeSetDefaults();
		}

		public sealed override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			this.SafeShoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			return false;
		}

		/*
		public sealed override void UseStyle(Player player)
		{
			player.itemLocation = (player.MountedCenter + new Vector2(player.direction * 12, player.gravDir * 24)).Floor();
			player.itemRotation = -player.direction * player.gravDir * MathHelper.PiOver4;
		}
		*/

		/*
		public sealed override void HoldItem(Player player)
		{
			var shaman = player.GetOrchidPlayer();
			var catalystType = ModContent.ProjectileType<CatalystAnchor>();

			if (player.ownedProjectileCounts[catalystType] == 0)
			{
				var index = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, catalystType, 0, 0f, player.whoAmI);
				shaman.shamanCatalystIndex = index;

				var proj = Main.projectile[index];
				if (!(proj.modProjectile is CatalystAnchor catalyst))
				{
					proj.Kill();
					shaman.shamanCatalystIndex = -1;
				}
				else catalyst.OnChangeSelectedItem(player);
			}
			else
			{
				var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == catalystType);
				if (proj != null && proj.modProjectile is CatalystAnchor catalyst)
				{
					if (catalyst.SelectedItem != player.selectedItem)
					{
						catalyst.OnChangeSelectedItem(player);
					}
				}
				else shaman.shamanCatalystIndex = -1;
			}

			this.SafeHoldItem();
		}
		*/

		public sealed override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			mult *= player.GetModPlayer<OrchidModPlayer>().shamanDamage;
			this.SafeModifyWeaponDamage(player, ref add, ref mult, ref flat);
		}

		public override void GetWeaponCrit(Player player, ref int crit)
		{
			crit += player.GetModPlayer<OrchidModPlayer>().shamanCrit;
		}

		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if (Main.rand.Next(101) <= player.GetOrchidPlayer().shamanCrit) crit = true;
			else crit = false;
		}

		// ...

		public virtual void SafeSetStaticDefaults() { }
		public virtual void SafeHoldItem() { }
		public virtual void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) { }
		public virtual bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) => true;

		public virtual void ExtraAICatalyst(Projectile projectile, bool after) { }
		public virtual void PostAICatalyst(Projectile projectile) { }
		public virtual void PostDrawCatalyst(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor) { }
		public virtual bool PreAICatalyst(Projectile projectile) { return true; }
		public virtual bool PreDrawCatalyst(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor) { return true; }

		public virtual string ShieldTexture => "OrchidMod/Guardian/ShieldTextures/" + this.Name + "_Shield";

		// ...
	}
}
