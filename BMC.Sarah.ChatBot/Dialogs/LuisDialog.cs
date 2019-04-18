using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Threading;

namespace BMC.Sarah.ChatBot.Dialogs
{
    [Serializable]
    [LuisModel("36e195ea-3d25-4d11-b509-910d5ee15aa9", "52a0ca570af84ed5a7e770244223d4be")]//, LuisApiVersion.V2, "westus.api.cognitive.microsoft.com", Staging = false)]
    public class LuisDialog : LuisDialog<object>
    {
        [LuisIntent("SayHello")]
        public async Task SayHello(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Hi there (づ｡◕‿‿◕｡)づ ");
            //context.Done<object>(null);
            context.Wait(this.MessageReceived);
        }
        [LuisIntent("ToiletLamp")] 
        public async Task ToiletLamp(IDialogContext context, LuisResult result)
        {
            context.Call(new ToiletLamp(), ResumeAndEndMenuDialogAsync);
        }
        [LuisIntent("KitchenFan")] 
        public async Task KitchenFan(IDialogContext context, LuisResult result)
        {
            context.Call(new KitchenFan(), ResumeAndEndMenuDialogAsync);
        }
        [LuisIntent("SignIn")]
        public async Task SignIn(IDialogContext context, LuisResult result)
        {
            context.Call(new AuthDialog(), ResumeAndEndMenuDialogAsync);
        }
        [LuisIntent("Thanks")]
        public async Task Thanks(IDialogContext context, LuisResult result)
        {
            await context.SayAsync(text: "You're welcome (づ￣ ³￣)づ", speak: "You're welcome");
            //context.Done<object>(null);
            context.Wait(this.MessageReceived);
        }
        [LuisIntent("TurnOn")]
        public async Task TurnOn(IDialogContext context, LuisResult result)
        {
            context.Call(new TurnOnDialog(), ResumeAndEndMenuDialogAsync);
        }
        [LuisIntent("TurnOff")]
        public async Task TurnOff(IDialogContext context, LuisResult result)
        {
            context.Call(new TurnOffDialog(), ResumeAndEndMenuDialogAsync);
        }
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"What? I don't get it (¬_¬)");
            //context.Done<object>(null);
            context.Wait(this.MessageReceived);
        }
        private async Task ResumeAndEndMenuDialogAsync(IDialogContext context, IAwaitable<object> argument)
        {
            context.Done<object>(null);
        }
    }
}