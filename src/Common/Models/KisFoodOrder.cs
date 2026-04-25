namespace KisV4.Common.Models;

public class KisFoodOrderRequest {
    public required int[] ProductIds { get; set; }
    public required int[] ProductQuantities { get; set; }
    public string?[]? ProductNotes { get; set; }
    public int[]? PreferredQueues { get; set; }
    public string? CustomerName { get; set; }
    public string? OrderNote { get; set; }
    public string? OrderId { get; set; }
}

public class KisFoodQueueItemDetails {
    public required int Id { get; set; }
    public required int Number { get; set; }
    public required int QueueId { get; set; }
    public required string QueueName { get; set; }
    public required long Timestamp { get; set; }
    public required int ProductCount { get; set; }
    public required KisFoodProductInfo Product { get; set; }
    public KisFoodQueueItemState State { get; set; } = KisFoodQueueItemState.InPreparation;
    public bool Printed { get; set; }
    public string? CustomerName { get; set; }
    public string? Note { get; set; }
}

public class KisFoodProductInfo {
    public required int Id { get; set; }
    public required string Name { get; set; }
}

public enum KisFoodQueueItemState {
    InPreparation,
    ReadyToCollect,
    Completed,
    Cancelled
}

