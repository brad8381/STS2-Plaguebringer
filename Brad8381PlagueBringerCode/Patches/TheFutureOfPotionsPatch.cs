using System.Collections;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Models.Events;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Patches;

/// <summary>
/// The Future of Potions asks for three upgraded cards of one exact rarity/type.
/// Keep the patch independent of PotionModel's namespace/type location on the beta
/// branch by replacing the event's private dictionary through reflection.
/// </summary>
[HarmonyPatch(typeof(TheFutureOfPotions), "GenerateInitialOptions")]
internal static class TheFutureOfPotionsPatch
{
    private const int RewardSize = 3;
    private const int PreferredMinimum = RewardSize + 1;

    [HarmonyPrefix]
    private static void Prefix(TheFutureOfPotions __instance)
    {
        var owner = __instance.Owner;
        if (owner == null || owner.Character.CardPool is not PlagueBringerCardPool)
            return;

        var cardTypesField = AccessTools.Field(typeof(TheFutureOfPotions), "_cardTypes");
        if (cardTypesField == null)
        {
            MainFile.Logger.Error("Future of Potions patch could not find _cardTypes field.");
            return;
        }

        if (Activator.CreateInstance(cardTypesField.FieldType) is not IDictionary map)
        {
            MainFile.Logger.Error($"Future of Potions _cardTypes field is not an IDictionary: {cardTypesField.FieldType.FullName}");
            return;
        }

        var cards = owner.Character.CardPool.AllCards.ToArray();

        foreach (var potion in owner.Potions)
        {
            var rarity = ToCardRarity(potion.Rarity);
            var allowedTypes = potion.Rarity is PotionRarity.Common or PotionRarity.Token
                ? new[] { CardType.Attack, CardType.Skill }
                : new[] { CardType.Attack, CardType.Skill, CardType.Power };

            var counts = allowedTypes.ToDictionary(
                type => type,
                type => cards.Count(card => card.Rarity == rarity && card.Type == type));

            var validTypes = allowedTypes
                .Where(type => counts[type] >= PreferredMinimum)
                .ToList();

            if (validTypes.Count == 0)
                validTypes = allowedTypes.Where(type => counts[type] >= RewardSize).ToList();

            if (validTypes.Count == 0)
            {
                var bestCount = allowedTypes.Max(type => counts[type]);
                validTypes = allowedTypes.Where(type => counts[type] == bestCount).ToList();
            }

            var selectedType = __instance.Rng.NextItem(validTypes);
            map[potion] = selectedType;

            MainFile.Logger.Info(
                $"Future of Potions safe mapping: {potion.Id.Entry} -> {rarity} {selectedType} " +
                $"({counts[selectedType]} candidates)");
        }

        cardTypesField.SetValue(__instance, map);
    }

    private static CardRarity ToCardRarity(PotionRarity rarity) => rarity switch
    {
        PotionRarity.Rare or PotionRarity.Event => CardRarity.Rare,
        PotionRarity.Uncommon => CardRarity.Uncommon,
        PotionRarity.Common or PotionRarity.Token => CardRarity.Common,
        _ => CardRarity.Common
    };
}
