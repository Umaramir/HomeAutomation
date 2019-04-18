using BMC.Sarah.ChatBot.Helpers;
//using BMC.Security.Web.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BMC.Security.Models;
using BMC.Sarah.ChatBot.Services;
//using BMC.Sarah;

namespace BMC.Sarah.ChatBot.Dialogs
{
    public class SecurityDeviceDialog : IDialog<object>
    {
        const string Monster = "Monster Voice";
        const string Scream = "Scream Voice";
        const string Tornado = "Tornado Voice";
        const string Police = "Police Voice";

        static MqttService iot;

        public async Task StartAsync(IDialogContext context)
        {
            this.ShowOptions(context);
            iot = new MqttService();
        }
        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.TurnOnDevice, new List<string>() { Monster, Scream, Tornado, Police }, "Worry about home?", "Sorry I'm not focus (ಥ﹏ಥ) What again?", 4);
        }
        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.PostAsync($"Okeyyy (~˘▾˘)~ ");
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
                        //iot = new MqttService();
                        //Activity reply = context.MakeMessage() as Activity;
                        /*reply.Attachments.Add(GetAudioCard().ToAttachment());
                        await context.PostAsync(reply);
                        context.Wait(MessageReceivedAsync); */
                        await iot.InvokeMethod("BMCSecurityBot", "PlaySound", new string[] { "monster.mp3" });
                        context.Done<object>(null);
                        break;
                    case Scream:
                        await iot.InvokeMethod("BMCSecurityBot", "PlaySound", new string[] { "scream.mp3" });
                        context.Done<object>(null);
                        break;
                    case Tornado:
                        await iot.InvokeMethod("BMCSecurityBot", "PlaySound", new string[] { "tornado.mp3" });
                        context.Done<object>(null);
                        break;
                    case Police:
                        await iot.InvokeMethod("BMCSecurityBot", "PlaySound", new string[] { "police.mp3" });
                        context.Done<object>(null);
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