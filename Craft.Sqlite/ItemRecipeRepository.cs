using Dapper;
using System.Data;

namespace Craft.Sqlite;

/// <summary>
/// アイテムレシピのリポジトリー
/// </summary>
/// <param name="connection">コネクション</param>
/// <param name="languageCode">言語コード</param>
public class ItemRecipeRepository(IDbConnection connection, string languageCode) : IItemRecipeRepository
{
	#region Fields

	/// <summary>
	/// アイテムのリポジトリー
	/// </summary>
	private readonly ItemRepository _itemRepository = new(connection, languageCode);

	/// <summary>
	/// スキルのリポジトリー
	/// </summary>
	private readonly SkillRepository _skillRepository = new(connection, languageCode);

	#endregion

	#region Methods

	/// <summary>
	/// アイテムレシピを検索します。
	/// </summary>
	/// <param name="itemRecipeId">アイテムレシピID</param>
	/// <returns>検索したアイテムレシピを返します。</returns>
	public ItemRecipe Find(ItemRecipeId itemRecipeId)
	{
		RecipeRecord recipe;
		{
			const string sql = @"SELECT
	  item_recipe_id
	, item_id
	, quantity
FROM
	item_recipes
WHERE
	item_recipe_id = :item_recipe_id";

			var param = new
			{
				item_recipe_id = itemRecipeId.Value,
			};

			recipe = connection.QuerySingle<RecipeRecord>(sql, param);
		}

		List<SkillRecord> skills;
		{
			const string sql = @"SELECT
	irs.skill_id
FROM
	item_recipe_skills irs
	INNER JOIN skills ski
		ON irs.skill_id = ski.skill_id
WHERE
	irs.item_recipe_id = :item_recipe_id";

			var param = new
			{
				item_recipe_id = itemRecipeId.Value,
			};

			skills = [.. connection.Query<SkillRecord>(sql, param)];
		}

		List<IngredientRecord> ingredients;
		{
			const string sql = @"SELECT
	  item_id
	, quantity
FROM
	item_recipe_ingredients
WHERE
	item_recipe_id = :item_recipe_id";

			var param = new
			{
				item_recipe_id = itemRecipeId.Value,
			};

			ingredients = [.. connection.Query<IngredientRecord>(sql, param)];
		}

		ItemRecipe result;
		{
			ItemRecipeId resItemRecipeId = new(recipe.ItemRecipeId);

			Item resItem;
			{
				ItemId cdItemId = new(recipe.ItemId);

				resItem = _itemRepository.Find(cdItemId);
			}

			List<Skill> resSkills = [];
			foreach (SkillRecord source in skills)
			{
				Skill resSkill;
				{
					SkillId resSkillId = new(source.SkillId);

					resSkill = _skillRepository.Find(resSkillId);
				}

				resSkills.Add(resSkill);
			}

			Quantity resQuantity = new((int)recipe.Quantity);

			List<RecipeIngredient> resIngredients = [];
			foreach (IngredientRecord source in ingredients)
			{
				RecipeIngredient ingredient;
				{
					Item ingItem;
					{
						ItemId cdItemId = new(source.ItemId);

						ingItem = _itemRepository.Find(cdItemId);
					}

					Quantity ingQuantity = new((int)source.Quantity);

					ingredient = new RecipeIngredient(ingItem, ingQuantity);
				}

				resIngredients.Add(ingredient);
			}

			result = new(resItemRecipeId, resSkills, resItem, resQuantity, resIngredients);
		}

		return result;
	}

	#endregion

	#region Nested types

	/// <summary>
	/// レシピのレコード
	/// </summary>
	/// <param name="ItemRecipeId">アイテムレシピID</param>
	/// <param name="ItemId">アイテムID</param>
	/// <param name="Quantity">数量</param>
	private record class RecipeRecord(string ItemRecipeId, string ItemId, long Quantity);

	/// <summary>
	/// スキルのレコード
	/// </summary>
	/// <param name="SkillId">スキルID</param>
	private record class SkillRecord(string SkillId);

	/// <summary>
	/// 素材のレコード
	/// </summary>
	/// <param name="ItemId">アイテムID</param>
	/// <param name="Quantity">数量</param>
	private record class IngredientRecord(string ItemId, long Quantity);

	#endregion
}
