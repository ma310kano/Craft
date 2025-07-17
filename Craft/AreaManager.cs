namespace Craft;

/// <summary>
/// エリアマネージャー
/// </summary>
/// <param name="areaQueryService">エリアの問い合わせサービス</param>
public class AreaManager(IAreaQueryService areaQueryService) : IAreaManager
{
	#region Fields

	/// <summary>
	/// エリアのコレクション
	/// </summary>
	private static readonly List<Area> _areas = [];

	#endregion

	#region Methods

	/// <summary>
	/// エリアを移動します。
	/// </summary>
	/// <param name="human">人間</param>
	/// <param name="areaId">エリアID</param>
	public void Move(Human human, AreaId areaId)
	{
		{
			Area? area = _areas.SingleOrDefault(x => x.AreaId == human.AreaId);
			if (area is not null)
			{
				area.RemoveHuman(human);
				human.SetAreaId(null);
			}
		}

		{
			Area? area = _areas.SingleOrDefault(x => x.AreaId == areaId);

			if (area is null)
			{
				area = areaQueryService.QuerySingle(areaId);

				_areas.Add(area);
			}

			area.AddHuman(human);
			human.SetAreaId(areaId);
		}
	}

	#endregion
}
