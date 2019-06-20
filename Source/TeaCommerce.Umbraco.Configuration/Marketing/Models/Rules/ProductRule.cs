using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TeaCommerce.Api.Marketing.Models.Rules;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;

namespace TeaCommerce.Umbraco.Configuration.Marketing.Models.Rules
{
    [Rule("ProductRule")]
    public class ProductRule : ARule, IOrderLineRule
    {
        public int? NodeId { get; set; }

        private readonly IProductService _productService;

        public ProductRule()
        {
            _productService = ProductService.Instance;
        }

        public override void LoadSettings()
        {
            var settings = JsonConvert.DeserializeAnonymousType(Settings, new { NodeId = (int?)null });

            if (settings == null) return;

            NodeId = settings.NodeId;
        }

        public IEnumerable<OrderLine> IsFulfilledBy(Order order, IEnumerable<OrderLine> previouslyFulfilledOrderLines)
        {
            try
            {
                return NodeId != null
                    ? ProductUtils.OrderLinesThatMatchProductOrProductCategory(_productService, NodeId.Value, previouslyFulfilledOrderLines)
                    : new List<OrderLine>();
            }
            catch (ArgumentException ex)
            {
                // See issue #84 - https://github.com/TeaCommerce/Tea-Commerce-for-Umbraco/issues/84
                // If the exception is due to no Store ID being found, this might be because
                // the product node is unpublished. Ultimately, this isn't worth throwing
                // an exception over, so if this is the case, just let the rule fail.
                if (ex.Message.ToLowerInvariant().Contains("doesn't have a store id"))
                {
                    return new List<OrderLine>();
                }

                throw;
            }
        }

    }
}