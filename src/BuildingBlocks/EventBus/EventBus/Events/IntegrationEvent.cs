﻿using Newtonsoft.Json;
using System;

namespace CodeChallenge.BuildingBlocks.EventBus.Events
{
    public record IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        [JsonProperty]
        public Guid Id { get; private init; }

        [JsonProperty]
        public DateTime CreationDate { get; private init; }
    }
}
