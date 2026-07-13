using static System.Runtime.InteropServices.JavaScript.JSType;

namespace 마크런처
{
    //MIT License

    //Copyright(c) 2020 권세인(AlphaBs)

    //Permission is hereby granted, free of charge, to any person obtaining a copy
    //of this software and associated documentation files (the "Software"), to deal
    //in the Software without restriction, including without limitation the rights
    //to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    //copies of the Software, and to permit persons to whom the Software is
    //furnished to do so, subject to the following conditions:

    //The above copyright notice and this permission notice shall be included in all
    //copies or substantial portions of the Software.

    //THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    //IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    //FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    //AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    //LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    //OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    //SOFTWARE.


    //MIT License

    //Copyright(c) 2023 CmlLib

    //Permission is hereby granted, free of charge, to any person obtaining a copy
    //of this software and associated documentation files (the "Software"), to deal
    //in the Software without restriction, including without limitation the rights
    //to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    //copies of the Software, and to permit persons to whom the Software is
    //furnished to do so, subject to the following conditions:

    //The above copyright notice and this permission notice shall be included in all
    //copies or substantial portions of the Software.

    //THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    //IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    //FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    //AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    //LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    //OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    //SOFTWARE.
    using System;
    using System.Diagnostics;
    using System.Drawing.Interop;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using CmlLib.Core;
    using CmlLib.Core.Auth;
    using CmlLib.Core.Auth.Microsoft;
    using CmlLib.Core.ProcessBuilder;
    using CmlLib.Core.Utils;
    using CmlLib.Core.Version;
    using static System.Collections.Specialized.BitVector32;

    using System.ComponentModel;

    using CmlLib.Core.VersionMetadata;
    using CmlLib.Core.Installers;
    using MojangAPI;
    using MojangAPI.Model;
    using CmlLib.Core.Rules;
    using System.Reflection.Emit;
    using System.Xml.Linq;
    using static System.Windows.Forms.DataFormats;
    using static System.Net.WebRequestMethods;
    using static System.Runtime.InteropServices.JavaScript.JSType;
    using System.Security.Principal;
    using System.Net;
    using System.Text.Json;
    using System.Text;
    using File = File;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
    using System.Drawing;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement;

    //CmlLib의 winfrom 런처 코드 참고함 

    public partial class Form1 : Form
    {
        string defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft"); //마크 기본 경로
        string installPath; // 사용자 선택 경로 저장
        private static string ConfigFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "mindyApp", "mindyconfig.txt"); //설정 저장 경로
        private MSession _session = null; //세션 저장 변수
        private System.Windows.Forms.Timer eventTimer; //이벤트 타이머

        private Form3 form3Instance;
        private Form2 from2;
        public Form1()
        {

            InitializeComponent();
            this.MaximizeBox = false; //전체화면 꺼짐

            installPath = defaultPath; // 기본값으로 초기화
            //기본값 세팅
            textBox2.Text = "4096";
            textBox3.Text = "1600";
            textBox4.Text = "900";
            comboBox2.Text = "옵션 1";

            // 타이머 초기화
            eventTimer = new System.Windows.Forms.Timer();
            eventTimer.Interval = 1000; // 1초 간격으로 설정
            eventTimer.Tick += new EventHandler(eventTimer_Tick); // Tick 이벤트에 eventTimer_Tick 메서드 연결
            eventTimer.Start(); // 타이머 시작
            comboBox2.SelectedIndexChanged += ComboBox_SelectedIndexChanged;

        }

