﻿namespace Awc.BuildingBlocks.EventBus.EventBus;

public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
{
    public sealed class SubscriptionInfo
    {
        public bool IsDynamic { get; }
        public Type HandlerType { get; }

        private SubscriptionInfo(bool isDynamic, Type handlerType)
        {
            IsDynamic = isDynamic;
            HandlerType = handlerType;
        }

        public static SubscriptionInfo Dynamic(Type handlerType) =>
            new(true, handlerType);

        public static SubscriptionInfo Typed(Type handlerType) =>
            new(false, handlerType);
    }
}
