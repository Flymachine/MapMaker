using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapMaker.MapHelper
{
    public class MapHelper
    {
        /// <summary>  
        ///   像素/MC平面坐标互转
        /// </summary>  
        /// <param name="value"></param>  
        /// <param name="level">地图级别</param>
        /// <param name="OutPixel">trueMC平面坐标转像素,false像素转MC平面坐标</param>
        private double convert(double value, int level, bool OutPixel = false)
        {
            double value_1 = 0;
            if(OutPixel)
            {
                value_1 = Math.Floor(value * Math.Pow(2, level - 18));
            }
            else
            {
                value_1 = value / Math.Pow(2, level - 18);
            }
            return value_1;
        }
        /// <summary>  
        ///   计算点的平面坐标区域
        /// </summary>  
        /// <param name="level">地图级别</param>
        /// <param name="x">经度</param>  
        /// <param name="y">纬度</param>  
        /// <param name="bleedu">出血区域up</param>
        /// <param name="bleedr">出血区域right</param>
        /// <param name="bleedb">出血区域bloom</param>
        /// <param name="bleedl">出血区域left</param>
        public double[] convertArea(int level, double x, double y, double bleedu, double bleedr, double bleedb, double bleedl)
        {
            double[] MCArea = new double[4];
            MCObject mco = new MCObject(x, y);
            double bleedu_MC = convert(bleedu, level);
            double bleedr_MC = convert(bleedr, level);
            double bleedb_MC = convert(bleedb, level);
            double bleedl_MC = convert(bleedl, level);

            MCArea[0] = mco.MCy - bleedu_MC;
            MCArea[1] = mco.MCx + bleedr_MC;
            MCArea[2] = mco.MCy + bleedb_MC;
            MCArea[3] = mco.MCx - bleedl_MC;
            return MCArea;
        }
        /// <summary>  
        ///   计算地图的平面坐标区域
        /// </summary>  
        /// <param name="level">地图级别</param>
        /// <param name="x">经度</param>  
        /// <param name="y">纬度</param>  
        /// <param name="bleedu">出血区域up</param>
        /// <param name="bleedr">出血区域right</param>
        /// <param name="bleedb">出血区域bloom</param>
        /// <param name="bleedl">出血区域left</param>
    }
}
