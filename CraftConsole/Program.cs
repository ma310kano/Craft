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

	serviceProvider = services.BuildServiceProvider();
}

IItemQueryService itemQueryService = serviceProvider.GetRequiredService<IItemQueryService>();

ItemId itemId = new("grass");

Item item = itemQueryService.QuerySingle(itemId);

Console.WriteLine(item);
