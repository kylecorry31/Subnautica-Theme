using System;
using System.Collections.Generic;
using System.Text;

namespace SubnauticaTheme
{
    public class ObservationsDto
    {
        //public List<ObservationsFeaturesDto> Features { get; set; }
        public ObservationPropertiesDto Properties { get; set; }
    }

    public class ObservationsFeaturesDto
    {
        public ObservationPropertiesDto Properties {get; set;}
    }

    public class ObservationPropertiesDto
    {
        public ObservationValueDto Temperature { get; set; }
        public ObservationValueDto WindSpeed { get; set; }
        public ObservationValueDto BarometricPressure { get; set; }
        public ObservationValueDto SeaLevelPressure { get; set; }

        public ObservationValueDto RelativeHumidity { get; set; }
    }

    public class ObservationValueDto
    {
        public double? Value { get; set; }
    }
}
