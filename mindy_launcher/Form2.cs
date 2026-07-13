using System;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using CmlLib.Core.Utils;

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
    public partial class Form2 : Form
    {
        string v;
        public Form2(string version)
        {
            v = version;
            InitializeComponent();
            this.MaximizeBox = false;
            this.Text = v + " 패치노트";
        }

        private Changelogs? changelogs;

        private async void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                changelogs = await Changelogs.GetChangelogs(new HttpClient());

                if (changelogs == null)
                {
                    MessageBox.Show("아무 버전도 선택하지 않았습니다.");
                    this.Close();
                    return;
                }

                var version = v;
                if (string.IsNullOrEmpty(version))
                {
                    MessageBox.Show("아무 버전도 선택하지 않았습니다.");
                    this.Close();
                    return;
                }

                var body = await changelogs.GetChangelogHtml(version);

                // AppData 경로 가져오기
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string webView2UserDataFolder = Path.Combine(appDataPath, "마크런처", "WebView2");

                // WebView2 환경 설정 및 초기화
                var env = await CoreWebView2Environment.CreateAsync(null, webView2UserDataFolder, new CoreWebView2EnvironmentOptions("--allow-file-access-from-files"));
                await webView21.EnsureCoreWebView2Async(env);

                // HTML 콘텐츠를 WebView2에 로드
                webView21.NavigateToString(body);

                // HTML 파일 저장
                string directoryPath = Path.Combine(appDataPath, "마크런처");
                string filePath = Path.Combine(directoryPath, v + " changelog.html");

                // 디렉토리가 존재하지 않으면 생성
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // 파일 쓰기
                File.WriteAllText(filePath, body);
                
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("파일 접근 권한이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("디렉토리를 찾을 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show($"파일 IO 오류: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"오류 발생: 인터넷이 연결되지 않았거나 이 버전에는 패치노트가 없습니다. {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}