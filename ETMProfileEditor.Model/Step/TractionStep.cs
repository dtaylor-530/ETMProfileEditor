using System.Collections.Generic;

namespace ETMProfileEditor.Model
{
    public class TractionStep : Step
    {
        public double RollingSpeed { get; set; }
        public double BallLoad { get; set; }
        public bool UnloadAtEnd { get; set; }
        public bool MeasureDiscTrackRadius { get; set; }
        public List<double> SlideRollRatios { get; set; }
        public bool TemperatureControlEnabled { get; set; }
        public ControlProbe TemperatureControlProbe { get; set; }
        public double Temperature { get; set; }
        public bool WaitForTemperatureToStabilise { get; set; }
        public double IdleSpeed { get; set; }
        public double IdleSlideRollRatio { get; set; }
        public double IdleLoad { get; set; }
    }
}