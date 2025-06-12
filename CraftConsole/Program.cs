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

	services.AddSingleton<IItemQueryService, ItemQueryService>();
	services.AddSingleton<IItemMatterCreationService, ItemMatterCreationService>();

	serviceProvider = services.BuildServiceProvider();
}

IItemMatterCreationService itemMatterCreationService = serviceProvider.GetRequiredService<IItemMatterCreationService>();

ItemId itemId = new("grass");
Quantity quantity = new(1);

ItemMatter itemMatter = itemMatterCreationService.Create(itemId, quantity);

Console.WriteLine(itemMatter);
