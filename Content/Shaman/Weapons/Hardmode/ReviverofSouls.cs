using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Hardmode
{
	public class ReviverofSouls : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 61;
			Item.channel = true;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.knockBack = 4.15f;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 6, 0, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			//Item.shoot = ModContent.ProjectileType<ReviverofSoulsProj>();
			this.Element = ShamanElement.AIR;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Reviver of Souls");
			/* Tooltip.SetDefault("Successful hits summon spirit flames, increasing weapon damage"
							  + "\nCompleting the flame circle empowers you, further boosting damage"
							  + "\nWhile empowered, hits will reset every active empowerment duration"); */
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void SafeModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			/*
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			if (modPlayer.shamanOrbCircle == ShamanOrbCircle.REVIVER)
				damage += modPlayer.orbCountCircle * 0.035f;
			*/
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 3; i++)
				this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			return false;
		}

		/*
		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.SpectreBar, 20)
			.AddTile(TileID.MythrilAnvil)
			.Register();
		*/
	}
}
