using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

public sealed class RelapsePower : PlagueBringerPower
{
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Relapse",
            "At the start of your turn, trigger Plague on ALL enemies.",
            "At the start of your turn, trigger Plague on ALL enemies."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner) || !Owner.IsAlive) return;

        Flash();
        var choiceContext = new ThrowingPlayerChoiceContext();
        foreach (var enemy in Owner.CombatState.HittableEnemies.Where(enemy => enemy.IsAlive).ToArray())
        {
            var plague = enemy.GetPower<PlaguePower>();
            if (plague != null && plague.Amount > 0)
                await plague.TriggerPlague(choiceContext);
        }
    }
}
