using System.Windows.Forms;
using MetroSuite;
using System.Collections.Generic;
using System;
using CapMonsterCloud;
using CapMonsterCloud.Models.CaptchaTasks;
using CapMonsterCloud.Models.CaptchaTasksResults;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;

public partial class MainForm : MetroForm
{
    private bool generating = false;
    private List<string> usernames, passwords, avatars, aboutMes, proxies;
    private string specifiedAvatar;
    private string[] mediaFormats = new string[] { "jpg", "png", "bmp", "jpeg", "jfif", "jpe", "rle", "dib", "svg", "svgz" };
    private string captchaKey = "", phoneKey = "";
    private ServiceCaptcha captchaService = ServiceCaptcha.TwoCaptcha;
    private ServicePhone phoneService = ServicePhone.FiveSIM;
    private CapMonsterClient capMonsterClient;
    private HttpClient twoCaptchaClient;
    private bool usingCaptchas = false, usingPhone = false;
    internal static readonly char[] hypesquad = "123".ToCharArray();
    internal static readonly char[] everything = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
    internal static readonly char[] characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    internal static readonly char[] passwordCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789#?=)(/&%$£€{}òçù§@".ToCharArray();
    private int currentUsername = 0, currentAvatar = 0, currentPassword = 0, currentAboutMe = 0, currentProxy = 0;
    private bool informationsReloaded = false;
    private int limitTokens = 100;

    public MainForm()
    {
        InitializeComponent();

        CheckForIllegalCrossThreadCalls = false;
        siticoneComboBox1.SelectedIndex = 0;

        this.usernames = new List<string>();
        this.passwords = new List<string>();
        this.avatars = new List<string>();
        this.aboutMes = new List<string>();
        this.proxies = new List<string>();

        this.twoCaptchaClient = CreateCleanRequest();
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;

        Thread thread = new Thread(ReloadServicesInformations);
        thread.Priority = ThreadPriority.Highest;
        thread.Start();

        Thread thread1 = new Thread(ReloadGlobalVariables);
        thread1.Priority = ThreadPriority.Highest;
        thread1.Start();
    }

    public void ReloadGlobalVariables()
    {
        while (true)
        {
            Thread.Sleep(1000);

            metroLabel2.Text = GlobalVariables.tokensGenerated.ToString();
            metroLabel3.Text = GlobalVariables.captchaSuccess.ToString();
            metroLabel5.Text = GlobalVariables.captchaFails.ToString();
            metroLabel7.Text = GlobalVariables.generationFails.ToString();
            metroLabel9.Text = GlobalVariables.operationsExecuted.ToString();
        }
    }

    public void ReloadLoadedInformations()
    {
        metroLabel29.Text = "Loaded usernames: " + this.usernames.Count.ToString() + Environment.NewLine +
            "Loaded passwords: " + this.passwords.Count.ToString() + Environment.NewLine +
            "Loaded avatars: " + this.avatars.Count.ToString();
        metroLabel23.Text = "Loaded proxies: " + this.proxies.Count.ToString();
        metroLabel26.Text = "Loaded about me: " + this.aboutMes.Count.ToString();
    }

    public string GetInviteCodeByInviteLink(string inviteLink)
    {
        try
        {
            if (inviteLink.EndsWith("/"))
            {
                inviteLink = inviteLink.Substring(0, inviteLink.Length - 1);
            }

            if (inviteLink.Contains("discord") && inviteLink.Contains("/") && inviteLink.Contains("http"))
            {
                string[] splitter = Microsoft.VisualBasic.Strings.Split(inviteLink, "/");

                return splitter[splitter.Length - 1];
            }
        }
        catch
        {

        }

        return inviteLink;
    }

