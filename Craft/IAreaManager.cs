namespace Craft;

/// <summary>
/// エリアマネージャー
/// </summary>
public interface IAreaManager
{
	#region Methods

	/// <summary>
	/// エリアを移動します。
	/// </summary>
	/// <param name="human">人間</param>
	/// <param name="areaId">エリアID</param>
	void Move(Human human, AreaId areaId);

	#endregion
}