        private async void Form1_Load(object sender, EventArgs e)
        {

            var launcher = new MinecraftLauncher(); //런처 초기화
            textBox5.Text = launcher.GetDefaultJavaPath();
            textBox1.Text = installPath;
            try
            {
                //초기 로그인
                var loginHandler = JELoginHandlerBuilder.BuildDefault();
                var session = await loginHandler.Authenticate();
                _session = session; //변수에 세션 저장하고 텍스트 박스에 계정정보 보여주기
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string DataFolder = Path.Combine(appDataPath, "마크런처", "skin.png");
                string DataFolder1 = Path.Combine(appDataPath, "마크런처", "cape.png");

                getskin1(DataFolder, DataFolder1);


                textBox6.Text = _session.Username;
                textBox7.Text = _session.UUID;
                textBox8.Text = _session.Xuid;



            }
            catch (Exception)
            {
                try
                {

                    MessageBox.Show(this, $"오류 발생:다시 로그인 해 주세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    var loginHandler = JELoginHandlerBuilder.BuildDefault();
                    var session = await loginHandler.AuthenticateInteractively();
                    _session = session;
                    string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string DataFolder = Path.Combine(appDataPath, "마크런처", "skin.png");
                    string DataFolder1 = Path.Combine(appDataPath, "마크런처", "cape.png");

                    getskin1(DataFolder, DataFolder1);


                    textBox6.Text = _session.Username;
                    textBox7.Text = _session.UUID;
                    textBox8.Text = _session.Xuid;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"오류 발생:{ex}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            try
            {
                // 마인크래프트 런처 초기화 및 버전 목록 불러오기

                var versions = await launcher.GetAllVersionsAsync();
                comboBox1.Items.AddRange(versions.Select(v => v.Name).ToArray());
                comboBox1.Text = versions.LatestReleaseName;

            }
            catch (Exception)
            {
                MessageBox.Show(this, "인터넷이 연결되지 않았습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            comboBox2.Items.Add("옵션 1");
            comboBox2.Items.Add("옵션 2");
            comboBox2.Items.Add("옵션 3");
            comboBox2.Items.Add("옵션 4");
            comboBox2.Items.Add("옵션 5");


            LoadConfig();

        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedOption = comboBox2.SelectedItem.ToString();

            // 선택된 옵션에 따라 작업 실행
            if (selectedOption == "옵션 1")
            {
                ConfigFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "mindyApp", "mindyconfig.txt"); //설정 저장 경로

                LoadConfig();
            }
            else if (selectedOption == "옵션 2")
            {
                ConfigFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "mindyApp", "mindyconfig1.txt"); //설정 저장 경로

                LoadConfig();
            }
            else if (selectedOption == "옵션 3")
            {
                ConfigFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "mindyApp", "mindyconfig2.txt"); //설정 저장 경로

                LoadConfig();
            }
            else if (selectedOption == "옵션 4")
            {
                ConfigFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "mindyApp", "mindyconfig3.txt"); //설정 저장 경로

                LoadConfig();
            }
            else if (selectedOption == "옵션 5")
            {
                ConfigFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "mindyApp", "mindyconfig4.txt"); //설정 저장 경로

                LoadConfig();
            }
            else
            {
                MessageBox.Show(this, "유효한 옵션을 선택하세요.");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(textBox6.Text) &&
    string.IsNullOrEmpty(textBox7.Text) &&
    string.IsNullOrEmpty(textBox8.Text)
    
   )
{
   
}
else
{
      //로그인
                var loginHandler = JELoginHandlerBuilder.BuildDefault();
                var session = await loginHandler.Authenticate();
                _session = session; //변수에 세션 저장하고 텍스트 박스에 계정정보 보여주기
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string DataFolder = Path.Combine(appDataPath, "마크런처", "skin.png");
                string DataFolder1 = Path.Combine(appDataPath, "마크런처", "cape.png");

                getskin1(DataFolder, DataFolder1);


                textBox6.Text = _session.Username;
                textBox7.Text = _session.UUID;
                textBox8.Text = _session.Xuid;
               
                        checkBox1.Enabled = true;
}
              


            }
            catch (Exception)
            {
                try
                {

                    MessageBox.Show(this, $"오류 발생:다시 로그인 해 주세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    var loginHandler = JELoginHandlerBuilder.BuildDefault();
                    var session = await loginHandler.AuthenticateInteractively();
                    _session = session;
                    string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string DataFolder = Path.Combine(appDataPath, "마크런처", "skin.png");
                    string DataFolder1 = Path.Combine(appDataPath, "마크런처", "cape.png");

                    getskin1(DataFolder, DataFolder1);


                    textBox6.Text = _session.Username;
                    textBox7.Text = _session.UUID;
                    textBox8.Text = _session.Xuid;
                }
                catch (Exception ex)
                {
                    
                    textBox6.Text = null;
                    textBox7.Text = null;
                    textBox8.Text = null;
                    _session.Username = " ";
                    _session.UUID = " ";
                    _session.Xuid = " ";
                    pictureBox1.Image = null;

                     if (ex.Message.Contains("NOT_FOUND"))
    {
        checkBox1.Checked = true;
                        checkBox1.Enabled = false;
       
    }
    else
    {
        // 다른 오류는 메시지만 출력
        MessageBox.Show(this, $"오류 발생:{ex}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

                }
                LoadConfig();

            }
            int wite = 0;
            int high = 0;
            int number = 0;
            //메모리 입력 확인
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show(this, $"오류 발생: 숫자를 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    number = int.Parse(textBox2.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            //가로 입력확인
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show(this, $"오류 발생: 숫자를 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    wite = int.Parse(textBox3.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            //세로 입력확인
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show(this, $"오류 발생: 숫자를 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    high = int.Parse(textBox4.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            var fileProgress = new SyncProgress<InstallerProgressChangedEventArgs>(Launcher_FileChanged); //파일 체인지 변수
            var byteProgress = new SyncProgress<ByteProgress>(Launcher_ProgressChanged); //바이트 프로세스
            if (form3Instance == null)
            {

            }
            else
            {
                form3Instance.Close();
            }
            if (from2 == null)
            {

            }
            else
            {
                from2.Close();
            }


            if (_session == null)
            {
              if (checkBox1.Enabled == false) // 비교

                {
                    _session.Username = "Demo";
                     //ui비활성화
                uioff();
                label9.Text = "잠시만 기다리세요...";

                try
                {
                    // 선택된 설치 경로로 MinecraftPath 설정
                    var minecraftPath = new MinecraftPath(installPath);

                    var launcher = new MinecraftLauncher(minecraftPath);
                    MLaunchOption launchOption;
                    if (checkBox4.Checked)
                    {
                        string updatedPath = textBox5.Text.Replace("javaw.exe", "java.exe");
                        launchOption = new MLaunchOption
                        {
                            Session = _session,
                            MaximumRamMb = number,
                            MinimumRamMb = 512,
                            ScreenWidth = wite,
                            ScreenHeight = high,
                            GameLauncherName = "mindy",
                            GameLauncherVersion = "1.0",
                            IsDemo = checkBox1.Checked,
                            FullScreen = checkBox2.Checked,
                            JavaPath = updatedPath,
                            


                        };
                    }
                    else
                    {
                        launchOption = new MLaunchOption
                        {
                            Session = _session,
                            MaximumRamMb = number,
                            MinimumRamMb = 512,
                            ScreenWidth = wite,
                            ScreenHeight = high,
                            GameLauncherName = "mindy",
                            GameLauncherVersion = "1.0",
                            IsDemo = checkBox1.Checked,
                            FullScreen = checkBox2.Checked,
                            JavaPath = textBox5.Text,


                        };
                    }



                    // 선택된 버전 설치 및 실행 프로세스 빌드
                    var process = await launcher.InstallAndBuildProcessAsync(comboBox1.Text, launchOption, fileProgress, byteProgress);




                    // 마인크래프트 실행
                    process.Start();

                    this.Hide();


                    SaveConfig(); //설정 저장
                    process.WaitForExit(); //꺼질때가지 기다리기
                    Process.Start(Application.ExecutablePath); //다시시작
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // UI 다시 활성화
                    uion();

                }
                }
                else
                {
                    MessageBox.Show(this, $"오류 발생: 계정에 로그인하지 않음", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

            }
            else
            {
                //ui비활성화
                uioff();
                label9.Text = "잠시만 기다리세요...";

                try
                {
                    // 선택된 설치 경로로 MinecraftPath 설정
                    var minecraftPath = new MinecraftPath(installPath);

                    var launcher = new MinecraftLauncher(minecraftPath);
                    MLaunchOption launchOption;
                    if (checkBox4.Checked)
                    {
                        string updatedPath = textBox5.Text.Replace("javaw.exe", "java.exe");
                        launchOption = new MLaunchOption
                        {
                            Session = _session,
                            MaximumRamMb = number,
                            MinimumRamMb = 512,
                            ScreenWidth = wite,
                            ScreenHeight = high,
                            GameLauncherName = "mindy",
                            GameLauncherVersion = "1.0",
                            IsDemo = checkBox1.Checked,
                            FullScreen = checkBox2.Checked,
                            JavaPath = updatedPath,


                        };
                    }
                    else
                    {
                        launchOption = new MLaunchOption
                        {
                            Session = _session,
                            MaximumRamMb = number,
                            MinimumRamMb = 512,
                            ScreenWidth = wite,
                            ScreenHeight = high,
                            GameLauncherName = "mindy",
                            GameLauncherVersion = "1.0",
                            IsDemo = checkBox1.Checked,
                            FullScreen = checkBox2.Checked,
                            JavaPath = textBox5.Text,


                        };
                    }



                    // 선택된 버전 설치 및 실행 프로세스 빌드
                    var process = await launcher.InstallAndBuildProcessAsync(comboBox1.Text, launchOption, fileProgress, byteProgress);




                    // 마인크래프트 실행
                    process.Start();

                    this.Hide();


                    SaveConfig(); //설정 저장
                    process.WaitForExit(); //꺼질때가지 기다리기
                    Process.Start(Application.ExecutablePath); //다시시작
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // UI 다시 활성화
                    uion();

                }

            }

        }

      


        //프로세스 변수
        ByteProgress byteProgress;
        InstallerProgressChangedEventArgs? fileProgress;

        private void Launcher_ProgressChanged(ByteProgress e) // 바이트프로세스 변경
        {
            byteProgress = e;
        }

        public void uioff()
        {
            comboBox1.Enabled = false;
            textBox5.Enabled = false;
            checkBox3.Enabled = false;
            button9.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            button7.Enabled = false;
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            button8.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            button12.Enabled = false;
            textBox9.Enabled = false;
            button13.Enabled = false;
            checkBox4.Enabled = false;
            button14.Enabled = false;
            button15.Enabled = false;
            comboBox2.Enabled = false;

        }
        public void uion()
        {
            comboBox1.Enabled = true;
            button3.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;
            textBox8.Enabled = true;
            button7.Enabled = true;
            checkBox1.Enabled = true;
            checkBox2.Enabled = true;
            textBox5.Enabled = true;
            checkBox3.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            button12.Enabled = true;
            textBox9.Enabled = true;
            button13.Enabled = true;
            checkBox4.Enabled = true;
            button14.Enabled = true;
            button15.Enabled = true;
            comboBox2.Enabled = true;
        }
        private void Launcher_FileChanged(InstallerProgressChangedEventArgs e) //파일 체인지 변경
        {
            if (e.EventType == InstallerEventType.Done)
                fileProgress = e;
        }

        private void eventTimer_Tick(object sender, EventArgs e) //특정시간마다 프로세스바 수정
        {
            var bytePercentage = (int)(byteProgress.ProgressedBytes / (double)byteProgress.TotalBytes * 100);
            if (bytePercentage >= 0 && bytePercentage <= 100)
            {
                progressBar1.Value = bytePercentage;
                progressBar1.Maximum = 100;
            }

            if (fileProgress != null)
                label9.Text = $"[{fileProgress.ProgressedTasks}/{fileProgress.TotalTasks}] {fileProgress.Name}";
        }

        private void button2_Click(object sender, EventArgs e) //설치경로 설정
        {
            // 폴더 선택 창 열기
            installPath = SelectMinecraftFolder(defaultPath);
            textBox1.Text = installPath;
            MessageBox.Show(this, $"설치 경로가 설정되었습니다: {installPath}", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        static string SelectMinecraftFolder(string defaultPath) //설치경로 찾기 버튼
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "마인크래프트를 설치할 폴더를 선택하세요. 선택하지 않으면 기본값을 사용합니다.";
                folderDialog.UseDescriptionForTitle = true;

                if (folderDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(folderDialog.SelectedPath))
                {
                    return folderDialog.SelectedPath; // 사용자가 선택한 경로 반환
                }

                return defaultPath; // 선택하지 않으면 기본 경로 반환
            }
        }

        private async void button3_Click(object sender, EventArgs e) //로그인 버튼
        {
            try
            {
                var loginHandler = JELoginHandlerBuilder.BuildDefault();
                var session = await loginHandler.Authenticate();
                _session = session;
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string DataFolder = Path.Combine(appDataPath, "마크런처", "skin.png");
                string DataFolder1 = Path.Combine(appDataPath, "마크런처", "cape.png");

                getskin1(DataFolder, DataFolder1);

                textBox6.Text = _session.Username;
                textBox7.Text = _session.UUID;
                textBox8.Text = _session.Xuid;
                 checkBox1.Checked = false;
                        checkBox1.Enabled = true;


            }

            catch (Exception)
            {
                try
                {
                    MessageBox.Show(this, $"오류 발생:다시 로그인 해 주세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var loginHandler = JELoginHandlerBuilder.BuildDefault();
                var session = await loginHandler.AuthenticateInteractively();
                _session = session;
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string DataFolder = Path.Combine(appDataPath, "마크런처", "skin.png");
                string DataFolder1 = Path.Combine(appDataPath, "마크런처", "cape.png");

                getskin1(DataFolder, DataFolder1);


                textBox6.Text = _session.Username;
                textBox7.Text = _session.UUID;
                textBox8.Text = _session.Xuid;
                } 
                catch (Exception ex)
                {
                       
                    textBox6.Text = null;
                    textBox7.Text = null;
                    textBox8.Text = null;
                    _session.Username = " ";
                    _session.UUID = " ";
                    _session.Xuid = " ";

                    pictureBox1.Image = null;

                     if (ex.Message.Contains("NOT_FOUND"))
    {
        checkBox1.Checked = true;
                        checkBox1.Enabled = false;
       
    }
    else
    {
        // 다른 오류는 메시지만 출력
        MessageBox.Show(this, $"오류 발생:{ex}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

                }
                LoadConfig();
                }
               
            }

        




        private async void button5_Click(object sender, EventArgs e) //다른계정 로그인
        {
            try
            {
                var loginHandler = JELoginHandlerBuilder.BuildDefault();
                var session = await loginHandler.AuthenticateInteractively();
                _session = session;
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string DataFolder = Path.Combine(appDataPath, "마크런처", "skin.png");
                string DataFolder1 = Path.Combine(appDataPath, "마크런처", "cape.png");

                getskin1(DataFolder, DataFolder1);

                textBox6.Text = _session.Username;
                textBox7.Text = _session.UUID;
                textBox8.Text = _session.Xuid;

                checkBox1.Checked = false;
                        checkBox1.Enabled = true;

            }
            catch (Exception ex)
            {
                
                    textBox6.Text = null;
                    textBox7.Text = null;
                    textBox8.Text = null;
                    _session.Username = " ";
                    _session.UUID = " ";
                    _session.Xuid = " ";
                    pictureBox1.Image = null;

                     if (ex.Message.Contains("NOT_FOUND"))
    {
        checkBox1.Checked = true;
                        checkBox1.Enabled = false;
       
    }
    else
    {
        // 다른 오류는 메시지만 출력
        MessageBox.Show(this, $"오류 발생:{ex}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

                }
                LoadConfig();
            }
        

        private async void button6_Click(object sender, EventArgs e) //로그아웃 버튼
        {
            if (_session == null)
            {
                MessageBox.Show(this, $"오류 발생: 계정에 로그인하지 않음", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult result = MessageBox.Show("로그아웃 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    var loginHandler = JELoginHandlerBuilder.BuildDefault();
                    await loginHandler.Signout();

                    _session = null;
                    pictureBox1.ImageLocation = null;
                    textBox6.Text = null;
                    textBox7.Text = null;
                    textBox8.Text = null;
                    MessageBox.Show(this, "로그아웃 성공", "알림", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else
                {

                }

            }

        }


        private async void LoadConfig()
        {
            try
            {
                if (System.IO.File.Exists(ConfigFilePath))
                {
                    var lines = System.IO.File.ReadAllLines(ConfigFilePath);
                    if (lines.Length >= 6)
                    {
                        comboBox1.Text = lines[0];
                        installPath = lines[1];
                        textBox2.Text = lines[2];
                        textBox3.Text = lines[3];
                        textBox4.Text = lines[4];
                        checkBox2.Checked = bool.Parse(lines[5]);
                        checkBox3.Checked = bool.Parse(lines[6]);
                        textBox5.Text = lines[7];
                        checkBox4.Checked = bool.Parse(lines[8]);

                    }
                    textBox1.Text = installPath;
                }
                else
                {
                    // 파일이 없으면 기본값을 사용하고 파일을 생성
                    installPath = defaultPath;
                    textBox2.Text = "4096";
                    textBox3.Text = "1600";
                    textBox4.Text = "900";
                    checkBox2.Checked = false;
                    checkBox3.Checked = true;
                    checkBox1.Checked = false;
                    checkBox4.Checked = false;
                    var launcher = new MinecraftLauncher();
                    var versions = await launcher.GetAllVersionsAsync();
                    comboBox1.Text = versions.LatestReleaseName;
                    comboBox2.Text = "옵션 1";
                    SaveConfig();
                }
            }
            catch (Exception ex)
            {
                DialogResult result = MessageBox.Show(this, $"오류 발생: {ex.Message} 버전이 업데이트되었거나 파일이 손상되었습니다. 다시 설정파일을 만드시겠습니까?", "오류", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (result == DialogResult.Yes)
                {
                    System.IO.File.Delete(ConfigFilePath);
                    var launcher = new MinecraftLauncher();
                    var versions = await launcher.GetAllVersionsAsync();
                    comboBox1.Text = versions.LatestReleaseName;
                    installPath = defaultPath;
                    textBox2.Text = "4096";
                    textBox3.Text = "1600";
                    textBox4.Text = "900";
                    textBox1.Text = installPath;
                    checkBox2.Checked = false;
                    checkBox3.Checked = true;
                    checkBox1.Checked = false;
                    checkBox4.Checked = false;
                    SaveConfig();
                    LoadConfig();
                }
                else
                {

                }
            }
        }

        private void SaveConfig()
        {
            try
            {
                var lines = new string[]
                {
            comboBox1.Text,
            installPath,
            textBox2.Text,
            textBox3.Text,
            textBox4.Text,
            checkBox2.Checked.ToString(),
            checkBox3.Checked.ToString(),
            textBox5.Text,
             checkBox4.Checked.ToString(),

                };
                Directory.CreateDirectory(Path.GetDirectoryName(ConfigFilePath));
                System.IO.File.WriteAllLines(ConfigFilePath, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("초기화 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var launcher = new MinecraftLauncher();
                var versions = await launcher.GetAllVersionsAsync();
                comboBox1.Text = versions.LatestReleaseName;
                installPath = defaultPath;
                textBox2.Text = "4096";
                textBox3.Text = "1600";
                textBox4.Text = "900";
                textBox1.Text = installPath;
                checkBox2.Checked = false;
                checkBox3.Checked = true;
                checkBox1.Checked = false;
                SaveConfig();
            }
            else
            {

            }


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                textBox5.ReadOnly = true;
                var launcher = new MinecraftLauncher();
                textBox5.Text = launcher.GetDefaultJavaPath();

            }
            else
            {
                textBox5.ReadOnly = false;
                textBox5.Text = null;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            from2 = new Form2(comboBox1.Text);
            from2.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string pach = textBox1.Text + "\\options.txt";
            form3Instance = new Form3(pach);
            form3Instance.Show();




        }

        private void button9_Click(object sender, EventArgs e)
        {
            string fileId = "1umgdd8uQxMoF6oY2ambC3b1QGEmFs7BM";
            string updateFileUrl = $"https://drive.google.com/uc?export=download&id={fileId}";
            string fileId1 = "1v_CRkO55qHJkYQ7gt1gmLlKz_t26Rnrp";
            string updateFileUrl1 = $"https://drive.google.com/uc?export=download&id={fileId1}";
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "minupdate");
            try
            {
                uioff();
                string updateFilePathExe1 = Path.Combine(appDataPath, "varsion.txt");  // 확장자를 변경할 업데이트 파일 경로
                if (File.Exists(updateFilePathExe1))
                {
                    File.Delete(updateFilePathExe1);
                }
                if (!Directory.Exists(appDataPath))
                {
                    Directory.CreateDirectory(appDataPath);
                }

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(updateFileUrl1), updateFilePathExe1);

                }

                // 기존 파일이 있을 경우 덮어쓰기

                string fileContent = File.ReadAllText(updateFilePathExe1);
                int threshold = 24;

                // 숫자가 기준보다 크면 명령 실행
                if (int.Parse(fileContent) > threshold)
                {
                    try
                    {
                        MessageBox.Show(this, "잠시 시간이 걸릴 수 있습니다.");

                        if (!Directory.Exists(appDataPath))
                        {
                            Directory.CreateDirectory(appDataPath);
                        }

                        string updateFilePathPng = Path.Combine(appDataPath, "update_mindyluncher.exe.png");  // AppData 경로에 업데이트 파일 저장
                        string updateFilePathExe = Path.Combine(appDataPath, "update_mindyluncher.exe");  // 확장자를 변경할 업데이트 파일 경로

                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(new Uri(updateFileUrl), updateFilePathPng);

                        }

                        // 기존 파일이 있을 경우 덮어쓰기
                        if (File.Exists(updateFilePathExe))
                        {
                            File.Delete(updateFilePathExe);
                        }

                        // 확장자 변경
                        File.Move(updateFilePathPng, updateFilePathExe);

                        // 다운로드 후 실행 (관리자 권한으로 실행)
                        var startInfo = new System.Diagnostics.ProcessStartInfo(updateFilePathExe)
                        {

                            UseShellExecute = true
                        };
                        System.Diagnostics.Process.Start(startInfo);
                        Application.Exit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, $"오류 발생: {ex.Message}");
                        uion();
                    }
                }
                else
                {
                    MessageBox.Show("이미 최신버전 입니다.");
                    uion();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"오류 발생: {ex.Message}");
                uion();
            }




        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (_session == null)
            {
                MessageBox.Show(this, "로그인 하세요.");
            }
            else
            {
                string capeFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                string skinFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                getskin(skinFilePath, capeFilePath);
            }




        }
        private async void getskin(string inputFolderPath, string outputFolderPath)
        {
            string sessionUrl = $"https://sessionserver.mojang.com/session/minecraft/profile/{_session.UUID}";
            try
            {
                // Mojang API에서 프로필 정보 가져오기
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync(sessionUrl);

                    // JSON 데이터 파싱
                    using (JsonDocument document = JsonDocument.Parse(response))
                    {
                        JsonElement properties = document.RootElement.GetProperty("properties")[0];
                        string textureDataBase64 = properties.GetProperty("value").GetString();

                        // Base64 디코딩
                        string textureDataJson = Encoding.UTF8.GetString(Convert.FromBase64String(textureDataBase64));
                        using (JsonDocument textureDocument = JsonDocument.Parse(textureDataJson))
                        {
                            JsonElement textures = textureDocument.RootElement.GetProperty("textures");

                            // 사용자 이름 가져오기
                            string username = document.RootElement.GetProperty("name").GetString();

                            // SKIN 다운로드
                            string skinUrl = textures.GetProperty("SKIN").GetProperty("url").GetString();
                            string model = "steve"; // 기본값은 steve로 설정

                            if (textures.GetProperty("SKIN").TryGetProperty("metadata", out JsonElement metadata))
                            {
                                if (metadata.TryGetProperty("model", out JsonElement modelElement))
                                {
                                    model = modelElement.GetString();
                                }
                            }

                            // 파일 경로 설정
                            string skinFilePath = Path.Combine(outputFolderPath, $"{username}_{model}_skin.png");

                            byte[] skinData = await client.GetByteArrayAsync(skinUrl);
                            await System.IO.File.WriteAllBytesAsync(skinFilePath, skinData);
                            MessageBox.Show(this, $"{skinUrl}에서 스킨이 성공적으로 다운로드되었습니다: {skinFilePath}", "다운로드");

                            // CAPE 다운로드 (존재할 경우)
                            if (textures.TryGetProperty("CAPE", out JsonElement capeElement))
                            {
                                string capeUrl = capeElement.GetProperty("url").GetString();
                                string capeFilePath = Path.Combine(outputFolderPath, $"{username}_cape.png");

                                byte[] capeData = await client.GetByteArrayAsync(capeUrl);
                                await System.IO.File.WriteAllBytesAsync(capeFilePath, capeData);
                                MessageBox.Show(this, $"{capeUrl}에서 망토가 성공적으로 다운로드되었습니다: {capeFilePath}", "다운로드");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"오류 발생: {ex.Message}");
            }
        }
        private async void getskin1(string skinFilePath, string capeFilePath)
        {
            string sessionUrl = $"https://sessionserver.mojang.com/session/minecraft/profile/{_session.UUID}";
            try
            {
                // Mojang API에서 프로필 정보 가져오기
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync(sessionUrl);

                    // JSON 데이터 파싱
                    using (JsonDocument document = JsonDocument.Parse(response))
                    {
                        JsonElement properties = document.RootElement.GetProperty("properties")[0];
                        string textureDataBase64 = properties.GetProperty("value").GetString();

                        // Base64 디코딩
                        string textureDataJson = Encoding.UTF8.GetString(Convert.FromBase64String(textureDataBase64));
                        using (JsonDocument textureDocument = JsonDocument.Parse(textureDataJson))
                        {
                            JsonElement textures = textureDocument.RootElement.GetProperty("textures");

                            // SKIN 다운로드
                            string skinUrl = textures.GetProperty("SKIN").GetProperty("url").GetString();

                            byte[] skinData = await client.GetByteArrayAsync(skinUrl);
                            await System.IO.File.WriteAllBytesAsync(skinFilePath, skinData);


                            // CAPE 다운로드 (존재할 경우)
                            if (textures.TryGetProperty("CAPE", out JsonElement capeElement))
                            {
                                string capeUrl = capeElement.GetProperty("url").GetString();

                                byte[] capeData = await client.GetByteArrayAsync(capeUrl);
                                await System.IO.File.WriteAllBytesAsync(capeFilePath, capeData);

                            }
                        }
                    }
                }

                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string DataFolder = Path.Combine(appDataPath, "마크런처", "skin.png");
                string DataFolder2 = Path.Combine(appDataPath, "마크런처");
                string DataFolder3 = Path.Combine(appDataPath, "마크런처", "combined_head.png");
                CombineHeadAndSave(DataFolder, DataFolder2);
                pictureBox1.ImageLocation = DataFolder3;

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"오류 발생: {ex.Message}");
            }

        }

        private async void button11_Click(object sender, EventArgs e)
        {
            string filePath;
            HttpClient httpClient = new HttpClient();
            Mojang mojang = new Mojang(httpClient);
            if (_session == null)
            {
                MessageBox.Show(this, "로그인 하세요.");
            }
            else
            {
                try
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "png files (png)|*.png";
                    openFileDialog.Title = "마크 스킨 파일 열기";

                    // 대화 상자를 띄우고 사용자가 선택한 파일 경로를 텍스트 박스에 표시
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        filePath = openFileDialog.FileName;
                        if (radioButton1.Checked)
                        {
                            await mojang.UploadSkin(_session.AccessToken, SkinType.Steve, filePath);
                            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                            string DataFolder2 = Path.Combine(appDataPath, "마크런처");
                            string DataFolder3 = Path.Combine(appDataPath, "마크런처", "combined_head.png");
                            CombineHeadAndSave(filePath, DataFolder2);
                            pictureBox1.ImageLocation = DataFolder3;

                        }
                        else if (radioButton2.Checked)
                        {
                            await mojang.UploadSkin(_session.AccessToken, SkinType.Alex, filePath);


                            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                            string DataFolder2 = Path.Combine(appDataPath, "마크런처");
                            string DataFolder3 = Path.Combine(appDataPath, "마크런처", "combined_head.png");
                            CombineHeadAndSave(filePath, DataFolder2);
                            pictureBox1.ImageLocation = DataFolder3;

                        }
                        else
                        {
                            MessageBox.Show(this, $"스킨의 형태를 지정해 주세요.");
                            return;
                        }


                        MessageBox.Show(filePath + "을 스킨으로 지정했습니다. 스킨 보여주는 사이트에 스킨은 변경되는데 시간이 걸릴 수 있습니다. 하지만 인게임에서는 잘 적용되어 있습니다.");


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"오류 발생: {ex.Message}");
                }


            }
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            if (textBox9.Text == "")
            {
                MessageBox.Show(this, "변경할 닉네임을 입력해 주세요.");
            }
            else
            {
                DialogResult result = MessageBox.Show("이름을 변경하면 몇 개월 동안 변경할 수 없습니다 변경하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DialogResult result1 = MessageBox.Show("이름을 변경하면 몇 개월 동안 변경할 수 없습니다 정말 변경하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result1 == DialogResult.Yes)
                    {
                        if (_session == null)
                        {
                            MessageBox.Show(this, "로그인 하세요.");
                        }
                        else
                        {
                            try
                            {
                                Mojang mojang = new Mojang(new HttpClient());
                                PlayerProfile profile = await mojang.ChangeName(_session.AccessToken, textBox9.Text);
                                textBox6.Text = _session.Username;
                                textBox7.Text = _session.UUID;
                                textBox8.Text = _session.Xuid;
                                MessageBox.Show(textBox9.Text, "으로 닉네임을 변경했습니다.");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(this, $"오류 발생: {ex.Message}");
                            }
                        }

                    }

                }
            }


        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                // 파일 탐색기로 폴더 열기
                Process.Start("explorer.exe", textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        static void CombineHeadAndSave(string inputPath, string outputPath)
        {
            try
            {
                // 입력 경로에서 이미지를 로드
                using (Bitmap skin = new Bitmap(inputPath))
                {
                    // 이미지 크기 확인
                    if (skin.Width == 64 && skin.Height == 64)
                    {
                        Process64x64Skin(skin, outputPath);
                    }
                    else if (skin.Width == 64 && skin.Height == 32)
                    {
                        Process64x32Skin(skin, outputPath);
                    }
                    else
                    {
                        Console.WriteLine("지원하지 않는 스킨 크기입니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 발생: {ex.Message}");
            }

        }

        static void Process64x64Skin(Bitmap skin, string outputPath)
        {
            try
            {
                // 머리 부분과 앞 레이어의 영역 설정
                Rectangle headRect = new Rectangle(8, 8, 8, 8);
                Rectangle headFrontLayerRect = new Rectangle(40, 8, 8, 8);

                // 머리 부분 추출
                Bitmap head = skin.Clone(headRect, skin.PixelFormat);

                // 머리 앞 레이어 추출 및 머리 부분에 겹침
                using (Bitmap headFrontLayer = skin.Clone(headFrontLayerRect, skin.PixelFormat))
                {
                    using (Graphics g = Graphics.FromImage(head))
                    {
                        g.DrawImage(headFrontLayer, new Rectangle(0, 0, head.Width, head.Height));
                    }
                }

                SaveImage(head, outputPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 발생: {ex.Message}");
            }

        }

        static void Process64x32Skin(Bitmap skin, string outputPath)
        {

            try
            {
                // 머리 부분의 영역 설정 (다중 레이어 없음)
                Rectangle headRect = new Rectangle(8, 8, 8, 8);

                // 머리 부분 추출
                Bitmap head = skin.Clone(headRect, skin.PixelFormat);

                SaveImage(head, outputPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 발생: {ex.Message}");
            }

        }

        static void SaveImage(Bitmap image, string outputPath)
        {
            try
            {
                // 100x100 크기의 새로운 이미지를 생성
                Bitmap enlargedHead = new Bitmap(100, 100);

                // 확대된 이미지에 원본 이미지를 그리기
                using (Graphics g = Graphics.FromImage(enlargedHead))
                {
                    // 이미지를 100x100 크기로 확대
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                    g.DrawImage(image, new Rectangle(0, 0, 100, 100));
                }

                // 출력 경로가 존재하지 않으면 생성
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }

                // 확대된 이미지를 출력 경로에 저장
                string outputFilePath = Path.Combine(outputPath, "combined_head.png");
                enlargedHead.Save(outputFilePath, System.Drawing.Imaging.ImageFormat.Png);

                Console.WriteLine($"Combined head image saved to: {outputFilePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 발생: {ex.Message}");
            }

        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (_session == null)
            {
                MessageBox.Show(this, "로그인 하세요.");
            }
            else
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string DataFolder = Path.Combine(appDataPath, "마크런처", "skin.png");
                string headFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                string oldoutputFilePath = Path.Combine(headFilePath, "combined_head.png");
                string outputFilePath = Path.Combine(headFilePath, $"{_session.Username}_head.png");
                try
                {
                    CombineHeadAndSave(DataFolder, headFilePath);
                    if (File.Exists(outputFilePath))
                    {
                        File.Delete(outputFilePath);

                    }
                    File.Move(oldoutputFilePath, outputFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                MessageBox.Show(this, $"{outputFilePath}에 스킨의 얼굴이 다운로드 되었습니다.");
            }


        }

        private void button15_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}



