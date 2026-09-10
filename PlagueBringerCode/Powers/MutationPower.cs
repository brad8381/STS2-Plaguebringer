using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PB.Powers;

public sealed class MutationPower : PlagueBringerPower
{
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Mutation",
            "At the start of your turn, increase Plague on ALL enemies by Mutation percent, rounded up.",
            "At the start of your turn, increase Plague on ALL enemies by Mutation percent, rounded up."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner) || !Owner.IsAlive) return;
        Flash();
        var choiceContext = new ThrowingPlayerChoiceContext();
        foreach (var enemy in combatState.GetOpponentsOf(Owner).Where(enemy => enemy.IsAlive).ToArray())
        {
            var plague = enemy.GetPower<PlaguePower>();
            if (plague == null || plague.Amount <= 0) continue;
            var increase = Math.Ceiling(plague.Amount * Amount / 100m);
            if (increase > 0)
                await PowerCmd.ModifyAmount(choiceContext, plague, increase, Owner, null);
        }
    }
}
