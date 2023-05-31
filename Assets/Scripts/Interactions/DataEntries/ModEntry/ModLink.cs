using System.Collections.Generic;
using UnityEngine.UI.Extensions;

namespace ReachModLauncher
{
	public sealed class ModLink : TextLink
	{
		public List<BoundTooltipTriggerExtended> Tooltips = new List<BoundTooltipTriggerExtended>();
	}
}