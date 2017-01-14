using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gecko;
using System.Data.SQLite;
using Freezer.Core;
using System.IO;
using MapMaker.MapHelper;


namespace MapMaker
{
    public partial class MainForm1 : Form
    {
        SQLiteConnection conn = new SQLiteConnection("data source=./mapmaker.db");
        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteHelper sh;

        public MainForm1()
        {
            InitializeComponent();
            Xpcom.Initialize("Firefox");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmd.Connection = conn;
            conn.Open();
            sh = new SQLiteHelper(cmd);
            DisplayProjectSet(GetProjectSet(sh, 1));
            DisplayProjects(sh);
            DisplayPoints(sh, "1");
            DisplayMapstyleSet(sh);
            DisplayFonts(sh);


            //geckoWebBrowser1.Navigate("file:///E:/git/map.html");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var screenshotJob = ScreenshotJobBuilder.Create("file:///E:/git/map.html")
              .SetBrowserSize(3000, 2000) //3000,3010
              .SetCaptureZone(CaptureZone.FullPage) // Set what should be captured
              .SetTrigger(new WindowLoadTrigger());  // Set when the picture is taken new FreezerJsEventTrigger()

                File.WriteAllBytes("1.png", screenshotJob.Freeze());
                MessageBox.Show("ok");
            }
            catch (Freezer.Engines.CaptureEngineException ex)
            {
                MessageBox.Show("可能是由于设定尺寸太大，无法加载。建议缩小项目的地图尺寸或截图尺寸再重试。"+ System.Environment.NewLine + ex.ToString(), "错误警告！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("不明错误，请与作者联系，并提供错误截图。"+ System.Environment.NewLine + ex.ToString(), "错误警告！");
            }
            finally
            {
                var workers = typeof(FreezerGlobal).GetField("_workers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                var workerPool = workers?.GetValue(null) as IDisposable;
                workerPool?.Dispose();
            }
            
     
        }

        private void MainForm1_FormClosed(object sender, FormClosedEventArgs e)
        {
            conn.Close();
        }

        #region Project设置相关
        private void DisplayProjectSet(Dictionary<string, string> dic)
        {
            textBox8.Text = dic["projectid"];
            textBox1.Text = dic["title"];
            numericUpDown1.Value = Convert.ToInt32(dic["level"]);
            numericUpDown2.Value = Convert.ToInt32(dic["bleedu"]);
            numericUpDown3.Value = Convert.ToInt32(dic["bleedr"]);
            numericUpDown4.Value = Convert.ToInt32(dic["bleedb"]);
            numericUpDown5.Value = Convert.ToInt32(dic["bleedl"]);
            checkBox7.Checked = Convert.ToBoolean(Convert.ToInt32(dic["ucenter"]));
            numericUpDown6.Value = Convert.ToDecimal(dic["usercenterx"]);
            numericUpDown7.Value = Convert.ToDecimal(dic["usercentery"]);
            checkBox8.Checked = Convert.ToBoolean(Convert.ToInt32(dic["upixel"]));
            numericUpDown8.Value = Convert.ToInt32(dic["userpxx"]);
            numericUpDown9.Value = Convert.ToInt32(dic["userpxy"]);
            numericUpDown10.Value = Convert.ToInt32(dic["cutpxx"]);
            numericUpDown11.Value = Convert.ToInt32(dic["cutpxy"]);
            numericUpDown12.Value = Convert.ToInt32(dic["delay"]);
        }
        private void DisplayProjects(SQLiteHelper sh)
        {
            DataTable dt = sh.Select("select * from project;");
            foreach (DataColumn col in dt.Columns)
            {
                string colName = col.ColumnName;
                col.ColumnName = ToChineseName(colName);
            }
            dataGridView3.DataSource = dt;
        }
        private void SaveProjectSet(SQLiteHelper sh)
        {
            var dicData = new Dictionary<string, object>();
            dicData["title"] = textBox1.Text;
            dicData["level"] = numericUpDown1.Value;
            dicData["bleedu"] = numericUpDown2.Value;
            dicData["bleedr"] = numericUpDown3.Value;
            dicData["bleedb"] = numericUpDown4.Value;
            dicData["bleedl"] = numericUpDown5.Value;
            dicData["ucenter"] = Convert.ToInt32(checkBox7.Checked).ToString();
            dicData["usercenterx"] = numericUpDown6.Value;
            dicData["usercentery"] = numericUpDown7.Value;
            dicData["upixel"] = Convert.ToInt32(checkBox8.Checked).ToString();
            dicData["userpxx"] = numericUpDown8.Value;
            dicData["userpxy"] = numericUpDown9.Value;
            dicData["cutpxx"] = numericUpDown10.Value;
            dicData["cutpxy"] = numericUpDown11.Value;
            dicData["delay"] = numericUpDown12.Value;
            if (textBox8.Text.Trim() == "" || textBox8.Text.Trim() == "0")
            {
                sh.Insert("project", dicData);
            }
            else
            {
                sh.Update("project", dicData, "projectid", textBox8.Text.Trim());
            }
        }
        private Dictionary<string, string> GetProjectSet(SQLiteHelper sh, int id)
        {
            DataTable dt;
            if (id == 0)
            {
                dt = sh.Select("select * from project where projectid = max(projectid);");
            }
            else
            {
                dt = sh.Select("select * from project where projectid = " + id + ";");
            }
            var dic = new Dictionary<string, string>();
            if (dt == null)
            {
                return dic;
            }
            dic["projectid"] = dt.Rows[0]["projectid"].ToString();
            dic["title"] = dt.Rows[0]["title"].ToString();
            dic["level"] = dt.Rows[0]["level"].ToString();
            dic["bleedu"] = dt.Rows[0]["bleedu"].ToString();
            dic["bleedr"] = dt.Rows[0]["bleedr"].ToString();
            dic["bleedb"] = dt.Rows[0]["bleedb"].ToString();
            dic["bleedl"] = dt.Rows[0]["bleedl"].ToString();
            dic["upixel"] = dt.Rows[0]["upixel"].ToString();
            dic["userpxx"] = dt.Rows[0]["userpxx"].ToString();
            dic["userpxy"] = dt.Rows[0]["userpxy"].ToString();
            dic["ucenter"] = dt.Rows[0]["ucenter"].ToString();
            dic["usercenterx"] = dt.Rows[0]["usercenterx"].ToString();
            dic["usercentery"] = dt.Rows[0]["usercentery"].ToString();
            dic["cutpxx"] = dt.Rows[0]["cutpxx"].ToString();
            dic["cutpxy"] = dt.Rows[0]["cutpxy"].ToString();
            dic["delay"] = dt.Rows[0]["delay"].ToString();
            return dic;
        }
        private void DelProjectSet(SQLiteHelper sh, int projectid)
        {
            sh.Execute("delete from project where projectid = " + projectid + ";");
        }
        #endregion

        #region Point设置相关
        private void DisplayPointSet(Dictionary<string, string> dic)
        {
            textBox9.Text = dic["pointid"];
            textBox2.Text = htmldecode(dic["title"]);
            textBox3.Text = htmldecode(dic["content"]);
            numericUpDown13.Value = Convert.ToDecimal(dic["pointx"]);
            numericUpDown14.Value = Convert.ToDecimal(dic["pointy"]);
            checkBox3.Checked = Convert.ToBoolean(Convert.ToInt32(dic["isopen"]));
            checkBox1.Checked = Convert.ToBoolean(Convert.ToInt32(dic["switch"]));
            checkBox2.Checked = Convert.ToBoolean(Convert.ToInt32(dic["stealth"]));
            textBox4.Text = dic["icon"];
            pictureBox1.ImageLocation = dic["icon"];
            comboBox1.Text = dic["font"];
            numericUpDown15.Value = Convert.ToInt32(dic["skewx"]);
            numericUpDown16.Value = Convert.ToInt32(dic["skewy"]);
            numericUpDown17.Value = Convert.ToInt32(dic["bleedu"]);
            numericUpDown18.Value = Convert.ToInt32(dic["bleedr"]);
            numericUpDown19.Value = Convert.ToInt32(dic["bleedb"]);
            numericUpDown20.Value = Convert.ToInt32(dic["bleedl"]);
        }
        private void DisplayPoints(SQLiteHelper sh, string project)
        {
            DataTable dt = sh.Select("select * from point where project = " + project + ";");
            foreach(DataColumn col in dt.Columns)
            {
                string colName = col.ColumnName;
                col.ColumnName = ToChineseName(colName);
            }
            dataGridView1.DataSource = dt;
        }
        private void SavePointSet(SQLiteHelper sh)
        {
            var dic = new Dictionary<string, object>();
            dic["title"] = htmlencode(textBox2.Text);
            dic["content"] = htmlencode(textBox3.Text);
            dic["pointx"] = numericUpDown13.Value;
            dic["pointy"] = numericUpDown14.Value;
            dic["isopen"] = Convert.ToInt32(checkBox3.Checked);
            dic["icon"] = textBox4.Text.Trim();
            dic["font"] = comboBox1.Text.Trim();
            dic["skewx"] = numericUpDown15.Value;
            dic["skewy"] = numericUpDown16.Value;
            dic["switch"] = Convert.ToInt32(checkBox1.Checked);
            dic["stealth"] = Convert.ToInt32(checkBox2.Checked);
            dic["bleedu"] = numericUpDown17.Value;
            dic["bleedr"] = numericUpDown18.Value;
            dic["bleedb"] = numericUpDown19.Value;
            dic["bleedl"] = numericUpDown20.Value;
            if (textBox9.Text.Trim() == ""|| textBox9.Text.Trim() == "0")
            {
                if(textBox8.Text.Trim() == "" || textBox8.Text.Trim() == "0")
                {
                    MessageBox.Show("项目未保存，无法添加此点，请先保存项目", "警告");
                    return ;
                }
                dic["project"] = textBox8.Text.Trim();
                sh.Insert("point", dic);
            }
            else
            {
                int pointid = Convert.ToInt32(textBox9.Text.Trim());
                sh.Update("point", dic, "pointid", pointid);
            }
        }
        private Dictionary<string, string> GetPointSet(SQLiteHelper sh, int pointid)
        {
            DataTable dt;
            if (pointid == 0)
            {
              dt = sh.Select("select * from point where pointid = max(pointid);");
            }
            else
            {
                dt = sh.Select("select * from point where pointid = " + pointid + ";");
            }
            var dic = new Dictionary<string, string>();
            if (dt == null)
            {
                return dic;
            }
            dic["pointid"] = pointid.ToString();
            dic["title"] = htmldecode(dt.Rows[0]["title"].ToString());
            dic["content"] = htmldecode(dt.Rows[0]["content"].ToString());
            dic["pointx"] = dt.Rows[0]["pointx"].ToString();
            dic["pointy"] = dt.Rows[0]["pointy"].ToString();
            dic["isopen"] = dt.Rows[0]["isopen"].ToString();
            dic["icon"] = dt.Rows[0]["icon"].ToString();
            dic["font"] = dt.Rows[0]["font"].ToString();
            dic["skewx"] = dt.Rows[0]["skewx"].ToString();
            dic["skewy"] = dt.Rows[0]["skewy"].ToString();
            dic["project"] = dt.Rows[0]["project"].ToString();
            dic["switch"] = dt.Rows[0]["switch"].ToString();
            dic["stealth"] = dt.Rows[0]["stealth"].ToString();
            dic["bleedu"] = dt.Rows[0]["bleedu"].ToString();
            dic["bleedr"] = dt.Rows[0]["bleedr"].ToString();
            dic["bleedb"] = dt.Rows[0]["bleedb"].ToString();
            dic["bleedl"] = dt.Rows[0]["bleedl"].ToString();
            return dic;
        }
        private void DelPointSet(SQLiteHelper sh, int pointid)
        {
            sh.Execute("delete from point where pointid = " + pointid + ";");
        }
        private void SwitchPointSet(SQLiteHelper sh, int pointid, int value)
        {
            ShotCutPointSet(sh, pointid, "switch", value);
        }
        private void StealthPointSet(SQLiteHelper sh, int pointid, int value)
        {
            ShotCutPointSet(sh, pointid, "stealth", value);
        }
        private void ShotCutPointSet(SQLiteHelper sh, int pointid, string key, int value)
        {
            sh.Execute("update point set " + key + " = " + value + " where pointid = " + pointid + ";");
        }
        #endregion

        #region Mapstyle设置相关
        private void DisplayMapstyleSet(SQLiteHelper sh)
        {
            var dicData1 = GetMapstyleSet(sh, 1);
            var dicData2 = GetMapstyleSet(sh, 2);
            var dicData3 = GetMapstyleSet(sh, 3);
            textBox11.Text = dicData1["styleid"];
            textBox12.Text = dicData1["key"];
            label15.Text = dicData1["title"];
            textBox5.Text = dicData1["content"];
            checkBox6.Checked = Convert.ToBoolean(Convert.ToInt32(dicData2["switch"]));

            textBox16.Text = dicData2["styleid"];
            textBox15.Text = dicData2["key"];
            label29.Text = dicData2["title"];
            textBox13.Text = dicData2["content"];
            checkBox4.Checked = Convert.ToBoolean(Convert.ToInt32(dicData2["switch"]));

            textBox18.Text = dicData3["styleid"];
            textBox17.Text = dicData3["key"];
            label30.Text = dicData3["title"];
            textBox14.Text = dicData3["content"];
            checkBox5.Checked = Convert.ToBoolean(Convert.ToInt32(dicData3["switch"]));
        }
        private void SaveMapstyleSet(SQLiteHelper sh)
        {
            var dicData = new Dictionary<string, object>();
            dicData["key"] = textBox12.Text;
            dicData["content"] = textBox5.Text;
            dicData["switch"] = Convert.ToInt32(checkBox6.Checked);

            sh.Update("mapstyle", dicData, "styleid", textBox11.Text.Trim());


            var dicData1 = new Dictionary<string, object>();
            dicData1["key"] = textBox15.Text;
            dicData1["content"] = textBox13.Text;
            dicData1["switch"] = Convert.ToInt32(checkBox4.Checked);

            sh.Update("mapstyle", dicData, "styleid", textBox16.Text.Trim());


            var dicData2 = new Dictionary<string, object>();
            dicData2["key"] = textBox17.Text;
            dicData2["content"] = textBox14.Text;
            dicData2["switch"] = Convert.ToInt32(checkBox5.Checked);

            sh.Update("mapstyle", dicData, "styleid", textBox18.Text.Trim());
        }
        private Dictionary<string, string> GetMapstyleSet(SQLiteHelper sh, int styleid)
        {
            DataTable dt = sh.Select("select * from mapstyle where styleid = " + styleid + ";");
            var dic = new Dictionary<string, string>();
            if (dt == null)
            {
                return dic;
            }
            dic["styleid"] = styleid.ToString();
            dic["key"] = dt.Rows[0]["key"].ToString();
            dic["title"] = dt.Rows[0]["title"].ToString();
            dic["content"] = dt.Rows[0]["content"].ToString();
            dic["switch"] = dt.Rows[0]["switch"].ToString();
            return dic;
        }
        #endregion

        #region Font设置相关
        private void DisplayFontSet(Dictionary<string, string> dic)
        {
            textBox6.Text = dic["title"];
            numericUpDown21.Value = Convert.ToInt32(dic["fontSize"]);
            textBox7.Text = dic["content"];
            textBox10.Text = dic["fontid"];
        }
        private void DisplayFonts(SQLiteHelper sh)
        {
            DataTable dt = sh.Select("select * from font;");
            comboBox1.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                string fontid = row[0].ToString();
                comboBox1.Items.Add(fontid);
            }
            foreach (DataColumn col in dt.Columns)
            {
                string colName = col.ColumnName;
                col.ColumnName = ToChineseName(colName);
            }
            dataGridView2.DataSource = dt;
        }
        private void SaveFontSet(SQLiteHelper sh)
        {
            var dic = new Dictionary<string, object>();
            dic["title"] = textBox6.Text;
            dic["fontSize"] = numericUpDown21.Value;
            dic["content"] = textBox7.Text;
            if (textBox9.Text.Trim() == "" || textBox9.Text.Trim() == "0")
            {
                sh.Insert("font", dic);
            }
            else
            {
                int fontid = Convert.ToInt32(textBox10.Text.Trim());
                sh.Update("font", dic, "fontid", fontid);
            }
        }
        private Dictionary<string, string> GetFontSet(SQLiteHelper sh, int id)
        {
            DataTable dt;
            if (id == 0)
            {
                dt = sh.Select("select * from font where fontid = max(fontid);");
            }else
            {
                dt = sh.Select("select * from font where fontid = " + id + ";");
            }
            var dic = new Dictionary<string, string>();
            if (dt == null)
            {
                return dic;
            }
            dic["fontid"] = id.ToString();
            dic["title"] = dt.Rows[0]["title"].ToString();
            dic["fontSize"] = dt.Rows[0]["fontSize"].ToString();
            dic["content"] = dt.Rows[0]["content"].ToString();
            return dic;
        }
        private void DelFontSet(SQLiteHelper sh, int Fontid)
        {
            sh.Execute("delete from font where fontid = " + Fontid + ";");
        }
        #endregion

