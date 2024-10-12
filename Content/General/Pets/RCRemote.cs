using Microsoft.Xna.Framework;
using OrchidMod.Assets;
using OrchidMod.Common.ModObjects;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.General.Pets
{
	public class RCRemote : ModItem
	{
		public override string Texture => OrchidAssets.ItemsPath + Name;

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.ZephyrFish);

			Item.shoot = ModContent.ProjectileType<RCRemoteProjectile>();
			Item.buffType = ModContent.BuffType<RCRemoteBuff>();
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("RC Remote");
			// Tooltip.SetDefault("'Highly advanced elven technology enabling control over new, recently developed non-reindeer aircraft'");
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}

	public class RCRemoteBuff : ModBuff
	{
		public override string Texture => OrchidAssets.BuffsPath + Name;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Conspicuous Helicopter");
			// Description.SetDefault("'That whirring is getting repetitive...'");

			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<OrchidPlayer>().remoteCopterPet = true;

			var projType = ModContent.ProjectileType<RCRemoteProjectile>();
			var petProjectileNotSpawned = player.ownedProjectileCounts[projType] <= 0;

			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, projType, 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}

	public class RCRemoteProjectile : ModProjectile
	{
		public override string Texture => OrchidAssets.ProjectilesPath + Name;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("RC Copter");

			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			AIType = ProjectileID.ZephyrFish;
		}

		public override bool PreAI()
		{
			Main.player[Projectile.owner].zephyrfish = false;
			return true;
		}

		public override void AI()
		{
			var player = Main.player[Projectile.owner];
			var modPlayer = player.GetModPlayer<OrchidPlayer>();

			if (player.dead) modPlayer.remoteCopterPet = false;
			if (modPlayer.remoteCopterPet) Projectile.timeLeft = 2;
		}
	}
}