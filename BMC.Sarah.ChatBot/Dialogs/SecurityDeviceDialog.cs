using BMC.Sarah.ChatBot.Helpers;
using BMC.Security.Web.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BMC.Sarah.ChatBot.Dialogs
{
    public class SecurityDeviceDialog : IDialog<object>
    {
        const string Monster = "Monster Voice";

        static MqttService iot;

        public async Task StartAsync(IDialogContext context)
        {
            this.ShowOptions(context);
        }
        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.TurnOnDevice, new List<string>() { Monster }, "Worry about home?", "Sorry I'm not focus (ಥ﹏ಥ) What again?", 2);
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
                    case Monster:
                        Activity reply = context.MakeMessage() as Activity;
                        /*reply.Attachments.Add(GetAudioCard().ToAttachment());
                        await context.PostAsync(reply);
                        context.Wait(MessageReceivedAsync); */
                        await iot.InvokeMethod("BMCSecurityBot", "PlaySound", new string[] { "monster.mp3" });
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync(MESSAGESINFO.TOO_MANY_ATTEMPT);

                context.Wait(this.MessageReceivedAsync);
            }
        }

        /*private static AudioCard GetAudioCard()
        {
            var audioCard = new AudioCard
            {
                Title = "Monster",
                Subtitle = "Monster Voice",
                Text = "aaarrggghhhh",
                Image = new ThumbnailUrl
                {
                    Url = "https://padidata.blob.core.windows.net/files/emoticon-scared.jpg",
                },
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = "https://padidata.blob.core.windows.net/files/monster.mp3",
                    },
                },
                Buttons = new List<CardAction>
                {
                    new CardAction()
                    {
                        Title = "Read More",
                        Type = ActionTypes.OpenUrl,
                        Value = "https://en.wikipedia.org/wiki/The_Empire_Strikes_Back",
                    },
                },
            };

            return audioCard;
        }*/
    }
}