using HarmonyLib;
using Outposts;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace VEOutpostAddictionFix
{
    public class VEOutpostAddictionFixMod : Mod
    {
        public VEOutpostAddictionFixMod(ModContentPack pack) : base(pack)
        {
			new Harmony("VEOutpostAddictionFixMod").PatchAll();
        }
    }

    [HarmonyPatch]
    public static class Hediff_Tick_Patch
    {
        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(Hediff), nameof(Hediff.Tick));
            yield return AccessTools.Method(typeof(Hediff), nameof(Hediff.PostTick));
        }
        public static bool Prefix(Hediff __instance)
        {
            if (Outpost_OutpostHealthTick_Patch.tickedPawn == __instance.pawn)
            {
                if (__instance is Hediff_ChemicalDependency || __instance is Hediff_Addiction)
                {
                    return false;
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Outpost), nameof(Outpost.OutpostHealthTick))]
    public static class Outpost_OutpostHealthTick_Patch
    {
        public static Pawn tickedPawn;
        public static void Prefix(Pawn pawn)
        {
            tickedPawn = pawn;
        }

        public static void Postfix()
        {
            tickedPawn = null;
        }
    }
}
