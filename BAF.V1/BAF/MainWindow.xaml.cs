using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static BAF.Shows.Shows;
using System.Drawing;
using static BAF.Shows.ShowBools;
using static BAF.Div.Div;
using BAF.Shows;

namespace BAF
{
    public partial class MainWindow : Window
    {
        public static Thread FM;
        bool FastMovementThreadStartet = false;
        public BaseClass.Objects.LightDefinitions.CosmoLight CosmoLight;
        public BaseClass.Objects.LightDefinitions.CosmoLight CosmoLight2;
        public BaseClass.Objects.LightDefinitions.CosmoLight CosmoMovement;
        public BaseClass.Objects.LightDefinitions.CosmoLight CosmoMovement2;
        public static List<BaseClass.Objects.LightDefinitions.CosmoLight> myLights;
        public static int ColorDuration;
        public static int MovementDuration;
        public MainWindow()
        {
            CosmoLight = new BaseClass.Objects.LightDefinitions.CosmoLight();
            CosmoLight.CosmoLightID = 1;
            CosmoLight.StartChannel = 168;
            CosmoLight.AllLedsOffset = 19;
            CosmoLight.AutoOffset = 20;
            CosmoLight.IntensityOffset = 6;
            CosmoLight.TurnOffset = 0;
            CosmoLight.RotateOffset = 5;
            CosmoLight.SpeedOffset = 4;
            CosmoLight.Colors = new List<BaseClass.Objects.LightDefinitions.Color>();
            CosmoLight.Movements = new List<BaseClass.Objects.LightDefinitions.Movement>();

            var turn3 = new BaseClass.Objects.LightDefinitions.Movement();
            turn3.MovementName = "Turn3";
            turn3.Axis = "y";
            turn3.MovementDirection = 0;
            turn3.MovementSpeed = 0;
            CosmoLight.Movements.Add(turn3);

            var turn1 = new BaseClass.Objects.LightDefinitions.Movement();
            turn1.MovementName = "Turn1";
            turn1.Axis = "x";
            turn1.MovementDirection = 255;
            turn1.MovementSpeed = 1;
            CosmoLight.Movements.Add(turn1);

            var turn2 = new BaseClass.Objects.LightDefinitions.Movement();
            turn2.MovementName = "Turn2";
            turn2.Axis = "x";
            turn2.MovementSpeed = 1;
            turn2.MovementDirection = 1;
            CosmoLight.Movements.Add(turn2);

            CosmoLight2 = new BaseClass.Objects.LightDefinitions.CosmoLight();
            CosmoLight2.CosmoLightID = 2;
            CosmoLight2.StartChannel = 189;
            CosmoLight2.AllLedsOffset = 19;
            CosmoLight2.AutoOffset = 20;
            CosmoLight2.IntensityOffset = 6;
            CosmoLight2.TurnOffset = 0;
            CosmoLight2.RotateOffset = 5;
            CosmoLight2.SpeedOffset = 4;
            CosmoLight2.Colors = new List<BaseClass.Objects.LightDefinitions.Color>();
            CosmoLight2.Movements = new List<BaseClass.Objects.LightDefinitions.Movement>();

            turn3.MovementName = "Turn3";
            turn3.MovementDirection = 160;
            turn3.MovementSpeed = 0;
            CosmoLight2.Movements.Add(turn3);

            turn1.MovementName = "Turn1";
            turn1.MovementDirection = 255;
            turn1.MovementSpeed = 1;
            CosmoLight2.Movements.Add(turn1);

            turn2.MovementName = "Turn2";
            turn2.MovementDirection = 1;
            turn2.MovementSpeed = 1;
            CosmoLight2.Movements.Add(turn2);

           


            InitializeComponent();

            OpenDMX.setDmxValue(1, 255);
            OpenDMX.start();
            EnttecStatus();

            Thread t = new Thread(DataThread);
            t.Start();
        }

