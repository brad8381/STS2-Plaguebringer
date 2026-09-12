using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PB.Powers;

public sealed class SwayPower : PlagueBringerPower
{
    public const int MaxStacks = 5;
    public const int MaxReductionStacks = 5;

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Sway",
            "Each Sway reduces Attack damage by 10%. Lose 1 Sway after this creature's turn. Max 5.",
            "[red]Attack damage -{Amount:choose(1|2|3|4|5):10|20|30|40|50|50}%[/red] ([red]10% per Sway[/red]). Lose [gold]1 Sway[/gold] after this creature's turn. Max [gold]5[/gold]."
        );

    public override string CustomPackedIconPath =>
        "res://PlagueBringer/images/powers/sway_power.png";

    public override string CustomBigIconPath =>
        "res://PlagueBringer/images/powers/big/sway_power.png";

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != Owner.Side || !participants.Contains(Owner))
            return;

        await PowerCmd.TickDownDuration(this);
    }
}
