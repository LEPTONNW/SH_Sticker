using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SH_Sticker
{
    public partial class TextMemo : Form
    {
        public TextMemo()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.ShowInTaskbar = false;

        }
        [DllImport("kernel32")] private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")] private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private string TextMemo_value;
        public string MoFname, MoText, MoColor, Motitle, MoLocation, MoSize;

        public string FilePath = "C:\\Program Files\\SH Sticker\\data", filename, ColCode = "";
        public int Model, temp, x, Count = 0, StCount = 0;
        private const int cGrip = 16;      // 그립사이즈
        private const int cCaption = 32;   // 캡션 바 높이
        public string FNA, FNA2 = "0", FNA3, FNA4, FNA5, FAN6, FAN7; //전달값부분

        protected override void WndProc(ref Message m) //FormboaderStyle 이 None 일 경우 마우스제어, 크기조절 함수
        {
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16); //크기조절
                pos = this.PointToClient(pos); //Y축
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip) //X축
                {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
                switch (m.Msg) //마우스 제어
                {
                    case 0x84:
                        base.WndProc(ref m);
                        if ((int)m.Result == 0x1)
                            m.Result = (IntPtr)0x2;
                        return;
                }
            }
            base.WndProc(ref m); //반환
        }
        protected override void OnPaint(PaintEventArgs e) //크기조절 컨트롤 모양
        {
            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            rc = new Rectangle(0, 0, this.ClientSize.Width, cCaption);
            //e.Graphics.FillRectangle(Brushes.DarkBlue, rc);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) //단축키 제어
        {
            if (!base.ProcessCmdKey(ref msg, keyData))
            {
                if (keyData.Equals(Keys.Control | Keys.A)) {richTextBox1.SelectAll(); return true;}
                else if (keyData.Equals(Keys.Control | Keys.F4)) { return true; }
                else { return false; }
            }
            else { return true; }
        }
        public void Start() //메모Form시작시 실행되는 코드
        {
            this.MinimumSize = new Size(350, 30);
            Model = 0; //새 폼과 기존데이터를 불러온 폼을 구별하기위한 변수

            MoFname = FNA; //파일이름받는구간
            Model = Convert.ToInt32(FNA2); //Model값 받는 구간
            MoText = FNA3; if (Model == 1) { richTextBox1.Text = MoText; } //텍스트 받는 구간

            MoColor = FNA4;//컬러코드 받는 구간
            try
            {
                if (Model == 1) //폼의 색을 결정하기위해 MainForm에서 받은 RGB데이터를 가공하는 함수
                {
                    var p = MoColor.Split(new char[] { ',', ']' }); 
                    int A = Convert.ToInt32(p[0].Substring(p[0].IndexOf('=') + 1));
                    int R = Convert.ToInt32(p[1].Substring(p[1].IndexOf('=') + 1));
                    int G = Convert.ToInt32(p[2].Substring(p[2].IndexOf('=') + 1));
                    int B = Convert.ToInt32(p[3].Substring(p[3].IndexOf('=') + 1));
                    this.BackColor = Color.FromArgb(A, R, G, B);
                    ColCode = this.BackColor.ToString();
                }
                else { ColCode = this.BackColor.ToString(); }
            }
            catch
            {
                try
                {
                    var p = MoColor.Split(new char[] { ',', ']' }); //위와 동일하기 Color코드에 대한 데이터를 받지만 지정된 "아쿠아색"같은 색을 받는 예외처리이다.
                    string c = Convert.ToString(p[0].Substring(p[0].IndexOf('[') + 1));
                    this.BackColor = Color.FromName(c);
                }
                catch
                {
                }
            }
            
            Motitle = FNA5; //제목값받는구간
            if (Model == 1) { label2.Text = Motitle; textBox1.Text = Motitle; }
            MoLocation = FAN6; //폼위치정보받는구간
            if (Model == 1) //MainForm 에서 받은 폼의 X,Y축 위치데이터를 가공하는 함수이다.
            {
                var q = MoLocation.Split(new char[] { ',', '}' });
                int X = Convert.ToInt32(q[0].Substring(q[0].IndexOf('=') + 1));
                int Y = Convert.ToInt32(q[1].Substring(q[1].IndexOf('=') + 1));
                this.Location = new Point(X, Y);
            }
            MoSize = FAN7; //폼사이즈받는구간
            if (MoSize == "1") { this.Size = MinimumSize; }
            else { this.Size = new Size(400, 443);}
            Save.Hide();
            StCount = 1;

            filename = "\\" + MoFname + ".ini";
            string[] ttpath = filename.Split('.');
            string textname = ttpath[0] + ".txt";
        }
        /// <summary>
        /// 파일 저장구간
        /// </summary>
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (StCount == 0) { Start();}

            string STR = Encrypt("K", richTextBox1.Text);
            string TL = Encrypt("K", textBox1.Text);
            string LO = Encrypt("K", this.Location.ToString());

            if (Model == 0)
            {
                filename = "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".ini";
                string[] ttpath = filename.Split('.');
                string textname = ttpath[0] + ".txt";
                File.AppendAllText(FilePath + textname, STR);
                WritePrivateProfileString("Section2", "BgColor", ColCode, FilePath + filename);
                WritePrivateProfileString("Section3", "title", TL, FilePath + filename);
                WritePrivateProfileString("Section4", "Location", LO, FilePath + filename);
                Model = 2;
            }
            else if (Model == 1)
            {
                filename = "\\" + MoFname + ".ini";
                string[] ttpath = filename.Split('.');
                string textname = ttpath[0] + ".txt";
                File.Delete(FilePath + textname);
                File.AppendAllText(FilePath + textname, STR);
                WritePrivateProfileString("Section2", "BgColor", ColCode, FilePath + filename);
                WritePrivateProfileString("Section3", "title", TL, FilePath + filename);
                WritePrivateProfileString("Section4", "Location", LO, FilePath + filename);
                Model = 2;
            }
            else if (Model == 2)
            {
                string[] ttpath = filename.Split('.');
                string textname = ttpath[0] + ".txt";
                File.Delete(FilePath + textname);
                File.AppendAllText(FilePath + textname, STR);
                WritePrivateProfileString("Section2", "BgColor", ColCode, FilePath + filename);
                WritePrivateProfileString("Section3", "title", TL, FilePath + filename);
                WritePrivateProfileString("Section4", "Location", LO, FilePath + filename);
            }
            else { MessageBox.Show("Error");}
            Thread.Sleep(5);
        }
        private void TextMemo_Move(object sender, EventArgs e)
        {
            string LO = Encrypt("K", this.Location.ToString());
            WritePrivateProfileString("Section4", "Location", LO, FilePath + filename);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            string STR = Encrypt("K", richTextBox1.Text);
            string TL = Encrypt("K", textBox1.Text);
            string LO = Encrypt("K", this.Location.ToString());

            if (Model == 0)
            {
                filename = "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".ini";
                string[] ttpath = filename.Split('.');
                string textname = ttpath[0] + ".txt";
                File.AppendAllText(FilePath + textname, STR);
                WritePrivateProfileString("Section2", "BgColor", ColCode, FilePath + filename);
                WritePrivateProfileString("Section3", "title", TL, FilePath + filename);
                WritePrivateProfileString("Section4", "Location", LO, FilePath + filename);
                Model = 2;
            }
            else if (Model == 1)
            {
                filename = "\\" + MoFname + ".ini";
                string[] ttpath = filename.Split('.');
                string textname = ttpath[0] + ".txt";
                File.Delete(FilePath + textname);
                File.AppendAllText(FilePath + textname, STR);
                WritePrivateProfileString("Section2", "BgColor", ColCode, FilePath + filename);
                WritePrivateProfileString("Section3", "title", TL, FilePath + filename);
                WritePrivateProfileString("Section4", "Location", LO, FilePath + filename);
                Model = 2;
            }
            else if (Model == 2)
            {
                string[] ttpath = filename.Split('.');
                string textname = ttpath[0] + ".txt";
                File.Delete(FilePath + textname);
                File.AppendAllText(FilePath + textname, STR);
                WritePrivateProfileString("Section2", "BgColor", ColCode, FilePath + filename);
                WritePrivateProfileString("Section3", "title", TL, FilePath + filename);
                WritePrivateProfileString("Section4", "Location", LO, FilePath + filename);
            }
            else { MessageBox.Show("Error"); }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (StCount == 0) { Start(); }

            label2.Text = textBox1.Text;
            string STR = Encrypt("K", richTextBox1.Text);
            string TL = Encrypt("K", textBox1.Text);
            string LO = Encrypt("K", this.Location.ToString());

            if (Model == 0)
            {
                filename = "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".ini";
                string[] ttpath = filename.Split('.');
                string textname = ttpath[0] + ".txt";
                File.AppendAllText(FilePath + textname, STR);
                WritePrivateProfileString("Section2", "BgColor", ColCode, FilePath + filename);
                WritePrivateProfileString("Section3", "title", TL, FilePath + filename);
                WritePrivateProfileString("Section4", "Location", LO, FilePath + filename);
                Model = 2;
            }
            else if (Model == 1)
            {
                filename = "\\" + MoFname + ".ini";
                string[] ttpath = filename.Split('.');
                string textname = ttpath[0] + ".txt";
                File.Delete(FilePath + textname);
                File.AppendAllText(FilePath + textname, STR);
                WritePrivateProfileString("Section2", "BgColor", ColCode, FilePath + filename);
                WritePrivateProfileString("Section3", "title", TL, FilePath + filename);
                WritePrivateProfileString("Section4", "Location", LO, FilePath + filename);
                Model = 2;
            }
            else if (Model == 2)
            {
                string[] ttpath = filename.Split('.');
                string textname = ttpath[0] + ".txt";
                File.Delete(FilePath + textname);
                File.AppendAllText(FilePath + textname, STR);
                WritePrivateProfileString("Section2", "BgColor", ColCode, FilePath + filename);
                WritePrivateProfileString("Section3", "title", TL, FilePath + filename);
                WritePrivateProfileString("Section4", "Location", LO, FilePath + filename);
            }
            else { MessageBox.Show("Error"); }
            Thread.Sleep(5);
        }

        /// <summary>
        /// 버튼별 이벤트 구간
        /// </summary>
        private void TextMemo_Load(object sender, EventArgs e) { Start();}
        private void SCColor_Click(object sender, EventArgs e)
        {
            this.BackColor = MirSelColor();
            ColCode = this.BackColor.ToString();
            WritePrivateProfileString("Section2", "BgColor", ColCode, FilePath + filename);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Size == MinimumSize)
            {
                this.Size = new Size(400, 443);
                Count = 0;
                WritePrivateProfileString("Section5", "Size", Count.ToString(), FilePath + filename);
            }
            else
            {
                this.Size = MinimumSize;
                Count = 1;
                WritePrivateProfileString("Section5", "Size", Count.ToString(), FilePath + filename);
            }
        }
        private void ExButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("삭제하시겠습니까?", "확인 취소", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //DirectoryInfo BDIR = new DirectoryInfo(FilePath + "\\");
                //FileInfo[] F = BDIR.GetFiles("*.ini", SearchOption.AllDirectories);
                try { File.Delete("C:\\Program Files\\SH Sticker\\data\\.ini"); }
                catch { }
                

                try
                {
                    string[] ttpath = filename.Split('.');
                    string textname = ttpath[0] + ".txt";
                    File.Delete(FilePath + filename);
                    File.Delete(FilePath + textname);
                }
                catch { Exit(); }
            }
            else { return; }
            Exit();
        }

        /// <summary>
        /// 컨트롤 이벤트 구간
        /// </summary>
        private void textBox1_Leave(object sender, EventArgs e) { label2.Text = textBox1.Text; }
        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_DoubleClick(object sender, EventArgs e)
        {
            if (this.Size == MinimumSize)
            {
                this.Size = new Size(400, 443);
                Count = 0;
                WritePrivateProfileString("Section5", "Size", Count.ToString(), FilePath + filename);
            }
            else
            {
                this.Size = MinimumSize;
                Count = 1;
                WritePrivateProfileString("Section5", "Size", Count.ToString(), FilePath + filename);
            }
        }

        /// <summary>
        /// 암호화 구간
        /// </summary>
        public static string Encrypt(string key, string data) //RC4 암호화
        {
            Encoding unicode = Encoding.Unicode;
            return Convert.ToBase64String(Encrypt(unicode.GetBytes(key), unicode.GetBytes(data)));
        }
        public static string Decrypt(string key, string data)
        {
            Encoding unicode = Encoding.Unicode;
            return unicode.GetString(Encrypt(unicode.GetBytes(key), Convert.FromBase64String(data)));
        }
        public static byte[] Encrypt(byte[] key, byte[] data) { return EncryptOutput(key, data).ToArray();}
        public static byte[] Decrypt(byte[] key, byte[] data) { return EncryptOutput(key, data).ToArray();}
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

        /// <summary>
        /// 일반 함수 구간
        /// </summary>
        /// <returns>색상을 선택하지 않았을경우 Black리턴</returns>
        public Color MirSelColor() ///색상 선택창을 로드후 선택한 Color를 리턴한다.
        {

            if (colorDialog1.ShowDialog() == DialogResult.OK) { return colorDialog1.Color;}
            else { return this.BackColor; }
        }

        public void Exit() { this.Close(); }
    }
}
