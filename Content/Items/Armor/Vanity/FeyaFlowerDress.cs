using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;

namespace FyrfysMod.Content.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Body)]
    public class FeyaFlowerDress : ModItem
    {
        public override void Load()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}_Female", EquipType.Legs, this, Name + "_Female");
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 32;
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            robes = true;

            equipSlot = EquipLoader.GetEquipSlot(Mod, Name + "_Female", EquipType.Legs);
        }
    }
}
