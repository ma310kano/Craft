namespace Craft;

/// <summary>
/// 家族のファクトリー
/// </summary>
public class FamilyFactory : IFamilyFactory
{
	#region Methods

	/// <summary>
	/// 家族を作成します。
	/// </summary>
	/// <param name="familyName">家族名</param>
	/// <returns>作成した家族を返します。</returns>
	public Family Create(FamilyName familyName)
	{
		Family product;
		{
			FamilyId familyId = FamilyId.Create();

			product = new Family(familyId, familyName);
		}

		return product;
	}

	#endregion
}
