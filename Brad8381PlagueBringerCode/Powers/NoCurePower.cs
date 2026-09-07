using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

public sealed class NoCurePower : PlagueBringerPower
{
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "No Cure",
            "Whenever an enemy's Plague grows, it grows by {Amount} additional Plague.",
            "Whenever an enemy's Plague grows, it grows by {Amount} additional Plague."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}
