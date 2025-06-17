namespace Craft;

/// <summary>
/// エリアの問い合わせサービス
/// </summary>
public interface IAreaQueryService
{
	#region Methods

	/// <summary>
	/// エリアを問い合わせします。
	/// </summary>
	/// <param name="areaId">エリアID</param>
	/// <returns>問い合わせしたエリアを返します。</returns>
	Area QuerySingle(AreaId areaId);

	/// <summary>
	/// エリアを問い合わせします。
	/// </summary>
	/// <param name="areaId">エリアID</param>
	/// <returns>問い合わせしたエリアを返します。</returns>
	Task<Area> QuerySingleAsync(AreaId areaId);

	#endregion
}
