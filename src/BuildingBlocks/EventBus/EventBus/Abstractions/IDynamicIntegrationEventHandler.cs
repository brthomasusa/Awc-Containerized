﻿namespace Awc.BuildingBlocks.EventBus.EventBus.Abstractions;

public interface IDynamicIntegrationEventHandler
{
    Task Handle(dynamic eventData);
}
