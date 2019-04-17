using Autofac;
using BMC.Sarah.ChatBot.Dialogs;
using BMC.Sarah.ChatBot.HandOff;
using BMC.Sarah.ChatBot.Helpers;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace BMC.Sarah.ChatBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Microsoft.Bot.Builder.Dialogs.Conversation.UpdateContainer(
            builder =>
            {
                builder.RegisterModule(new AzureModule(Assembly.GetExecutingAssembly()));

                // Bot Storage: Here we register the state storage for your bot. 
                // Default store: volatile in-memory store - Only for prototyping!
                // We provide adapters for Azure Table, CosmosDb, SQL Azure, or you can implement your own!
                // For samples and documentation, see: [https://github.com/Microsoft/BotBuilder-Azure](https://github.com/Microsoft/BotBuilder-Azure)
                //var store = new InMemoryDataStore();

                // Other storage options
                var store = new TableBotDataStore(ConfigurationManager.AppSettings["StorageConnectionString"]); // requires Microsoft.BotBuilder.Azure Nuget package 
                // var store = new DocumentDbBotDataStore("cosmos db uri", "cosmos db key"); // requires Microsoft.BotBuilder.Azure Nuget package 
                builder.RegisterModule(new ReflectionSurrogateModule());
                builder.RegisterModule<GlobalMessageHandlersBotModule>();

                // Hand Off Scorables, Provider and UserRoleResolver
                builder.Register(c => new RouterScorable(c.Resolve<IBotData>(), c.Resolve<ConversationReference>(), c.Resolve<Provider>()))
                    .As<IScorable<IActivity, double>>().InstancePerLifetimeScope();
                builder.Register(c => new CommandScorable(c.Resolve<IBotData>(), c.Resolve<ConversationReference>(), c.Resolve<Provider>()))
                    .As<IScorable<IActivity, double>>().InstancePerLifetimeScope();
                builder.RegisterType<Provider>()
                    .SingleInstance();

                // Bot Scorables
                //builder.Register(c => new AgentLoginScorable(c.Resolve<IBotData>(), c.Resolve<Provider>()))
                //    .As<IScorable<IActivity, double>>()
                //    .InstancePerLifetimeScope();

                builder.RegisterType<SearchScorable>()
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();

                builder.RegisterType<ShowArticleDetailsScorable>()
                    .As<IScorable<IActivity, double>>()
                    .InstancePerLifetimeScope();


                builder.Register(c => store)
                    .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                    .AsSelf()
                    .SingleInstance();
            });
        }
    }
}