    public HttpClient CreateCleanRequest()
    {
        HttpClientHandler handler = new HttpClientHandler();

        handler.UseProxy = false;
        handler.UseDefaultCredentials = false;
        handler.UseCookies = false;
        handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
        handler.ServerCertificateCustomValidationCallback = null;
        handler.Proxy = null;
        handler.PreAuthenticate = false;
        handler.MaxResponseHeadersLength = 64;
        handler.MaxConnectionsPerServer = 1;
        handler.MaxAutomaticRedirections = 1;
        handler.DefaultProxyCredentials = null;
        handler.Credentials = null;
        handler.AutomaticDecompression = DecompressionMethods.None;
        handler.AllowAutoRedirect = false;

        HttpClient client = new HttpClient(handler);

        client.DefaultRequestHeaders.Clear();
        //client.DefaultRequestVersion = new Version(3, 0, 0, 0);

        return client;
    }

    public string GetBalance(string apiKey)
    {
        try
        {
            twoCaptchaClient.DefaultRequestHeaders.Clear();
            return twoCaptchaClient.GetAsync("http://2captcha.com/res.php?key=" + apiKey + "&action=getbalance").Result.Content.ReadAsStringAsync().Result;
        }
        catch
        {
            return "0.00";
        }
    }

    public void ReloadServicesInformations()
    {
        while (true)
        {
            Thread.Sleep(1000);

            string tCaptchaService = "", tCaptchaBalance = "", tPhoneService = "", tPhoneBalance = "";

            if (usingCaptchas && captchaKey != "")
            {
                if (captchaService.Equals(ServiceCaptcha.CapMonster))
                {
                    tCaptchaService = "CapMonster";

                    try
                    {
                        tCaptchaBalance = capMonsterClient.GetBalanceAsync().Result.ToString().Replace(",", ".") + " $";
                    }
                    catch
                    {
                        tCaptchaBalance = "/";
                    }
                }
                else
                {
                    tCaptchaService = "TwoCaptcha";
                    tCaptchaBalance = GetBalance(this.captchaKey) + " $";
                }
            }
            else
            {
                tCaptchaService = "/";
                tCaptchaBalance = "/";
            }

            if (usingPhone && phoneKey != "")
            {
                if (phoneService.Equals(ServicePhone.FiveSIM))
                {
                    tPhoneService = "5SIM";
                    tPhoneBalance = GetPhoneBalance(phoneKey, ServicePhone.FiveSIM) + " ₽";
                }
                else if (phoneService.Equals(ServicePhone.SMSPVA))
                {
                    tPhoneService = "SMSPVA";
                    tPhoneBalance = GetPhoneBalance(phoneKey, ServicePhone.SMSPVA) + " $";
                }
            }
            else
            {
                tPhoneService = "/";
                tPhoneBalance = "/";
            }

            metroLabel13.Text = "Captcha service: " + tCaptchaService + Environment.NewLine +
                "Captcha balance: " + tCaptchaBalance + Environment.NewLine +
                "Phone service: " + tPhoneService + Environment.NewLine +
                "Phone balance: " + tPhoneBalance;
        }
    }

