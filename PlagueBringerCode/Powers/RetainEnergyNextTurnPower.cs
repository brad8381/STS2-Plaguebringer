using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace PB.Powers;

public sealed class RetainEnergyNextTurnPower : PlagueBringerPower
{
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Lingering Energy",
            "Energy is not reset at the start of your next turn.",
            "[gold]Energy is not reset[/gold] at the start of your next turn."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override bool ShouldPlayerResetEnergy(Player player)
    {
        return Owner.Player == null || player != Owner.Player || Amount <= 0;
    }

    public override async Task AfterEnergyReset(Player player)
    {
        if (Owner.Player == null || player != Owner.Player || Amount <= 0)
            return;

        Flash();
        await PowerCmd.Remove(this);
    }
}
