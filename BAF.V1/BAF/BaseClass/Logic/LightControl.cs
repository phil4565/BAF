using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BAF.Div.Div;

namespace BAF.BaseClass.Logic
{
    public class LightControl
    {
        public static void SetColors(int channel, List<string> bools)
        {
            OpenDMX.setDmxValue(channel, 0); //auto off
            foreach (string i in bools)
            {
                switch (i)
                {
                    case "Auto": OpenDMX.setDmxValue(channel, 0); break;
                    case "Red": OpenDMX.setDmxValue(channel, 6); break;
                    case "Green": OpenDMX.setDmxValue(channel, 15); ; break;
                    case "Blue": OpenDMX.setDmxValue(channel, 20); break;
                    case "White": OpenDMX.setDmxValue(channel, 25); ; break;
                }
            }
        }
    }
}
