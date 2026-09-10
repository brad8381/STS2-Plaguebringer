using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PB.Powers;

public sealed class LastPatientPower : PlagueBringerPower
{
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Last Patient",
            "At the start of your turn, if no enemy has Plague, apply Last Patient Plague to an enemy.",
            "At the start of your turn, if no enemy has Plague, apply Last Patient Plague to an enemy."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner) || !Owner.IsAlive) return;
        var enemies = combatState.GetOpponentsOf(Owner).Where(enemy => enemy.IsAlive).ToArray();
        if (enemies.Length == 0 || enemies.Any(enemy => (enemy.GetPower<PlaguePower>()?.Amount ?? 0) > 0)) return;

        Flash();
        var choiceContext = new ThrowingPlayerChoiceContext();
        await PowerCmd.Apply<PlaguePower>(choiceContext, enemies[0], Amount, Owner, null);
    }
}
