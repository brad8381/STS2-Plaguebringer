using BaseLib.Abstracts;
using PB.Mechanics;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace PB.Relics;

public sealed class EmptyAmpoule : PlagueBringerRelic
{
    private bool _gainedFromExhaustThisTurn;

    public override List<(string, string)>? Localization =>
        new RelicLoc(
            "Empty Ampoule",
            "At the start of combat, gain 1 Specimen. The first time each turn you Exhaust a card, gain 1 Specimen.",
            "Empty does not mean clean."
        );

    public override RelicRarity Rarity => RelicRarity.Rare;

    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player != Owner || Owner.PlayerCombatState is not { TurnNumber: 1 })
            return;

        Flash();
        await SpecimenActions.Gain(choiceContext, Owner.Creature, 1, null);
    }

    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side == Owner.Creature.Side && participants.Contains(Owner.Creature))
            _gainedFromExhaustThisTurn = false;

        return Task.CompletedTask;
    }

    public override async Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool causedByEthereal)
    {
        if (_gainedFromExhaustThisTurn || card.Owner.Creature != Owner.Creature)
            return;

        _gainedFromExhaustThisTurn = true;
        Flash();
        await SpecimenActions.Gain(choiceContext, Owner.Creature, 1, null);
    }
}
