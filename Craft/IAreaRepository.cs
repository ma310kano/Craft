namespace Craft;

/// <summary>
/// エリアのリポジトリー
/// </summary>
public interface IAreaRepository
{
	#region Methods

	/// <summary>
	/// エリアを検索します。
	/// </summary>
	/// <param name="areaId">エリアID</param>
	/// <returns>検索したエリアを返します。</returns>
	Area Find(AreaId areaId);

	#endregion
}
