using ThinkUp.Sdk.Components;
using ThinkUp.Sdk.Services;

namespace ThinkUp.Sdk.Modules
{
    public class ThinkUpModuleFactory: IModuleFactory
    {
        private readonly IUserService userService;
        private readonly IConversationService conversationService;
        private readonly INotificationService notificationService;
        private readonly ISerializer serializer;

        public ThinkUpModuleFactory(IUserService userService, IConversationService conversationService, 
            INotificationService notificationService, ISerializer serializer)
        {
            this.userService = userService;
            this.conversationService = conversationService;
            this.notificationService = notificationService;
            this.serializer = serializer;
        }

        public IModule Create()
        {
            var thinkUpModule = new Module(this.serializer);

            thinkUpModule.RegisterComponent(new UsersComponent(this.userService, this.notificationService, this.serializer));
            thinkUpModule.RegisterComponent(new ConversationsComponent(this.conversationService, this.notificationService, this.serializer));

            return thinkUpModule;
        }
    }
}
