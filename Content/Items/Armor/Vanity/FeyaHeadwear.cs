using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace FyrfysMod.Content.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class FeyaHeadwear : ModItem
    {
        
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 36;
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
            Item.value = Item.sellPrice(gold: 5);
        }
    }
}