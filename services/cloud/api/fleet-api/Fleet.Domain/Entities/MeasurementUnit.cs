namespace Fleet.Domain.Entities;

public class MeasurementUnit
{
    public Guid Id { get; private set; }

    public string Code { get; private set; } = default!; // e.g. "ug/m3", "mph", "deg", "ppb"

    public string UnitName { get; private set; } = default!; // e.g. "Micrograms per cubic meter"

    public string? Description { get; private set; }

    public int DisplayOrder { get; private set; }

    private MeasurementUnit() { } // For EF Core
}