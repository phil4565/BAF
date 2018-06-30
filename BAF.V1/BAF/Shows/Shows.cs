using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static BAF.Shows.ShowBools;
using static BAF.Div.Div;

namespace BAF.Shows
{
      class Shows 
    {
        public static Thread FM;
        public static Thread CT;
        public static bool ColorThreadStartet = false;
        public static bool MovementThreadStartet = false;

        public static void Par64_show(int channelDuration = 1000)
        {

            List<int> channel = new List<int>() { 64, 65, 66, 67 };

            int i = 0;
            int y = 1;
            while (Par64_bool == true)
            {
                OpenDMX.setDmxValue(channel[i],0);
                OpenDMX.setDmxValue(channel[y], 255);
                i++;
                if (i > channel.Count() - 1)
                {
                    i = 0;
                }
                y++;
                if (y > channel.Count() - 1)
                {
                    y = 0;
                }
                Thread.Sleep(channelDuration);
            }




            //    OpenDMX.setDmxValue(64, 255);
            //    Thread.Sleep(469);
            //    if (GPar64_bool == false)
            //    {
            //        break;
            //    }
            //    OpenDMX.setDmxValue(64, 0);
            //    OpenDMX.setDmxValue(65, 255);
            //    Thread.Sleep(469);
            //    if (GPar64_bool == false)
            //    {
            //        break;
            //    }
            //    OpenDMX.setDmxValue(65, 0);
            //    OpenDMX.setDmxValue(66, 255);
            //    Thread.Sleep(469);
            //    if (GPar64_bool == false)
            //    {
            //        break;
            //    }
            //    OpenDMX.setDmxValue(66, 0);
            //    OpenDMX.setDmxValue(67, 255);
            //    Thread.Sleep(469);
            //    if (GPar64_bool == false)
            //    {
            //        break;
            //    }
            //    OpenDMX.setDmxValue(64, 0);
            //    OpenDMX.setDmxValue(65, 0);
            //    OpenDMX.setDmxValue(66, 0);
            //    OpenDMX.setDmxValue(67, 0);
            //}
        }

        public static void TurnRotateCosmo(List<BaseClass.Objects.LightDefinitions.CosmoLight> lights, int turnDuration = 2500)
        {
            MainWindow.myLights = lights;
            MainWindow.MovementDuration = turnDuration;
            MovementThreadStartet = true;

            FM = new Thread(MovementThread);
            FM.Start();
        }

        public static void MovementThread()
        {
            int i = 0;

            MovementThreadStartet = true;

            var myLights = MainWindow.myLights;

            while (ShowBools.TurnRotateCosmo == true)
            {
                bool pause = false;
                foreach (var light in myLights)
                {
                    int channel = 0;

                    if (light.Movements[i].Axis == "x")
                    {
                        channel = light.StartChannel + light.TurnOffset;
                        pause = true;
                    }
                    else if (light.Movements[i].Axis == "y")
                    {
                        channel = light.StartChannel + light.RotateOffset;
                    }

                    if (light.Movements[i].MovementSpeed > 0)
                    {
                        OpenDMX.setDmxValue(light.StartChannel + light.SpeedOffset, light.Movements[i].MovementSpeed);
                    }

                    OpenDMX.setDmxValue(channel, light.Movements[i].MovementDirection);
                }

                i++;
                if (i > myLights[0].Movements.Count() - 1)
                {
                    i = 0;
                }

                if (pause)
                {
                    Thread.Sleep(MainWindow.MovementDuration);
                }

                pause = false;
            }
        }

        public static void CosmoColorChangeShow(List<BaseClass.Objects.LightDefinitions.CosmoLight> lights, int colorDuration = 1000) // Color Change
        {
            MainWindow.myLights = lights;
            MainWindow.ColorDuration = colorDuration;
            
            if (ColorThreadStartet)
            {
                CT.Abort();
                ColorThreadStartet = false;
            }

            CT = new Thread(ColorThread);
            CT.Start();
        }

        public static void ColorThread()
        {
            ColorThreadStartet = true;

            var myLights = MainWindow.myLights;

            int i = 0;
            while (CosmoColorChange == true)
            {
                if (i > myLights[0].Colors.Count() - 1)
                {
                    i = 0;
                }
                foreach (var light in myLights)
                {
                    OpenDMX.setDmxValue(light.StartChannel + light.AllLedsOffset, light.Colors[i].ColorValue);
                }

                i++;

                Thread.Sleep(MainWindow.ColorDuration);
            }     
        }

        public static void Show8() // 3 Red
        {
            while (ShowBool8 == true)
            {
                OpenDMX.setDmxValue(175, 6);
                OpenDMX.setDmxValue(178, 6);
                OpenDMX.setDmxValue(177, 6);
                if (ShowBool8 == false)
                {
                    break;
                }
            }
        }

        public static void CosmoAutoShow() //Auto Program
        {
            while (CosmoAutoProgram == true)
            {
                OpenDMX.setDmxValue(188, 255);

                if (CosmoAutoProgram == false)
                {
                    break;
                }
            }
            OpenDMX.setDmxValue(188, 0);
        }
    }
}
