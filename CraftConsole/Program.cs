using Craft;
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

	serviceProvider = services.BuildServiceProvider();
}

Human human;
{
	IHumanFactory humanFactory = serviceProvider.GetRequiredService<IHumanFactory>();

	FirstName firstName = new("John");
	FamilyName familyName = new("Smith");

	human = humanFactory.CreateWithFamily(firstName, familyName);
}

{
	ItemId itemId = new("grass");
	Quantity quantity = new(1);

	human.Inventory.AddItem(itemId, quantity);
}

Console.WriteLine(human);
