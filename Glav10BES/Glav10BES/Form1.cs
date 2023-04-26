using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Glav10BES
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Заполняем первый узел. Используем диск C:
            TreeNode rootNode = new TreeNode(@"C:\");
            treeDirectory.Nodes.Add(rootNode);
            // Заполнеяем первый уровень и раскрываем его
            FillNodes(rootNode);
            treeDirectory.Nodes[0].Expand();
            // Заполняем второй узел. Используем диск D:
            TreeNode rootNode2 = new TreeNode(@"D:\");
            treeDirectory.Nodes.Add(rootNode2);
            FillNodes(rootNode2);
            // Не будем раскрывать его
            //treeDirectory.Nodes[1].Expand();
        }
        private void FillNodes(TreeNode dirNode)
        {
            DirectoryInfo dir = new DirectoryInfo(dirNode.FullPath);
            foreach (DirectoryInfo dirItem in dir.GetDirectories())
            {
                // Добавляем узел для каждой папки
                TreeNode newNode = new TreeNode(dirItem.Name);
                dirNode.Nodes.Add(newNode);
                newNode.Nodes.Add("*");
            }
        }
        private void treeDirectory_BeforeExpand(object sender,
         TreeViewCancelEventArgs e)
        {
            // Если найден узел со звездочкой *, то удаляем его
            // и получаем список подпапок.
            if (e.Node.Nodes[0].Text == "*")
            {
                e.Node.Nodes.Clear();
                FillNodes(e.Node);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Получим список папок, где встречается буквосочетание pro
            string[] astrLogicalDrives = System.Environment.GetLogicalDrives();
            foreach (string disk in astrLogicalDrives)
                listBox1.Items.Add(disk);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] astrLogicalDrives = System.IO.Directory.GetLogicalDrives();
            foreach (string disk in astrLogicalDrives)
                listBox1.Items.Add(disk);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.IO.DriveInfo drv = new System.IO.DriveInfo(@"d:\");
            listBox1.Items.Clear();
            listBox1.Items.Add("Диск: " + drv.Name);

            if (drv.IsReady)
            {
                listBox1.Items.Add("Тип диска: " + drv.DriveType.ToString());
                listBox1.Items.Add("Файловая система: " +
                drv.DriveFormat.ToString());
                listBox1.Items.Add("Свободное место на диске: " +
                drv.AvailableFreeSpace.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string[] astrFolders = System.IO.Directory.GetDirectories(@"d:\");
            foreach (string folder in astrFolders)
                listBox1.Items.Add(folder);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@"d:\");
            System.IO.DirectoryInfo[] folders = di.GetDirectories("*pro*");
            foreach (System.IO.DirectoryInfo maskdirs in folders)
                listBox1.Items.Add(maskdirs);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists(@"C:\windows"))
                label1.Text = "Папка " + @"C:\Windows" + " существует";
            else
                label1.Text = "Папка не существует";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Переименовываем папку MyFolder в папку NewFolder
            string oldPathString = @"C:\MyFolder";
            string newPathString = @"C:\NewFolder";
            Directory.Move(oldPathString, newPathString);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                // Попытаемся удалить папку C:\WUTEMP
                System.IO.Directory.Delete(@"c:\wutemp");
                MessageBox.Show("Папка удалена.");
            }
            catch (Exception)
            {
                MessageBox.Show("Нельзя удалить непустую папку.");
            }
            finally { }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            // задаем папку верхнего уровня
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            // Заголовок в диалоговом окне
            fbd.Description = "Выберите папку";
            // Не выводим кнопку Новая папка
            fbd.ShowNewFolderButton = false;
            // Получаем папку, выбранную пользователем
            if (fbd.ShowDialog() == DialogResult.OK)
                this.Text = fbd.SelectedPath;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\wutemp");
            listBox1.Items.Clear();
            listBox1.Items.Add("Проверка папки: " + dir.Name);
            listBox1.Items.Add("Родительская папка: " + dir.Parent.Name);
            listBox1.Items.Add("Папка существует: ");
            listBox1.Items.Add(dir.Exists.ToString());
            if (dir.Exists)
            {
                listBox1.Items.Add("Папка создана: ");
                listBox1.Items.Add(dir.CreationTime.ToString());
                listBox1.Items.Add("Папка изменена: ");
                listBox1.Items.Add(dir.LastWriteTime.ToString());
                listBox1.Items.Add("Время последнего доступа: ");
                listBox1.Items.Add(dir.LastAccessTime.ToString());
                listBox1.Items.Add("Атрибуты папки: ");
                listBox1.Items.Add(dir.Attributes.ToString());
                listBox1.Items.Add("Папка содержит: " +
                dir.GetFiles().Length.ToString() + " файла");
            }
        }
        static long GetDirectorySize(System.IO.DirectoryInfo directory,
 bool includeSubdir)
        {
            long totalSize = 0;
            // Считаем все файлы
            System.IO.FileInfo[] files = directory.GetFiles();
            foreach (System.IO.FileInfo file in files)
            {
                totalSize += file.Length;
            }
 // Считаем все подпапки
 if (includeSubdir)
            {
                System.IO.DirectoryInfo[] dirs = directory.GetDirectories();
                foreach (System.IO.DirectoryInfo dir in dirs)
                {
                    totalSize += GetDirectorySize(dir, true);
                }
            }
            return totalSize;
        }
        private void butFolderSize_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            System.IO.DirectoryInfo dir = new
            System.IO.DirectoryInfo(@"d:\help");
            textBox1.Text = "Общий размер: " +
            GetDirectorySize(dir, true).ToString() + " байт.";
            this.Cursor = Cursors.Default;
        }
    }
}
