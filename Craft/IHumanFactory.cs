namespace Craft;

/// <summary>
/// 人間のファクトリー
/// </summary>
public interface IHumanFactory
{
	#region Methods

	/// <summary>
	/// 人間を作成します。
	/// </summary>
	/// <param name="firstName">個人名</param>
	/// <param name="family">家族</param>
	/// <returns>作成した人間を返します。</returns>
	Human Create(FirstName firstName, Family family);

	/// <summary>
	/// 人間を作成します。
	/// </summary>
	/// <param name="firstName">個人名</param>
	/// <param name="familyName">家族名</param>
	/// <returns>作成した人間を返します。</returns>
	Human CreateWithFamily(FirstName firstName, FamilyName familyName);

	#endregion
}
