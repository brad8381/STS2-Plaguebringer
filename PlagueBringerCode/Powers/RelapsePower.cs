using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PB.Powers;

public sealed class RelapsePower : PlagueBringerPower
{
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Relapse",
            "At the start of your turn, trigger Plague on ALL enemies once for each stack of Relapse.",
            "At the start of your turn, trigger Plague on ALL enemies once for each stack of Relapse."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner) || !Owner.IsAlive || Amount <= 0) return;

        var ownerCombatState = Owner.CombatState;
        if (ownerCombatState == null) return;

        Flash();
        var choiceContext = new ThrowingPlayerChoiceContext();

        for (var trigger = 0; trigger < Amount; trigger++)
        {
            foreach (var enemy in ownerCombatState.HittableEnemies.Where(enemy => enemy.IsAlive).ToArray())
            {
                var plague = enemy.GetPower<PlaguePower>();
                if (plague != null && plague.Amount > 0)
                    await plague.TriggerPlague(choiceContext);
            }
        }
    }
}
