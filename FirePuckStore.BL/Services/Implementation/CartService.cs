﻿using System;
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
        #region Fields

        private readonly ICardService _cardService;
        private readonly ICartRepository _cartRepository;

        #endregion

        #region Constructor

        public CartService(ICartRepository cartRepository, ICardService cardService)
        {
            _cartRepository = cartRepository;
            _cardService = cardService;
        }

        #endregion

        #region ICartService Implementation

        public List<Order> GetCart()
        {
            var result = _cartRepository.GetCartOrder();
            result.ForEach(CalculateTotalPriceForOrder);
            return result;
        }

        private void CalculateTotalPriceForOrder(Order order)
        {
            order.Price = order.Card.Price * order.Quantity;
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
            ChangeQuantityForCardInStore(card, -1);
            return order;
        }

        public Order UnPlace1QuantityOrderAndReturnSummaryOrderForCard(int cardId)
        {
            var card = _cardService.GetById(cardId);

            var order = _cartRepository.GetOrderForCardId(cardId);
            if (order == null)
            {
                throw new InvalidOperationException("There is no order for this item");
            }

            if (order.Quantity > 1)
            {
                --order.Quantity;

                _cartRepository.UpdateOrder(order);
            }
            else
            {
                _cartRepository.DeleteOrder(order.OrderId);
            }

            order.Card = card;
            CalculateTotalPriceForOrder(order);
            ChangeQuantityForCardInStore(card, 1);
            return order;
        }

        private void ChangeQuantityForCardInStore(Card card, int quantity)
        {
            card.Quantity += quantity;
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

        #endregion
    }
}