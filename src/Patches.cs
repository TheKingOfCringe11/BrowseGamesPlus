using DuckGame;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using static System.Reflection.Emit.OpCodes;

namespace BrowseGamesPlus
{
    internal sealed class Patches
    {
        [HarmonyPatch(typeof(TitleScreen), "Initialize")]
        private static class TitleScreenPatched
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return instructions.InsertInstructions(Stfld, AccessTools.Field(typeof(TitleScreen), "_cloudConfigMenu"), Ldarg_0, Newobj, new List<CodeInstruction>
                {
                    new CodeInstruction(Ldarg_0),
                    new CodeInstruction(Ldfld, AccessTools.Field(typeof(TitleScreen), "_optionsMenu")),
                    new CodeInstruction(Ldarg_0),
                    new CodeInstruction(Ldfld, AccessTools.Field(typeof(TitleScreen), "_optionsGroup")),
                    new CodeInstruction(Call, AccessTools.Method(typeof(OptionsMenu), "AddToOptionsMenu"))
                });
            }
        }

        [HarmonyPatch(typeof(UIServerBrowser), "Draw")]
        private static class UIServerBrowserDrawPatched
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var code = new List<CodeInstruction>(instructions);

                LocalBuilder lobby = code.GetLocalBuilder(Ldloc_S, typeof(UIServerBrowser.LobbyData), Brfalse, Stloc_S);

                LocalBuilder x = code.GetLocalBuilder(Ldloc_S, typeof(float), Ldloc_S, Stloc_S);
                LocalBuilder y = code.GetLocalBuilder(Ldloc_S, typeof(float), Newobj, Ldloc_S);

                code = instructions.InsertInstructions(Ldfld, AccessTools.Field(typeof(UIServerBrowser), "_noImage"), Ldarg_0, Ldarg_0, new List<CodeInstruction>
                {
                    new CodeInstruction(Ldloc, lobby),
                    new CodeInstruction(Ldloc, x),
                    new CodeInstruction(Ldloc, y),

                    new CodeInstruction(Call, AccessTools.Method(typeof(Visuals), "Draw"))
                }).ToList();

                int insertionInsex = 0;

                if (code.TryGetIndex(ref insertionInsex, Callvirt, AccessTools.Method(typeof(List<UIServerBrowser.LobbyData>), "Sort"), Ldc_I4_0, Ldfld))
                    code[insertionInsex] = new CodeInstruction(Call, AccessTools.Method(typeof(Utilities), "SortLobbies"));

                return code;
            }
        }

        [HarmonyPatch(typeof(UIServerBrowser), "UpdateLobbySearch")]
        private static class UIServerBrowserUpdateLobbySearchPatched
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                LocalBuilder lobby = instructions.GetLocalBuilder(Ldloc_S, typeof(UIServerBrowser.LobbyData), Ldc_I4_0, Pop);

                return instructions.InsertInstructions(Callvirt, AccessTools.Method(typeof(List<UIServerBrowser.LobbyData>), "Add"), Ldloc_S, Ldloc_S, new List<CodeInstruction>
                {
                    new CodeInstruction(Ldloc, lobby),
                    new CodeInstruction(Call, AccessTools.Method(typeof(Utilities), "InitializeLobbyUserData"))
                });
            }
        }

        [HarmonyPatch(typeof(UIServerBrowser.LobbyData), "userCount", MethodType.Getter)]
        private static class LobbyDataPatched
        {
            private static bool Prefix(UIServerBrowser.LobbyData __instance, ref int __result, ref int ____userCount)
            {
                Lobby lobby = __instance.lobby;

                if (lobby is null)
                {
                    __result = ____userCount;
                    return false;
                }

                __result = lobby.users.Where(user => user != Steam.user).Count();

                return false;
            }
        }
    }
}
