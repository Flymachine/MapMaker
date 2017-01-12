using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapMaker.MapHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapMaker.MapHelper.Tests
{
    [TestClass()]
    public class MapHelperTests
    {
        [TestMethod()]
        public void convertTest()
        {
            //arrange
            double expected = 100;
            double delta = 0.0001;
            MapHelper MHtest = new MapHelper();

            // act
            double value = 100;
            int level = 18;
            bool OutPixel = false;
            double actual = MHtest.convert(value, level, OutPixel);
            Assert.AreEqual(expected, actual, delta);

            //arrange
            expected = 790;
            // act
            value = 12958175;
            level = 4;
            OutPixel = true;
            actual = MHtest.convert(value, level, OutPixel);
            Assert.AreEqual(expected, actual, delta);

            //arrange
            expected = 12958175;
            delta = 15000;
            // act
            value = 790;
            level = 4;
            OutPixel = false;
            actual = MHtest.convert(value, level, OutPixel);
            Assert.AreEqual(expected, actual, delta);

            //arrange
            expected = 0;
            delta = 0.0001;
            // act
            value = 0;
            level = 4;
            OutPixel = false;
            actual = MHtest.convert(value, level, OutPixel);
            Assert.AreEqual(expected, actual, delta);

            //arrange
            expected = 0;
            delta = 0.0001;
            // act
            value = 0;
            level = 1;
            OutPixel = true;
            actual = MHtest.convert(value, level, OutPixel);
            Assert.AreEqual(expected, actual, delta);

            //arrange
            expected = -12958175;
            delta = 15000;
            // act
            value = -790;
            level = 4;
            OutPixel = false;
            actual = MHtest.convert(value, level, OutPixel);
            Assert.AreEqual(expected, actual, delta);
            //Assert.Fail();
        }

        [TestMethod()]
        public void convertMC2LLTest()
        {
            //arrange
            double expected = 12958175,
                expected1 = 4825923.77;
            double delta = 0.01;
            MapHelper MHtest = new MapHelper();

            // act
            double value = 116.404,
                 value1 = 39.915;
            bool OutLongitudeAndLatitude = false;
            double[] act_arr = MHtest.convertMC2LL(value, value1, OutLongitudeAndLatitude);
            double actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);

            //arrange
            expected = 0;
            delta = 0.01;

            // act
            value = 0;
            OutLongitudeAndLatitude = false;
            act_arr = MHtest.convertMC2LL(value, value, OutLongitudeAndLatitude);
            actual = act_arr[1];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);

            //arrange
            expected = -12958175;
            expected1 = -4825923.77;
            delta = 0.01;

            // act
            value = -116.404;
            value1 = -39.915;
            OutLongitudeAndLatitude = false;
            act_arr = MHtest.convertMC2LL(value, value1, OutLongitudeAndLatitude);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);

            //arrange
            expected = 116.404;
            expected1 = 39.915;
            delta = 0.01;

            // act
            value = 12958175;
            value1 = 4825923.77;
            OutLongitudeAndLatitude = true;
            act_arr = MHtest.convertMC2LL(value, value1, OutLongitudeAndLatitude);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);

            //arrange
            expected = -116.404;
            expected1 = -39.915;
            delta = 0.01;

            // act
            value = -12958175;
            value1 = -4825923.77;
            OutLongitudeAndLatitude = true;
            act_arr = MHtest.convertMC2LL(value, value1, OutLongitudeAndLatitude);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);

            //arrange
            expected = 0;
            expected1 = 0;
            delta = 0.01;

            // act
            value = 0;
            value1 = 0;
            OutLongitudeAndLatitude = true;
            act_arr = MHtest.convertMC2LL(value, value1, OutLongitudeAndLatitude);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);

            //arrange
            expected = 0.000179664;
            expected1 = -0.000090412;
            delta = 0.0000001;

            // act
            value = 20;//20
            value1 = -10;//-10
            OutLongitudeAndLatitude = true;
            act_arr = MHtest.convertMC2LL(value, value1, OutLongitudeAndLatitude);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);

            //Assert.Fail();
        }

        [TestMethod()]
        public void convertP2AreaTest()
        {
            //arrange
            double expected = 4825923.77,
                expected1 = 12958175,
                expected2 = 4825923.77,
                expected3 = 12958175;
            double delta = 0.01,
                delta1 = 10000;
            MapHelper MHtest = new MapHelper();

            // act
            int level = 18;
            double MCx = 12958175,
                 MCy = 4825923.77,
                 bleedu = 0,
                 bleedr = 0,
                 bleedb = 0,
                 bleedl = 0;
            double[] act_arr = MHtest.convertP2Area(level, MCx, MCy, bleedu, bleedr, bleedb, bleedl);
            double actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);
            actual = act_arr[2];
            Assert.AreEqual(expected2, actual, delta);
            actual = act_arr[3];
            Assert.AreEqual(expected3, actual, delta);

            //arrange
            expected = 4825933.77;
            expected1 = 12958185;
            expected2 = 4825913.77;
            expected3 = 12958165;
            delta = 0.01;

            // act
            level = 18;
            MCx = 12958175;
            MCy = 4825923.77;
            bleedu = 10;
            bleedr = 10;
            bleedb = 10;
            bleedl = 10;
            act_arr = MHtest.convertP2Area(level, MCx, MCy, bleedu, bleedr, bleedb, bleedl);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);
            actual = act_arr[2];
            Assert.AreEqual(expected2, actual, delta);
            actual = act_arr[3];
            Assert.AreEqual(expected3, actual, delta);

            //arrange
            expected = 4825933.77;
            expected1 = 12958185;
            expected2 = 0;
            expected3 = 0;
            delta = 10;
            delta1 = 10000;

            // act
            level = 4;
            MCx = 12958175;
            MCy = 4825923.77;
            bleedu = 0;
            bleedr = 0;
            bleedb = 294;
            bleedl = 791;
            act_arr = MHtest.convertP2Area(level, MCx, MCy, bleedu, bleedr, bleedb, bleedl);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);
            actual = act_arr[2];
            Assert.AreEqual(expected2, actual, delta1);
            actual = act_arr[3];
            Assert.AreEqual(expected3, actual, delta1);

            //arrange
            expected = 0;
            expected1 = 0;
            expected2 = 0;
            expected3 = 0;
            delta = 0.001;
            delta1 = 10000;

            // act
            level = 4;
            MCx = 0;
            MCy = 0;
            bleedu = 0;
            bleedr = 0;
            bleedb = 0;
            bleedl = 0;
            act_arr = MHtest.convertP2Area(level, MCx, MCy, bleedu, bleedr, bleedb, bleedl);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);
            actual = act_arr[2];
            Assert.AreEqual(expected2, actual, delta);
            actual = act_arr[3];
            Assert.AreEqual(expected3, actual, delta);

            //arrange
            expected = -4825913.77;
            expected1 = -12958165;
            expected2 = -4825933.77;
            expected3 = -12958185;
            delta = 0.01;

            // act
            level = 18;
            MCx = -12958175;
            MCy = -4825923.77;
            bleedu = 10;
            bleedr = 10;
            bleedb = 10;
            bleedl = 10;
            act_arr = MHtest.convertP2Area(level, MCx, MCy, bleedu, bleedr, bleedb, bleedl);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);
            actual = act_arr[2];
            Assert.AreEqual(expected2, actual, delta);
            actual = act_arr[3];
            Assert.AreEqual(expected3, actual, delta);

            //arrange
            expected = 4825913.77;
            expected1 = 12958165;
            expected2 = 4825933.77;
            expected3 = 12958185;
            delta = 0.01;

            // act
            level = 18;
            MCx = 12958175;
            MCy = 4825923.77;
            bleedu = -10;
            bleedr = -10;
            bleedb = -10;
            bleedl = -10;
            act_arr = MHtest.convertP2Area(level, MCx, MCy, bleedu, bleedr, bleedb, bleedl);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);
            actual = act_arr[2];
            Assert.AreEqual(expected2, actual, delta);
            actual = act_arr[3];
            Assert.AreEqual(expected3, actual, delta);
            // Assert.Fail();
        }

        [TestMethod()]
        public void sumPsAreaTest()
        {
            //arrange
            double expected = 30,
                expected1 = 40,
                expected2 = -10,
                expected3 = -10;
            double delta = 0.01;
            MapHelper MHtest = new MapHelper();

            // act
            double[] area_bleed_MC = new double[] { 10, 10, 10, 10 };
            Dictionary<int, double>
                 P_area_MC_up = new Dictionary<int, double>(),
                 P_area_MC_right = new Dictionary<int, double>(),
                 P_area_MC_bottom = new Dictionary<int, double>(),
                 P_area_MC_left = new Dictionary<int, double>();
            P_area_MC_up[1] = 10;
            P_area_MC_right.Add(1, 10);
            P_area_MC_bottom[1] = 0;
            P_area_MC_left.Add(1, 0);
            P_area_MC_up[3] = 20;
            P_area_MC_right.Add(3, 20);
            P_area_MC_bottom[3] = 10;
            P_area_MC_left.Add(3, 10);
            P_area_MC_up[5] = 10;
            P_area_MC_right.Add(5, 30);
            P_area_MC_bottom[5] = 0;
            P_area_MC_left.Add(5, 20);

            double[] act_arr = MHtest.sumPsArea(area_bleed_MC, P_area_MC_up, P_area_MC_right, P_area_MC_bottom, P_area_MC_left);
            double actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);
            actual = act_arr[2];
            Assert.AreEqual(expected2, actual, delta);
            actual = act_arr[3];
            Assert.AreEqual(expected3, actual, delta);

            int[] Pid_exp = P_area_MC_up.Keys.ToArray<int>();
            int[] Pid_act = new int[] { 1, 3, 5 };
            for (int i = 0; i < Pid_act.Length; i++)
            {
                Assert.AreEqual(Pid_exp[i], Pid_act[i]);
            }

            //Assert.Fail();
        }

        [TestMethod()]
        public void convertArea_MC2P_LLTest()
        {
            //arrange
            double expected = 0.000045,
                expected1 = 0;
            double delta = 0.0000001;
            MapHelper MHtest = new MapHelper();

            // act
            double[] area_MC = new double[] { 10, 10, -10, 0 };
            double[] act_arr = MHtest.convertArea_MC2P_LL(area_MC);
            double actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);

            //arrange
            expected = -0.000045;
            expected1 = 0;
            delta = 0.0000001;

            // act
            area_MC = new double[] { -10, -10, 10, 0 };
            act_arr = MHtest.convertArea_MC2P_LL(area_MC);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);
            //Assert.Fail();

        }

        [TestMethod()]
        public void convertArea2UserAreaTest()
        {

            //arrange
            double expected = 10,
                expected1 = 22,
                expected2 = -30,
                expected3 = -2;
            double delta = 0.01;
            MapHelper MHtest = new MapHelper();

            // act
            double[] user_center_LL = new double[] { 0.000089833, -0.000090412 },
                area_MC = new double[] { 10, 22, -10, 0 };
            double[] act_arr = MHtest.convertArea2UserArea(user_center_LL, area_MC);
            double actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);
            actual = act_arr[2];
            Assert.AreEqual(expected2, actual, delta);
            actual = act_arr[3];
            Assert.AreEqual(expected3, actual, delta);

            //arrange
            expected = 30;
            expected1 = 40;
            expected2 = -10;
            expected3 = 0;
            delta = 0.01;

            // act
            user_center_LL = new double[] { 0.000179664, 0.000090412 };
            area_MC = new double[] { 10, 22, -10, 0 };
            act_arr = MHtest.convertArea2UserArea(user_center_LL, area_MC);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);
            actual = act_arr[2];
            Assert.AreEqual(expected2, actual, delta);
            actual = act_arr[3];
            Assert.AreEqual(expected3, actual, delta);
            //Assert.Fail();
        }

        [TestMethod()]
        public void convertAreaMC2PixelTest()
        {
            //arrange
            int expected = 22,
                expected1 = 20;
            MapHelper MHtest = new MapHelper();

            // act
            double[] area_MC = new double[] { -10, -22, 10, 0 };
            int level = 18;
            int[] act_arr = MHtest.convertAreaMC2Pixel(area_MC, level);
            int actual = act_arr[0];
            Assert.AreEqual(expected, actual);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual);

            //arrange
            expected = 11;
            expected1 = 10;

            // act
            area_MC = new double[] { -10, -22, 10, 0 };
            level = 17;
            act_arr = MHtest.convertAreaMC2Pixel(area_MC, level);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual);

            //arrange
            expected = 0;
            expected1 = 0;

            // act
            area_MC = new double[] { 0, 0, 0, 0 };
            level = 18;
            act_arr = MHtest.convertAreaMC2Pixel(area_MC, level);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual);
            //Assert.Fail();
        }

        [TestMethod()]
        public void convertAreaPixel2MCTest()
        {
            //arrange
            double expected = 0,
                expected1 = 11,
                expected2 = 0,
                expected3 = 11;
            double delta = 0.01;
            MapHelper MHtest = new MapHelper();

            // act
            int[] user_area_pixel = new int[] { 0, 0 };
            int level = 18;
            double[] area_MC = new double[] { 10, 22, -10, 0 };
            double[] act_arr = MHtest.convertAreaPixel2MC(user_area_pixel, level, area_MC);
            double actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);
            actual = act_arr[2];
            Assert.AreEqual(expected2, actual, delta);
            actual = act_arr[3];
            Assert.AreEqual(expected3, actual, delta);

            //arrange
            expected = 60;
            expected1 = 100;
            expected2 = -60;
            expected3 = -100;
            delta = 0.01;

            // act
            user_area_pixel = new int[] { 100, 60 };
            level = 17;
            area_MC = new double[] { 0, 0, 0, 0 };
            act_arr = MHtest.convertAreaPixel2MC(user_area_pixel, level, area_MC);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);
            actual = act_arr[2];
            Assert.AreEqual(expected2, actual, delta);
            actual = act_arr[3];
            Assert.AreEqual(expected3, actual, delta);

            //Assert.Fail();
        }

        [TestMethod()]
        public void checkLackPsTest()
        {
            //arrange
            int expected = 2,
                expected1 = 6,
                expected2 = 8,
                len_exp = 3;
            double delta = 0.01;
            MapHelper MHtest = new MapHelper();

            // act
            int level = 17;
            double[] area_MC = new double[] { 10, 10, -10, -10 };
            Dictionary<int, double>
                 P_area_MC_up = new Dictionary<int, double>(),
                 P_area_MC_right = new Dictionary<int, double>(),
                 P_area_MC_bottom = new Dictionary<int, double>(),
                 P_area_MC_left = new Dictionary<int, double>();
            P_area_MC_up[1] = 5;
            P_area_MC_right.Add(1, 5);
            P_area_MC_bottom[1] = 0;
            P_area_MC_left.Add(1, 0);
            P_area_MC_up[3] = -5;
            P_area_MC_right.Add(3, 0);
            P_area_MC_bottom[3] = -10;
            P_area_MC_left.Add(3, -5);
            P_area_MC_up[5] = 10;
            P_area_MC_right.Add(5, 10);
            P_area_MC_bottom[5] = -10;
            P_area_MC_left.Add(5, -10);
            P_area_MC_up[2] = -9;
            P_area_MC_right.Add(2, -9);
            P_area_MC_bottom[2] = -11;
            P_area_MC_left.Add(2, -11);
            P_area_MC_up[6] = 10;
            P_area_MC_right.Add(6, 10);
            P_area_MC_bottom[6] = -20;
            P_area_MC_left.Add(6, -20);
            P_area_MC_up[8] = -11;
            P_area_MC_right.Add(8, -11);
            P_area_MC_bottom[8] = -20;
            P_area_MC_left.Add(8, -20);

            List<int> act_arr = MHtest.checkLackPs(area_MC, level, P_area_MC_up, P_area_MC_right, P_area_MC_bottom, P_area_MC_left);
            int actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);
            actual = act_arr[2];
            Assert.AreEqual(expected2, actual, delta);
            actual = act_arr.Count();
            Assert.AreEqual(len_exp, actual, delta);

            //Assert.Fail();
        }

        [TestMethod()]
        public void checkCoverPsTest()
        {
            //arrange
            int expected = 5,
                expected1 = 6,
                expected2 = 8,
                expected3 = 9,
                expected4 = 10,
                len_exp = 5;
            double delta = 0.01;
            MapHelper MHtest = new MapHelper();

            // act
            int level = 17;
            Dictionary<int, double>
                 P_area_MC_up = new Dictionary<int, double>(),
                 P_area_MC_right = new Dictionary<int, double>(),
                 P_area_MC_bottom = new Dictionary<int, double>(),
                 P_area_MC_left = new Dictionary<int, double>();
            P_area_MC_up[1] = -10;
            P_area_MC_right.Add(1, -10);
            P_area_MC_bottom[1] = -20;
            P_area_MC_left.Add(1, -20);
            P_area_MC_up[3] = 0;
            P_area_MC_right.Add(3, 0);
            P_area_MC_bottom[3] = 0;
            P_area_MC_left.Add(3, 0);
            P_area_MC_up[5] = 9;
            P_area_MC_right.Add(5, 35);
            P_area_MC_bottom[5] = 1;
            P_area_MC_left.Add(5, 18);
            P_area_MC_up[2] = 10;
            P_area_MC_right.Add(2, 10);
            P_area_MC_bottom[2] = 0;
            P_area_MC_left.Add(2, 0);
            P_area_MC_up[6] = 10;
            P_area_MC_right.Add(6, 20);
            P_area_MC_bottom[6] = 0;
            P_area_MC_left.Add(6, 10);
            P_area_MC_up[8] = 10;
            P_area_MC_right.Add(8, 40);
            P_area_MC_bottom[8] = 0;
            P_area_MC_left.Add(8, 30);
            P_area_MC_up[9] = -5;
            P_area_MC_right.Add(9, 40);
            P_area_MC_bottom[9] = -15;
            P_area_MC_left.Add(9, 30);
            P_area_MC_up[10] = -7;
            P_area_MC_right.Add(10, 37);
            P_area_MC_bottom[10] = -12;
            P_area_MC_left.Add(10, 35);

            List<int> act_arr = MHtest.checkCoverPs(level, P_area_MC_up, P_area_MC_right, P_area_MC_bottom, P_area_MC_left);
            int actual = act_arr.Count();
            Assert.AreEqual(len_exp, actual, delta);
            actual = act_arr[0];
            Assert.AreEqual(expected, actual, delta);
            actual = act_arr[1];
            Assert.AreEqual(expected1, actual, delta);
            actual = act_arr[2];
            Assert.AreEqual(expected2, actual, delta);
            actual = act_arr[3];
            Assert.AreEqual(expected3, actual, delta);
            actual = act_arr[4];
            Assert.AreEqual(expected4, actual, delta);

            //Assert.Fail();
        }

        [TestMethod()]
        public void CutArea2DicTest()
        {
            //arrange
            string expected = "0_0",
                expected1 = "100",
                expected2 = "100",
                expected3 = "0.000449",
                expected4 = "0.000904",

                expected6 = "0_1",
                expected7 = "100",
                expected8 = "50",
                expected9 = "0.000449",
                expected10 = "0.000226",

                expected11 = "1_0",
                expected12 = "50",
                expected13 = "100",
                expected14 = "0.001123",
                expected15 = "0.000904",

                expected16 = "1_1",
                expected17 = "50",
                expected18 = "50",
                expected19 = "0.001123",
                expected20 = "0.000226";
            double delta = 0.000001;
            MapHelper MHtest = new MapHelper();

            // act
            int[] cut_area_pixel = new int[] { 100, 100 };
            int level = 18;
            double[] area_MC = new double[] { 150, 150, 0, 0 };
            Dictionary<string, string[]> act_arr = MHtest.CutArea2Dic(area_MC, cut_area_pixel, level);
            string[] act_key_arr = act_arr.Keys.ToArray();
            
            string actual = act_key_arr[0];
            Assert.AreEqual(expected, actual);
            actual = act_key_arr[1];
            Assert.AreEqual(expected6, actual);
            actual = act_key_arr[2];
            Assert.AreEqual(expected11, actual);
            actual = act_key_arr[3];
            Assert.AreEqual(expected16, actual);

            actual = act_arr[expected][0];
            Assert.AreEqual(expected1, actual);
            actual = act_arr[expected][1];
            Assert.AreEqual(expected2, actual);
            actual = act_arr[expected][2];
            Assert.AreEqual(Convert.ToDouble(expected3), Convert.ToDouble(actual), delta);
            actual = act_arr[expected][3];
            Assert.AreEqual(Convert.ToDouble(expected4), Convert.ToDouble(actual), delta);

            actual = act_arr[expected6][0];
            Assert.AreEqual(expected7, actual);
            actual = act_arr[expected6][1];
            Assert.AreEqual(expected8, actual);
            actual = act_arr[expected6][2];
            Assert.AreEqual(Convert.ToDouble(expected9), Convert.ToDouble(actual), delta);
            actual = act_arr[expected6][3];
            Assert.AreEqual(Convert.ToDouble(expected10), Convert.ToDouble(actual), delta);

            actual = act_arr[expected11][0];
            Assert.AreEqual(expected12, actual);
            actual = act_arr[expected11][1];
            Assert.AreEqual(expected13, actual);
            actual = act_arr[expected11][2];
            Assert.AreEqual(Convert.ToDouble(expected14), Convert.ToDouble(actual), delta);
            actual = act_arr[expected11][3];
            Assert.AreEqual(Convert.ToDouble(expected15), Convert.ToDouble(actual), delta);

            actual = act_arr[expected16][0];
            Assert.AreEqual(expected17, actual);
            actual = act_arr[expected16][1];
            Assert.AreEqual(expected18, actual);
            actual = act_arr[expected16][2];
            Assert.AreEqual(Convert.ToDouble(expected19), Convert.ToDouble(actual), delta);
            actual = act_arr[expected16][3];
            Assert.AreEqual(Convert.ToDouble(expected20), Convert.ToDouble(actual), delta);

            //arrange
            expected = "00_00";
            // act
            cut_area_pixel = new int[] { 100, 100 };
            level = 18;
            area_MC = new double[] { 1500, 1500, 0, 0 };
            act_arr = MHtest.CutArea2Dic(area_MC, cut_area_pixel, level);
            act_key_arr = act_arr.Keys.ToArray();
            actual = act_key_arr[0];
            Assert.AreEqual(expected, actual);
            //Assert.Fail();
        }
    }
}