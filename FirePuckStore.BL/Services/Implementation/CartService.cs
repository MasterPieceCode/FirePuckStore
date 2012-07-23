using System;
using System.Collections.Generic;
using System.Linq;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.DAL;
using FirePuckStore.DAL.Repositories.Interfaces;
using FirePuckStore.Models;

namespace FirePuckStore.BL.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly ICardService _cardService;
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository, ICardService cardService)
        {
            _cartRepository = cartRepository;
            _cardService = cardService;
        }

        public List<Order> GetCart()
        {
            var result = _cartRepository.GetCartOrder();
            result.ForEach(CalculateTotalPriceForOrder);
            return result;
        }

        private void CalculateTotalPriceForOrder(Order order)
        {
            order.Price = order.Card.Price*order.Quantity;
        }

        public Order Place1QuantityOrderAndReturnSummaryOrderForCard(int cardId)
        {
            var card = _cardService.GetById(cardId);
            if (card.Quantity == 0)
            {
                throw new InvalidOperationException("You can not add item to card due to missing this card available for selling");
            }

            var order = _cartRepository.GetOrderForCardId(cardId);
            if (order == null)
            {
                order = CreateNew1QuanityOrderForCard(cardId);
                _cartRepository.AddOrder(order);
            }
            else
            {
                order.Quantity += 1;
                _cartRepository.UpdateOrder(order);
            }
            order.Card = card;
            CalculateTotalPriceForOrder(order);
            ReduceAmountBy1QuantityForCardInStore(card);
            return order;
        }

        private void ReduceAmountBy1QuantityForCardInStore(Card card)
        {
            --card.Quantity;
            _cardService.Update(card);
        }

        private static Order CreateNew1QuanityOrderForCard(int cardId)
        {
            return new Order{CardId = cardId, Quantity = 1};
        }

        public void Dispose()
        {
            _cartRepository.Dispose();
        }
    }
}