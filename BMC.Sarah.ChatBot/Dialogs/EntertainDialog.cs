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
    public class EntertainDialog : IDialog<object>
    {
        const string RelaxMsc = "Relaxing Music";
        const string UpliftMsc = "Uplift Your Spirit";

        public async Task StartAsync(IDialogContext context)
        {
            this.ShowOptions(context);
        }
        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.SelectMusic, new List<string>() { RelaxMsc, UpliftMsc }, "Need to cooldown mate?", "Sorry I'm not focus (ಥ﹏ಥ) What again?", 2);
        }
        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            //await context.PostAsync($"");
            context.Done<object>(null);
        }
        private async Task SelectMusic (IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;
                switch(optionSelected)
                {
                    case RelaxMsc:
                        var data = MusicList.GetAllMusicRelax();
                        Activity reply = context.MakeMessage() as Activity;
                        //reply.Attachments.Add(GetAudioRelaxCard().ToAttachment());
                        reply.Attachments = new List<Attachment>();
                        reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                        foreach (var item in data)
                        {
                            var audioCard = new AudioCard
                            {
                                Title = item.Title,
                                Subtitle = item.SubTitle,
                                Text = item.Text,
                                Image = new ThumbnailUrl
                                {
                                    Url = item.Img,
                                },
                                Media = new List<MediaUrl>
                                {
                                  new MediaUrl()
                                     {
                                        Url = item.Url,
                                     }
                                }
                            };
                            reply.Attachments.Add(audioCard.ToAttachment());
                        }
                        await context.PostAsync(reply);
                        context.Wait(MessageReceivedAsync);
                        break;
                    case UpliftMsc:
                        var dataRock = MusicList.GetAllMusicSpirit();
                        Activity replyRock = context.MakeMessage() as Activity;
                        //reply.Attachments.Add(GetAudioRelaxCard().ToAttachment());
                        replyRock.Attachments = new List<Attachment>();
                        replyRock.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                        foreach (var item in dataRock)
                        {
                            var audioCard = new AudioCard
                            {
                                Title = item.Title,
                                Subtitle = item.SubTitle,
                                Text = item.Text,
                                Image = new ThumbnailUrl
                                {
                                    Url = item.Img,
                                },
                                Media = new List<MediaUrl>
                                {
                                  new MediaUrl()
                                     {
                                        Url = item.Url,
                                     }
                                }
                            };
                            replyRock.Attachments.Add(audioCard.ToAttachment());
                        }
                        await context.PostAsync(replyRock);
                        context.Wait(MessageReceivedAsync);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync(MESSAGESINFO.TOO_MANY_ATTEMPT);

                context.Wait(this.MessageReceivedAsync);
            }
        }
        //private static AudioCard GetAudioRelaxCard()
        //{
        //    var data = MusicList.GetAllMusicLists();
        //    foreach(var item in data)
        //    {
        //        var audioCard = new AudioCard
        //        {
        //            Title = item.Title,
        //            Subtitle = item.SubTitle,
        //            Text = item.Text,
        //            Image = new ThumbnailUrl
        //            {
        //                Url = item.Img,
        //            },
        //            Media = new List<MediaUrl>
        //        {
        //            new MediaUrl()
        //            {
        //                Url = item.Url,
        //            },
        //        }
        //            //Buttons = new List<CardAction>
        //            //{
        //            //    new CardAction()
        //            //    {
        //            //        Title = "Read More",
        //            //        Type = ActionTypes.OpenUrl,
        //            //        Value = "https://en.wikipedia.org/wiki/The_Empire_Strikes_Back",
        //            //    },
        //            //},
        //        };
        //    }
        //}
    }
}