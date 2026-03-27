using System;

namespace Fleet.Domain.Entities
{
    public class Station
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid SiteId { get; set; }

        public Guid StationStatusId { get; set; }

        public string StationCode { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public DateTime UpdatedAtUtc { get; set; }

        public StationStatus Status { get; set; }

        public Site Site { get; set; }

        public ICollection<Sensor> Sensors { get; set; } = [];

        public ICollection<Device> Devices { get; set; } = [];
    }
}