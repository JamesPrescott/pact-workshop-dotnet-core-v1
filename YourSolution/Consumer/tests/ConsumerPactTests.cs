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
            /* Create a test that validates that when you send a request to the partnerconnect endpoint /orders/creationinfo
            * with valid query strings that a 200 response returns with a valid body.
            */
        }

        [Fact]
        public void ItReturnsBadRequestWithInvalidPartnerSubscriptionId()
        {
            /* Create a test that validates that when you send a request to the partnerconnect endpoint /orders/creationinfo
            with an invalid partnerSubscriptionId that a 400 response returns */
        }
    }
}