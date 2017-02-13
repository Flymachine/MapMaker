using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MapMaker.TxtHelper
{
    class TxtHelper
    {
        private string _temp_path = "./template/temp.html";
        private string _ptemp_path = "./temp/ptemp.html";
        private string _stemp_path = "./temp/stemp.html";
        private string _ctemp_path = "./temp/ctemp.html";
        private string _current_html = "";
        private Dictionary<string, string> _conDic = new Dictionary<string, string>();
        private Dictionary<string, string> _varDic = new Dictionary<string, string>();

        /// <summary>  
        ///   模板网页存放位置
        /// </summary>
        public string temp_path
        {
            get
            {
                return _temp_path;
            }
            set
            {
                if(value.Length >= 4)
                {
                    _temp_path = value;
                }
            }
        }

        /// <summary>  
        ///   点预览网页存放位置
        /// </summary>
        public string ptemp_path
        {
            get
            {
                return _ptemp_path;
            }
            protected set
            {
                if (value.Length >= 4)
                {
                    _ptemp_path = value;
                }
            }
        }

        /// <summary>  
        ///   全图预览网页存放位置
        /// </summary>
        public string stemp_path
        {
            get
            {
                return _stemp_path;
            }
            protected set
            {
                if (value.Length >= 4)
                {
                    _stemp_path = value;
                }
            }
        }

        /// <summary>  
        ///   切片网页暂存位置
        /// </summary>
        public string ctemp_path
        {
            get
            {
                return _ctemp_path;
            }
            protected set
            {
                if (value.Length >= 4)
                {
                    _ctemp_path = value;
                }
            }
        }

        /// <summary>  
        ///   网页字符串暂存
        /// </summary>
        public string current_html
        {
            get
            {
                return _current_html;
            }
            protected set
            {
                if (value.Length >= 4)
                {
                    _current_html = value;
                }
            }
        }

        /// <summary>  
        ///   常量字典（Key = Value）
        /// </summary>
        public Dictionary<string, string> conDic
        {
            get
            {
                return _conDic;
            }
            protected set
            {
                _conDic = value;
            }
        }

        /// <summary>  
        ///   变量字典（Key = Value）
        /// </summary>
        public Dictionary<string, string> varDic
        {
            get
            {
                return _varDic;
            }
            protected set
            {
                _varDic = value;
            }
        }

        /// <summary>  
        ///   txthelper初始化（无参数，不建议使用）
        /// </summary>
        public TxtHelper()
        {

        }

        /// <summary>  
        ///   txthelper初始化
        /// </summary>
        /// <param name="con_dic">常量字典（Key = Value）</param>
        /// <param name="tempPath">模板网页存放位置</param>
        /// <param name="ptempPath">点预览网页存放位置</param>
        /// <param name="stempPath">全图预览网页存放位置</param>
        /// <param name="ctempPath">切片网页暂存位置</param>
        /// <param name="currentHtml">网页字符串暂存</param>
        public TxtHelper(Dictionary<string, string> con_dic, string tempPath = "", string ptempPath = "", string stempPath = "", string ctempPath = "", string currentHtml = "")
        {
            //赋值
            conDic = con_dic;
            temp_path = tempPath;
            ptemp_path = ptempPath;
            stemp_path = stempPath;
            ctemp_path = ctempPath;
            current_html = GetContent();
            current_html = currentHtml;

            //动作
            current_html = replace(conDic, current_html);
        }

        /// <summary>  
        ///   替换变量，生成文件,用于预览
        /// </summary>  
        /// <param name="var_dic">变量字典（Key = Value）</param>
        /// <param name="select">选择模块（"P","S","C"）</param>
        /// <returns>修改是否成功</returns>
        public string Change(Dictionary<string, string> var_dic, string select)
        {
            try
            {
                varDic = var_dic;
                string current = replace(varDic, current_html);
                switch (select.Trim().ToUpper())
                {
                    case "P":
                        SetContent(ptemp_path, current);
                        return ptemp_path;
                    case "S":
                        SetContent(stemp_path, current);
                        return stemp_path;
                    case "C":
                        SetContent(ctemp_path, current);
                        return ctemp_path;
                    default:
                        return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>  
        ///   获取基础模板
        /// </summary>  
        /// <returns>文件内容</returns>
        private string GetContent()
        {
            return File.ReadAllText(_temp_path, Encoding.UTF8);
        }

        /// <summary>  
        ///   撰写html文件
        /// </summary>  
        private void SetContent(string path, string content)
        {
            File.WriteAllText(path, content, Encoding.UTF8);
        }

        /// <summary>  
        ///   替换变量
        /// </summary>  
        /// <param name="Dic">key与value的词典</param>
        /// <param name="content">内容文本</param>
        /// <returns>替换完成后的内容文本</returns>
        public string replace(Dictionary<string, string> dic, string content)
        {
            string[] vars = dic.Keys.ToArray();
            foreach (string var in vars)
            {
                content.Replace("@" + var + "@", dic[var]);
            }
            return content;
        }

    }
}
