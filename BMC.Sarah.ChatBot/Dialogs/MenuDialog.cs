namespace BMC.Sarah.ChatBot.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using BMC.Sarah.ChatBot.Helpers;
    using Microsoft.Bot.Connector;
    using BMC.Security.Web.Helpers;
    using Microsoft.Bot.Builder.Luis;

    [Serializable]
    [LuisModel("36e195ea-3d25-4d11-b509-910d5ee15aa9", "52a0ca570af84ed5a7e770244223d4be")]//, LuisApiVersion.V2, "westus.api.cognitive.microsoft.com", Staging = false)]
    public class MenuDialog : IDialog<object>
    {
        
        const string TurnOn = "TurnOn";
        const string TurnOff = "TurnOff";
        const string Entertain = "Entertain";

        const string BMCDevice = "BMSpace Tools";
        const string IOTDevice = "IOT Tools";

        const string Ga = "Nope";
        const string Ya = "Let see";

        public async Task StartAsync(IDialogContext context)
        {
            this.ShowOptions(context);
        }
        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text.ToLower().Contains("tolong") || message.Text.ToLower().Contains("bantuan") || message.Text.ToLower().Contains("help") || message.Text.ToLower().Contains("support") || message.Text.ToLower().Contains("problem"))
            {
                await context.PostAsync($"Dialog Forced Close :(");
            }
            else
            {
                this.ShowOptions(context);
            }
        }
        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { TurnOn, TurnOff, Entertain }, "You can choose (ᵔᴥᵔ)", "Sorry I'm not focus (ಥ﹏ಥ) What again?", 2);
        }
        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case TurnOn:
                        context.Call(new TurnOnDialog(), ResumeAfterOptionDialog);
                        break;
                    case TurnOff:
                        context.Call(new TurnOffDialog(), ResumeAfterOptionDialog);
                        break;
                    case Entertain:
                        context.Call(new EntertainDialog(), ResumeAfterOptionDialog);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync(MESSAGESINFO.TOO_MANY_ATTEMPT);

                context.Wait(this.MessageReceivedAsync);
            }
        }
        //[LuisIntent("TurnOn")]
        //private async Task TurnOnDevice(IDialogContext context, IAwaitable<string> result)
        //{
        //    try
        //    {
        //        string optionSelected = await result;
        //        switch (optionSelected)
        //        {
        //            case BMCDevice:
        //                var data = DeviceData.GetAllDevices();
        //                Activity replyToConversation = context.MakeMessage() as Activity;
        //                replyToConversation.Attachments = new List<Attachment>();
        //                replyToConversation.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        //                var img = "https://padidata.blob.core.windows.net/amirruinabotblob/sarah.png";
        //                foreach (var item in data)
        //                {
        //                    List<CardAction> cardButtons = new List<CardAction>();
        //                    CardAction button = new CardAction()
        //                    {
        //                        Type = "openUrl",
        //                        Title = "TurnOn",
        //                        Value = $"http://{item.IP}/cm?cmnd=Power%20On"
        //                    };
        //                    cardButtons.Add(button);

        //                    CardImage cardImage = null;
        //                    if (!string.IsNullOrEmpty(img))
        //                    {
        //                        cardImage = string.IsNullOrEmpty(img) ? new CardImage(img) :
        //                             new CardImage(img);
        //                    }

        //                    ThumbnailCard card = new ThumbnailCard()
        //                    {
        //                        Title = item.Name,
        //                        //Subtitle = $"{item.Name}",
        //                        //Text = item.Name,
        //                        Images = cardImage == null ? null : new CardImage[] { cardImage },
        //                        Buttons = cardButtons
        //                    };
        //                    replyToConversation.Attachments.Add(card.ToAttachment());
        //                }
        //                await context.PostAsync(replyToConversation);
        //                PromptDialog.Choice(context, this.ResumeTurnONBMCDevice, new List<string>() { Ga, Ya }, "Ada yang mau dihidukan lagi?", "Silahkan pilih ulang", 2);
        //                break;
        //        }
        //    }
        //    catch (TooManyAttemptsException ex)
        //    {
        //        await context.PostAsync(MESSAGESINFO.TOO_MANY_ATTEMPT);

        //        context.Wait(this.MessageReceivedAsync);
        //    }
        //}

        //private async Task TurnOffDevice(IDialogContext context, IAwaitable<string> result)
        //{
        //    try
        //    {
        //        string optionSelected = await result;
        //        switch (optionSelected)
        //        {
        //            case BMCDevice:
        //                var data = DeviceData.GetAllDevices();
        //                Activity replyToConversation = context.MakeMessage() as Activity;
        //                replyToConversation.Attachments = new List<Attachment>();
        //                replyToConversation.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        //                var img = "https://padidata.blob.core.windows.net/amirruinabotblob/sarah.png";
        //                foreach (var item in data)
        //                {
        //                    List<CardAction> cardButtons = new List<CardAction>();
        //                    CardAction button = new CardAction()
        //                    {
        //                        Type = "openUrl",
        //                        Title = "TurnOff",
        //                        Value = $"http://{item.IP}/cm?cmnd=Power%20Off "
        //                    };
        //                    cardButtons.Add(button);

        //                    CardImage cardImage = null;
        //                    if (!string.IsNullOrEmpty(img))
        //                    {
        //                        cardImage = string.IsNullOrEmpty(img) ? new CardImage(img) :
        //                             new CardImage(img);
        //                    }

        //                    ThumbnailCard card = new ThumbnailCard()
        //                    {
        //                        Title = item.Name,
        //                        //Subtitle = $"{item.Name}",
        //                        //Text = item.Name,
        //                        Images = cardImage == null ? null : new CardImage[] { cardImage },
        //                        Buttons = cardButtons
        //                    };
        //                    replyToConversation.Attachments.Add(card.ToAttachment());
        //                }
        //                await context.PostAsync(replyToConversation);
        //                PromptDialog.Choice(context, this.ResumeTurnOffBMCDevice, new List<string>() { Ga, Ya }, "Ada yang mau di matiin lagi?", "Silahkan pilih ulang", 2);
        //                break;
        //        }
        //    }
        //    catch (TooManyAttemptsException ex)
        //    {
        //        await context.PostAsync(MESSAGESINFO.TOO_MANY_ATTEMPT);

        //        context.Wait(this.MessageReceivedAsync);
        //    }
        //}

        //private async Task ResumeTurnONBMCDevice(IDialogContext context, IAwaitable<string> result)
        //{
        //    try
        //    {
        //        string optionSelected = await result;
        //        switch (optionSelected)
        //        {
        //            case Ga:
        //                var Msg = "";
        //                context.Call(new CommonResponseDialog(Msg), this.ResumeAfterOptionDialog);
        //                context.Done<object>(null);
        //                break;
        //            case Ya:
        //                PromptDialog.Choice(context, this.TurnOnDevice, new List<string>() { BMCDevice, IOTDevice }, "Nyalain apa?", "wah ada kesalahan nih, pilih ulang ya", 2);
        //                break;
        //        }
        //    }
        //    catch (TooManyAttemptsException ex)
        //    {
        //        await context.PostAsync(MESSAGESINFO.TOO_MANY_ATTEMPT);

        //        context.Wait(this.MessageReceivedAsync);
        //    }
        //}
        //private async Task ResumeTurnOffBMCDevice(IDialogContext context, IAwaitable<string> result)
        //{
        //    try
        //    {
        //        string optionSelected = await result;
        //        switch (optionSelected)
        //        {
        //            case Ga:
        //                var Msg = "";
        //                context.Call(new CommonResponseDialog(Msg), this.ResumeAfterOptionDialog);
        //                context.Done<object>(null);
        //                break;
        //            case Ya:
        //                PromptDialog.Choice(context, this.TurnOffDevice, new List<string>() { BMCDevice, IOTDevice }, "Matikan apa?", "wah ada kesalahan nih, pilih ulang ya", 2);
        //                break;
        //        }
        //    }
        //    catch (TooManyAttemptsException ex)
        //    {
        //        await context.PostAsync(MESSAGESINFO.TOO_MANY_ATTEMPT);

        //        context.Wait(this.MessageReceivedAsync);
        //    }
        //}
        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            //var message = await result;
            //context.Done<object>(null);
            try
            {
                var message = await result;
                await context.PostAsync($"Thats it? Okey (づ￣ ³￣)づ ");
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Uhh hmm I'm trying to fix : {ex.Message}");
            }
            finally
            {
                context.Wait(this.MessageReceivedAsync);
                context.Done<object>(null);
            }
        }
    }
}