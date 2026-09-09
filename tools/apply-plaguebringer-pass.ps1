cd C:\Users\brada\GitHub\STS2-Plaguebringer

New-Item -ItemType Directory -Force .\tools | Out-Null

$ErrorActionPreference = "Stop"

$repo  = "C:\Users\brada\GitHub\STS2-Plaguebringer"
$game  = "C:\Program Files (x86)\Steam\steamapps\common\Slay the Spire 2"
$mod   = "$game\mods\Brad8381PlagueBringer"
$godot = "C:\megadot\MegaDot_v4.5.1-stable_mono_win64_console.exe"

Set-Location $repo

function Write-NoBom {
    param(
        [string]$Path,
        [string]$Text
    )

    $utf8 = New-Object System.Text.UTF8Encoding($false)
    [System.IO.File]::WriteAllText($Path, $Text, $utf8)
}

function Backup-File {
    param([string]$Path)

    if (Test-Path $Path) {
        $relative = $Path.Replace($repo, "").TrimStart("\")
        $dest = Join-Path $backup $relative
        New-Item -ItemType Directory -Force (Split-Path $dest) | Out-Null
        Copy-Item $Path $dest -Force
    }
}

if (-not (Get-Command ffmpeg -ErrorAction SilentlyContinue)) {
    throw "ffmpeg is not available in PATH."
}

if (-not (Test-Path $godot)) {
    throw "MegaDot not found: $godot"
}

$stamp = Get-Date -Format "yyyyMMdd-HHmmss"
$backup = "C:\Users\brada\GitHub\STS2-Plaguebringer-backups\ui-mechanics-$stamp"
New-Item -ItemType Directory -Force $backup | Out-Null

Write-Host "Backup: $backup"

# -------------------------------------------------------------------
# BACKUP FILES WE ARE ABOUT TO TOUCH
# -------------------------------------------------------------------

$touchFiles = @(
    "$repo\Brad8381PlagueBringer\images\charui\char_select_char_name.png",
    "$repo\Brad8381PlagueBringer\images\charui\char_select_char_name_locked.png",
    "$repo\Brad8381PlagueBringerCode\Character\PlagueBringer.cs",
    "$repo\Brad8381PlagueBringerCode\Character\PlagueBringerCardPool.cs",
    "$repo\Brad8381PlagueBringerCode\Character\PlagueBringerIdle.cs",
    "$repo\Brad8381PlagueBringerCode\Character\PlagueBringerSelectSway.cs",
    "$repo\Brad8381PlagueBringerCode\Mechanics\SpecimenActions.cs",
    "$repo\Brad8381PlagueBringerCode\Mechanics\SpecimenUiPatch.cs",
    "$repo\Brad8381PlagueBringerCode\Powers\SpecimenPower.cs",
    "$repo\Brad8381PlagueBringerCode\Cards\ConcentratedDose.cs",
    "$repo\Brad8381PlagueBringer\scenes\plaguebringer_character.tscn",
    "$repo\Brad8381PlagueBringer\scenes\plaguebringer_select_bg.tscn",
    "$repo\Brad8381PlagueBringer\scenes\plaguebringer_rest_site.tscn"
)

foreach ($f in $touchFiles) {
    Backup-File $f
}

# -------------------------------------------------------------------
# IMAGE RESIZING
# -------------------------------------------------------------------

$charui = "$repo\Brad8381PlagueBringer\images\charui"

$energySource = "$charui\energy_orb.png"
$specimenSource = "$charui\specimen_counter.png"

if (-not (Test-Path $energySource)) {
    throw "Missing energy source: $energySource"
}

if (-not (Test-Path $specimenSource)) {
    throw "Missing specimen source: $specimenSource"
}

# Card / tooltip energy icon.
# 74 canvas, actual artwork only 58px.
ffmpeg -hide_banner -loglevel error -y `
    -i "$energySource" `
    -vf "scale=58:58:force_original_aspect_ratio=decrease,pad=74:74:(ow-iw)/2:(oh-ih)/2:color=0x00000000,format=rgba" `
    -frames:v 1 `
    "$charui\energy_big.png"

# Tiny inline/card-text energy icon.
# 24 canvas, artwork 18px.
ffmpeg -hide_banner -loglevel error -y `
    -i "$energySource" `
    -vf "scale=18:18:force_original_aspect_ratio=decrease,pad=24:24:(ow-iw)/2:(oh-ih)/2:color=0x00000000,format=rgba" `
    -frames:v 1 `
    "$charui\energy_text.png"

# Actual combat Energy counter.
# Large enough for HUD but with padding.
ffmpeg -hide_banner -loglevel error -y `
    -i "$energySource" `
    -vf "scale=94:94:force_original_aspect_ratio=decrease,pad=128:128:(ow-iw)/2:(oh-ih)/2:color=0x00000000,format=rgba" `
    -frames:v 1 `
    "$charui\energy_counter.png"

# Specimen HUD resource.
ffmpeg -hide_banner -loglevel error -y `
    -i "$specimenSource" `
    -vf "scale=54:54:force_original_aspect_ratio=decrease,pad=76:76:(ow-iw)/2:(oh-ih)/2:color=0x00000000,format=rgba" `
    -frames:v 1 `
    "$charui\specimen_hud.png"

# -------------------------------------------------------------------
# CHARACTER SELECT MINI ICON
# Remove roughly 6% from every edge, then resize/pad.
# This removes a literal outer frame/border.
# -------------------------------------------------------------------

$selectIcon = "$charui\char_select_char_name.png"
$selectLocked = "$charui\char_select_char_name_locked.png"

$tmp = "$charui\char_select_char_name.tmp.png"

ffmpeg -hide_banner -loglevel error -y `
    -i "$selectIcon" `
    -vf "crop=iw*0.88:ih*0.88:iw*0.06:ih*0.06,scale=220:220:force_original_aspect_ratio=decrease,pad=256:256:(ow-iw)/2:(oh-ih)/2:color=0x00000000,format=rgba" `
    -frames:v 1 `
    "$tmp"

Move-Item "$tmp" "$selectIcon" -Force

# Build locked icon from cleaned normal icon.
ffmpeg -hide_banner -loglevel error -y `
    -i "$selectIcon" `
    -vf "hue=s=0,eq=brightness=-0.18,format=rgba" `
    -frames:v 1 `
    "$selectLocked"

# -------------------------------------------------------------------
# PLAGUEBRINGER CARD POOL
# Different files for text energy and tooltip/card energy.
# -------------------------------------------------------------------

$path = "$repo\Brad8381PlagueBringerCode\Character\PlagueBringerCardPool.cs"

$text = @"
using BaseLib.Abstracts;
using Godot;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;

public class PlagueBringerCardPool : CustomCardPoolModel
{
    public override string Title => PlagueBringer.CharacterId;

    public override string BigEnergyIconPath =>
        "res://Brad8381PlagueBringer/images/charui/energy_big.png";

    public override string TextEnergyIconPath =>
        "res://Brad8381PlagueBringer/images/charui/energy_text.png";

    public override float H => 1f;
    public override float S => 0.08f;
    public override float V => 0.3f;

    public override Color DeckEntryCardColor => new("55545a");

    public override bool IsColorless => false;
}
"@

Write-NoBom $path $text

# -------------------------------------------------------------------
# ACTUAL COMBAT ENERGY COUNTER
#
# BaseLib supports a custom NEnergyCounter-compatible scene with
# EnergyVfxBack / Layers / RotationLayers / EnergyVfxFront / Label.
# -------------------------------------------------------------------

$energyScene = "$repo\Brad8381PlagueBringer\scenes\plaguebringer_energy_counter.tscn"

$text = @'
[gd_scene load_steps=2 format=3]

[ext_resource type="Texture2D" path="res://Brad8381PlagueBringer/images/charui/energy_counter.png" id="1_energy"]

[node name="PlagueBringerEnergyCounter" type="Control"]
layout_mode = 3
anchors_preset = 0
offset_right = 128.0
offset_bottom = 128.0

[node name="EnergyVfxBack" type="Node2D" parent="."]
unique_name_in_owner = true
position = Vector2(64, 64)

[node name="Layers" type="Control" parent="."]
unique_name_in_owner = true
layout_mode = 1
anchors_preset = 15
anchor_right = 1.0
anchor_bottom = 1.0
grow_horizontal = 2
grow_vertical = 2

[node name="RotationLayers" type="Control" parent="Layers"]
unique_name_in_owner = true
offset_right = 40.0
offset_bottom = 40.0

[node name="Layer1" type="TextureRect" parent="Layers"]
layout_mode = 1
anchors_preset = 15
anchor_right = 1.0
anchor_bottom = 1.0
grow_horizontal = 2
grow_vertical = 2
texture = ExtResource("1_energy")
expand_mode = 1
stretch_mode = 5
mouse_filter = 2

[node name="EnergyVfxFront" type="Node2D" parent="."]
unique_name_in_owner = true
position = Vector2(64, 64)

[node name="Label" type="Label" parent="."]
layout_mode = 1
anchors_preset = 15
anchor_right = 1.0
anchor_bottom = 1.0
grow_horizontal = 2
grow_vertical = 2
theme_override_colors/font_color = Color(1, 0.965, 0.886, 1)
theme_override_colors/font_outline_color = Color(0.04, 0.10, 0.03, 1)
theme_override_constants/outline_size = 8
theme_override_font_sizes/font_size = 32
text = "3/3"
horizontal_alignment = 1
vertical_alignment = 1
mouse_filter = 2
'@

Write-NoBom $energyScene $text

# Add CustomEnergyCounterPath if not already there.

$characterPath = "$repo\Brad8381PlagueBringerCode\Character\PlagueBringer.cs"
$c = [System.IO.File]::ReadAllText($characterPath)

if ($c -notmatch "CustomEnergyCounterPath") {

    $needle = @'
    public override string CustomMerchantAnimPath =>
        "res://Brad8381PlagueBringer/scenes/plaguebringer_merchant.tscn";
'@

    $replace = @'
    public override string CustomMerchantAnimPath =>
        "res://Brad8381PlagueBringer/scenes/plaguebringer_merchant.tscn";

    public override string CustomEnergyCounterPath =>
        "res://Brad8381PlagueBringer/scenes/plaguebringer_energy_counter.tscn";
'@

    if (-not $c.Contains($needle)) {
        throw "Could not find CustomMerchantAnimPath block in PlagueBringer.cs"
    }

    $c = $c.Replace($needle, $replace)
    Write-NoBom $characterPath $c
}

# -------------------------------------------------------------------
# SPECIMEN POWER
#
# +6 maximum
# can go arbitrarily negative
# survives turns
# negative value becomes a debuff automatically because AllowNegative=true
# negative debt costs HP at combat end
#
# First version floors HP at 1 during teardown.
# -------------------------------------------------------------------

$path = "$repo\Brad8381PlagueBringerCode\Powers\SpecimenPower.cs"

$text = @'
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

public sealed class SpecimenPower : PlagueBringerPower
{
    public const int MaxAmount = 6;

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Specimen",
            "A persistent combat resource. Persists between turns. Maximum +6. It can become negative. At the end of combat, lose HP equal to negative Specimen.",
            "A persistent combat resource. Persists between turns. Maximum +6. It can become negative. At the end of combat, lose HP equal to negative Specimen."
        );

    public override string CustomPackedIconPath =>
        "res://Brad8381PlagueBringer/images/powers/specimen_power.png";

    public override string CustomBigIconPath =>
        "res://Brad8381PlagueBringer/images/powers/big/specimen_power.png";

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool AllowNegative => true;

    protected override bool IsVisibleInternal => false;

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        if (Amount >= 0 || !Owner.IsPlayer)
            return;

        var debt = -Amount;
        var newHp = Math.Max(1, Owner.CurrentHp - debt);

        await CreatureCmd.SetCurrentHp(Owner, newHp);
    }
}
'@