        #region 其他
        public static string htmlencode(string str)
        {
            str.Replace(" ", " ");
            str.Replace("　", " ");
            str.Replace("/n", "<br/>");
            return str;
        }
        public static string htmldecode(string str)
        {
            str.Replace("<br/>", "/n");
            return str;
        }
        private string ToChineseName(string en)
        {
            string cn = en;
            switch (en)
            {
                case "pointid":
                    cn = "序号";
                    break;
                case "fontid":
                    cn = "序号";
                    break;
                case "projectid":
                    cn = "序号";
                    break;
                case "title":
                    cn = "标题";
                    break;
                case "content":
                    cn = "内容";
                    break;
                case "pointx":
                    cn = "X坐标(经纬)";
                    break;
                case "pointy":
                    cn = "Y坐标(经纬)";
                    break;
                case "isopen":
                    cn = "显示内容";
                    break;
                case "icon":
                    cn = "图标";
                    break;
                case "font":
                    cn = "字体模板";
                    break;
                case "skewx":
                    cn = "X偏移";
                    break;
                case "skewy":
                    cn = "Y偏移";
                    break;
                case "project":
                    cn = "所属项目";
                    break;
                case "switch":
                    cn = "失效";
                    break;
                case "stealth":
                    cn = "隐形";
                    break;
                case "bleedu":
                    cn = "上出血区";
                    break;
                case "bleedr":
                    cn = "右出血区";
                    break;
                case "bleedb":
                    cn = "下出血区";
                    break;
                case "bleedl":
                    cn = "左出血区";
                    break;
                case "ctime":
                    cn = "创建时间";
                    break;
                case "ltime":
                    cn = "修改时间";
                    break;
                case "fontSize":
                    cn = "fontSize(尺寸)";
                    break;
                case "level":
                    cn = "等级";
                    break;
                case "upixel":
                    cn = "设尺寸";
                    break;
                case "userpxx":
                    cn = "设宽";
                    break;
                case "userpxy":
                    cn = "设高";
                    break;
                case "ucenter":
                    cn = "设中心";
                    break;
                case "usercenterx":
                    cn = "设经度";
                    break;
                case "usercentery":
                    cn = "设纬度";
                    break;
                case "cut":
                    cn = "整切";
                    break;
                case "cutpxx":
                    cn = "切宽";
                    break;
                case "cutpxy":
                    cn = "切高";
                    break;
                case "delay":
                    cn = "延迟";
                    break;

                default:
                    cn = en;
                    break;
            }
            return cn;
        }
        #endregion

