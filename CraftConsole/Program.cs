using Craft;
using Craft.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IServiceProvider serviceProvider;
{
	ServiceCollection services = new();

	{
		IConfiguration configuration = new ConfigurationBuilder().
			AddJsonFile("appsettings.json").
			Build();

		services.AddSingleton(configuration);
	}

	services.AddSingleton<IAreaManager, AreaManager>();
	services.AddSingleton<IAreaQueryService, AreaQueryService>();
	services.AddSingleton<IHumanCreationService, HumanCreationService>();
	services.AddSingleton<IItemRecipeQueryService, ItemRecipeQueryService>();

	serviceProvider = services.BuildServiceProvider();
}

IAreaManager areaManager = serviceProvider.GetRequiredService<IAreaManager>();

Human human;
{
	IHumanCreationService humanCreationService = serviceProvider.GetRequiredService<IHumanCreationService>();

	FirstName firstName = new("John");
	FamilyName familyName = new("Smith");

	human = humanCreationService.CreateWithFamily(firstName, familyName);
}

// Move area: Grassland
{
	AreaId areaId = new("grassland");

	areaManager.Move(human, areaId);
}

// Add item recipe
IItemRecipeQueryService itemRecipeQueryService = serviceProvider.GetRequiredService<IItemRecipeQueryService>();

ItemRecipeId recipeFiber = new("fiber-01");
{
	ItemRecipe itemRecipe = itemRecipeQueryService.QuerySingle(recipeFiber);

	human.ItemRecipes.Add(itemRecipe);
}

ItemRecipeId recipeString = new("string-01");
{
	ItemRecipe itemRecipe = itemRecipeQueryService.QuerySingle(recipeString);

	human.ItemRecipes.Add(itemRecipe);
}

ItemRecipeId recipeRope = new("rope-01");
{
	ItemRecipe itemRecipe = itemRecipeQueryService.QuerySingle(recipeRope);

	human.ItemRecipes.Add(itemRecipe);
}

ItemRecipeId recipeWoodenStick = new("wooden-stick-01");
{
	ItemRecipe itemRecipe = itemRecipeQueryService.QuerySingle(recipeWoodenStick);

	human.ItemRecipes.Add(itemRecipe);
}

ItemRecipeId recipeStoneAxe = new("stone-axe-01");
{
	ItemRecipe itemRecipe = itemRecipeQueryService.QuerySingle(recipeStoneAxe);

	human.ItemRecipes.Add(itemRecipe);
}

// Add item: Grass
{
	ItemId itemId = new("grass");
	Quantity quantity = new(2);

	human.PickUpItem(itemId, quantity);
}

human.MakeItem(recipeFiber);
human.MakeItem(recipeFiber);
human.MakeItem(recipeString);
human.MakeItem(recipeString);
human.MakeItem(recipeRope);

Console.WriteLine(human);

// Move area: River
{
	AreaId areaId = new("river");

	areaManager.Move(human, areaId);
}

// Add item: Stone(Medium)
{
	ItemId itemId = new("stone-medium");
	Quantity quantity = new(1);

	human.PickUpItem(itemId, quantity);
}

// Move area: Forest
{
	AreaId areaId = new("forest");

	areaManager.Move(human, areaId);
}

// Add item: Tree branch
{
	ItemId itemId = new("tree-branch");
	Quantity quantity = new(1);

	human.PickUpItem(itemId, quantity);
}

human.MakeItem(recipeWoodenStick);
human.MakeItem(recipeStoneAxe);

// Equip item: Stone axe
{
	ItemId itemId = new("stone-axe");

	human.EquipItem(itemId);
}

// Add item: Log
{
	ItemId itemId = new("log");
	Quantity quantity = new(1);

	human.PickUpItem(itemId, quantity);
}

Console.WriteLine(human);