Write-NoBom $path $text

# -------------------------------------------------------------------
# SPECIMEN ACTIONS
# Spending can go below zero.
# Gaining stops at +6.
# -------------------------------------------------------------------

$path = "$repo\Brad8381PlagueBringerCode\Mechanics\SpecimenActions.cs"

$text = @'
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;

public static class SpecimenActions
{
    public static int Count(Creature owner) =>
        owner.GetPower<SpecimenPower>()?.Amount ?? 0;

    public static async Task<int> Gain(
        PlayerChoiceContext choiceContext,
        Creature owner,
        int amount,
        CardModel? source)
    {
        if (amount <= 0)
            return 0;

        var current = Count(owner);
        var gain = Math.Min(amount, SpecimenPower.MaxAmount - current);

        if (gain <= 0)
            return 0;

        await PowerCmd.Apply<SpecimenPower>(
            choiceContext,
            owner,
            gain,
            owner,
            source);

        return gain;
    }

    public static async Task<int> Spend(
        PlayerChoiceContext choiceContext,
        Creature owner,
        int amount,
        CardModel? source)
    {
        if (amount <= 0)
            return 0;

        var power = owner.GetPower<SpecimenPower>();

        if (power == null)
        {
            await PowerCmd.Apply<SpecimenPower>(
                choiceContext,
                owner,
                -amount,
                owner,
                source);

            return amount;
        }

        await PowerCmd.ModifyAmount(
            choiceContext,
            power,
            -amount,
            owner,
            source);

        return amount;
    }
}
'@

