using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using GlowmaskHelper.Content;
using Microsoft.Xna.Framework;

namespace FyrfysMod.Content.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Body)]
    [AutoloadGlowmask]
    public class FyrfyX8Chestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            //GlowmaskLoader.AssignGlowmaskTexture_Equip_Arms(Item.glowMask,EquipLoader.GetEquipSlot(Mod,Name,EquipType.Body));
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
            Item.value = Item.sellPrice(gold: 5);
        }
    }
}
