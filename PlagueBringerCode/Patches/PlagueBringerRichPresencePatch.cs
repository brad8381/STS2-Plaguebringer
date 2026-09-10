using PB.Character;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Platform;

namespace PB.Patches;

/// <summary>
/// Steam's IN_RUN Rich Presence template expects a built-in character
/// localization token. Custom character IDs have no Steam-side token and
/// render as a blank status, so use the game's existing LOCKED character
/// token ("???") as a safe fallback while preserving Act/Ascension display.
/// </summary>
[HarmonyPatch(typeof(PlatformUtil), nameof(PlatformUtil.SetRichPresenceValue))]
internal static class PlagueBringerRichPresencePatch
{
    [HarmonyPrefix]
    private static void Prefix(string key, ref string? value)
    {
        if (!string.Equals(key, "Character", StringComparison.Ordinal) || string.IsNullOrEmpty(value))
            return;

        var plagueBringerId = ModelDb.Character<PlagueBringer>().Id.Entry;
        if (string.Equals(value, plagueBringerId, StringComparison.Ordinal))
            value = "LOCKED";
    }
}
