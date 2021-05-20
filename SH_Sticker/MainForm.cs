using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SH_Sticker
{
    public partial class MainForm : Form
    {
        public string PB_Path = "C:\\Program Files\\SH Sticker";
        public int Ex = 0;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public MainForm()
        {
            InitializeComponent();

            this.ShowInTaskbar = false;
            this.Visible = false;
            this.notifyIcon1.Visible = true;
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string procName = Process.GetCurrentProcess().ProcessName;

            Process[] procArray = Process.GetProcessesByName(procName);
            if (procArray.Length > 1)
            {
                MessageBox.Show("Error : 관리자 권한으로 실행되지 않았거나, 이미 실행중입니다.");
                Exit();
            }
            else
            {

            }

            if (File.Exists("C:\\Program Files\\SH Sticker\\data\\.ini"))
            {
                File.Delete("C:\\Program Files\\SH Sticker\\data\\.ini");

                if(File.Exists("C:\\Program Files\\SH Sticker\\data\\.txt"))
                {
                    File.Delete("C:\\Program Files\\SH Sticker\\data\\.txt");
                }

            }

            this.WindowState = FormWindowState.Minimized;
            this.ShowIcon = false; //작업표시줄에서 제거.
            notifyIcon1.Visible = true; //트레이 아이콘을 표시한다.
            
            if (!Directory.Exists(PB_Path))
            {
                Directory.CreateDirectory(PB_Path);
                Directory.CreateDirectory(PB_Path + "\\data");
                Directory.CreateDirectory(PB_Path + "\\recycle");
            }
            else
            {
                try
                {
                    DirectoryInfo DIR = new DirectoryInfo(PB_Path + "\\data\\");
                    FileInfo[] F = DIR.GetFiles("*.ini", SearchOption.AllDirectories);
                    FileInfo[] FT = DIR.GetFiles("*.txt", SearchOption.AllDirectories);
                    int co = 0;
                    foreach (FileInfo file in F)
                    {
                        string name = file.Name.ToString();
                        string[] Fname = name.Split('.'); //파일이름
                        TextMemo frm2 = new TextMemo();

                        frm2.FNA = Fname[0];
                        frm2.FNA2 = "1";

                        string TP1 = Decrypt("K", File.ReadAllText(PB_Path + "\\data\\" + FT[co]));//내용
                        frm2.FNA3 = TP1;

                        StringBuilder Ptext2 = new StringBuilder();
                        int ret2 = GetPrivateProfileString("Section2", "BgColor", "", Ptext2, 2147483647, PB_Path + "\\data\\" + file); //색깔코드
                        frm2.FNA4 = Ptext2.ToString();

                        StringBuilder Ptext3 = new StringBuilder();
                        int ret3 = GetPrivateProfileString("Section3", "title", "", Ptext3, 2147483647, PB_Path + "\\data\\" + file); //제목
                        string TP2 = Decrypt("K", Ptext3.ToString());
                        frm2.FNA5 = TP2;

                        StringBuilder Ptext4 = new StringBuilder();
                        int ret4 = GetPrivateProfileString("Section4", "Location", "", Ptext4, 2147483647, PB_Path + "\\data\\" + file); //위치정보
                        string TP3 = Decrypt("K", Ptext4.ToString());
                        frm2.FAN6 = TP3;

                        StringBuilder Ptext5 = new StringBuilder();
                        int ret5 = GetPrivateProfileString("Section5", "Size", "", Ptext5, 2147483647, PB_Path + "\\data\\" + file); //사이즈
                        frm2.FAN7 = Ptext5.ToString();

                        frm2.Show();
                        co++;
                    }
                    co = 0;
                }
                catch
                {
                    MessageBox.Show("Error");
                }
            }
        }
        //한 때 경로지정 기능이였음
        //folderBrowserDialog1.ShowDialog();
        //string Path = folderBrowserDialog1.SelectedPath;
        //PAT_Box1.Text = Path;
        private void NewM_Click(object sender, EventArgs e)
        {
            var myForm = new TextMemo();
            myForm.Show();
        }
        public static string Encrypt(string key, string data) //복호화
        {
            Encoding unicode = Encoding.Unicode;
            return Convert.ToBase64String(Encrypt(unicode.GetBytes(key), unicode.GetBytes(data)));
        }
        public static string Decrypt(string key, string data)
        {
            Encoding unicode = Encoding.Unicode;
            return unicode.GetString(Encrypt(unicode.GetBytes(key), Convert.FromBase64String(data)));
        }
        public static byte[] Encrypt(byte[] key, byte[] data)
        {
            return EncryptOutput(key, data).ToArray();
        }
        public static byte[] Decrypt(byte[] key, byte[] data)
        {
            return EncryptOutput(key, data).ToArray();
        }
        private static byte[] EncryptInitalize(byte[] key)
        {
            byte[] s = Enumerable.Range(0, 256)
              .Select(i => (byte)i)
              .ToArray();

            for (int i = 0, j = 0; i < 256; i++)
            {
                j = (j + key[i % key.Length] + s[i]) & 255;
                Swap(s, i, j);
            }
            return s;
        }
        private static IEnumerable<byte> EncryptOutput(byte[] key, IEnumerable<byte> data)
        {
            byte[] s = EncryptInitalize(key);
            int i = 0;
            int j = 0;

            return data.Select((b) =>
            {
                i = (i + 1) & 255;
                j = (j + s[i]) & 255;
                Swap(s, i, j);
                return (byte)(b ^ s[(s[i] + s[j]) & 255]);
            });
        }

        private static void Swap(byte[] s, int i, int j)
        {
            byte c = s[i];
            s[i] = s[j];
            s[j] = c;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            RegistryKey rkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rkey.SetValue("ProgramName", Application.ExecutablePath.ToString());
            MessageBox.Show("시작프로그램으로 등록되었습니다.");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            RegistryKey rkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rkey.DeleteValue("ProgramName", false);
            MessageBox.Show("시작프로그램 등록해제되었습니다.");
        }
        private void button3_Click(object sender, EventArgs e) //전체삭제버튼
        {
            if (MessageBox.Show("정말로 전체삭제 하시겠습니까?"+Environment.NewLine+"확인을 누르면 모든 메모가 삭제됩니다.", "확인 취소", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if(MessageBox.Show("정말로 삭제를 하시는게 맞나요?", "확인 취소", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        DirectoryInfo DR = new DirectoryInfo(PB_Path + "\\data");
                        FileInfo[] GetF = DR.GetFiles("*.*", SearchOption.AllDirectories);
                        foreach (FileInfo FI in GetF)
                        {
                            File.Delete(PB_Path + "\\data\\" + FI);
                        }
                    }
                    catch
                    {
                        Exit();
                    }
                    Exit();
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
        void Exit()
        {
            Ex = 1;
            Application.ExitThread();
            Application.Exit();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (File.Exists("C:\\Program Files\\SH Sticker\\data\\.ini"))
            {
                File.Delete("C:\\Program Files\\SH Sticker\\data\\.ini");

                if (File.Exists("C:\\Program Files\\SH Sticker\\data\\.txt"))
                {
                    File.Delete("C:\\Program Files\\SH Sticker\\data\\.txt");
                }

            }

            string RPath = null;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                RPath = folderBrowserDialog1.SelectedPath;
                DirectoryInfo R = new DirectoryInfo(PB_Path + "\\data\\");
                FileInfo[] FR = R.GetFiles("*.ini", SearchOption.AllDirectories);
                FileInfo[] FRT = R.GetFiles("*.txt", SearchOption.AllDirectories);

                int Dco = 0;
                foreach (FileInfo Rfile in FR)
                {
                    StringBuilder INIInfo = new StringBuilder();
                    int ret0 = GetPrivateProfileString("Section3", "title", "", INIInfo, 2147483647, PB_Path + "\\data\\" + Rfile); //제목
                    string DTitle = Decrypt("K", INIInfo.ToString()) + ".txt";
                    string DText = Decrypt("K", File.ReadAllText(PB_Path + "\\data\\" + FRT[Dco]));//내용
                    File.AppendAllText(RPath + "\\" + DTitle, DText);
                    Dco++;
                }
                Dco = 0;
            }
            else
            {
                RPath = null;
                return;
            }
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true; // 폼의 표시
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal; // 최소화를 멈춘다 
            this.Activate(); // 폼을 활성화 시킨다
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void 옵션ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true; // 폼의 표시
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal; // 최소화를 멈춘다 
            this.Activate(); // 폼을 활성화 시킨다
        }

        private void 새메모ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var myForm = new TextMemo();
            myForm.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            this.Hide();
            this.WindowState = FormWindowState.Minimized;
            this.ShowIcon = false; //작업표시줄에서 제거.
            notifyIcon1.Visible = true; //트레이 아이콘을 표시한다.
            if (Ex == 0) { notifyIcon1.ShowBalloonTip(1000); }
        }
    }
}
