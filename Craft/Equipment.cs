namespace Craft;

/// <summary>
/// 装備
/// </summary>
public class Equipment : IReadOnlyEquipment
{
	#region Fields

	/// <summary>
	/// 部位とアイテム物質のディクショナリー
	/// </summary>
	private readonly Dictionary<EquipmentParts, ItemMatter> _partItemMatters = [];

	#endregion

	#region Methods

	/// <summary>
	/// アイテム物質を追加します。
	/// </summary>
	/// <param name="part">部位</param>
	/// <param name="itemMatter">アイテム物質</param>
	public void AddItemMatter(EquipmentParts part, ItemMatter itemMatter)
	{
		bool exists = _partItemMatters.TryGetValue(part, out ItemMatter? removedItemMatter);
		if (exists)
		{
			_partItemMatters.Remove(part);

			if (ItemMatterRemoved is not null)
			{
				ItemMatterRemovedEventArgs e = new(removedItemMatter!);

				ItemMatterRemoved(this, e);
			}
		}

		_partItemMatters.Add(part, itemMatter);
	}

	#endregion

	#region Events

	/// <summary>
	/// アイテム物質を除去すると発生します。
	/// </summary>
	public event EventHandler<ItemMatterRemovedEventArgs>? ItemMatterRemoved;

	#endregion
}
