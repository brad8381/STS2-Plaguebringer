using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using PB.Character;
using PB.Extensions;

namespace PB.Potions;

[Pool(typeof(PlagueBringerPotionPool))]
public abstract class PlagueBringerPotion : CustomPotionModel
{
	public override string? CustomPackedImagePath =>
		$"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
	public override string? CustomPackedOutlinePath =>
		$"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionOutlineImagePath();
}