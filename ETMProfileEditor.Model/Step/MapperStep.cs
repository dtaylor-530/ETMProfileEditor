namespace ETMProfileEditor.Model
{
    public class MapperStep : Step
    {
        public double CameraWindowLoad { get; set; }

        public bool TemperatureControlEnabled { get; set; }

        public ControlProbe TemperatureControlProbe { get; set; }
        public double Temperature { get; set; }
        public double IdleSpeed { get; set; }
    }
}