        private void CosmoColorChange_Click(object sender, RoutedEventArgs e)
        {
            EnttecStatus();

            if (ColorThreadStartet)
            {
                CT.Abort();
                ColorThreadStartet = false;
            }

            var currentLights = GetCurrentLights();
            foreach (var currentLight in currentLights)
            {
                OpenDMX.setDmxValue(currentLight.StartChannel + currentLight.IntensityOffset, 255);

                var red = new BaseClass.Objects.LightDefinitions.Color();
                red.ColorName = "Red";
                red.ColorValue = 6;
                currentLight.Colors.Add(red);

                var green = new BaseClass.Objects.LightDefinitions.Color();
                green.ColorName = "Green";
                green.ColorValue = 15;
                currentLight.Colors.Add(green);

                var blue = new BaseClass.Objects.LightDefinitions.Color();
                blue.ColorName = "Blue";
                blue.ColorValue = 20;
                currentLight.Colors.Add(blue);
            }

            CosmoColorChange = true;
            CosmoColorChangeShow(currentLights);
        }

        private void CosmoColorRed_Click(object sender, RoutedEventArgs e)
        {
            EnttecStatus();

            var currentLights = GetCurrentLights();

            foreach (var currentLight in currentLights)
            {
                currentLight.Colors.Clear();

                var c = new BaseClass.Objects.LightDefinitions.Color();
                c.ColorName = "Red";
                c.ColorValue = 6;
                currentLight.Colors.Add(c);

                OpenDMX.setDmxValue(currentLight.StartChannel + currentLight.IntensityOffset, 255);

                //BaseClass.Logic.LightControl.SetColors(currentLight.StartChannel + currentLight.AllLedsOffset, new List<string> { "Red" });
            }
            CosmoColorChange = true;

            Shows.Shows.CosmoColorChangeShow(currentLights);

        }

        private List<BaseClass.Objects.LightDefinitions.CosmoLight> GetCurrentLights()
        {
            var lights = new List<BaseClass.Objects.LightDefinitions.CosmoLight>();

            if (radioButton_Light1.IsChecked.Value)
            {
                lights.Add(CosmoLight);
            }
            else if (radioButton_Light2.IsChecked.Value)
            {
                lights.Add(CosmoLight2);
            }
            else
            {
                lights.Add(CosmoLight);
                lights.Add(CosmoLight2);
            }

            return lights;
        }

        public static void DataThread()
        {
            while (true)
            {
                OpenDMX.writeData();
                Thread.Sleep(50);
            }
        }

        private void TurnRotateCosmoFast_Click(object sender, RoutedEventArgs e)
        {
            EnttecStatus();

            if (FastMovementThreadStartet)
            {
                FM.Abort();
                FastMovementThreadStartet = false;
            }

            var currentLights = GetCurrentLights();

            foreach (var light in currentLights)
            {
                light.Movements.Clear();

                var turn3 = new BaseClass.Objects.LightDefinitions.Movement();
                turn3.MovementName = "Turn3";
                turn3.Axis = "y";
                turn3.MovementDirection = 160;
                turn3.MovementSpeed = 0;
                light.Movements.Add(turn3);

                var turn1 = new BaseClass.Objects.LightDefinitions.Movement();
                turn1.MovementName = "Turn1";
                turn1.Axis = "x";
                turn1.MovementDirection = 255;
                turn1.MovementSpeed = 1;
                light.Movements.Add(turn1);

                var turn2 = new BaseClass.Objects.LightDefinitions.Movement();
                turn2.MovementName = "Turn2";
                turn2.Axis = "x";
                turn2.MovementDirection = 1;
                turn2.MovementSpeed = 1;
                light.Movements.Add(turn2);
            }
            ShowBools.TurnRotateCosmo = true;
            TurnRotateCosmo(currentLights, 1500);
        }

        //private void FastThread()
        //{
        //    MovementThreadStartet = true;

        //    var currentLights = GetCurrentLights();

        //    foreach (var light in currentLights)
        //    {

        //        var turn3 = new BaseClass.Objects.LightDefinitions.Movement();
        //        turn3.MovementName = "Turn3";
        //        turn3.Axis = "y";
        //        turn3.MovementDirection = 160;
        //        turn3.MovementSpeed = 0;
        //        light.Movements.Add(turn3);

        //        var turn1 = new BaseClass.Objects.LightDefinitions.Movement();
        //        turn1.MovementName = "Turn1";
        //        turn1.Axis = "x";
        //        turn1.MovementDirection = 255;
        //        turn1.MovementSpeed = 1;
        //        light.Movements.Add(turn1);

