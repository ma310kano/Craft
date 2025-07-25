﻿using Craft;
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

Console.WriteLine(human);
