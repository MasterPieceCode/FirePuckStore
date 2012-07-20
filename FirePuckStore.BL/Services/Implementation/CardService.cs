using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.DAL;
using FirePuckStore.DAL.Repositories.Interfaces;
using FirePuckStore.Models;

namespace FirePuckStore.BL.Services.Implementation
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IFileService _fileService;
        private const int MixedCardsCount = 5;

        public static string CardImagesServerPath = "/Content/Cards/Images/";

        public CardService(ICardRepository cardRepository, IFileService fileService)
        {
            _cardRepository = cardRepository;
            _fileService = fileService;
        }

        public IDictionary<string, List<Card>> GetOrderedByLeagueWithMixedCards()
        {
            var cards = _cardRepository.GetCards().ToList();

            var result = new Dictionary<string, List<Card>>{{"Mixed", new List<Card>()}};

            var orderedCards = cards.GroupBy(c => c.Category);
            foreach(var card in orderedCards)
            {
                result[card.Key] = card.ToList();
                // get 5 cards from every category to mixed category
                result["Mixed"].AddRange(card.Take(MixedCardsCount).Select(ConverToMixedLeagueCard));
            }
            return result;
        }

        public List<Card> GetAllCards()
        {
            return _cardRepository.GetCardsWithPlayerInfo();
        }

        public void DeleteCard(int cardId)
        {
            var card = _cardRepository.FindCardById(cardId);

            if (card == null)
            {
                throw new KeyNotFoundException(string.Format("Card with id {0} not found", cardId));
            }

            if (!string.IsNullOrEmpty(card.ImageUrl))
            {
                DeleteUploadedImageFromServer(card.ImageUrl);
            }

            _cardRepository.DeleteCard(card);
        }

        private void DeleteUploadedImageFromServer(string imageUrl)
        {
            _fileService.DeleteFileFromServer(_fileService.GetPhysicalPath(HttpContext.Current, imageUrl));
        }

        public Card GetById(int cardId)
        {
            return _cardRepository.FindCardById(cardId);
        }

        public void Add(Card card)
        {
            if (card.FileInput != null)
            {
                card.ImageUrl = UploadImage(card.FileInput);
            }

            _cardRepository.AddCard(card);
        }

        public void Update(Card card)
        {
            var dbCard = _cardRepository.FindCardByIdAsNoTracking(card.CardId);

            if (card.FileInput == null)
            {
                card.ImageUrl = dbCard.ImageUrl;
            }
            else
            {
                if (!string.IsNullOrEmpty(dbCard.ImageUrl))
                {
                    DeleteUploadedImageFromServer(dbCard.ImageUrl);
                }

                card.ImageUrl = UploadImage(card.FileInput);
            }

            _cardRepository.UpdateCard(card);
        }

        private string UploadImage(HttpPostedFileBase fileInput)
        {
            var imagePhysicalPath = _fileService.GetPhysicalPath(HttpContext.Current, CardImagesServerPath);
            return Path.Combine(CardImagesServerPath, _fileService.UploadToServerPath(imagePhysicalPath, fileInput));
        }

        private Card ConverToMixedLeagueCard(Card card)
        {
            var result = new Card
                             {
                                 CardId = card.CardId,
                                 Category = "Mixed",
                                 Description = card.Description,
                                 ImageUrl = card.ImageUrl,
                                 Player = card.Player,
                                 Price = card.Price,
                                 Quantity = card.Quantity
                             };
            return result;
        }

        public void Dispose()
        {
            _cardRepository.Dispose();
        }
    }
}