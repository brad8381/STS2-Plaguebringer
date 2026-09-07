using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Relics;

public sealed class RustedNeedle : PlagueBringerRelic
{
    private bool _usedThisTurn;

    public override List<(string, string)>? Localization =>
        new RelicLoc(
            "Rusted Needle",
            "The first time each turn you deal unblocked Attack damage, apply 1 [gold]Plague[/gold] to the target.",
            "Single use was always a suggestion."
        );

    public override RelicRarity Rarity => RelicRarity.Common;

    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side == Owner.Creature.Side && participants.Contains(Owner.Creature))
            _usedThisTurn = false;

        return Task.CompletedTask;
    }

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource)
    {
        if (_usedThisTurn || dealer != Owner.Creature || !props.IsPoweredAttack() || result.UnblockedDamage <= 0 || !target.IsAlive)
            return;

        _usedThisTurn = true;
        Flash();
        await PowerCmd.Apply<PlaguePower>(choiceContext, target, 1, Owner.Creature, cardSource);
    }
}
