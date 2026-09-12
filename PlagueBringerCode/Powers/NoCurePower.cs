using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace PB.Powers;

public sealed class NoCurePower : PlagueBringerPower
{
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "No Cure",
            "Whenever an enemy's Plague grows, add an additional percentage of its current Plague, rounded up.",
            "Whenever an enemy's Plague grows, add an additional [gold]{Amount}%[/gold] of its current Plague, rounded up."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}
