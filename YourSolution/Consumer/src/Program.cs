using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            string restaurantReference = "TestRestaurant";
            string partnerSubscriptionId = "5224";
            string baseUri = "http://uk-partnerconnectapi-qa28.cogpart.je-labs.com";

            if(args.Length <= 2)
            {
                Console.WriteLine("-------------------");
                WriteoutArgsUsed(restaurantReference, partnerSubscriptionId, baseUri);
                WriteoutUsageInstructions();
                Console.WriteLine("-------------------");
            }
            else
            {
                restaurantReference = args[0];
                partnerSubscriptionId = args[1];
                baseUri = args[2];
                Console.WriteLine("-------------------");
                WriteoutArgsUsed(restaurantReference, partnerSubscriptionId, baseUri);
                Console.WriteLine("-------------------");
            }

            Console.WriteLine("Validating date...");
            var result = ConsumerApiClient.GetOrderDetails(restaurantReference, Int32.Parse(partnerSubscriptionId), baseUri).GetAwaiter().GetResult();
            var resultContentText = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Console.WriteLine(resultContentText);
            Console.WriteLine("...Date validation complete. Goodbye.");
        }

        static private void WriteoutArgsUsed(string restaurantReferenceArg, string partnerSubscriptionIdArg, string baseUriArg)
        {
            Console.WriteLine($"Running consumer with args: restaurantReference = {restaurantReferenceArg}, partnerSubscriptionId = {partnerSubscriptionIdArg}, baseUri = {baseUriArg}");
        }

        static private void WriteoutUsageInstructions()
        {
            Console.WriteLine("To use with your own parameters:");
            Console.WriteLine("Usage: dotnet run [DateTime To Validate] [Provider Api Uri]");
            Console.WriteLine("Usage Example: dotnet run 01/01/2018 http://localhost:9000");
        }
    }
}
