using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

public sealed class MiasmaPower : PlagueBringerPower
{
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Miasma",
            "At the start of your turn, apply {Amount} Plague to ALL enemies.",
            "At the start of your turn, apply {Amount} Plague to ALL enemies."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner) || !Owner.IsAlive) return;

        var ownerCombatState = Owner.CombatState;
        if (ownerCombatState == null) return;

        var enemies = ownerCombatState.HittableEnemies.Where(enemy => enemy.IsAlive).ToArray();
        if (enemies.Length == 0) return;

        Flash();
        await PowerCmd.Apply<PlaguePower>(
            new ThrowingPlayerChoiceContext(),
            enemies,
            Amount,
            Owner,
            null);
    }
}
