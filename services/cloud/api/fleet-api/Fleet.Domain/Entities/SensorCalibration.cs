using System;

namespace Fleet.Domain.Entities
{
    public class SensorCalibration
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid SensorId { get; set; }

        public int CalibrationVersion { get; set; }

        public DateTime CalibrationDate { get; set; }

        public string CalibrationMethod { get; set; }

        public string PerformedBy { get; set; }
            
        public DateTime CreatedAtUtc { get; set; }

        public DateTime UpdatedAtUtc { get; set; }

        public Sensor Sensor { get; set; }
    }
}
