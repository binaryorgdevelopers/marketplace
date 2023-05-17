namespace IntegrationEventLogEF;

public enum EventStateEnum
{
    NotPublished = 0,
    InProgress = 1,
    Published = 2,
    PublishedFail = 3
}