namespace Craft;

/// <summary>
/// 家族のファクトリー
/// </summary>
public interface IFamilyFactory
{
	#region Methods

	/// <summary>
	/// 家族を作成します。
	/// </summary>
	/// <param name="familyName">家族名</param>
	/// <returns>作成した家族を返します。</returns>
	Family Create(FamilyName familyName);

	#endregion
}