        //        var turn2 = new BaseClass.Objects.LightDefinitions.Movement();
        //        turn2.MovementName = "Turn2";
        //        turn2.Axis = "x";
        //        turn2.MovementDirection = 1;
        //        turn2.MovementSpeed = 1;
        //        light.Movements.Add(turn2);
        //    }

        //    Shows.ShowBools.TurnRotateCosmo = true;
        //    TurnRotateCosmo(currentLights);
        //}

        private void BO_Button_Click(object sender, RoutedEventArgs e)
        {
            var lights = GetCurrentLights();

            if (BlackOut == false)
            {
                foreach (var light in lights)
                {
                    int i = 0;

                    while (i < 21)
                    {
                        OpenDMX.setDmxValue(light.StartChannel + i, 0);
                        i++;
                    }
                }



                    OpenDMX.setDmxValue(1, 0);
                    OpenDMX.setDmxValue(173, 0);
                    OpenDMX.setDmxValue(174, 0);
            


              
                TurnRotateCosmoFast_Button.IsEnabled = false;
                CosmoColorChange_Button.IsEnabled = false;
                CosmoColorRed_Button.IsEnabled = false;
                CosmoColorGreen_Button.IsEnabled = false;
                CosmoColorBlue_Button.IsEnabled = false;
                CosmoColorWhite_Button.IsEnabled = false;
                GPar64.IsEnabled = false;
                LED.IsEnabled = false;
                Shows.ShowBools.TurnRotateCosmo = false;
                if (Par64_bool == true)
                {
                    Par64_bool = false;
                    GP_check = true;
                    OpenDMX.setDmxValue(64, 0);
                    OpenDMX.setDmxValue(65, 0);
                    OpenDMX.setDmxValue(66, 0);
                    OpenDMX.setDmxValue(67, 0);
                }
                BlackOut = true;
            }
            else
            {
                
                OpenDMX.setDmxValue(1, 255);
                TurnRotateCosmoFast_Button.IsEnabled = true;
                CosmoColorChange_Button.IsEnabled = true;
                CosmoColorRed_Button.IsEnabled = true;
                CosmoColorGreen_Button.IsEnabled = true;
                CosmoColorBlue_Button.IsEnabled = true;
                CosmoColorWhite_Button.IsEnabled = true;
                GPar64.IsEnabled = true;
                LED.IsEnabled = true;
                if (GP_check == true)
                {
                    Par64_bool = true;
                    GP_check = false;
                }
                BlackOut = false;

                foreach (var light in lights)
                {
                         
                        OpenDMX.setDmxValue(light.StartChannel + light.IntensityOffset, 255);
                }

                CosmoColorChange = true;
                Shows.Shows.CosmoColorChangeShow(GetCurrentLights());

                ShowBools.TurnRotateCosmo = true;
                Shows.Shows.TurnRotateCosmo(GetCurrentLights());
            }
        }

        private void CosmoColorGreen_Click(object sender, RoutedEventArgs e)
        {
            EnttecStatus();

            var currentLights = GetCurrentLights();

            foreach (var currentLight in currentLights)
            {
                currentLight.Colors.Clear();

                var c = new BaseClass.Objects.LightDefinitions.Color();
                c.ColorName = "Green";
                c.ColorValue = 15;
                currentLight.Colors.Add(c);

                OpenDMX.setDmxValue(currentLight.StartChannel + currentLight.IntensityOffset, 255);

                //BaseClass.Logic.LightControl.SetColors(currentLight.StartChannel + currentLight.AllLedsOffset, new List<string> { "Red" });
            }
            CosmoColorChange = true;

            Shows.Shows.CosmoColorChangeShow(currentLights);

        }

        private void CosmoColorBlue_Click(object sender, RoutedEventArgs e)
        {
            EnttecStatus();

            var currentLights = GetCurrentLights();

            foreach (var currentLight in currentLights)
            {
                currentLight.Colors.Clear();

                var c = new BaseClass.Objects.LightDefinitions.Color();
                c.ColorName = "Blue";
                c.ColorValue = 20;
                currentLight.Colors.Add(c);

                OpenDMX.setDmxValue(currentLight.StartChannel + currentLight.IntensityOffset, 255);

                //BaseClass.Logic.LightControl.SetColors(currentLight.StartChannel + currentLight.AllLedsOffset, new List<string> { "Red" });
            }
            CosmoColorChange = true;

            Shows.Shows.CosmoColorChangeShow(currentLights);

        }

