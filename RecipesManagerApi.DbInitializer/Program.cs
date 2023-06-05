namespace RecipesManagerApi.DbInitializer;

internal class Program
{
	private static void Main(string[] args)
	{
		DbInitializer.ExistingRecipesTransfer();
		Console.WriteLine("Done.");
	}
}