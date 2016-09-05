using CheckFile.dataHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                StringBuilder builder = new StringBuilder();
                foreach (FileData iFileData in lstFileData)
                {
                    if (isInValidTexture(iFileData.Name))
                    {
                        builder.Append("无效的格式：").AppendLine(iFileData.FullName);
                    }
                    else if (isValidTexture(iFileData.Name))
                    {
                        if (!checkf.Contains(iFileData.NameOnly))
                        {
                            checkf.Add(iFileData.NameOnly);

                            foreach (FileData jFileData in lstFileData)
                            {
                                bool exist = false;
                                if (isValidTexture(jFileData.Name)
                                    && jFileData.FullName != iFileData.FullName 
                                    && jFileData.NameOnly == iFileData.NameOnly)
                                {
                                    builder.AppendLine(jFileData.FullName);
                                    exist = true;
                                }

                                if (exist)
                                {
                                    builder.AppendLine(iFileData.FullName);
                                    builder.AppendLine("重复！");
                                }
                            }
                        }
                    }
                }

                BatMessage pBatMessage = new BatMessage(builder.ToString());
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