    public string GetPhoneBalance(string serviceKey, ServicePhone service)
    {
        try
        {
            if (service.Equals(ServicePhone.FiveSIM))
            {
                twoCaptchaClient.DefaultRequestHeaders.Clear();

                twoCaptchaClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + serviceKey);
                twoCaptchaClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");

                dynamic jss = JObject.Parse(twoCaptchaClient.GetAsync("https://5sim.net/v1/user/profile").Result.Content.ReadAsStringAsync().Result);
                return (string)jss.balance;
            }
            else
            {
                twoCaptchaClient.DefaultRequestHeaders.Clear();
                dynamic jss = JObject.Parse(twoCaptchaClient.GetAsync("http://smspva.com/priemnik.php?metod=get_balance&apikey=" + serviceKey).Result.Content.ReadAsStringAsync().Result);
                return (string)jss.balance;
            }
        }
        catch
        {
            return "/";
        }
    }

    private void gunaButton16_Click(object sender, System.EventArgs e)
    {
        captchaService = ServiceCaptcha.TwoCaptcha;

        if (siticoneRadioButton2.Checked)
        {
            captchaService = ServiceCaptcha.CapMonster;
        }
        else
        {
            captchaService = ServiceCaptcha.TwoCaptcha;
        }

        phoneService = ServicePhone.FiveSIM;

        if (siticoneRadioButton5.Checked)
        {
            phoneService = ServicePhone.SMSPVA;
        }
        else
        {
            phoneService = ServicePhone.FiveSIM;
        }

        string tempCaptchaKey = gunaLineTextBox2.Text, tempPhoneKey = gunaLineTextBox3.Text;

        if (usingCaptchas)
        {
            if (captchaService.Equals(ServiceCaptcha.TwoCaptcha))
            {
                try
                {
                    string theBalance = GetBalance(tempCaptchaKey);

                    if (theBalance != "ERROR_WRONG_USER_KEY" && theBalance != "ERROR_KEY_DOES_NOT_EXIST" && theBalance != "IP_BANNED")
                    {
                        captchaKey = tempCaptchaKey;
                    }
                    else
                    {
                        MessageBox.Show("Your service API key for 2Captcha is not valid!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("Your service API key for 2Captcha is not valid!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (captchaService.Equals(ServiceCaptcha.CapMonster))
            {
                try
                {
                    capMonsterClient = new CapMonsterClient(tempCaptchaKey);
                    capMonsterClient.GetBalanceAsync().Result.ToString();
                    captchaKey = tempCaptchaKey;
                }
                catch
                {
                    MessageBox.Show("Your service API key for CapMonster is not valid!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        if (usingPhone)
        {
            if (phoneService.Equals(ServicePhone.FiveSIM))
            {
                twoCaptchaClient.DefaultRequestHeaders.Clear();

                twoCaptchaClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + tempPhoneKey);
                twoCaptchaClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");

                string thingy = twoCaptchaClient.GetAsync("https://5sim.net/v1/user/profile").Result.Content.ReadAsStringAsync().Result;

                if (!thingy.Contains("email"))
                {
                    MessageBox.Show("Your service API key for 5SIM is not valid!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    phoneKey = tempPhoneKey;
                }
            }
            else if (phoneService.Equals(ServicePhone.SMSPVA))
            {
                twoCaptchaClient.DefaultRequestHeaders.Clear();

                if (twoCaptchaClient.GetAsync("http://smspva.com/priemnik.php?metod=get_balance&apikey=" + tempPhoneKey).Result.Content.ReadAsStringAsync().Result.Contains("FOUND"))
                {
                    MessageBox.Show("Your service API key for SMSPVA is not valid!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    phoneKey = tempPhoneKey;
                }
            }
        }

        informationsReloaded = true;
    }

    private void gunaButton2_Click(object sender, System.EventArgs e)
    {
        openFileDialog1.FileName = "";
        openFileDialog1.Filter = "Text file (*.txt)|*.txt";
        openFileDialog1.Title = "Open your username list.";

        if (openFileDialog1.ShowDialog().Equals(DialogResult.OK))
        {
            try
            {
                foreach (string line in System.IO.File.ReadAllLines(openFileDialog1.FileName))
                {
                    usernames.Add(line);
                }
            }
            catch
            {

            }
        }

        ReloadLoadedInformations();
    }

    private void gunaButton3_Click(object sender, System.EventArgs e)
    {
        this.usernames.Clear();
        ReloadLoadedInformations();
    }

    private void gunaButton6_Click(object sender, System.EventArgs e)
    {
        openFileDialog1.FileName = "";
        openFileDialog1.Filter = "Text file (*.txt)|*.txt";
        openFileDialog1.Title = "Open your password list.";

        if (openFileDialog1.ShowDialog().Equals(DialogResult.OK))
        {
            try
            {
                foreach (string line in System.IO.File.ReadAllLines(openFileDialog1.FileName))
                {
                    passwords.Add(line);
                }
            }
            catch
            {

            }
        }

        ReloadLoadedInformations();
    }

    private void gunaButton5_Click(object sender, System.EventArgs e)
    {
        this.passwords.Clear();
        ReloadLoadedInformations();
    }

    private void gunaButton9_Click(object sender, System.EventArgs e)
    {
        openFileDialog1.FileName = "";

        openFileDialog1.Filter = "All files (*.*)|*.*";

        foreach (string format in mediaFormats)
        {
            openFileDialog1.Filter += "|" + format.ToUpper() + " Image (*." + format + ")|*." + format;
        }

        openFileDialog1.Title = "Open your avatar list.";

        if (openFileDialog1.ShowDialog().Equals(DialogResult.OK))
        {
            bool isIt = false;

            foreach (string format in mediaFormats)
            {
                if (System.IO.Path.GetExtension(openFileDialog1.FileName).ToLower().Equals("." + format))
                {
                    isIt = true;
                }
            }

            if (isIt)
            {
                specifiedAvatar = openFileDialog1.FileName;
            }
            else
            {
                MessageBox.Show("Invalid file format!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void gunaButton8_Click(object sender, System.EventArgs e)
    {
        if (folderBrowserDialog1.ShowDialog().Equals(DialogResult.OK))
        {
            foreach (string file in System.IO.Directory.GetFiles(folderBrowserDialog1.SelectedPath))
            {
                foreach (string format in mediaFormats)
                {
                    if (System.IO.Path.GetExtension(file).ToLower().Equals("." + format))
                    {
                        avatars.Add(file);
                    }
                }
            }

            ReloadLoadedInformations();
        }
    }

    private void gunaButton7_Click(object sender, System.EventArgs e)
    {
        this.avatars.Clear();
        ReloadLoadedInformations();
    }

    private void gunaButton10_Click(object sender, System.EventArgs e)
    {
        openFileDialog1.FileName = "";
        openFileDialog1.Filter = "Text file (*.txt)|*.txt";
        openFileDialog1.Title = "Open your proxy list.";

        if (openFileDialog1.ShowDialog().Equals(DialogResult.OK))
        {
            try
            {
                foreach (string line in System.IO.File.ReadAllLines(openFileDialog1.FileName))
                {
                    proxies.Add(line);
                }
            }
            catch
            {

            }
        }

        ReloadLoadedInformations();
    }

    private void gunaButton11_Click(object sender, System.EventArgs e)
    {
        this.proxies.Clear();
        ReloadLoadedInformations();
    }

    private void gunaButton12_Click(object sender, System.EventArgs e)
    {

    }

    private void siticoneCheckBox6_CheckedChanged(object sender, EventArgs e)
    {
        usingCaptchas = siticoneCheckBox6.Checked;
    }

    private void siticoneCheckBox7_CheckedChanged(object sender, EventArgs e)
    {
        usingPhone = siticoneCheckBox7.Checked;
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        Process.GetCurrentProcess().Kill();
    }

    private void gunaButton13_Click(object sender, System.EventArgs e)
    {
        saveFileDialog1.FileName = "";

        if (saveFileDialog1.ShowDialog().Equals(DialogResult.OK))
        {
            gunaLineTextBox7.Text = saveFileDialog1.FileName;
        }
    }

    private void gunaButton15_Click(object sender, System.EventArgs e)
    {
        openFileDialog1.FileName = "";
        openFileDialog1.Filter = "Text file (*.txt)|*.txt";
        openFileDialog1.Title = "Open your about me list.";

        if (openFileDialog1.ShowDialog().Equals(DialogResult.OK))
        {
            try
            {
                foreach (string line in System.IO.File.ReadAllLines(openFileDialog1.FileName))
                {
                    aboutMes.Add(line);
                }
            }
            catch
            {

            }
        }

        ReloadLoadedInformations();
    }

    private void gunaButton14_Click(object sender, System.EventArgs e)
    {
        this.aboutMes.Clear();
        ReloadLoadedInformations();
    }

    private void gunaButton4_Click(object sender, System.EventArgs e)
    {
        gunaButton4.Enabled = false;

        Thread thread = new Thread(CheckGeneration);
        thread.Priority = ThreadPriority.Highest;
        thread.Start();
    }

    public void CheckGeneration()
    {
        if (!informationsReloaded)
        {
            MessageBox.Show("Please, insert your captcha key / phone key in 'Gen Settings' and reload your informations in the 'Dashboard'.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            gunaButton4.Enabled = true;
            return;
        }

        if (siticoneCheckBox5.Checked)
        {
            string limit = gunaLineTextBox4.Text;

            if (!Microsoft.VisualBasic.Information.IsNumeric(limit))
            {
                MessageBox.Show("Please, insert a valid number as limit number for generation.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                gunaButton4.Enabled = true;
                return;
            }

            try
            {
                int i = int.Parse(limit);
            }
            catch
            {
                MessageBox.Show("Please, insert a valid number as limit number for generation.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                gunaButton4.Enabled = true;
                return;
            }
        }

        if (siticoneCheckBox4.Checked)
        {
            try
            {
                DiscordInvite invite = Utils.GetInviteInformations(Utils.GetInviteCodeByInviteLink(gunaLineTextBox1.Text), false);

                if (!invite.valid)
                {
                    MessageBox.Show("Please, insert a valid Discord invite link / code.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    gunaButton4.Enabled = true;
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Please, insert a valid Discord invite link / code.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                gunaButton4.Enabled = true;
                return;
            }
        }

        if (siticoneRadioButton7.Checked)
        {
            if (gunaLineTextBox5.Text.Replace(" ", "").Replace('\t'.ToString(), "").Trim() == "")
            {
                MessageBox.Show("Please, insert a valid username.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                gunaButton4.Enabled = true;
                return;
            }
        }

        if (siticoneRadioButton6.Checked)
        {
            if (!(this.usernames.Count >= 5))
            {
                MessageBox.Show("Please, insert some usernames (at least 5).", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                gunaButton4.Enabled = true;
                return;
            }
        }

        if (siticoneRadioButton10.Checked)
        {
            if (gunaLineTextBox6.Text.Replace(" ", "").Replace('\t'.ToString(), "").Trim() == "")
            {
                MessageBox.Show("Please, insert a valid password.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                gunaButton4.Enabled = true;
                return;
            }
        }

        if (siticoneRadioButton9.Checked)
        {
            if (!(this.passwords.Count >= 5))
            {
                MessageBox.Show("Please, insert some passwords (at least 5).", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                gunaButton4.Enabled = true;
                return;
            }
        }

        if (siticoneCheckBox14.Checked)
        {
            if (siticoneRadioButton13.Checked)
            {
                if (specifiedAvatar.Replace(" ", "").Replace('\t'.ToString(), "").Trim() == "")
                {
                    MessageBox.Show("Please, insert a valid avatar.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    gunaButton4.Enabled = true;
                    return;
                }
            }

            if (siticoneRadioButton12.Checked)
            {
                if (!(this.avatars.Count >= 5))
                {
                    MessageBox.Show("Please, insert some avatars (at least 5).", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    gunaButton4.Enabled = true;
                    return;
                }
            }
        }

        if (siticoneCheckBox1.Checked)
        {
            if (!(this.proxies.Count >= 5))
            {
                MessageBox.Show("Please, insert some proxies (at least 5).", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                gunaButton4.Enabled = true;
                return;
            }
        }

        if (System.IO.File.Exists(gunaLineTextBox7.Text))
        {
            MessageBox.Show("Please, delete the existing file you have imported to save your generated tokens.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            gunaButton4.Enabled = true;
            return;
        }

        if (siticoneCheckBox15.Checked)
        {
            if (siticoneRadioButton29.Checked)
            {
                if (gunaLineTextBox8.Text.Replace(" ", "").Replace('\t'.ToString(), "").Trim() == "")
                {
                    MessageBox.Show("Please, insert a valid about me.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    gunaButton4.Enabled = true;
                    return;
                }
            }

            if (siticoneRadioButton26.Checked)
            {
                if (!(this.aboutMes.Count >= 5))
                {
                    MessageBox.Show("Please, insert some about me (at least 5).", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    gunaButton4.Enabled = true;
                    return;
                }
            }
        }

        GlobalVariables.tokensGenerated = 0;
        GlobalVariables.captchaFails = 0;
        GlobalVariables.captchaSuccess = 0;
        GlobalVariables.generationFails = 0;
        GlobalVariables.operationsExecuted = 0;

        currentUsername = 0;
        currentAboutMe = 0;
        currentAvatar = 0;
        currentPassword = 0;
        currentProxy = 0;

        generating = true;

        gunaButton4.Enabled = false;
        gunaButton1.Enabled = true;

        for (int i = 0; i < siticoneSlider1.Value; i++)
        {
            Thread thread = new Thread(GenerateToken);
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }
    }

    public void GenerateToken()
    {
        try
        {
            if (generating)
            {
                while (true)
                {
                    if (generating)
                    {
                        try
                        {
                            if (siticoneCheckBox5.Checked)
                            {
                                try
                                {
                                    int i = int.Parse(gunaLineTextBox4.Text);

                                    if (GlobalVariables.tokensGenerated >= i)
                                    {
                                        generating = false;

                                        gunaButton1.Enabled = false;
                                        gunaButton4.Enabled = true;

                                        return;
                                    }
                                }
                                catch
                                {

                                }
                            }

                            TokenGenerator generator = new TokenGenerator();

                            generator.emailVerification = siticoneCheckBox9.Checked;
                            generator.phoneVerification = siticoneCheckBox7.Checked;
                            generator.useProxies = siticoneCheckBox1.Checked;
                            generator.customAvatar = siticoneCheckBox14.Checked;
                            generator.customHypeSquad = siticoneCheckBox13.Checked;
                            generator.customAboutMe = siticoneCheckBox15.Checked;
                            generator.capMonster = captchaService.Equals(ServiceCaptcha.CapMonster);
                            generator.captchaKey = captchaKey;
                            generator.phoneKey = phoneKey;
                            generator.smspva = phoneService.Equals(ServicePhone.SMSPVA);

                            //string token = generator.Generate(GetUsername(), GetAvatar(), GetProxy(), GetPassword(), GetHypesquad(), GetAboutMe(), siticoneCheckBox4.Checked ? GetInviteCodeByInviteLink(gunaLineTextBox1.Text) : "");
                            //EmitToken(token);
                            /*Thread thread = new Thread(() => Utils.EmitTheToken(token, siticoneCheckBox5, gunaLineTextBox4, gunaTextBox1, gunaLineTextBox9, gunaLineTextBox7, siticoneCheckBox2, siticoneCheckBox3));
                            thread.Priority = ThreadPriority.Highest;
                            thread.Start();*/

                            generator.Generate(GetUsername(), GetAvatar(), GetProxy(), GetPassword(), GetHypesquad(), GetAboutMe(), siticoneCheckBox4.Checked ? GetInviteCodeByInviteLink(gunaLineTextBox1.Text) : "", siticoneCheckBox5, gunaLineTextBox4, gunaTextBox1, gunaLineTextBox9, gunaLineTextBox7, siticoneCheckBox2, siticoneCheckBox3);                  

                            if (siticoneCheckBox5.Checked)
                            {
                                try
                                {
                                    int i = int.Parse(gunaLineTextBox4.Text);

                                    if (GlobalVariables.tokensGenerated >= i)
                                    {
                                        generating = false;

                                        gunaButton1.Enabled = false;
                                        gunaButton4.Enabled = true;

                                        return;
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                        catch
                        {
                            GlobalVariables.generationFails++;
                        }
                    }
                }
            }
        }
        catch
        {
            GlobalVariables.generationFails++;
        }
    }

    public void EmitToken(string token)
    {
        try
        {
            if (token == "")
            {
                GlobalVariables.generationFails++;
                return;
            }

            GlobalVariables.tokensGenerated++;

            try
            {
                if (siticoneCheckBox2.Checked)
                {
                    Utils.SendMessageToWebhook(gunaLineTextBox9.Text, "AstarothGenerator | Tokens", "", token);
                }
            }
            catch
            {

            }

            try
            {
                if (gunaTextBox1.Text == "")
                {
                    gunaTextBox1.Text = token;
                }
                else
                {
                    gunaTextBox1.Text += Environment.NewLine + token;
                }
            }
            catch
            {

            }

            try
            {
                if (siticoneCheckBox3.Checked)
                {
                    if (System.IO.File.Exists(gunaLineTextBox7.Text))
                    {
                        System.IO.File.AppendAllText(gunaLineTextBox7.Text, token);
                    }
                    else
                    {
                        System.IO.File.WriteAllText(gunaLineTextBox7.Text, token);
                    }
                }
            }
            catch
            {

            }
        }
        catch
        {
            GlobalVariables.generationFails++;
        }
    }

    private void gunaButton1_Click(object sender, System.EventArgs e)
    {
        generating = false;

        gunaButton1.Enabled = false;
        gunaButton4.Enabled = true;
    }

    private void siticoneSlider1_Scroll(object sender, System.EventArgs e)
    {
        if (siticoneSlider1.Value > limitTokens)
        {
            siticoneSlider1.Value = limitTokens;
        }

        metroLabel11.Text = "Threads of generation: (" + siticoneSlider1.Value.ToString() + ")";
    }

    public string GetUsername()
    {
        SettingMode usernameMode = SettingMode.Random;

        if (siticoneRadioButton7.Checked)
        {
            usernameMode = SettingMode.Specified;
        }
        else if (siticoneRadioButton6.Checked)
        {
            usernameMode = SettingMode.Listed;
        }

        if (usernameMode.Equals(SettingMode.Random))
        {
            return GenerateString(6) + " | AstarothGenerator";
        }
        else if (usernameMode.Equals(SettingMode.Specified))
        {
            return gunaLineTextBox5.Text.Replace("[rndstr]", GenerateString(6));
        }
        else if (usernameMode.Equals(SettingMode.Listed))
        {
            try
            {
                try
                {
                    if (currentUsername >= usernames.Count)
                    {
                        currentUsername = -1;
                    }

                    currentUsername++;

                    if (currentUsername >= usernames.Count)
                    {
                        currentUsername = 0;
                    }

                    return usernames[currentUsername];
                }
                catch
                {
                    return GenerateString(6) + " | AstarothGenerator";
                }
            }
            catch
            {
                return GenerateString(6) + " | AstarothGenerator";
            }
        }

        return GenerateString(6) + " | AstarothGenerator";
    }

    public string GetPassword()
    {
        SettingMode passwordMode = SettingMode.Random;

        if (siticoneRadioButton10.Checked)
        {
            passwordMode = SettingMode.Specified;
        }
        else if (siticoneRadioButton9.Checked)
        {
            passwordMode = SettingMode.Listed;
        }

        if (passwordMode.Equals(SettingMode.Random))
        {
            return GeneratePassword(20);
        }
        else if (passwordMode.Equals(SettingMode.Specified))
        {
            return gunaLineTextBox6.Text;
        }
        else if (passwordMode.Equals(SettingMode.Listed))
        {
            try
            {
                try
                {
                    if (currentPassword >= passwords.Count)
                    {
                        currentUsername = -1;
                    }

                    currentPassword++;

                    if (currentPassword >= passwords.Count)
                    {
                        currentPassword = 0;
                    }

                    return passwords[currentPassword];
                }
                catch
                {
                    return GeneratePassword(20);
                }
            }
            catch
            {
                return GeneratePassword(20);
            }
        }

        return GeneratePassword(20);
    }

    public string GetAvatar()
    {
        if (!siticoneCheckBox14.Checked)
        {
            return "";
        }

        SettingMode avatarMode = SettingMode.Random;

        if (siticoneRadioButton13.Checked)
        {
            avatarMode = SettingMode.Specified;
        }
        else if (siticoneRadioButton12.Checked)
        {
            avatarMode = SettingMode.Listed;
        }

        if (avatarMode.Equals(SettingMode.Random))
        {
            return "";
        }
        else if (avatarMode.Equals(SettingMode.Specified))
        {
            return specifiedAvatar;
        }
        else if (avatarMode.Equals(SettingMode.Listed))
        {
            try
            {
                try
                {
                    if (currentAvatar >= avatars.Count)
                    {
                        currentAvatar = -1;
                    }

                    currentAvatar++;

                    if (currentAvatar >= avatars.Count)
                    {
                        currentAvatar = 0;
                    }

                    return avatars[currentAvatar];
                }
                catch
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

        return "";
    }

    public string GetProxy()
    {
        if (!siticoneCheckBox1.Checked)
        {
            return "";
        }

        try
        {
            try
            {
                if (currentProxy >= proxies.Count)
                {
                    currentProxy = -1;
                }

                currentProxy++;

                if (currentProxy >= proxies.Count)
                {
                    currentProxy = 0;
                }

                return proxies[currentProxy];
            }
            catch
            {
                return "";
            }
        }
        catch
        {
            return "";
        }
    }

    public string GetAboutMe()
    {
        if (!siticoneCheckBox15.Checked)
        {
            return "";
        }

        SettingMode aboutMeMode = SettingMode.Random;

        if (siticoneRadioButton29.Checked)
        {
            aboutMeMode = SettingMode.Specified;
        }
        else if (siticoneRadioButton26.Checked)
        {
            aboutMeMode = SettingMode.Listed;
        }

        if (aboutMeMode.Equals(SettingMode.Random))
        {
            return GenerateString(20) + " | AstarothGenerator";
        }
        else if (aboutMeMode.Equals(SettingMode.Specified))
        {
            return gunaLineTextBox8.Text.Replace("[rndstr]", GenerateString(6));
        }
        else if (aboutMeMode.Equals(SettingMode.Listed))
        {
            try
            {
                try
                {
                    if (currentAboutMe >= aboutMes.Count)
                    {
                        currentAboutMe = -1;
                    }

                    currentAboutMe++;

                    if (currentAboutMe >= aboutMes.Count)
                    {
                        currentAboutMe = 0;
                    }

                    return aboutMes[currentAboutMe];
                }
                catch
                {
                    return GenerateString(20) + " | AstarothGenerator";
                }
            }
            catch
            {
                return GenerateString(20) + " | AstarothGenerator";
            }
        }

        return GenerateString(20) + " | AstarothGenerator";
    }

    public string GetHypesquad()
    {
        if (!siticoneCheckBox13.Checked)
        {
            return "";
        }

        if (siticoneRadioButton28.Checked)
        {
            return RandomHypeSquad();
        }

        return (siticoneComboBox1.SelectedIndex + 1).ToString();
    }

    public string GeneratePassword(int size)
    {
        try
        {
            byte[] data = new byte[4 * size];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }

            StringBuilder result = new StringBuilder(size);

            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % passwordCharacters.Length;

                result.Append(passwordCharacters[idx]);
            }

            return result.ToString();
        }
        catch
        {
            return "";
        }
    }

    public string GenerateString(int size)
    {
        try
        {
            byte[] data = new byte[4 * size];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }

            StringBuilder result = new StringBuilder(size);

            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % characters.Length;

                result.Append(characters[idx]);
            }

            return result.ToString();
        }
        catch
        {
            return "";
        }
    }

    public string GenerateUID(int size)
    {
        try
        {
            byte[] data = new byte[4 * size];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }

            StringBuilder result = new StringBuilder(size);

            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % everything.Length;

                result.Append(everything[idx]);
            }

            return result.ToString();
        }
        catch
        {
            return "";
        }
    }

    public string RandomHypeSquad()
    {
        try
        {
            byte[] data = new byte[4 * 1];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }

            StringBuilder result = new StringBuilder(1);

            for (int i = 0; i < 1; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % hypesquad.Length;

                result.Append(hypesquad[idx]);
            }

            return result.ToString();
        }
        catch
        {
            return "";
        }
    }
}