Write-NoBom $path $text

# -------------------------------------------------------------------
# SPECIMEN HUD
# -------------------------------------------------------------------

$path = "$repo\Brad8381PlagueBringerCode\Mechanics\SpecimenUiPatch.cs"
$c = [System.IO.File]::ReadAllText($path)

$c = $c.Replace(
    'A persistent combat resource gained mainly from Exhausting cards. Persists between turns, up to 6. Lost at the end of combat.',
    'A persistent combat resource. Persists between turns, up to +6. It can become negative. At the end of combat, lose HP equal to negative Specimen.'
)

$c = $c.Replace(
    'specimen_counter.png',
    'specimen_hud.png'
)

$oldRefresh = 'void Refresh() => label.Text = SpecimenActions.Count(player.Creature).ToString();'

$newRefresh = @'
void Refresh()
            {
                var amount = SpecimenActions.Count(player.Creature);

                label.Text = amount.ToString();

                label.AddThemeColorOverride(
                    "font_color",
                    amount < 0
                        ? new Color("ef6666")
                        : new Color("f2eee4"));
            }
'@

if ($c.Contains($oldRefresh)) {
    $c = $c.Replace($oldRefresh, $newRefresh)
}

Write-NoBom $path $c

# -------------------------------------------------------------------
# CONCENTRATED DOSE
# X ENERGY = X SEPARATE PLAGUE APPLICATIONS.
# -------------------------------------------------------------------

