using FyrfysMod.Content.NPCs;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FyrfysMod.Common.Systems
{
    public class TownNPCRespawnSystem : ModSystem
    {
        // Tracks if Gravitraxer has ever been spawned in this world
        public static bool unlockedGravitraxerSpawn = false;

        public override void ClearWorld()
        {
            unlockedGravitraxerSpawn = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag[nameof(unlockedGravitraxerSpawn)] = unlockedGravitraxerSpawn;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            unlockedGravitraxerSpawn = tag.GetBool(nameof(unlockedGravitraxerSpawn));

            // This line sets unlockedGravitraxerSpawn to true if a Gravitraxer is already in the world. This is only needed because unlockedGravitraxerSpawn was added in an update to this mod, meaning that existing users might have unlockedGravitraxerSpawn incorrectly set to false.
            // If you are tracking Town NPC unlocks from your initial mod release, then this isn't necessary.
            unlockedGravitraxerSpawn |= NPC.AnyNPCs(ModContent.NPCType<Gravitraxer>());
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.WriteFlags(unlockedGravitraxerSpawn);
        }

        public override void NetReceive(BinaryReader reader)
        {
            reader.ReadFlags(out unlockedGravitraxerSpawn);
        }
    }
}