        private void CosmoColorWhite_Click(object sender, RoutedEventArgs e)
        {
            EnttecStatus();

            var currentLights = GetCurrentLights();

            foreach (var currentLight in currentLights)
            {
                currentLight.Colors.Clear();

                var c = new BaseClass.Objects.LightDefinitions.Color();
                c.ColorName = "White";
                c.ColorValue = 25;
                currentLight.Colors.Add(c);

                OpenDMX.setDmxValue(currentLight.StartChannel + currentLight.IntensityOffset, 255);

                //BaseClass.Logic.LightControl.SetColors(currentLight.StartChannel + currentLight.AllLedsOffset, new List<string> { "Red" });
            }
            CosmoColorChange = true;

            Shows.Shows.CosmoColorChangeShow(currentLights);

        }

        private void TurnRotateCosmoSlow_Click(object sender, RoutedEventArgs e)
        {
       

            EnttecStatus();

            var currentLights = GetCurrentLights();

            foreach (var light in currentLights)
            {
                light.Movements.Clear();

                var turn3 = new BaseClass.Objects.LightDefinitions.Movement();
                turn3.MovementName = "Turn3";
                turn3.Axis = "y";
                turn3.MovementDirection = 255;
                turn3.MovementSpeed = 0;
                light.Movements.Add(turn3);

                var turn1 = new BaseClass.Objects.LightDefinitions.Movement();
                turn1.MovementName = "Turn1";
                turn1.Axis = "x";
                turn1.MovementDirection = 255;
                turn1.MovementSpeed = 255;
                light.Movements.Add(turn1);

                var turn2 = new BaseClass.Objects.LightDefinitions.Movement();
                turn2.MovementName = "Turn2";
                turn2.Axis = "x";
                turn2.MovementDirection = 1;
                turn2.MovementSpeed = 255;
                light.Movements.Add(turn2);
            }

            Shows.ShowBools.TurnRotateCosmo = true;
            TurnRotateCosmo(currentLights, 6000);


        }

        private void Show8_Button_Click_1(object sender, RoutedEventArgs e)
        {
            EnttecStatus();

            if (ShowBool8 == false)
            {
                ShowBool8 = true;
                Thread S8 = new Thread(Show8);
                S8.Start();
            }
            else
            {
                ShowBool8 = false;
            }
        }

        private void CosmoAuto_Click(object sender, RoutedEventArgs e)
        {
            EnttecStatus();

            var currentLights = GetCurrentLights();

            foreach (var currentLight in currentLights)
            {
                OpenDMX.setDmxValue(currentLight.StartChannel + currentLight.IntensityOffset, 255);
                BaseClass.Logic.LightControl.SetColors(currentLight.StartChannel + currentLight.AllLedsOffset, new List<string> { "Auto" });
            }
        }

        private void LED_Click(object sender, RoutedEventArgs e)
        {
            if (bool_LED == false)
            {
                OpenDMX.setDmxValue(5, 170);
                bool_LED = true;
            }
            else
            {
                OpenDMX.setDmxValue(5, 0);
                bool_LED = false;
            }
        }

        private void GPar64_Click(object sender, RoutedEventArgs e)
        {
            if (Par64_bool == false)
            {
                Par64_bool = true;
            }
            else
            {
                Par64_bool = false;
            }
        }

        private void StropeInternational_Click(object sender, RoutedEventArgs e)
        {
            if (StroInternational == false)
            {
                OpenDMX.setDmxValue(2, 185);
                OpenDMX.setDmxValue(3, 255);
                StroInternational = true;
                BO_Button_Click(this, null);
            }
            else
            {
                OpenDMX.setDmxValue(3, 0);
                OpenDMX.setDmxValue(2, 0);
                StroInternational = false;
                BO_Button_Click(this, null);
                
           
            }
        }

        private void EnttecStatus()
        {
            try
            {
                if (OpenDMX.status == FT_STATUS.FT_DEVICE_NOT_FOUND)
                {
                    StatusBox.Text = "No Enttec USB Device Found";
                }
                else if (OpenDMX.status == FT_STATUS.FT_OK)
                {
                    StatusBox.Text = "Found DMX on USB";
                }
                else
                    StatusBox.Text = "Error Opening Device";
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
                StatusBox.Text = "Error Connecting to Enttec USB Device";
            }
        }
    }
}