using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

public sealed class IsolationWardPower : PlagueBringerPower
{
    private bool _triggeredThisTurn;

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Isolation Ward",
            "The first time each turn you apply Plague to an enemy, gain {Amount} Block.",
            "The first time each turn you apply Plague to an enemy, gain {Amount} Block."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (participants.Contains(Owner))
            _triggeredThisTurn = false;
        return Task.CompletedTask;
    }

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (_triggeredThisTurn || amount <= 0 || applier != Owner || power is not PlaguePower)
            return;
        if (power.Owner.Side == Owner.Side)
            return;

        _triggeredThisTurn = true;
        Flash();
        await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
    }
}
