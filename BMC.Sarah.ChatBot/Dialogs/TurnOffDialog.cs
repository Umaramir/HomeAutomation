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
    public class TurnOffDialog : IDialog<object>
    {
        const string BMCDevice = "Home Tools";
        const string IOTDevice = "IOT Device";

        const string Ga = "Nope";
        const string Ya = "Let see";

        public async Task StartAsync(IDialogContext context)
        {
            //this.ShowOptions(context);
            await context.PostAsync("Type the SpellWord");
            context.Wait(this.AuthTurnOff);
        }
        public virtual async Task AuthTurnOff(IDialogContext context, IAwaitable<IMessageActivity> result)
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
            PromptDialog.Choice(context, this.TurnOffDevice, new List<string>() { BMCDevice, IOTDevice }, "Need to shutdown something?", "Sorry I'm not focus (ಥ﹏ಥ) What again?", 2);
        }
        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            //await context.PostAsync($"");
            context.Done<object>(null);
        }
        private async Task TurnOffDevice(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;
                switch (optionSelected)
                {
                    case BMCDevice:
                        var data = DeviceData.GetAllDevices();
                        var repvoice = context.Activity as Activity;
                        //Activity reply = repvoice.CreateReply($"Ok !");
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
                                Title = "TurnOff",
                                Value = $"http://{item.IP}/cm?cmnd=Power%20Off "
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
                        //reply.Speak = "Device turned off";
                        //await context.PostAsync(reply);
                        //await context.SayAsync(text: "Device turned off", speak: "Device turned off");
                        PromptDialog.Choice(context, this.ResumeTurnOffBMCDevice, new List<string>() { Ga, Ya }, "Anything else?", "Oh God, Sorry I'm not focus (ಥ﹏ಥ) What again?", 2);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync(MESSAGESINFO.TOO_MANY_ATTEMPT);

                context.Wait(this.MessageReceivedAsync);
            }
        }
        private async Task ResumeTurnOffBMCDevice(IDialogContext context, IAwaitable<string> result)
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
                        PromptDialog.Choice(context, this.TurnOffDevice, new List<string>() { BMCDevice, IOTDevice }, "Need to shutdown something?", "Sorry I'm not focus (ಥ﹏ಥ) What again?", 2);
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