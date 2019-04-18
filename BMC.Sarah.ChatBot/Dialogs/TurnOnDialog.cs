namespace BMC.Sarah.ChatBot.Dialogs
{
    using BMC.Sarah.ChatBot.Helpers;
    using BMC.Security.Web.Helpers;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    [Serializable]
    public class TurnOnDialog : IDialog<object>
    {
        static MqttService iot;
        
        const string BMCDevice = "Home Tools";
        const string IOTDevice = "IOT Device";

        const string BMSecurity = "Security Device";

        const string Ga = "Nope";
        const string Ya = "Let see";

        public async Task StartAsync(IDialogContext context)
        {
            //this.ShowOptions(context);
            await context.PostAsync("Type the SpellWord");
            context.Wait(this.AuthTurnOn);
        }
        public virtual async Task AuthTurnOn(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            string InputUser = message.ToString();
            string PassCode = "Champion";
            if (message.Text == PassCode)
            {
                await context.PostAsync($"Hello fellas (👍^-^👍)");
                this.ShowOptions(context);
            }
            else
            {
                await context.PostAsync($"Wrong SpellWords (¬‿¬)" +
                                        $"I am rebooting . . .");
                await context.PostAsync($"Hi there (づ｡◕‿‿◕｡)づ ");
                context.Done<object>(null);
            }
        }
        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.TurnOnDevice, new List<string>() { BMCDevice, IOTDevice }, "Need to turn on something?", "Sorry I'm not focus (ಥ﹏ಥ) What again?", 2);
        }
        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            //await context.PostAsync($"");
            context.Done<object>(null);
        }
        private async Task TurnOnDevice(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;
                switch (optionSelected)
                {
                    case BMCDevice:
                        var data = DeviceData.GetAllDevices();
                        Activity replyToConversation = context.MakeMessage() as Activity;
                        replyToConversation.Attachments = new List<Attachment>();
                        replyToConversation.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                        var img = "https://padidata.blob.core.windows.net/amirruinabotblob/sarah.png";
                        foreach (var item in data)
                        {
                            List<CardAction> cardButtons = new List<CardAction>();
                            CardAction button = new CardAction()
                            {
                                Type = "openUrl",
                                Title = "TurnOn",
                                Value = $"http://{item.IP}/cm?cmnd=Power%20On"
                            };
                            cardButtons.Add(button);

                            CardImage cardImage = null;
                            if (!string.IsNullOrEmpty(img))
                            {
                                cardImage = string.IsNullOrEmpty(img) ? new CardImage(img) :
                                     new CardImage(img);
                            }

                            ThumbnailCard card = new ThumbnailCard()
                            {
                                Title = item.Name,
                                //Subtitle = $"{item.Name}",
                                //Text = item.Name,
                                Images = cardImage == null ? null : new CardImage[] { cardImage },
                                Buttons = cardButtons
                            };
                            replyToConversation.Attachments.Add(card.ToAttachment());
                        }
                        await context.PostAsync(replyToConversation);
                        PromptDialog.Choice(context, this.ResumeTurnONBMCDevice, new List<string>() { Ga, Ya }, "Anything else", "Oh God, Sorry I'm not focus (ಥ﹏ಥ) What again?", 2);
                        break;
                    case IOTDevice:
                        context.Call(new SecurityDeviceDialog(), ResumeAfterOptionDialog);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync(MESSAGESINFO.TOO_MANY_ATTEMPT);

                context.Wait(this.MessageReceivedAsync);
            }
        }
        private async Task ResumeTurnONBMCDevice(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;
                switch (optionSelected)
                {
                    case Ga:
                        var Msg = "";
                        context.Call(new CommonResponseDialog(Msg), this.ResumeAfterOptionDialog);
                        context.Done<object>(null);
                        break;
                    case Ya:
                        PromptDialog.Choice(context, this.TurnOnDevice, new List<string>() { BMCDevice, IOTDevice }, "Need to turn on something?", "Sorry I'm not focus (ಥ﹏ಥ) What again?", 2);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync(MESSAGESINFO.TOO_MANY_ATTEMPT);

                context.Wait(this.MessageReceivedAsync);
            }
        }
        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            //var message = await result;
            //context.Done<object>(null);
            try
            {
                var message = await result;
                await context.PostAsync($"Thats it? Okey (づ￣ ³￣)づ");
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Uhh hmm I'm trying to fix : {ex.Message}");
            }
            finally
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}