using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Models.Events;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Patches;

/// <summary>
/// The Future of Potions asks for three upgraded cards of one exact rarity/type.
/// Small custom pools can have too few matching cards, which leaves the reward screen
/// with nothing usable. For the Plaguebringer, only let the event choose card types
/// that have enough cards available.
/// </summary>
[HarmonyPatch(typeof(TheFutureOfPotions), "GenerateInitialOptions")]
internal static class TheFutureOfPotionsPatch
{
    private const int RewardSize = 3;
    private const int PreferredMinimum = RewardSize + 1;

    [HarmonyPrefix]
    private static void Prefix(
        TheFutureOfPotions __instance,
        ref Dictionary<PotionModel, CardType>? ____cardTypes)
    {
        var owner = __instance.Owner;
        if (owner?.Character is not PlagueBringer || ____cardTypes != null)
            return;

        var cards = owner.Character.CardPool.AllCards.ToArray();
        var map = new Dictionary<PotionModel, CardType>();

        foreach (var potion in owner.Potions)
        {
            var rarity = ToCardRarity(potion.Rarity);
            var allowedTypes = potion.Rarity is PotionRarity.Common or PotionRarity.Token
                ? new[] { CardType.Attack, CardType.Skill }
                : new[] { CardType.Attack, CardType.Skill, CardType.Power };

            var counts = allowedTypes.ToDictionary(
                type => type,
                type => cards.Count(card => card.Rarity == rarity && card.Type == type));

            // Prefer a little headroom above the event's three-card reward. This avoids
            // exact-boundary failures if another game filter removes one candidate later.
            var validTypes = allowedTypes
                .Where(type => counts[type] >= PreferredMinimum)
                .ToList();

            if (validTypes.Count == 0)
            {
                validTypes = allowedTypes
                    .Where(type => counts[type] >= RewardSize)
                    .ToList();
            }

            if (validTypes.Count == 0)
            {
                // Last-resort fallback: choose the type with the largest available pool.
                // This keeps the event deterministic and avoids leaving a potion unmapped.
                var bestCount = allowedTypes.Max(type => counts[type]);
                validTypes = allowedTypes.Where(type => counts[type] == bestCount).ToList();
            }

            var selectedType = __instance.Rng.NextItem(validTypes);
            map[potion] = selectedType;

            MainFile.Logger.Info(
                $"Future of Potions: {potion.Id.Entry} -> {rarity} {selectedType} " +
                $"({counts[selectedType]} candidates)");
        }

        ____cardTypes = map;
    }

    private static CardRarity ToCardRarity(PotionRarity rarity) => rarity switch
    {
        PotionRarity.Rare or PotionRarity.Event => CardRarity.Rare,
        PotionRarity.Uncommon => CardRarity.Uncommon,
        PotionRarity.Common or PotionRarity.Token => CardRarity.Common,
        _ => CardRarity.Common
    };
}
