using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace FyrfysMod.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Back)]
    public class FyrfyX8Tail : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // Item.CloneDefaults(ItemID.FoxTail);
            Item.width = 32;
            Item.height = 22;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(gold: 5);
            Item.accessory = true;
            Item.vanity = true;

            // Add this accessory to the tail draw layer

            int equipSlot = EquipLoader.GetEquipSlot(Mod, "FyrfyX8Tail", EquipType.Back);
            ArmorIDs.Back.Sets.DrawInTailLayer[equipSlot] = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (hideVisual) return;

            
        }
    }
}