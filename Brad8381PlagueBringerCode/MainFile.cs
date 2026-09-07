using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "Brad8381PlagueBringer";
    public const string ResPath = $"res://{ModId}";
    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Logger.Info($"Loading Plaguebringer {typeof(MainFile).Assembly.GetName().Version}; BaseLib {typeof(BaseLib.Abstracts.CustomCardModel).Assembly.GetName().Version}");

        Harmony harmony = new(ModId);
        harmony.PatchAll();
    }
}
