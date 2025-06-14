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

	services.AddSingleton<IHumanFactory, HumanFactory>();
	services.AddSingleton<IItemRecipeQueryService, ItemRecipeQueryService>();

	serviceProvider = services.BuildServiceProvider();
}

Human human;
{
	IHumanFactory humanFactory = serviceProvider.GetRequiredService<IHumanFactory>();

	FirstName firstName = new("John");
	FamilyName familyName = new("Smith");

	human = humanFactory.CreateWithFamily(firstName, familyName);
}

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

	human.Inventory.AddItem(itemId, quantity);
}

human.MakeItem(itemRecipeId);

Console.WriteLine(human);