$path = "$repo\Brad8381PlagueBringerCode\Cards\ConcentratedDose.cs"

$text = @'
using BaseLib.Abstracts;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class ConcentratedDose : PlagueBringerCard, IPlagueCard
{
    protected override bool HasEnergyCostX => true;

    public ConcentratedDose() : base(-1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVars(
            new DynamicVar("PlaguePerEnergy", 4),
            new DynamicVar("EnergyRefund", 0));

        WithKeywords(CardKeyword.Exhaust);
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Concentrated Dose",
            "X times: Apply {PlaguePerEnergy:diff()} [gold]Plague[/gold].{IfUpgraded:show:\nGain 1 [gold]Energy[/gold].|}"
        );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        var x = ResolveEnergyXValue();

        if (x <= 0)
            return;

        var plaguePerEnergy =
            DynamicVars["PlaguePerEnergy"].IntValue;

        for (var i = 0; i < x; i++)
        {
            if (!target.IsAlive)
                break;

            await PowerCmd.Apply<PlaguePower>(
                choiceContext,
                target,
                plaguePerEnergy,
                Owner.Creature,
                this);
        }

        var refund =
            DynamicVars["EnergyRefund"].IntValue;

        if (refund > 0)
            await PlayerCmd.GainEnergy(refund, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PlaguePerEnergy"].UpgradeValueBy(1m);
        DynamicVars["EnergyRefund"].UpgradeValueBy(1m);
    }
}
'@

Write-NoBom $path $text

# -------------------------------------------------------------------
# COMBAT CHARACTER CONTROLLER
#
# - permanent sideways idle
# - no vertical bob
# - changes sprite while attack/cast state is active
# - automatically returns to idle if animation state stops
# -------------------------------------------------------------------

$path = "$repo\Brad8381PlagueBringerCode\Character\PlagueBringerIdle.cs"

$text = @'
using Godot;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;

public partial class PlagueBringerIdle : Node2D
{
    private Vector2 _basePosition;
    private Vector2 _baseScale;
    private float _baseRotation;
    private double _time;

    private Sprite2D? _sprite;
    private AnimationPlayer? _animationPlayer;

    private Texture2D? _idleTexture;
    private Texture2D? _attackTexture;
    private Texture2D? _castTexture;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        _basePosition = Position;
        _baseScale = Scale;
        _baseRotation = Rotation;

