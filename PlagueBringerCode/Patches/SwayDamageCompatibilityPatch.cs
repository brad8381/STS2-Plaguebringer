using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using PB.Powers;

namespace PB.Patches;

[HarmonyPatch]
internal static class SwayDamageCompatibilityPatch
{
    private static IEnumerable<MethodBase> TargetMethods()
    {
        return typeof(PowerModel)
            .GetMethods(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic)
            .Where(method =>
                method.Name == "ModifyDamageMultiplicative");
    }

    private static void Postfix(
        PowerModel __instance,
        object[] __args,
        ref decimal __result)
    {
        if (__instance is not SwayPower sway)
            return;

        if (__args.Length < 4)
            return;

        if (__args[2] is not ValueProp props)
            return;

        var dealer = __args[3] as Creature;

        if (dealer != sway.Owner || !props.IsPoweredAttack())
        {
            __result = 1m;
            return;
        }

        var effectiveStacks = Math.Min(
            sway.Amount,
            SwayPower.MaxReductionStacks);

        __result = 1m - (0.12m * effectiveStacks);
    }
}