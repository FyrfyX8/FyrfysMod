using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using GlowmaskHelper.Content;

namespace FyrfysMod.Content.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Legs)]
    [AutoloadGlowmask]
    public class FyrfyX8Leggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 14;
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
            Item.value = Item.sellPrice(gold: 5);
        }
    }
}