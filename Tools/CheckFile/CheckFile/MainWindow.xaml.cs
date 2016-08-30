using CheckFile.dataHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace CheckFile
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Path_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbRe.Text = dialog.SelectedPath;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(tbRe.Text))
            {
                MessageBox.Show("待检查路径不存在");
                return;
            }

            try
            {
                List<FileData> lstFileData = new List<FileData>();
                lstFileData = DirectoryHelper.GetAllFiles(tbRe.Text);
                List<string> checkf = new List<string>();
                string t = string.Empty;
                foreach (FileData iFileData in lstFileData)
                {
                    if (isInValidTexture(iFileData.Name))
                    {
                        t += "无效的格式：" + iFileData.FullName + Environment.NewLine;
                    }
                    else if (isValidTexture(iFileData.Name))
                    {
                        if (lstFileData.Where(x => x.NameOnly == iFileData.NameOnly).Count() > 1)
                        {
                            if (!checkf.Contains(iFileData.NameOnly))
                            {
                                checkf.Add(iFileData.NameOnly);

                                foreach (FileData jFileData in lstFileData)
                                {
                                    // 只检查图片
                                    if (isValidTexture(jFileData.Name))
                                    {
                                        if (lstFileData.Where(x => x.NameOnly == jFileData.NameOnly).Count() > 1)
                                        {
                                            t += jFileData.FullName + Environment.NewLine;
                                        }
                                    }
                                }
                                t += "重复！" + Environment.NewLine;
                            }
                        }
                    }
                }
               
                BatMessage pBatMessage = new BatMessage(t);
                pBatMessage.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        bool isInValidTexture(string name)
        {
            return name.ToUpper().Contains(".PSD")
                || name.ToUpper().Contains(".TGA");
        }

        bool isValidTexture(string name)
        {
            return name.ToUpper().Contains(".PNG")
                || name.ToUpper().Contains(".JPG");
        }
    }
}
