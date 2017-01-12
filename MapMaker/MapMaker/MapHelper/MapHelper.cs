/*---------------------------------------------------------------- 
// 开发者Lei Yu版权所有。  
// 
// 文件名： MapHelper.cs
// 文件功能描述： 自动创建包含一些点的百度地图用于生成图片。
// author：Lei Yu
// 时间：2017-1-11
// 创建标识： 2017/1/12 测试完毕
// 
// 修改标识： 
//  
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace MapMaker.MapHelper
{
    public class MapHelper
    {
        private double _center_x_LL = 0, _center_y_LL = 0;
        private int _area_pixel_x = 0, _area_pixel_y = 0, _level = 18;
        private int[] _cut_area_pixel = new int[2],
            _user_area_pixel = new int[2],
            _area_bleed_Pixel = new int[4];                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
        private double[] _area_bleed_MC = new double[4],
            _area_MC = new double[4],
            _user_center_LL = new double[2];
        private Dictionary<int, double> _P_area_MC_up = new Dictionary<int, double>(),
             _P_area_MC_right = new Dictionary<int, double>(),
             _P_area_MC_bottom = new Dictionary<int, double>(),
             _P_area_MC_left = new Dictionary<int, double>();
        private Dictionary<int, double[]> _Ps_LL_and_bleed = new Dictionary<int, double[]>();
        #region Get and Set
        /// <summary>  
        ///   推荐中心点，经度（Longitude）
        /// </summary> 
        public double center_x_LL
        {
            get
            {
                return _center_x_LL;
            } 
        }
        /// <summary>  
        ///   推荐中心点，纬度（Latitude）
        /// </summary>
        public double center_y_LL
        {
            get
            {
                return _center_y_LL;
            }
        }
        /// <summary>  
        ///   推荐地图像素尺寸，宽度（Width）
        /// </summary>
        public int area_pixel_x
        {
            get
            {
                return _area_pixel_x;
            }
        }
        /// <summary>  
        ///   推荐地图像素尺寸，高度（Height）
        /// </summary>
        public int area_pixel_y
        {
            get
            {
                return _area_pixel_y;
            }
        }
        /// <summary>  
        ///   自设中心点，经纬度（{Longitude,Latitude}）
        /// </summary>
        public double[] user_center_LL
        {
            get
            {
                return _user_center_LL;
            }
            set
            {
                _user_center_LL = value;
                _area_MC = convertArea2UserArea(_user_center_LL, _area_MC);
                int[] auto_area_pixel = convertAreaMC2Pixel(_area_MC, _level);
                _area_pixel_x = auto_area_pixel[0];
                _area_pixel_y = auto_area_pixel[1];
            }
        }
        /// <summary>  
        ///   自设地图像素尺寸，宽高（{Width,Height}）
        /// </summary>
        public int[] user_area_pixel
        {
            get
            {
                return _user_area_pixel;
            }
            set
            {
                _user_area_pixel = value;
                _area_MC = convertAreaPixel2MC(_user_area_pixel, _level, _area_MC);
            }
        }
        /// <summary>  
        ///   地图级别，最低1级（世界地图），最高18级（分辨率约20米）
        /// </summary>
        public int level
        {
            get
            {
                return _level;
            }
            protected set
            {
                if(value < 1)
                {
                    _level = 1;
                }
                else if(value > 18)
                {
                    _level = 18;
                }
                else
                {
                    _level = value;
                }
                
            }
        }
        /// <summary>  
        ///   切图像素尺寸，宽高（{Width,Height}）
        /// </summary>
        public int[] cut_area_pixel
        {
            get
            {
                return _cut_area_pixel;
            }
            set
            {
                _cut_area_pixel = value;
            }
        }
        /// <summary>  
        ///   地图出血区大小（像素），上右下左（{Up,Right,Bottom,Left}）
        /// </summary>
        public int[] area_bleed_Pixel
        {
            get
            {
                return _area_bleed_Pixel;
            }
        }
        /// <summary>  
        ///   地图出血区大小（平面坐标），上右下左（{Up,Right,Bottom,Left}）
        /// </summary>
        public double[] area_bleed_MC
        {
            get
            {
                return _area_bleed_MC;
            }
        }
        /// <summary>  
        ///   地图区域坐标（平面坐标），上右下左（{Up,Right,Bottom,Left}）
        /// </summary>
        public double[] area_MC
        {
            get
            {
                return _area_MC;
            }
        }
        /// <summary>  
        ///   地图未覆盖到的点，其序号（pointid）
        /// </summary>
        public List<int> user_area_lack
        {
            get
            {
                return checkLackPs(_area_MC, _level, _P_area_MC_up, _P_area_MC_right, _P_area_MC_bottom, _P_area_MC_left);
            }
        }
        /// <summary>  
        ///   互相遮挡的点，其序号（pointid）
        /// </summary>
        public List<int> user_area_cover
        {
            get
            {
                return checkCoverPs(_level, _P_area_MC_up, _P_area_MC_right, _P_area_MC_bottom, _P_area_MC_left);
            }
        }
        /// <summary>  
        ///   切图分块，分块号、像素尺寸及中心点（%001%_%001% => {Width,Height,Longitude,Latitude}）
        /// </summary>
        public Dictionary<string, string[]> cut_pixel_and_center_LL
        {
            get
            {
                return CutArea2Dic(_area_MC, _cut_area_pixel, _level);
            }
        }
        /// <summary>  
        ///   点块，上边缘，序号及上平面坐标（pointid => Up_Y）
        /// </summary>
        public Dictionary<int, double> P_area_MC_up
        {
            get
            {
                return _P_area_MC_up;
            }
        }
        /// <summary>  
        ///   点块，右边缘，序号及右平面坐标（pointid => Right_X）
        /// </summary>
        public Dictionary<int, double> P_area_MC_right
        {
            get
            {
                return _P_area_MC_right;
            }
        }
        /// <summary>  
        ///   点块，下边缘，序号及下平面坐标（pointid => Bottom_Y）
        /// </summary>
        public Dictionary<int, double> P_area_MC_bottom
        {
            get
            {
                return _P_area_MC_bottom;
            }
        }
        /// <summary>  
        ///   点块，左边缘，序号及左平面坐标（pointid => Left_X）
        /// </summary>
        public Dictionary<int, double> P_area_MC_left
        {
            get
            {
                return _P_area_MC_left;
            }
        }
        /// <summary>  
        ///   点集合，序号、中心点及出血大小（像素）（pointid => {Longitude,Latitude,Up,Right,Bottom,Left}）
        /// </summary>
        public Dictionary<int, double[]> Ps_LL_and_bleed
        {
            get
            {
                return _Ps_LL_and_bleed;
            }
        }
        /// <summary>  
        ///   地图制作助手（缺少必要参数）
        /// </summary>
        public MapHelper()
        {

        }
        /// <summary>  
        ///   地图制作助手（不自设中心点和地图像素尺寸）
        /// </summary>
        public MapHelper(int user_level, int[] area_bleed_Pixel, int[] cut_area_pixel, Dictionary<int, double[]> Ps_LL_and_bleed)
        {
            level = user_level;
            _area_bleed_Pixel = area_bleed_Pixel;
            _cut_area_pixel = cut_area_pixel;
            _Ps_LL_and_bleed = Ps_LL_and_bleed;
            //以下运行代码
            covertP_LL2MC(_Ps_LL_and_bleed, level);
            _area_bleed_MC = new double[] { Math.Abs(convert(_area_bleed_Pixel[0], level)), Math.Abs(convert(_area_bleed_Pixel[1], level)) };
            _area_MC = sumPsArea(_area_bleed_MC, _P_area_MC_up, _P_area_MC_right, _P_area_MC_bottom, _P_area_MC_left);
            double[] auto_center_LL = convertArea_MC2P_LL(_area_MC);
            _center_x_LL = auto_center_LL[0];
            _center_y_LL = auto_center_LL[1];
            _user_center_LL = auto_center_LL;
            int[] auto_area_pixel = convertAreaMC2Pixel(_area_MC, level);
            _area_pixel_x = auto_area_pixel[0];
            _area_pixel_y = auto_area_pixel[1];
            _user_area_pixel = auto_area_pixel;

        }
        /// <summary>  
        ///   地图制作助手（自设中心点和地图像素尺寸）
        /// </summary>
        public MapHelper(int user_level, int[] area_bleed_Pixel, int[] cut_area_pixel, Dictionary<int, double[]> Ps_LL_and_bleed, double[] user_center_LL, int[] user_area_pixel)
        {
            level = user_level;
            _area_bleed_Pixel = area_bleed_Pixel;
            _cut_area_pixel = cut_area_pixel;
            _Ps_LL_and_bleed = Ps_LL_and_bleed;
            _user_center_LL = user_center_LL;
            _user_area_pixel = user_area_pixel;
            //以下运行代码
            covertP_LL2MC(_Ps_LL_and_bleed, level);
            _area_bleed_MC = new double[] { Math.Abs(convert(_area_bleed_Pixel[0], level)), Math.Abs(convert(_area_bleed_Pixel[1], level)) };
            _area_MC = sumPsArea(_area_bleed_MC, _P_area_MC_up, _P_area_MC_right, _P_area_MC_bottom, _P_area_MC_left);
            double[] auto_center_LL = convertArea_MC2P_LL(_area_MC);
            _center_x_LL = auto_center_LL[0];
            _center_y_LL = auto_center_LL[1];

            _area_MC = convertArea2UserArea(_user_center_LL, _area_MC);
            int[] auto_area_pixel = convertAreaMC2Pixel(_area_MC, level);
            _area_pixel_x = auto_area_pixel[0];
            _area_pixel_y = auto_area_pixel[1];

            _area_MC = convertAreaPixel2MC(_user_area_pixel, level, _area_MC);
        }
        #endregion
        #region 私有方法
        /// <summary>  
        ///   生成点的平面坐标，并录入类属性
        /// </summary>  
        /// <param name="Ps_LL_and_bleed">初始点字典</param>  
        /// <param name="level">地图级别</param>
        private void covertP_LL2MC(Dictionary<int, double[]> Ps_LL_and_bleed, int level)
        {
            try
            {
                Dictionary<int, double[]> list = Ps_LL_and_bleed;
                foreach (var item in list)
                {
                    Double[] center_MC = convertMC2LL(item.Value[0], item.Value[1]);
                    Double[] P_Area = convertP2Area(level, center_MC[0], center_MC[1], item.Value[2], item.Value[3], item.Value[4], item.Value[5]);
                    _P_area_MC_up[item.Key] = P_Area[0];
                    _P_area_MC_right[item.Key] = P_Area[1];
                    _P_area_MC_bottom[item.Key] = P_Area[2];
                    _P_area_MC_left[item.Key] = P_Area[3];
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        /// <summary>  
        ///   像素/MC平面坐标互转
        /// </summary>  
        /// <param name="value"></param>  
        /// <param name="level">地图级别</param>
        /// <param name="OutPixel">trueMC平面坐标转像素,false像素转MC平面坐标</param>
        public double convert(double value, int level, bool OutPixel = false)
        {
            try
            {
                double value_1 = 0;
                if (OutPixel)
                {
                    value_1 = Math.Floor(value * Math.Pow(2, level - 18));
                }
                else
                {
                    value_1 = value * Math.Pow(2, 18 - level);
                }
                return value_1;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>  
        ///   经纬坐标/平面坐标互转
        /// </summary>  
        /// <param name="x">经度</param>  
        /// <param name="y">纬度</param>  
        public double[] convertMC2LL(double x, double y, bool OutLongitudeAndLatitude = false)
        {
            try
            {
                MCObject mco = new MCObject(Math.Abs(x), Math.Abs(y), OutLongitudeAndLatitude);
                if (OutLongitudeAndLatitude)
                {
                    double[] LL = new double[2];
                    LL[0] = mco.Longitude * (x < 0 ? -1 : 1);
                    LL[1] = mco.Latitude * (y < 0 ? -1 : 1);
                    return LL;
                }
                double[] MC = new double[2];
                MC[0] = mco.MCx * (x < 0 ? -1 : 1);
                MC[1] = mco.MCy * (y < 0 ? -1 : 1);
                return MC;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>  
        ///   计算点的平面坐标区域
        /// </summary>  
        /// <param name="level">地图级别</param>
        /// <param name="MCx">平面坐标x</param>  
        /// <param name="MCy">平面坐标y</param>  
        /// <param name="bleedu">出血区域up</param>
        /// <param name="bleedr">出血区域right</param>
        /// <param name="bleedb">出血区域bottom</param>
        /// <param name="bleedl">出血区域left</param>
        public double[] convertP2Area(int level, double MCx, double MCy, double bleedu, double bleedr, double bleedb, double bleedl)
        {
            try
            {
                double[] MCArea = new double[4];
                double bleedu_MC = convert(bleedu, level);
                double bleedr_MC = convert(bleedr, level);
                double bleedb_MC = convert(bleedb, level);
                double bleedl_MC = convert(bleedl, level);

                MCArea[0] = MCy + bleedu_MC;
                MCArea[1] = MCx + bleedr_MC;
                MCArea[2] = MCy - bleedb_MC;
                MCArea[3] = MCx - bleedl_MC;
                return MCArea;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>  
        ///   计算地图的平面坐标区域
        /// </summary>  
        /// <param name="area_bleed_MC">地图出血设置</param>
        /// <param name="P_area_MC_up">点区域up</param>
        /// <param name="P_area_MC_right">点区域right</param>
        /// <param name="_P_area_MC_bottom">点区域bottom</param>
        /// <param name="_P_area_MC_left">点区域left</param>
        public double[] sumPsArea(double[] area_bleed_MC, Dictionary<int, double> P_area_MC_up, Dictionary<int, double> P_area_MC_right, Dictionary<int, double> P_area_MC_bottom, Dictionary<int, double> P_area_MC_left)
        {
            try
            {
                double[] area_MC = new double[4];
                area_MC[0] = P_area_MC_up.Values.ToArray().Max() + area_bleed_MC[0];
                area_MC[1] = P_area_MC_right.Values.ToArray().Max() + area_bleed_MC[1];
                area_MC[2] = P_area_MC_bottom.Values.ToArray().Min() - area_bleed_MC[2];
                area_MC[3] = P_area_MC_left.Values.ToArray().Min() - area_bleed_MC[3];
                return area_MC;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>  
        ///   计算地图的中心点（LL）
        /// </summary>  
        /// <param name="area_MC">地图区域平面坐标</param>
        public double[] convertArea_MC2P_LL(double[] area_MC)
        {
            try
            {
                return convertMC2LL((area_MC[1] + area_MC[3]) / 2, (area_MC[0] + area_MC[2]) / 2, true);
            }
            catch(Exception)
            {
                throw;
            }
            
        }
        // <summary>  
        ///   地图的中心点（LL）偏移，计算偏移后地图的平面坐标区域
        /// </summary>  
        /// <param name="user_center_LL">自设中心点</param>
        /// <param name="center_LL">自动中心点</param>
        /// <param name="area_MC">自动地图区域平面坐标</param>
        public double[] convertArea2UserArea(double[] user_center_LL, double[] area_MC)
        {
            try
            {
                double[] center_MC = new double[] { (area_MC[1] + area_MC[3]) / 2, (area_MC[0] + area_MC[2]) / 2 };
                double[] user_center_MC = convertMC2LL(user_center_LL[0], user_center_LL[1], false);
                double[] skew_center_MC = new double[] { user_center_MC[0] - center_MC[0], user_center_MC[1] - center_MC[1] };
                if (skew_center_MC[0] < 0)
                {
                    area_MC[3] += 2 * skew_center_MC[0];
                }
                else
                {
                    area_MC[1] += 2 * skew_center_MC[0];
                }
                if (skew_center_MC[1] < 0)
                {
                    area_MC[2] += 2 * skew_center_MC[1];
                }
                else
                {
                    area_MC[0] += 2 * skew_center_MC[1];
                }
                return area_MC;
            }
            catch(Exception)
            {
                throw;
            }
            
        }
        /// <summary>  
        ///   计算地图的像素尺寸
        /// </summary>  
        /// <param name="area_MC">地图区域平面坐标</param>
        /// <param name="level">地图等级</param>
        public int[] convertAreaMC2Pixel(double[] area_MC, int level)
        {
            try
            {
                int[] area_pixel = new int[2];
                area_pixel[0] = Convert.ToInt32(Math.Abs(convert(area_MC[1] - area_MC[3], level, true)));
                area_pixel[1] = Convert.ToInt32(Math.Abs(convert(area_MC[0] - area_MC[2], level, true)));
                return area_pixel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>  
        ///   自设像素尺寸，计算地图区域平面坐标
        /// </summary>  
        /// <param name="user_area_pixel">自设像素尺寸</param>
        /// <param name="level">地图等级</param>
        /// <param name="center_LL">中心点坐标</param>
        public double[] convertAreaPixel2MC(int[] user_area_pixel, int level, double[] area_MC)
        {
            double area_MC_x, area_MC_y;
            double[] center_MC, user_area_MC = new double[4];
            area_MC_x = Math.Abs(convert(user_area_pixel[0], level));
            area_MC_y = Math.Abs(convert(user_area_pixel[1], level));
            center_MC = new double[] { (area_MC[1] + area_MC[3]) / 2, (area_MC[0] + area_MC[2]) / 2 };
            user_area_MC[0] = center_MC[1] + (area_MC_y / 2);
            user_area_MC[1] = center_MC[0] + (area_MC_x / 2);
            user_area_MC[2] = center_MC[1] - (area_MC_y / 2);
            user_area_MC[3] = center_MC[0] - (area_MC_x / 2);

            return user_area_MC;
        }
        /// <summary>  
        ///   检查漏点，返回漏点数组
        /// </summary>  
        /// <param name="area_MC">地图区域平面坐标</param>
        /// <param name="P_area_MC_up">点区域up</param>
        /// <param name="P_area_MC_right">点区域right</param>
        /// <param name="_P_area_MC_bottom">点区域bottom</param>
        /// <param name="_P_area_MC_left">点区域left</param>
        public List<int> checkLackPs(double[] area_MC, int level, Dictionary<int, double> P_area_MC_up, Dictionary<int, double> P_area_MC_right, Dictionary<int, double> P_area_MC_bottom, Dictionary<int, double> P_area_MC_left)
        {
            List<int> user_area_lack = new List<int>();
            int[] Pid = P_area_MC_up.Keys.ToArray<int>();
            for (int i = 0; i < Pid.Length; i++)
            {
                if(convert(P_area_MC_up[Pid[i]], level, true) > convert(area_MC[0], level, true))
                {
                    user_area_lack.Add(Pid[i]);
                }else if(convert(P_area_MC_right[Pid[i]], level, true) > convert(area_MC[1], level, true))
                {
                    user_area_lack.Add(Pid[i]);
                }else if(convert(P_area_MC_bottom[Pid[i]], level, true) < convert(area_MC[2], level, true))
                {
                    user_area_lack.Add(Pid[i]);
                }else if(convert(P_area_MC_left[Pid[i]], level, true) < convert(area_MC[3], level, true))
                {
                    user_area_lack.Add(Pid[i]);
                }
            }
            return user_area_lack.Distinct().ToList();
        }
        /// <summary>  
        ///   检查重叠点，返回重叠点数组
        /// </summary>  
        /// <param name="P_area_MC_up">点区域up</param>
        /// <param name="P_area_MC_right">点区域right</param>
        /// <param name="_P_area_MC_bottom">点区域bottom</param>
        /// <param name="_P_area_MC_left">点区域left</param>
        public List<int> checkCoverPs(int level, Dictionary<int, double> P_area_MC_up, Dictionary<int, double> P_area_MC_right, Dictionary<int, double> P_area_MC_bottom, Dictionary<int, double> P_area_MC_left)
        {
            List<int> user_area_cover = new List<int>();
            int[] Pid = P_area_MC_up.Keys.ToArray<int>();
            List < int > Pid_normal = new List<int>();
            
            for (int i = 0; i < Pid.Length; i++)
            {
                if (!user_area_cover.Contains(Pid[i]))
                {
                    bool cover = false;
                    for (int j = 0; j < Pid.Length; j++)
                    {
                        if (i != j && !Pid_normal.Contains(Pid[i]))
                        {
                            if (convert(P_area_MC_up[Pid[i]], level, true) <= convert(P_area_MC_bottom[Pid[j]], level, true))
                            {
                                continue;
                            }
                            if (convert(P_area_MC_right[Pid[i]], level, true) <= convert(P_area_MC_left[Pid[j]], level, true))
                            {
                                continue;
                            }
                            if (convert(P_area_MC_bottom[Pid[i]], level, true) >= convert(P_area_MC_up[Pid[j]], level, true))
                            {
                                continue;
                            }
                            if (convert(P_area_MC_left[Pid[i]], level, true) >= convert(P_area_MC_right[Pid[j]], level, true))
                            {
                                continue;
                            }
                            cover = true;
                            user_area_cover.Add(Pid[i]);
                            user_area_cover.Add(Pid[j]);
                        }
                    }
                    if(!cover)
                    {
                        Pid_normal.Add(Pid[i]);
                    }
                    
                }
                
            }
            return user_area_cover.Distinct().ToList();
        }
        /// <summary>  
        ///   切片，返回切片的序列号（XY）、像素尺寸、中心点（LL）
        /// </summary>  
        /// <param name="area_MC">地图区域平面坐标</param>
        /// <param name="cut_area_pixel">切片像素尺寸</param>
        /// <param name="level">地图等级</param>
        public Dictionary<string, string[]> CutArea2Dic(double[] area_MC, int[] cut_area_pixel, int level)
        {
            Dictionary<string, string[]> cut_pixel_and_center_LL = new Dictionary<string, string[]>();
            double cut_area_MC_x, cut_area_MC_y, area_MC_x, area_MC_y, last_cut_MC_x, last_cut_MC_y;
            int cut_x, cut_y, last_CA_pixel_x, last_CA_pixel_y, cut_x_len, cut_y_len;
            cut_area_MC_x = Math.Abs(convert(cut_area_pixel[0], level));
            cut_area_MC_y = Math.Abs(convert(cut_area_pixel[1], level));
            area_MC_x = area_MC[1] - area_MC[3];
            area_MC_y = area_MC[0] - area_MC[2];
            cut_x = Convert.ToInt32(Math.Ceiling(area_MC_x / cut_area_MC_x));
            cut_y = Convert.ToInt32(Math.Ceiling(area_MC_y / cut_area_MC_y));
            last_cut_MC_x = area_MC_x - (cut_area_MC_x * (cut_x - 1));
            last_cut_MC_y = area_MC_y - (cut_area_MC_y * (cut_y - 1));
            last_CA_pixel_x = Convert.ToInt32(Math.Abs(convert(last_cut_MC_x, level, true)));
            last_CA_pixel_y = Convert.ToInt32(Math.Abs(convert(last_cut_MC_y, level, true)));
            cut_x_len = cut_x.ToString().Length;
            cut_y_len = cut_y.ToString().Length;
            for (int i = 0; i < cut_x ; i++)
            {
                string title_x = i.ToString().PadLeft(cut_x_len, '0');
                int pixel_x;
                double[] cut_area_MC = new double[4];
                if (i == cut_x -1)
                {
                    if(last_CA_pixel_x < 1)
                    {
                        break;
                    }
                    pixel_x = last_CA_pixel_x;
                    cut_area_MC[1] = area_MC[1];
                    cut_area_MC[3] = area_MC[1] - last_CA_pixel_x;
                }
                else
                {
                    pixel_x = cut_area_pixel[0];
                    cut_area_MC[1] = area_MC[3] + (cut_area_MC_x * (i + 1));
                    cut_area_MC[3] = area_MC[3] + (cut_area_MC_x * i);
                }
                for(int j = 0; j < cut_y; j++)
                {
                    string title = title_x + "_" + j.ToString().PadLeft(cut_y_len, '0');
                    int pixel_y;
                    if (j == cut_y - 1)
                    {
                        if (last_CA_pixel_y < 1)
                        {
                            break;
                        }
                        pixel_y = last_CA_pixel_y;
                        cut_area_MC[0] = area_MC[2] + last_CA_pixel_y;
                        cut_area_MC[2] = area_MC[2];
                    }
                    else
                    {
                        pixel_y = cut_area_pixel[1];
                        cut_area_MC[0] = area_MC[0] - (cut_area_MC_y * j);
                        cut_area_MC[2] = area_MC[0] - (cut_area_MC_y * (j + 1)); 
                    }
                    double[] CA_center_LL = convertArea_MC2P_LL(cut_area_MC);
                    string[] content = new string[] { pixel_x.ToString(), pixel_y.ToString(), CA_center_LL[0].ToString(), CA_center_LL[1].ToString() };
                    cut_pixel_and_center_LL.Add(title, content);
                }
            }

            return cut_pixel_and_center_LL;
        }

    }
}
