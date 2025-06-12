namespace Craft;

/// <summary>
/// 人間のファクトリー
/// </summary>
public class HumanFactory : IHumanFactory
{
	#region Fields

	/// <summary>
	/// 家族のファクトリー
	/// </summary>
	private readonly IFamilyFactory _familyFactory = new FamilyFactory();

	#endregion

	#region Methods

	/// <summary>
	/// 人間を作成します。
	/// </summary>
	/// <param name="firstName">個人名</param>
	/// <param name="family">家族</param>
	/// <returns>作成した人間を返します。</returns>
	public Human Create(FirstName firstName, Family family)
	{
		Human product;
		{
			HumanId humanId = HumanId.Create();

			product = new Human(humanId, firstName, family);
		}

		return product;
	}

	/// <summary>
	/// 人間を作成します。
	/// </summary>
	/// <param name="firstName">個人名</param>
	/// <param name="familyName">家族名</param>
	/// <returns>作成した人間を返します。</returns>
	public Human CreateWithFamily(FirstName firstName, FamilyName familyName)
	{
		Human product;
		{
			Family family = _familyFactory.Create(familyName);

			product = Create(firstName, family);
		}

		return product;
	}

	#endregion
}
