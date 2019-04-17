using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BMC.Sarah.ChatBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BMC.Sarah.ChatBot.Helpers
{
    public static class CardUtil
    {
        public static async Task ShowSearchResults(IDialogContext context, SearchResult searchResult, string notResultsMessage)
        {
            Activity reply = ((Activity)context.Activity).CreateReply();

            await CardUtil.ShowSearchResults(reply, searchResult, notResultsMessage);
        }

        public static async Task ShowSearchResults(Activity reply, SearchResult searchResult, string notResultsMessage)
        {
            if (searchResult.Value.Length != 0)
            {
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;



                foreach (SearchResultHit item in searchResult.Value)
                {
                    List<CardAction> cardButtons = new List<CardAction>();
                    CardAction button = new CardAction()
                    {
                        Value = $"Lihat artikel {item.Title}",
                        Type = "imBack",
                        Title = "Selengkapnya"

                        //Type = "messageBack",
                        //Title = "Selengkapnya",
                        //DisplayText = "Baca Selengkapnya",
                        //Text = "Sahabat baru saja melihat sleengkapnya dari isu ini",
                        //Value = $"Isi: {item.Description}"
                    };
                    cardButtons.Add(button);
                    var cardImage = string.IsNullOrEmpty(item.ImageUrl) ? new CardImage("https://raw.githubusercontent.com/GeekTrainer/help-desk-bot-lab/master/assets/botimages/head-smiling-medium.png") :
                         new CardImage(item.ImageUrl);


                    ThumbnailCard card = new ThumbnailCard()
                    {
                        Title = item.Title,
                        Subtitle = $"Kategori: {item.Category} | Skor Pencarian: {item.SearchScore}",
                        Text = item.Description.Length > 550 ? item.Description.Substring(0, 550) + "..." : item.Description,
                        Images = new CardImage[] { cardImage },
                        Buttons = cardButtons
                    };
                    reply.Attachments.Add(card.ToAttachment());
                }

                ConnectorClient connector = new ConnectorClient(new Uri(reply.ServiceUrl));
                await connector.Conversations.SendToConversationAsync(reply);
            }
            else
            {
                reply.Text = notResultsMessage;
                ConnectorClient connector = new ConnectorClient(new Uri(reply.ServiceUrl));
                await connector.Conversations.SendToConversationAsync(reply);
            }
        }
    }
}