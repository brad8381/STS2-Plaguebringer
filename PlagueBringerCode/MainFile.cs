using System.Reflection;
using PB.Cards;
using PB.Potions;
using PB.Relics;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace PB;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    // Short content/model ID prefix used by BaseLib, e.g. PB-AIRBORNE_CULTURES.
    public const string ModId = "PB";

    // Keep the Godot resource root tied to the actual packed folder name.
    public const string ResPath = "res://PlagueBringer";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    private static Harmony? _harmony;
    private static bool _contentRegistered;

    public static void Initialize()
    {
        Logger.Info($"Loading Plaguebringer {typeof(MainFile).Assembly.GetName().Version}; BaseLib {typeof(BaseLib.Abstracts.CustomCardModel).Assembly.GetName().Version}");
        Logger.Info($"STS2 API detected: {(PB.Compatibility.GameCompat.IsBetaApi ? "Public Beta" : "Main")}");
        
        RegisterContentModels();

        _harmony ??= new Harmony(ModId);
        _harmony.PatchAll(typeof(MainFile).Assembly);
    }

    /// <summary>
    /// Explicitly construct each Plaguebringer card, relic and potion model once so
    /// BaseLib can register its type with the correct custom pool before ModelDb is
    /// finalised. BaseLib de-duplicates registrations, so this is safe even if another
    /// loader path has already constructed a model.
    /// </summary>
    private static void RegisterContentModels()
    {
        if (_contentRegistered)
            return;

        _contentRegistered = true;

        var assembly = typeof(MainFile).Assembly;
        var modelTypes = assembly.GetTypes()
            .Where(type =>
                !type.IsAbstract &&
                !type.IsGenericTypeDefinition &&
                type.GetConstructor(Type.EmptyTypes) != null &&
                (typeof(PlagueBringerCard).IsAssignableFrom(type) ||
                 typeof(PlagueBringerRelic).IsAssignableFrom(type) ||
                 typeof(PlagueBringerPotion).IsAssignableFrom(type)))
            .OrderBy(type => type.FullName)
            .ToArray();

        var registered = 0;

        foreach (var type in modelTypes)
        {
            try
            {
                Activator.CreateInstance(type);
                registered++;
                Logger.Info($"Registered content model: {type.Name}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to register content model {type.FullName}: {ex}");
            }
        }

        Logger.Info($"Plaguebringer content bootstrap constructed {registered}/{modelTypes.Length} card/relic/potion models.");
    }
}
