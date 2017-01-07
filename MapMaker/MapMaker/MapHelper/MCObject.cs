using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapMaker.MapHelper
{
    public class MCObject
    {

        /// <summary>  
        ///   
        /// </summary>  
        /// <param name="x"></param>  
        /// <param name="y"></param>  
        /// <param name="OutLongitudeAndLatitude">trueMC坐标转经纬度,false经纬度转MC坐标</param>  
        public MCObject(double x, double y, bool OutLongitudeAndLatitude = true)
        {
            if (OutLongitudeAndLatitude)
            {
                MCx = x;
                _MCy = y;

                convertMC2LL(MCx, _MCy);
            }
            else
            {
                _Longitude = x;
                _Latitude = y;

                convertLL2MC(_Longitude, _Latitude);
            }

        }

        private double _MCx;
        private double _MCy;

        public double MCx
        {
            get
            {
                    return _MCx;
            }
            set { _MCx = value; }
        }

        public double MCy
        {
            get
            {
                    return _MCy;
            }
            set { _MCy = value; }
        }


        private double _Longitude;
        private double _Latitude;

        public double Longitude
        {
            get
            {
                    return Math.Round(_Longitude, 6);
            }

            set { _Longitude = value; }
        }

        public double Latitude
        {
            get
            {
                    return Math.Round(_Latitude, 6);
            }
            set { _Latitude = value; }
        }



        private List<List<double>> LL2MC = new List<List<double>>(){new List<double>(){-0.0015702102444, 111320.7020616939, 1704480524535203, -10338987376042340, 26112667856603880, -35149669176653700, 26595700718403920, -10725012454188240, 1800819912950474, 82.5},
       new List<double>() {0.0008277824516172526, 111320.7020463578, 647795574.6671607, -4082003173.641316, 10774905663.51142, -15171875531.51559, 12053065338.62167,  -5124939663.577472, 913311935.9512032, 67.5},
       new List<double>() {0.00337398766765, 111320.7020202162, 4481351.045890365, -23393751.19931662, 79682215.47186455, -115964993.2797253, 97236711.15602145, -43661946.33752821, 8477230.501135234, 52.5},
       new List<double>() {0.00220636496208, 111320.7020209128, 51751.86112841131, 3796837.749470245, 992013.7397791013, -1221952.21711287, 1340652.697009075, -620943.6990984312, 144416.9293806241, 37.5},
       new List<double>() {-0.0003441963504368392, 111320.7020576856, 278.2353980772752, 2485758.690035394, 6070.750963243378, 54821.18345352118, 9540.606633304236, -2710.55326746645, 1405.483844121726, 22.5},
       new List<double>() {-0.0003218135878613132, 111320.7020701615, 0.00369383431289, 823725.6402795718, 0.46104986909093, 2351.343141331292, 1.58060784298199, 8.77738589078284, 0.37238884252424, 7.45}
            };

        private List<List<double>> MC2LL = new List<List<double>>() { new List<double>() { 1.410526172116255e-8, 0.00000898305509648872, -1.9939833816331, 200.9824383106796, -187.2403703815547, 91.6087516669843, -23.38765649603339, 2.57121317296198, -0.03801003308653, 17337981.2 }, new List<double>() { -7.435856389565537e-9, 0.000008983055097726239, -0.78625201886289, 96.32687599759846, -1.85204757529826, -59.36935905485877, 47.40033549296737, -16.50741931063887, 2.28786674699375, 10260144.86 }, new List<double>() { -3.030883460898826e-8, 0.00000898305509983578, 0.30071316287616, 59.74293618442277, 7.357984074871, -25.38371002664745, 13.45380521110908, -3.29883767235584, 0.32710905363475, 6856817.37 }, new List<double>() { -1.981981304930552e-8, 0.000008983055099779535, 0.03278182852591, 40.31678527705744, 0.65659298677277, -4.44255534477492, 0.85341911805263, 0.12923347998204, -0.04625736007561, 4482777.06 }, new List<double>() { 3.09191371068437e-9, 0.000008983055096812155, 0.00006995724062, 23.10934304144901, -0.00023663490511, -0.6321817810242, -0.00663494467273, 0.03430082397953, -0.00466043876332, 2555164.4 }, new List<double>() { 2.890871144776878e-9, 0.000008983055095805407, -3.068298e-8, 7.47137025468032, -0.00000353937994, -0.02145144861037, -0.00001234426596, 0.00010322952773, -0.00000323890364, 826088.5 } };
        private Double[] MCBAND = { 12890594.86, 8362377.87, 5591021d, 3481989.83, 1678043.12, 0d };
        private int[] LLBAND = { 75, 60, 45, 30, 15, 0 };



        private double getRange(double cf, double ce, double t)
        {
            if (ce != 0)
            {
                cf = cf > ce ? cf : ce;
            }
            if (t != 0)
            {
                cf = cf > t ? t : cf;
            }
            return cf;
        }

        private double getLoop(double cf, double ce, double t)
        {
            while (cf > t)
            {
                cf -= t - ce;
            }
            while (cf < ce)
            {
                cf += t - ce;
            }
            return cf;
        }

        private Tuple<double, double> convertor(double longitude, double latitude, double[] cg)
        {
            if (cg == null)
            {
                return null;
            }
            double t = cg[0] + cg[1] * longitude;
            double ce = Math.Abs(latitude) / cg[9];
            double ch = cg[2] + cg[3] * ce + cg[4] * Math.Pow(ce, 2) + cg[5] * Math.Pow(ce, 3) + cg[6] * Math.Pow(ce, 4) + cg[7] * Math.Pow(ce, 5) + cg[8] * Math.Pow(ce, 6);
            t = t * (longitude < 0 ? -1 : 1);
            ch = ch * (latitude < 0 ? -1 : 1);
            return Tuple.Create<double, double>(t, ch);
        }


        public void convertLL2MC(double longitude, double latitude)
        {
            try
            {


                longitude = getLoop(longitude, -180, 180);
                latitude = getRange(latitude, -74, 74);

                int countLLBAND = 7;
                double[] cg = null;
                for (int cf = 0; cf < countLLBAND; cf++)
                {
                    if (latitude >= LLBAND[cf])
                    {
                        cg = LL2MC[cf].ToArray();
                        break;
                    }
                }
                if (cg == null)
                {
                    for (int cf = countLLBAND - 1; cf >= 0; cf--)
                    {
                        if (longitude <= -LLBAND[cf])
                        {
                            cg = LL2MC[cf].ToArray();
                            break;
                        }
                    }
                }

                Tuple<double, double> result = convertor(longitude, latitude, cg);

                _MCx = result.Item1;
                _MCy = result.Item2;
            }
            catch (System.Exception )
            {
                _MCx = 0;
                _MCy = 0;
            }
        }
        public void convertMC2LL(double x, double y)
        {
            try
            {
                double[] cF = null;
                x = Math.Abs(x);
                y = Math.Abs(y);

                for (int cE = 0; cE < MCBAND.Length; cE++)
                {
                    if (y >= MCBAND[cE])
                    {
                        cF = MC2LL[cE].ToArray();
                        break;
                    }
                }
                Tuple<double, double> result = convertor(x, y, cF);
                _Longitude = result.Item1;
                _Latitude = result.Item2;
            }
            catch (System.Exception )
            {
                _Longitude = 0;
                _Latitude = 0;
            }
        }
    }
}
