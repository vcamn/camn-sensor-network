using System;

namespace Fleet.Domain.Entities
{
    public class Sensor
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid DeviceId { get; set; }

        public string SensorIdentifier { get; set; }

        public string SensorType { get; set; }

        public string UnitOfMeasure { get; set; }

        public DateTime? CalibrationDate { get; set; }

        public Device Device { get; set; }

        public ICollection<SensorCalibration> SensorCalibrations { get; set; }
    }
}