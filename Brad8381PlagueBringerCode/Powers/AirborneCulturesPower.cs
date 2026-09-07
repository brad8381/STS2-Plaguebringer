using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

public sealed class AirborneCulturesPower : PlagueBringerPower
{
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Airborne Cultures",
            "At the end of your turn, apply Plague equal to Airborne Cultures to ALL enemies.",
            "At the end of your turn, apply Plague equal to Airborne Cultures to ALL enemies."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side || Owner.CombatState == null) return;
        Flash();
        foreach (var enemy in Owner.CombatState.GetOpponentsOf(Owner).Where(enemy => enemy.IsAlive).ToArray())
            await PowerCmd.Apply<PlaguePower>(choiceContext, enemy, Amount, Owner, null);
    }
}