        #region 设置用键
        private void button15_Click(object sender, EventArgs e)
        {
            string resultFile = textBox4.Text;
            //openFileDialog1.ShowDialog();
           if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                resultFile = openFileDialog1.FileName;
            }
            string appPatch = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            if(resultFile.Contains(appPatch))
            {
                resultFile=resultFile.Replace(appPatch, "./");
                resultFile=resultFile.Replace(@"\", "/");
            }
            else
            {
                string ext = Path.GetExtension(resultFile);
                DateTime dt = DateTime.Now;
                string p_2 = "./icon/" + dt.ToShortDateString().ToString() + "/";
                //检查是否存在转移目录
                if (!Directory.Exists(p_2))
                {
                    Directory.CreateDirectory(p_2);
                }
                string resultFile_2 = p_2 + dt.ToFileTime().ToString() + ext;
                File.Copy(resultFile, resultFile_2, true);
                resultFile = resultFile_2;
            }
            textBox4.Text = resultFile;
            pictureBox1.ImageLocation = resultFile;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            MessageBox.Show("保存新项目仍需点击“添加”，为了方便建点，请及时保存", "提示");
            textBox8.Text = "";
        }

        private void button30_Click(object sender, EventArgs e)
        {
            SaveProjectSet(sh);
            DisplayProjectSet(GetProjectSet(sh, Convert.ToInt32(textBox8.Text.Trim())));
            DisplayProjects(sh);
            MessageBox.Show("保存成功", "提示");
        }

