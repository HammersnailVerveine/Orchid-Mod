using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using OrchidMod.Shaman.Projectiles.Thorium;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium.Hardmode
{
	[CrossmodContent("ThoriumMod")]
	public class PharaohScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 35;
			Item.width = 52;
			Item.height = 52;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.knockBack = 3.25f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 12f;
			Item.shoot = ModContent.ProjectileType<PharaohScepterProj>();
			this.empowermentType = 3;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Isis' Command");
			Tooltip.SetDefault("Fires out an ancient magic spell"
							+ "\nHitting may summon a golden miror, replicating your shots"
							+ "\nThe more shamanic bonds you have, the greater the chance to summon a mirror");
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == ModContent.ProjectileType<PharaohScepterPortal>() && proj.owner == player.whoAmI)
				{
					Vector2 target = Main.MouseWorld;
					Vector2 newVelocity = target - proj.position;
					newVelocity.Normalize();
					newVelocity *= velocity.Length();
					newVelocity.Y += Main.rand.Next(-40, 41) * 0.02f;
					int typeAlt = ModContent.ProjectileType<PharaohScepterProjAlt>();
					this.NewShamanProjectile(player, source, proj.Center, newVelocity, type, damage, knockback);
				}
			}

			return true;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 2);
				recipe.AddIngredient(thoriumMod, "PharaohsBreath", 8);
				recipe.Register();
			}
		}
	}
}

