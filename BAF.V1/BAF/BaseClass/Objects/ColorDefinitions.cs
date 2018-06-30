using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAF.BaseClass.Objects
{
    public class LightDefinitions
    {
        public class CosmoLight
        {
            public int CosmoLightID { get; set; }
            public int StartChannel { get; set; }
            public List<Color> Colors { get; set; }
            public int AllLedsOffset { get; set; }
            public int AutoOffset { get; set; }
            public int IntensityOffset { get; set; }
            public int TurnOffset { get; set; }
            public int RotateOffset { get; set; }
            public int SpeedOffset { get; set; }
            public List<Movement> Movements { get; set; }
        }

        public class Color
        {
            public byte ColorValue { get; set; }
            public string ColorName { get; set; }
        }

        public class Movement
        {
            public string Axis { get; set; }
            public byte MovementDirection { get; set; }
            public byte MovementSpeed { get; set; }
            public string MovementName { get; set; }
        }
    }
}
