using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PB.Powers;

public sealed class SepticReservePower : PlagueBringerPower
{
    private bool _usedThisTurn;

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Septic Reserve",
            "The first time each turn you remove Plague, gain Energy equal to Septic Reserve.",
            "The first time each turn you remove Plague, gain [gold]{Amount} Energy[/gold]."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side == Owner.Side && participants.Contains(Owner))
            _usedThisTurn = false;

        return Task.CompletedTask;
    }

    public async Task OnPlagueRemoved(PlayerChoiceContext choiceContext)
    {
        if (_usedThisTurn || Amount <= 0 || Owner.Player == null)
            return;

        _usedThisTurn = true;
        Flash();
        await PlayerCmd.GainEnergy(Amount, Owner.Player);
    }
}
