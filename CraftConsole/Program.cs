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

Area area;
{
	IAreaQueryService areaQueryService = serviceProvider.GetRequiredService<IAreaQueryService>();

	AreaId areaId = new("grassland");

	area = areaQueryService.QuerySingle(areaId);
}

Console.WriteLine(area);
Console.WriteLine();

Human human;
{
	IHumanCreationService humanCreationService = serviceProvider.GetRequiredService<IHumanCreationService>();

	FirstName firstName = new("John");
	FamilyName familyName = new("Smith");

	human = humanCreationService.CreateWithFamily(firstName, familyName);
}

area.AddHuman(human);

// Add item recipe
ItemRecipeId itemRecipeId = new("fiber-01");
{
	IItemRecipeQueryService itemRecipeQueryService = serviceProvider.GetRequiredService<IItemRecipeQueryService>();

	ItemRecipe itemRecipe = itemRecipeQueryService.QuerySingle(itemRecipeId);

	human.ItemRecipes.Add(itemRecipe);
}

// Add item
{
	ItemId itemId = new("grass");
	Quantity quantity = new(1);

	human.PickUpItem(itemId, quantity);
}

human.MakeItem(itemRecipeId);

Console.WriteLine(human);
