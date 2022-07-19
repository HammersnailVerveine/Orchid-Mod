using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Chips
{
	public class MeteorDetonator : OrchidModGamblerChipItem
	{

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.width = 24;
			Item.height = 36;
			Item.useStyle = 4;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 90;
			Item.useTime = 90;
			Item.knockBack = 1f;
			Item.damage = 30;
			Item.rare = ItemRarityID.Blue;
			Item.shootSpeed = 0f;
			Item.shoot = ProjectileType<Gambler.Projectiles.Chips.MeteorDetonatorProj>();
			Item.autoReuse = true;
			this.chipCost = 3;
			this.consumeChance = 100;
			this.pauseRotation = false;
		}


		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockBack, OrchidGambler modPlayer, float speed)
		{
			Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockBack, player.whoAmI);
			SoundEngine.PlaySound(SoundID.Item14, position);
			return false;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor Detonator");
			Tooltip.SetDefault("Timed explosions will deal double damage in a larger radius");
		}
		
		public sealed override void SafeHoldItem(Player player, OrchidGambler modPlayer)
		{
			modPlayer.gamblerUIChipSpinDisplay = false;
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			float index = 720f / 8;
			bool timed = (modPlayer.gamblerChipSpin > index * 4 && modPlayer.gamblerChipSpin < index * 5);
			damage *= (timed ? 5 : 2.5f);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.MeteoriteBar, 15);
			recipe.Register();
		}
	}
}
