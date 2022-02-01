using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace BrowseGamesPlus
{
    static class Extensions
    {
        public static IEnumerable<CodeInstruction> InsertInstructions(this IEnumerable<CodeInstruction> instructions, OpCode opCode, object operand, OpCode next, OpCode previous, List<CodeInstruction> instructionsToInsert)
        {
            List<CodeInstruction> code = instructions.ToList();
            int insertionIndex = 0;

            if (instructions.TryGetIndex(ref insertionIndex, opCode, operand, next, previous))
                code.InsertRange(insertionIndex, instructionsToInsert);

            return code;
        }

        public static bool TryGetIndex(this IEnumerable<CodeInstruction> instructions, ref int index, OpCode opCode, object operand, OpCode next, OpCode previous)
        {
            foreach (CodeInstruction instruction in instructions.ByOpCodes(opCode, next, previous))
            {
                if (instruction.operand == operand)
                {
                    index = instructions.ToList().IndexOf(instruction);
                    return true;
                }
            }

            return false;
        }

        public static LocalBuilder GetLocalBuilder(this IEnumerable<CodeInstruction> instructions, OpCode opCode, Type type, OpCode next, OpCode previous)
        {
            foreach (CodeInstruction instruction in instructions.ByOpCodes(opCode, next, previous))
                if (instruction.operand is LocalBuilder builder && builder.LocalType == type)
                    return builder;

            return null;
        }

        private static IEnumerable<CodeInstruction> ByOpCodes(this IEnumerable<CodeInstruction> instructions, OpCode opCode, OpCode next, OpCode previous)
        {
            for (int i = 0; i < instructions.Count(); i++)
                if (instructions.ElementAt(i).opcode == opCode && (i >= instructions.Count() - 1 || instructions.ElementAt(i + 1).opcode == next) && (i < 1 || instructions.ElementAt(i - 1).opcode == previous))
                    yield return instructions.ElementAt(i);
        }

        public static string LimitLength(this string line, int maxLength)
        {
            if (line.Length > maxLength)
                return line.Substring(0, maxLength);

            return line;
        }

        public static int GetNormalMapsCount(this Lobby lobby)
        {
            return int.Parse(lobby.GetLobbyData("normalmaps"));
        }

        public static int GetRandomMapsCount(this Lobby lobby)
        {
            return int.Parse(lobby.GetLobbyData("randommaps"));
        }

        public static int GetCustomMapsCount(this Lobby lobby)
        {
            return int.Parse(lobby.GetLobbyData("custommaps"));
        }

        public static int GetInternetMapsCount(this Lobby lobby)
        {
            return 100 - lobby.GetNormalMapsCount() - lobby.GetRandomMapsCount() - lobby.GetCustomMapsCount();
        }
    }
}