        _sprite = GetNodeOrNull<Sprite2D>("Sprite");
        _animationPlayer =
            GetNodeOrNull<AnimationPlayer>("AnimationPlayer");

        _idleTexture = GD.Load<Texture2D>(
            "res://Brad8381PlagueBringer/images/character/idle_pose.png");

        _attackTexture = GD.Load<Texture2D>(
            "res://Brad8381PlagueBringer/images/character/attack_pose.png");

        _castTexture = GD.Load<Texture2D>(
            "res://Brad8381PlagueBringer/images/character/cast_pose.png");

        if (_sprite != null && _idleTexture != null)
            _sprite.Texture = _idleTexture;

        SetProcess(true);
    }

    public override void _Process(double delta)
    {
        _time += delta;

        var sway =
            Mathf.Sin((float)_time * 1.05f);

        Position =
            _basePosition +
            new Vector2(sway * 3.5f, 0f);

        Rotation =
            _baseRotation +
            Mathf.DegToRad(sway * 0.35f);

        Scale = _baseScale;

        if (_sprite == null)
            return;

        var animation =
            _animationPlayer?.CurrentAnimation.ToString() ?? "";

        Texture2D? wanted = animation switch
        {
            "attack" => _attackTexture,
            "cast" => _castTexture,
            _ => _idleTexture
        };

        if (wanted != null &&
            _sprite.Texture != wanted)
        {
            _sprite.Texture = wanted;
        }
    }
}
'@

Write-NoBom $path $text

# -------------------------------------------------------------------
# REMOVE VERTICAL MOTION FROM TSCN IDLE ANIMATION.
# C# controller handles idle motion now.
# -------------------------------------------------------------------

$combatScene = "$repo\Brad8381PlagueBringer\scenes\plaguebringer_character.tscn"
$c = [System.IO.File]::ReadAllText($combatScene)

$c = $c.Replace(
    'path="res://Brad8381PlagueBringer/images/character/plaguebringer_combat.png"',
    'path="res://Brad8381PlagueBringer/images/character/idle_pose.png"'
)

$pattern = '(?s)\[sub_resource type="Animation" id="Animation_idle"\]\s*.*?(?=\[sub_resource type="Animation" id="Animation_attack"\])'

$replacement = @'
[sub_resource type="Animation" id="Animation_idle"]
resource_name = "idle"
length = 2.4
loop_mode = 1

'@

if ([regex]::IsMatch($c, $pattern)) {

    $newC = [regex]::Replace(
        $c,
        $pattern,
        $replacement
    )

    Write-NoBom $combatScene $newC
}
else {
    Write-Host "Animation_idle block already changed or uses a different layout - leaving it alone." -ForegroundColor Yellow
}

Write-NoBom $combatScene $newC

# -------------------------------------------------------------------
# CHARACTER SELECT SWAY
# Horizontal + extremely subtle breathing.
# -------------------------------------------------------------------

$path = "$repo\Brad8381PlagueBringerCode\Character\PlagueBringerSelectSway.cs"

$text = @'
using Godot;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;

public partial class PlagueBringerSelectSway : TextureRect
{
    private Vector2 _basePosition;
    private Vector2 _baseScale;
    private float _baseRotation;
    private double _time;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        _basePosition = Position;
        _baseScale = Scale;
        _baseRotation = Rotation;

        PivotOffset = Size * 0.5f;

        SetProcess(true);
    }

    public override void _Process(double delta)
    {
        _time += delta;

        var sway =
            Mathf.Sin((float)_time * 0.80f);

        var breathe =
            Mathf.Sin((float)_time * 1.20f + 0.8f);

        Position =
            _basePosition +
            new Vector2(sway * 6f, 0f);

        Rotation =
            _baseRotation +
            Mathf.DegToRad(sway * 0.35f);

        var scaleFactor =
            1f + breathe * 0.003f;

        Scale =
            _baseScale * scaleFactor;
    }
}
'@

Write-NoBom $path $text

# -------------------------------------------------------------------
# CHARACTER SELECT FRAMING
# Larger / lower / belt-up.
# -------------------------------------------------------------------

$selectScene = "$repo\Brad8381PlagueBringer\scenes\plaguebringer_select_bg.tscn"
$c = [System.IO.File]::ReadAllText($selectScene)

$c = $c.Replace(
    'path="res://Brad8381PlagueBringer/images/character/plaguebringer_combat.png"',
    'path="res://Brad8381PlagueBringer/images/character/select_pose.png"'
)

