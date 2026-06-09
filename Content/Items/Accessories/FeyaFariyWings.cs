using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace FyrfysMod.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class FeyaFariyWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // Fly time: 150 ticks = 2.5 seconds
            // Fly speed: 7
            // Acceleration multiplier: 1.5
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(150, 7f, 1.5f);
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 28;
            Item.rare = ItemRarityID.Cyan;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 8);
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f; // Falling glide speed
            ascentWhenRising = 0.15f; // Rising speed
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 1.5f;
            constantAscend = 0.135f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ModContent.ItemType<FeyaFariyWings_Inactive>())
                .AddIngredient(ItemID.HallowedBar, 12)
                .AddIngredient(ItemID.SoulofFlight, 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
