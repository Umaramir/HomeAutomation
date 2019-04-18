using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BMC.Sarah.ChatBot.Dialogs
{
    [Serializable]
    public class KitchenFan : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(ToiletLampu);
        }
        private async Task ToiletLampu(IDialogContext context, IAwaitable<object> result)
        {

            Activity reply = ((Activity)context.Activity).CreateReply();
            reply.AttachmentLayout = AttachmentLayoutTypes.List;
            var img = "https://padidata.blob.core.windows.net/amirruinabotblob/sarah.png";

            List<CardAction> cardButtons = new List<CardAction>();
            CardAction buttonON = new CardAction()
            {
                Type = "openUrl",
                Title = "TurnOn",
                Value = $"http://192.168.1.33/cm?cmnd=Power%20On"
            };
            cardButtons.Add(buttonON);

            CardAction buttonOFF = new CardAction()
            {
                Type = "openUrl",
                Title = "TurnOff",
                Value = $"http://192.168.1.33/cm?cmnd=Power%20Off"
            };
            cardButtons.Add(buttonOFF);

            CardImage cardImage = null;
            if (!string.IsNullOrEmpty(img))
            {
                cardImage = string.IsNullOrEmpty(img) ? new CardImage(img) :
                     new CardImage(img);
            }
            ThumbnailCard card = new ThumbnailCard()
            {
                Title = "Kitchen Fan",
                Images = cardImage == null ? null : new CardImage[] { cardImage },
                Buttons = cardButtons
            };
            reply.Attachments.Add(card.ToAttachment());
            await context.PostAsync(reply);
        }
    }
}