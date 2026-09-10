using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PB.Powers;

public sealed class CrossImmunityPower : PlagueBringerPower
{
    private int _usedThisTurn;

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Cross Immunity",
            "The first time you apply Plague each turn, apply 1 Sway to that enemy.",
            "The first 2 times you apply Plague each turn, apply 1 Sway to that enemy."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public bool CanTrigger => _usedThisTurn < (int)Amount;

    public void ConsumeTrigger()
    {
        _usedThisTurn++;
    }

    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side == Owner.Side)
            _usedThisTurn = 0;

        return Task.CompletedTask;
    }
}