        private void button28_Click(object sender, EventArgs e)
        {
            int a = dataGridView3.CurrentRow.Index;
            DisplayProjectSet(GetProjectSet(sh, Convert.ToInt32(dataGridView3.Rows[a].Cells["序号"].Value)));
        }

        private void button29_Click(object sender, EventArgs e)
        {
            DialogResult RSS = MessageBox.Show(this, "确定要删除全部选中行数据吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            switch (RSS)
            {
                case DialogResult.Yes:
                    for (int i = this.dataGridView3.SelectedRows.Count; i > 0; i--)
                    {
                        int ID = Convert.ToInt32(dataGridView3.SelectedRows[i - 1].Cells[0].Value);
                        dataGridView3.Rows.RemoveAt(dataGridView3.SelectedRows[i - 1].Index);
                        //使用获得的ID删除数据库的数据
                        DelProjectSet(sh, Convert.ToInt32(ID));
                    }
                    MessageBox.Show("成功删除选中行数据！");
                    break;
                case DialogResult.No:
                    break;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            SavePointSet(sh);
            textBox9.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            numericUpDown13.Value = 0;
            numericUpDown14.Value = 0;
            DisplayPoints(sh, textBox8.Text.Trim()=="" ? "0" : textBox8.Text.Trim());
            MessageBox.Show("保存成功", "提示");
        }

        private void button22_Click(object sender, EventArgs e)
        {
            MessageBox.Show("保存新点仍需点击“添加”", "提示");
            textBox9.Text = "";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int a = dataGridView1.CurrentRow.Index;
            DisplayPointSet(GetPointSet(sh, Convert.ToInt32(dataGridView1.Rows[a].Cells["序号"].Value)));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult RSS = MessageBox.Show(this, "确定要删除全部选中行数据吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            switch (RSS)
            {
                case DialogResult.Yes:
                    for (int i = this.dataGridView1.SelectedRows.Count; i > 0; i--)
                    {
                        int ID = Convert.ToInt32(dataGridView1.SelectedRows[i - 1].Cells[0].Value);
                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[i - 1].Index);
                        //使用获得的ID删除数据库的数据
                        DelPointSet(sh, Convert.ToInt32(ID));
                    }
                    MessageBox.Show("成功删除选中行数据！");
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult RSS = MessageBox.Show(this, "确定要将选中点全部生效吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            switch (RSS)
            {
                case DialogResult.Yes:
                    for (int i = this.dataGridView1.SelectedRows.Count; i > 0; i--)
                    {
                        int ID = Convert.ToInt32(dataGridView1.SelectedRows[i - 1].Cells[0].Value);
                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[i - 1].Index);
                        //使用获得的ID更改数据库的数据
                        SwitchPointSet(sh, Convert.ToInt32(ID), 0);
                    }
                    MessageBox.Show("成功更改选中点数据！");
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult RSS = MessageBox.Show(this, "确定要将选中点全部失效码？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            switch (RSS)
            {
                case DialogResult.Yes:
                    for (int i = this.dataGridView1.SelectedRows.Count; i > 0; i--)
                    {
                        int ID = Convert.ToInt32(dataGridView1.SelectedRows[i - 1].Cells[0].Value);
                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[i - 1].Index);
                        //使用获得的ID更改数据库的数据
                        SwitchPointSet(sh, Convert.ToInt32(ID), 1);
                    }
                    MessageBox.Show("成功更改选中点数据！");
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult RSS = MessageBox.Show(this, "确定要将选中点全部隐形码？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            switch (RSS)
            {
                case DialogResult.Yes:
                    for (int i = this.dataGridView1.SelectedRows.Count; i > 0; i--)
                    {
                        int ID = Convert.ToInt32(dataGridView1.SelectedRows[i - 1].Cells[0].Value);
                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[i - 1].Index);
                        //使用获得的ID更改数据库的数据
                        StealthPointSet(sh, Convert.ToInt32(ID), 1);
                    }
                    MessageBox.Show("成功更改选中点数据！");
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DialogResult RSS = MessageBox.Show(this, "确定要将选中点全部显形码？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            switch (RSS)
            {
                case DialogResult.Yes:
                    for (int i = this.dataGridView1.SelectedRows.Count; i > 0; i--)
                    {
                        int ID = Convert.ToInt32(dataGridView1.SelectedRows[i - 1].Cells[0].Value);
                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[i - 1].Index);
                        //使用获得的ID更改数据库的数据
                        StealthPointSet(sh, Convert.ToInt32(ID), 0);
                    }
                    MessageBox.Show("成功更改选中点数据！");
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            SaveMapstyleSet(sh);
            MessageBox.Show("保存成功", "提示");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            SaveFontSet(sh);
            textBox6.Text = "";
            textBox10.Text = "";
            DisplayFonts(sh);
            MessageBox.Show("保存成功", "提示");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int a = dataGridView2.CurrentRow.Index;
            DisplayFontSet(GetFontSet(sh, Convert.ToInt32(dataGridView2.Rows[a].Cells["序号"].Value)));
        }

        private void button13_Click(object sender, EventArgs e)
        {
            DialogResult RSS = MessageBox.Show(this, "确定要删除全部选中行数据吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            switch (RSS)
            {
                case DialogResult.Yes:
                    for (int i = this.dataGridView2.SelectedRows.Count; i > 0; i--)
                    {
                        int ID = Convert.ToInt32(dataGridView2.SelectedRows[i - 1].Cells[0].Value);
                        dataGridView2.Rows.RemoveAt(dataGridView2.SelectedRows[i - 1].Index);
                        //使用获得的ID删除数据库的数据
                        DelFontSet(sh, ID);
                    }
                    MessageBox.Show("成功删除选中行数据！");
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            MessageBox.Show("保存新字体仍需点击“添加”", "提示");
            textBox10.Text = "";
        }

        #endregion

        private void button27_Click(object sender, EventArgs e)
        {
            //字体部分
            Dictionary<string, string> font = GetFontSet(sh, Convert.ToInt32(comboBox1.Text.Trim()));
            int fontSize = Convert.ToInt32(font["fontSize"].Trim());
            int titleSize = textBox2.Text.Length ;
            string[] content = textBox3.Lines;
            int con_height = content.Length;
            int con_max_width = titleSize;
            foreach (string line in content)
            {
                if(line.Length > con_max_width) con_max_width = line.Length ;
            }
            int textSize_h = fontSize + 2;
            int textSize_w = titleSize * fontSize + 2;
            if (checkBox3.Checked)
            {
                textSize_h = (con_height + 2) * fontSize + 2;
                textSize_w = con_max_width * fontSize + 2;
            }

            //图标部分
            int imgSize_h = pictureBox1.Image ==null ? 25 : pictureBox1.Image.Height;
            int imgSize_w = pictureBox1.Image == null ? 10 : pictureBox1.Image.Width / 2;
            //偏移部分
            int skewSize_w = Convert.ToInt32(numericUpDown15.Value);
            int skewSize_h= Convert.ToInt32(numericUpDown16.Value);
            //额外部分
            int exSize_w = 0;
            int exSize_h = 0;
            //整合
            int[] x = new int[] { -imgSize_w, imgSize_w, -imgSize_w + skewSize_w, -imgSize_w + skewSize_w + textSize_w };
            int[] y = new int[] { 0, imgSize_h , imgSize_h - skewSize_h , imgSize_h - skewSize_h - textSize_h };
            int up = y.Max() + exSize_h;
            int right = x.Max() + exSize_w;
            int bottom = - y.Min() + exSize_h;
            int left = - x.Min() + exSize_w;
            numericUpDown17.Value = up;
            numericUpDown18.Value = right;
            numericUpDown19.Value = bottom;
            numericUpDown20.Value = left;
        }

    }
}
