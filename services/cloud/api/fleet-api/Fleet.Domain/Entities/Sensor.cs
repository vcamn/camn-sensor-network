using System;

namespace Fleet.Domain.Entities
{
    public class Sensor
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid SensorTypeId { get; set; } // ["AethLabs", "AggieAir", "LTwind", "PurpleAir", "Other"]

        public Guid StationId { get; set; }

        public Guid? DeviceId { get; set; }

        public Guid SensorStatusId { get; set; }
        
        public string SensorIdentifier { get; set; }
        
        public string UnitOfMeasure { get; set; }

        public DateTime? CalibrationDate { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public DateTime UpdatedAtUtc { get; set; }

        public SensorStatus Status { get; set; }

        public SensorType SensorType { get; set; }

        public Station Station { get; set; }

        public Device? Device { get; set; }

        public ICollection<SensorIntegration> SensorIntegrations { get; set; } = [];

        public ICollection<SensorCalibration> SensorCalibrations { get; set; } = [];
    }
}