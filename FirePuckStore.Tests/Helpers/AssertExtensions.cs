using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using FirePuckStore.Models;
using Xunit;

namespace FirePuckStore.Tests.Helpers
{
    public static class AssertExtensions
    {
        public static void WithNameHasModelType<TModel>(this ViewResultBase viewResult, string viewName)
        {
            Assert.NotNull(viewResult);
            Assert.Equal(viewName, viewResult.ViewName, StringComparer.OrdinalIgnoreCase);
            Assert.IsAssignableFrom<TModel>(viewResult.Model);
        }

        public static void ContainsCard(this IEnumerable<Card> cards, Card card)
        {
            Assert.Contains(card, cards, new CardComparer());
        }

        public static void ShouldBeRedirectionTo(this ActionResult actionResult, object expectedRouteValues)
        {
            var redirectToRouteResult = (RedirectToRouteResult) actionResult;
            Assert.NotNull(redirectToRouteResult);
            var actualValues = redirectToRouteResult.RouteValues;
            var expectedValues = new RouteValueDictionary(expectedRouteValues);

            foreach (var key in expectedValues.Keys)
            {
                Assert.Equal(expectedValues[key], actualValues[key]);
            }
        }
    }

    class CardComparer : IEqualityComparer<Card>
    {
        public bool Equals(Card x, Card y)
        {
            return x.CardId.Equals(y.CardId);
        }

        public int GetHashCode(Card obj)
        {
            return obj.CardId.GetHashCode();
        }
    }
}
