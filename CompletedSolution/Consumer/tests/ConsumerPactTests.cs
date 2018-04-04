using System;
using System.Net;
using Xunit;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Consumer;
using System.Collections.Generic;

namespace tests
{
    public class OrderDetails
    {
        public string RestaurantReference { get; set; }
        public int PartnerSubscriptionId { get; set; }
    }

    public class ConsumerPactTests : IClassFixture<ConsumerPactClassFixture>
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        public ConsumerPactTests(ConsumerPactClassFixture fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _mockProviderService.ClearInteractions(); //NOTE: Clears any previously registered interactions before the test is run
            _mockProviderServiceBaseUri = fixture.MockProviderServiceBaseUri;
        }

        [Fact]
        public void ItReturnsDetailsCorrectly()
        {
            var orderDetails = new OrderDetails
            {
                RestaurantReference = "TestRestaurant",
                PartnerSubscriptionId = 5224
            };

            _mockProviderService.UponReceiving("A valid GET request for Order Creation Info")
                                .With(new ProviderServiceRequest 
                                {
                                    Method = HttpVerb.Get,
                                    Path = "/orders/creationinfo",
                                    Query = $"restaurantReference={orderDetails.RestaurantReference}&partnerSubscriptionId={orderDetails.PartnerSubscriptionId}"
                                })
                                .WillRespondWith(new ProviderServiceResponse {
                                    Status = 200,
                                    Headers = new Dictionary<string, object>
                                    {
                                        { "Content-Type", "application/json; charset=utf-8" }
                                    },
                                    Body = new 
                                    {
                                        restaurantId = 298648,
                                        partnerSubscriptionId = 5224,
                                        partnerName = "JPTest",
                                        isRdsRestaurant = true,
                                        serviceType = "Rds",
                                    }
                                });

            var result = ConsumerApiClient.GetOrderDetails(orderDetails.RestaurantReference, orderDetails.PartnerSubscriptionId, _mockProviderServiceBaseUri).GetAwaiter().GetResult();
            var resultBodyText = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            Assert.Contains(orderDetails.PartnerSubscriptionId.ToString(), resultBodyText);
        }

        [Fact]
        public void ItReturnsBadRequestWithInvalidPartnerSubscriptionId()
        {
            var orderDetails = new OrderDetails
            {
                RestaurantReference = "TestRestaurant",
                PartnerSubscriptionId = 5225
            };

            _mockProviderService.UponReceiving("A invalid GET request for Order Creation Info with bad PartnerSubscriptionId")
                                .With(new ProviderServiceRequest 
                                {
                                    Method = HttpVerb.Get,
                                    Path = "/orders/creationinfo",
                                    Query = $"restaurantReference={orderDetails.RestaurantReference}&partnerSubscriptionId={orderDetails.PartnerSubscriptionId}"
                                })
                                .WillRespondWith(new ProviderServiceResponse {
                                    Status = 400,
                                    Headers = new Dictionary<string, object>
                                    {
                                        { "Content-Type", "application/json; charset=utf-8" }
                                    }
                                });

            var result = ConsumerApiClient.GetOrderDetails(orderDetails.RestaurantReference, orderDetails.PartnerSubscriptionId, _mockProviderServiceBaseUri).GetAwaiter().GetResult();
            var resultBodyText = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}