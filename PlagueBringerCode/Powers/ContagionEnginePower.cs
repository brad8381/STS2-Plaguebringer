using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PB.Powers;

public sealed class ContagionEnginePower : PlagueBringerPower
{
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Contagion Engine",
            "At the start of your turn, apply Plague equal to Contagion Engine to ALL enemies.",
            "At the start of your turn, apply [gold]{Amount} Plague[/gold] to ALL enemies."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner) || !Owner.IsAlive) return;

        Flash();
        var choiceContext = new ThrowingPlayerChoiceContext();

        foreach (var enemy in combatState.GetOpponentsOf(Owner).Where(enemy => enemy.IsAlive).ToArray())
            await PB.Mechanics.PlagueActions.Apply(choiceContext, enemy, Amount, Owner, null);
    }
}
