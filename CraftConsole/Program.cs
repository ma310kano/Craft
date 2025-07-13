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

	services.AddSingleton<IAreaQueryService, AreaQueryService>();
	services.AddSingleton<IHumanCreationService, HumanCreationService>();
	services.AddSingleton<IItemRecipeQueryService, ItemRecipeQueryService>();

	serviceProvider = services.BuildServiceProvider();
}

Area grassland;
Area river;
{
	IAreaQueryService areaQueryService = serviceProvider.GetRequiredService<IAreaQueryService>();

	// Grassland
	{
		AreaId areaId = new("grassland");

		grassland = areaQueryService.QuerySingle(areaId);
	}

	// river
	{
		AreaId areaId = new("river");

		river = areaQueryService.QuerySingle(areaId);
	}
}

Console.WriteLine(grassland);
Console.WriteLine();

Human human;
{
	IHumanCreationService humanCreationService = serviceProvider.GetRequiredService<IHumanCreationService>();

	FirstName firstName = new("John");
	FamilyName familyName = new("Smith");

	human = humanCreationService.CreateWithFamily(firstName, familyName);
}

grassland.AddHuman(human);

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

grassland.RemoveHuman(human);
river.AddHuman(human);

// Add item: Stone(Medium)
{
	ItemId itemId = new("stone-medium");
	Quantity quantity = new(1);

	human.PickUpItem(itemId, quantity);
}

Console.WriteLine(human);
