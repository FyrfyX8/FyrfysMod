using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace FyrfysMod.Content.Items.Ammo
{
    public class GravitraxMarble : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 67; // The damage for projectiles isn't actually 67, it actually is the damage combined with the projectile and the item together.
            Item.DamageType = DamageClass.Ranged;
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true; // This marks the item as consumable, making it automatically be consumed when it's used as ammunition, or something else, if possible.
            Item.knockBack = 1.5f;
            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Green;
            // Item.shoot = ModContent.ProjectileType<Projectiles.ExampleBullet>(); // The projectile that weapons fire when using this item as ammunition.
            Item.shootSpeed = 4.5f; // The speed of the projectile. This value equivalent to Silver Bullet since ExampleBullet's Projectile.extraUpdates is 1.
            Item.ammo = Item.type; // The ammo class this ammo belongs to.
        }
    }
}