$c = $c.Replace('offset_left = -1600.0', 'offset_left = -1450.0')
$c = $c.Replace('offset_top = -520.0', 'offset_top = -500.0')
$c = $c.Replace('offset_right = -580.0', 'offset_right = -100.0')
$c = $c.Replace('offset_bottom = 520.0', 'offset_bottom = 900.0')

# Darker than before.
$c = $c.Replace(
    'color = Color(0.018, 0.075, 0.034, 0.32)',
    'color = Color(0.008, 0.035, 0.016, 0.58)'
)

Write-NoBom $selectScene $c

# -------------------------------------------------------------------
# REST SITE
# Static sitting artwork.
# -------------------------------------------------------------------

$restScene = "$repo\Brad8381PlagueBringer\scenes\plaguebringer_rest_site.tscn"

$text = @'
[gd_scene load_steps=2 format=3]

[ext_resource type="Texture2D" path="res://Brad8381PlagueBringer/images/character/rest_pose.png" id="1_texture"]

[node name="PlagueBringerRestSite" type="Sprite2D"]
texture = ExtResource("1_texture")
position = Vector2(0, -75)
scale = Vector2(0.16, 0.16)
'@

Write-NoBom $restScene $text

# -------------------------------------------------------------------
# REMOVE MIASMA
# It duplicates Airborne Cultures.
# -------------------------------------------------------------------

Remove-Item `
    "$repo\Brad8381PlagueBringerCode\Cards\Miasma.cs" `
    -ErrorAction SilentlyContinue

Remove-Item `
    "$repo\Brad8381PlagueBringerCode\Powers\MiasmaPower.cs" `
    -ErrorAction SilentlyContinue

# -------------------------------------------------------------------
# LOOK FOR ANY OTHER X-COST CARDS
# Do not blindly modify semantics; report them.
# -------------------------------------------------------------------

Write-Host ""
Write-Host "X-cost files still present:" -ForegroundColor Cyan

Get-ChildItem `
    "$repo\Brad8381PlagueBringerCode\Cards" `
    -Filter *.cs `
    -Recurse |
    Select-String "HasEnergyCostX|ResolveEnergyXValue" |
    Select-Object Path, LineNumber, Line

# -------------------------------------------------------------------
# ENSURE GODOT CAN RESOLVE STS2 DURING IMPORT/EXPORT
# LOCAL ONLY. DO NOT COMMIT.
# -------------------------------------------------------------------

$targets = "$repo\Directory.Build.targets"

if (-not (Test-Path $targets)) {

    $text = @'
<Project>
  <ItemGroup>
    <Reference Update="sts2">
      <Private>true</Private>
    </Reference>
    <Reference Update="0Harmony">
      <Private>true</Private>
    </Reference>
  </ItemGroup>
</Project>
'@

    Write-NoBom $targets $text
}

# -------------------------------------------------------------------
# BUILD
# -------------------------------------------------------------------

Write-Host ""
Write-Host "Building C#..." -ForegroundColor Cyan

dotnet build

if ($LASTEXITCODE -ne 0) {
    throw "dotnet build failed."
}

# -------------------------------------------------------------------
# GODOT IMPORT
# -------------------------------------------------------------------

Write-Host ""
Write-Host "Importing Godot assets..." -ForegroundColor Cyan

& $godot `
    --headless `
    --path $repo `
    --import

if ($LASTEXITCODE -ne 0) {
    throw "Godot import failed."
}

# -------------------------------------------------------------------
# EXPORT PCK
# -------------------------------------------------------------------

Write-Host ""
Write-Host "Exporting PCK..." -ForegroundColor Cyan

New-Item -ItemType Directory -Force $mod | Out-Null

& $godot `
    --headless `
    --path $repo `
    --export-pack "BasicExport" `
    "$mod\Brad8381PlagueBringer.pck"

if ($LASTEXITCODE -ne 0) {
    throw "PCK export failed."
}

Write-Host ""
Write-Host "PCK:" -ForegroundColor Green

Get-Item `
    "$mod\Brad8381PlagueBringer.pck" |
    Select-Object Name, Length, LastWriteTime

Write-Host ""
Write-Host "Git status:" -ForegroundColor Cyan

git status --short

Write-Host ""
Write-Host "PASS COMPLETE" -ForegroundColor Green
Write-Host "Backup is at:"
Write-Host $backup