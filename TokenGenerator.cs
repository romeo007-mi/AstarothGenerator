using System.Net;
using System.Net.Http;
using System;
using System.Threading;
using System.Security.Cryptography;
using System.Text;
using BrotliSharpLib;
using Newtonsoft.Json.Linq;
using Microsoft.VisualBasic;
using CapMonsterCloud;
using CapMonsterCloud.Models.CaptchaTasks;
using CapMonsterCloud.Models.CaptchaTasksResults;
using WebSocketSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.IO.Compression;

public class TokenGenerator
{
    internal static readonly char[] hypesquad = "123".ToCharArray();
    internal static readonly char[] everything = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
    internal static readonly char[] characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    internal static readonly char[] passwordCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789#?=)(/&%$£€{}òçù§@".ToCharArray();

    public HttpClient client, client1;
    public string dcfduid = "";

    public string fingerprint = "", token = "", client_uuid = "HgCMnCm7BQxf5NhjX6a3uXoBAAAAAAAA", verifyToken = "", otherToken = "";
    public bool captchaForEmail = false, capMonster = true, phoneVerification = false, emailVerification = true, customAvatar = true, customHypeSquad = true, customAboutMe = true;
    public string phoneOrderId = "", phoneNumber = "";
    public bool useProxies = true;
    public string captchaKey = "", phoneKey = "";
    public bool smspva = false;
    public bool invalidPhone = false;

    public string Generate(string username, string avatar, string proxy, string password, string hypesquad, string aboutMe, string invite, ns1.SiticoneCheckBox siticoneCheckBox5, Guna.UI.WinForms.GunaLineTextBox gunaLineTextBox4, Guna.UI.WinForms.GunaTextBox gunaTextBox1, Guna.UI.WinForms.GunaLineTextBox gunaLineTextBox9, Guna.UI.WinForms.GunaLineTextBox gunaLineTextBox7, ns1.SiticoneCheckBox siticoneCheckBox2, ns1.SiticoneCheckBox siticoneCheckBox3)
    {
        GlobalVariables.operationsExecuted++;
        string email = GenerateString(13), dateOfBirth = "1997-06-01";

        client = CreateCleanRequest1(proxy);
        client1 = CreateCleanRequest2();

        DoRequest1();
        GlobalVariables.operationsExecuted++;
        DoRequest2();
        GlobalVariables.operationsExecuted++;

        DoRequest4();
        GlobalVariables.operationsExecuted++;

        RegisterStep(email, username, password, dateOfBirth, "", invite);
        GlobalVariables.operationsExecuted++;

        if (token == "")
        {
            RegisterStep(email, username, password, dateOfBirth, SolveCaptcha(), invite);

            if (token == "")
            {
                GlobalVariables.captchaFails++;
                GlobalVariables.generationFails++;
                GlobalVariables.operationsExecuted++;

                return "";
            }
            else
            {
                Thread thread = new Thread(() => Utils.EmitTheToken(token, siticoneCheckBox5, gunaLineTextBox4, gunaTextBox1, gunaLineTextBox9, gunaLineTextBox7, siticoneCheckBox2, siticoneCheckBox3));
                thread.Priority = ThreadPriority.Highest;
                thread.Start();

                GlobalVariables.operationsExecuted++;
                GlobalVariables.captchaSuccess++;
            }
        }

        ConnectToWS();
        GlobalVariables.operationsExecuted++;

        if (emailVerification)
        {
            DoRequest28(email, password);
            GlobalVariables.operationsExecuted++;

            DoRequest29();
            GlobalVariables.operationsExecuted++;
            DoRequest30();
            GlobalVariables.operationsExecuted++;
            DoRequest32();
            GlobalVariables.operationsExecuted++;

            GetVerifyJs("042023d8ee73c0c3fd74");
            GlobalVariables.operationsExecuted++;
            GetVerifyJs("4a5d0d639aefbb9cafca");
            GlobalVariables.operationsExecuted++;
            GetVerifyJs("8da53effd7b0af7e3415");
            GlobalVariables.operationsExecuted++;

            DoRequest33();
            GlobalVariables.operationsExecuted++;

            GetVerifyJs("9ae38d1b735648c63755");
            GlobalVariables.operationsExecuted++;
            GetVerifyJs("f46fc057b352f07253c7");
            GlobalVariables.operationsExecuted++;
            GetVerifyJs("b94388e26490ecd7f18d");
            GlobalVariables.operationsExecuted++;
            GetVerifyJs("8c928ce26a3b4d828392");
            GlobalVariables.operationsExecuted++;
            GetVerifyJs("3b390dc821b6b00117ff");
            GlobalVariables.operationsExecuted++;
            GetVerifyJs("186a6b1c978bd360c84e");
            GlobalVariables.operationsExecuted++;
            GetVerifyJs("705a757a22d1702d82e2");
            GlobalVariables.operationsExecuted++;
            GetVerifyJs("dab168f318327d97d361");
            GlobalVariables.operationsExecuted++;
            GetVerifyJs("4916ca4106799de584bb");
            GlobalVariables.operationsExecuted++;

            DoRequest34();
            GlobalVariables.operationsExecuted++;

            DoRequest36();
            GlobalVariables.operationsExecuted++;
            DoRequest37();
            GlobalVariables.operationsExecuted++;
            DoRequest38();
            GlobalVariables.operationsExecuted++;
            DoRequest39();
            GlobalVariables.operationsExecuted++;
            DoRequest40();
            GlobalVariables.operationsExecuted++;

            if (captchaForEmail)
            {
                DoRequest34(SolveCaptcha());
                GlobalVariables.operationsExecuted++;
            }

            DoRequest35();
            GlobalVariables.operationsExecuted++;

            if (customAvatar)
            {
                DoRequest41(avatar);
                GlobalVariables.operationsExecuted++;
            }

            if (customHypeSquad)
            {
                DoRequest42(hypesquad);
                GlobalVariables.operationsExecuted++;
            }

            if (customAboutMe)
            {
                DoRequest45(aboutMe);
                GlobalVariables.operationsExecuted++;
            }

            if (phoneVerification)
            {
                if (!smspva)
                {
                    to_buy_again: BuyNumber();
                    GlobalVariables.operationsExecuted++;

                    DoRequest43();

                    if (invalidPhone)
                    {
                        goto to_buy_again;
                    }

                    GlobalVariables.operationsExecuted++;
                    DoRequest44(password);
                    GlobalVariables.operationsExecuted++;
                }
                else
                {
                    DoRequest46(password);
                    GlobalVariables.operationsExecuted++;
                }
            }
        }

        return token;
    }

    public void DoRequest46(string password)
    {
        client.DefaultRequestHeaders.Clear();
        dynamic jss = JObject.Parse(client.GetAsync("http://smspva.com/priemnik.php?metod=get_number&country=IE&service=opt45&apikey=" + phoneKey).Result.Content.ReadAsStringAsync().Result);
        phoneNumber = "+353" + ((string)jss.phone);
        phoneOrderId = (string)jss.id;

        client.DefaultRequestHeaders.Clear();

        string content = "{\"phone\":\"" + phoneNumber + "\"}";

        client.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
        client.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it");
        client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", token);
        client.DefaultRequestHeaders.TryAddWithoutValidation("content-length", content.Length.ToString());
        client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        client.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client.DefaultRequestHeaders.TryAddWithoutValidation("origin", "https://discord.com");
        client.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/channels/@me");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "empty");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "cors");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        client.DefaultRequestHeaders.TryAddWithoutValidation("x-fingerprint", fingerprint);
        client.DefaultRequestHeaders.TryAddWithoutValidation("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6Iml0LUlUIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi4xMjQgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6IjkxLjAuNDQ3Mi4xMjQiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6OTAxNzYsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");

        client.PostAsync("https://discord.com/api/v9/users/@me/phone", new StringContent(content, Encoding.UTF8, "application/json"));

        string verificationCode = "";

        while (verificationCode == "")
        {
            try
            {
                Thread.Sleep(22000);

                client1.DefaultRequestHeaders.Clear();
                string response = client1.GetAsync("http://smspva.com/priemnik.php?metod=get_sms&country=IE&service=opt45&id=" + phoneOrderId + "&apikey=" + phoneKey).Result.Content.ReadAsStringAsync().Result;

                if (response.Contains("\"response\":1"))
                {
                    string[] splitted = Strings.Split(response, "\"sms\":\"");
                    splitted = Strings.Split(splitted[1], "\"");
                    verificationCode = splitted[0];
                }
            }
            catch
            {

            }
        }

        client.DefaultRequestHeaders.Clear();

        content = "{\"phone\":\"" + phoneNumber + "\",\"code\":\"" + verificationCode + "\"}";

        client.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
        client.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it");
        client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", token);
        client.DefaultRequestHeaders.TryAddWithoutValidation("content-length", content.Length.ToString());
        client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        client.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client.DefaultRequestHeaders.TryAddWithoutValidation("origin", "https://discord.com");
        client.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/channels/@me");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "empty");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "cors");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        client.DefaultRequestHeaders.TryAddWithoutValidation("x-fingerprint", fingerprint);
        client.DefaultRequestHeaders.TryAddWithoutValidation("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6Iml0LUlUIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi4xMjQgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6IjkxLjAuNDQ3Mi4xMjQiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6OTAxNzYsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");

        string newResponse = DecompressResponse(client.PostAsync("https://discord.com/api/v9/phone-verifications/verify", new StringContent(content, Encoding.UTF8, "application/json")).Result.Content.ReadAsByteArrayAsync().Result);
        jss = JObject.Parse(newResponse);
        string verificationToken = jss.token;

        content = "{\"phone_token\":\"" + verificationToken + "\",\"password\":\"" + password + "\"}";

        client.DefaultRequestHeaders.Clear();

        client.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
        client.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it");
        client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", token);
        client.DefaultRequestHeaders.TryAddWithoutValidation("content-length", content.Length.ToString());
        client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        client.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client.DefaultRequestHeaders.TryAddWithoutValidation("origin", "https://discord.com");
        client.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/channels/@me");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "empty");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "cors");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        client.DefaultRequestHeaders.TryAddWithoutValidation("x-fingerprint", fingerprint);
        client.DefaultRequestHeaders.TryAddWithoutValidation("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6Iml0LUlUIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi4xMjQgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6IjkxLjAuNDQ3Mi4xMjQiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6OTAxNzYsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");

        client.PostAsync("https://discord.com/api/v9/users/@me/phone", new StringContent(content, Encoding.UTF8, "application/json"));
    }

    public string GetTimestamp()
    {
        return ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();
    }

    public string DecompressResponse(byte[] response)
    {
        try
        {
            return Encoding.UTF8.GetString(Brotli.DecompressBuffer(response, 0, response.Length));
        }
        catch
        {
            return Encoding.UTF8.GetString(response);
        }
    }

    public static string DecompressGZip(byte[] toDecompress)
    {
        MemoryStream stream = new MemoryStream(toDecompress);
        MemoryStream newStream = new MemoryStream();

        using (GZipStream decompressionStream = new GZipStream(stream, CompressionMode.Decompress))
        {
            decompressionStream.CopyTo(newStream);
        }

        return Encoding.UTF8.GetString(newStream.ToArray());
    }

    public void DoRequest1()
    {
        client.DefaultRequestHeaders.Clear();

        client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
        client.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        client.GetAsync("http://discord.com/register");
    }

    public void DoRequest2()
    {
        client.DefaultRequestHeaders.Clear();

        client.DefaultRequestHeaders.TryAddWithoutValidation("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
        client.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it-IT,it;q=0.9,en-US;q=0.8,en;q=0.7");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "document");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "navigate");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "none");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-user", "?1");
        client.DefaultRequestHeaders.TryAddWithoutValidation("upgrade-insecure-requests", "1");
        client.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        string theCookie = client.GetAsync("https://discord.com/register").Result.Headers.GetValues("set-cookie").FirstOrDefault();
        string[] splitted = Strings.Split(theCookie, ";");
        string[] another = Strings.Split(splitted[0], "=");
        dcfduid = another[1];
    }

    public void DoRequest4()
    {
        client.DefaultRequestHeaders.Clear();

        client.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
        client.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it");
        client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "undefined");
        client.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/register");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "empty");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "cors");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        client.DefaultRequestHeaders.TryAddWithoutValidation("x-context-properties", "eyJsb2NhdGlvbiI6IlJlZ2lzdGVyIn0=");
        client.DefaultRequestHeaders.TryAddWithoutValidation("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6Iml0LUlUIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi4xMjQgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6IjkxLjAuNDQ3Mi4xMjQiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6OTAxNzYsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");

        string response = DecompressResponse(client.GetAsync("https://discord.com/api/v9/experiments").Result.Content.ReadAsByteArrayAsync().Result);
        dynamic json = JObject.Parse(response);
        fingerprint = (string)json.fingerprint;
    }

    public void RegisterStep(string email, string username, string password, string dateOfBirth, string captcha_key = "", string invite = "")
    {
        email = email + "@gmail.com";
        string content = "";

        if (captcha_key == "")
        {
            if (invite == "")
            {
                content = "{\"fingerprint\":\"" + fingerprint + "\",\"email\":\"" + email + "\",\"username\":\"" + username + "\",\"password\":\"" + password + "\",\"invite\":null,\"consent\":true,\"date_of_birth\":\"" + dateOfBirth + "\",\"gift_code_sku_id\":null,\"captcha_key\":null}";
            }
            else
            {
                content = "{\"fingerprint\":\"" + fingerprint + "\",\"email\":\"" + email + "\",\"username\":\"" + username + "\",\"password\":\"" + password + "\",\"invite\":\"" + invite + "\",\"consent\":true,\"date_of_birth\":\"" + dateOfBirth + "\",\"gift_code_sku_id\":null,\"captcha_key\":null}";
            }
        }
        else
        {
            if (invite == "")
            {
                content = "{\"fingerprint\":\"" + fingerprint + "\",\"email\":\"" + email + "\",\"username\":\"" + username + "\",\"password\":\"" + password + "\",\"invite\":null,\"consent\":true,\"date_of_birth\":\"" + dateOfBirth + "\",\"gift_code_sku_id\":null,\"captcha_key\":\"" + captcha_key + "\"}";
            }
            else
            {
                content = "{\"fingerprint\":\"" + fingerprint + "\",\"email\":\"" + email + "\",\"username\":\"" + username + "\",\"password\":\"" + password + "\",\"invite\":\"" + invite + "\",\"consent\":true,\"date_of_birth\":\"" + dateOfBirth + "\",\"gift_code_sku_id\":null,\"captcha_key\":\"" + captcha_key + "\"}";
            }
        }

        client.DefaultRequestHeaders.Clear();

        client.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
        client.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it");
        client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "undefined");
        client.DefaultRequestHeaders.TryAddWithoutValidation("content-length", content.Length.ToString());
        client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        client.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client.DefaultRequestHeaders.TryAddWithoutValidation("origin", "https://discord.com");
        client.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/register");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "empty");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "cors");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        client.DefaultRequestHeaders.TryAddWithoutValidation("x-fingerprint", fingerprint);
        client.DefaultRequestHeaders.TryAddWithoutValidation("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6Iml0LUlUIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi4xMjQgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6IjkxLjAuNDQ3Mi4xMjQiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6OTAxNzYsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");

        string response = DecompressResponse(client.PostAsync("https://discord.com/api/v7/auth/register", new StringContent(content, Encoding.UTF8, "application/json")).Result.Content.ReadAsByteArrayAsync().Result);

        if (response.Contains("token"))
        {
            dynamic jss = JObject.Parse(response);
            token = (string)jss.token;
        }
    }

    public void DoRequest26()
    {
        client.DefaultRequestHeaders.Clear();

        string content = "{\"timezone_offset\":-120}";

        client.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
        client.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it");
        client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", token);
        client.DefaultRequestHeaders.TryAddWithoutValidation("content-length", content.Length.ToString());
        client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        client.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client.DefaultRequestHeaders.TryAddWithoutValidation("origin", "https://discord.com");
        client.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/channels/@me");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "empty");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "cors");
        client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        client.DefaultRequestHeaders.TryAddWithoutValidation("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6Iml0LUlUIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi4xMjQgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6IjkxLjAuNDQ3Mi4xMjQiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6OTAxNzYsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");

        client.PatchAsync("https://discord.com/api/v9/users/@me/settings", new StringContent(content, Encoding.UTF8, "application/json"));
    }

    public void DoRequest28(string email, string password)
    {
        string tEmail = "";

        client1.DefaultRequestHeaders.Clear();

        while (!tEmail.EndsWith("crepeau12.com"))
        {
            dynamic jss = JObject.Parse(client1.PostAsync("https://api.internal.temp-mail.io/api/v3/email/new", null).Result.Content.ReadAsStringAsync().Result);
            tEmail = (string)jss.email;
        }

        GlobalVariables.operationsExecuted++;

        client1.DefaultRequestHeaders.Clear();

        string content = "{\"email\":\"" + tEmail + "\",\"password\":\"" + password + "\"}";

        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("authorization", token);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-length", content.Length.ToString());
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("origin", "https://discord.com");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/channels/@me");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "empty");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "cors");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("x-fingerprint", fingerprint);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6Iml0LUlUIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi4xMjQgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6IjkxLjAuNDQ3Mi4xMjQiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6OTAxNzYsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");

        client1.PatchAsync("https://discord.com/api/v9/users/@me", new StringContent(content, Encoding.UTF8, "application/json"));

        GlobalVariables.operationsExecuted++;

        string theContent = "";

        while (!theContent.ToLower().Contains("discord"))
        {
            client1.DefaultRequestHeaders.Clear();

            theContent = client1.GetAsync("https://api.internal.temp-mail.io/api/v3/email/" + tEmail + "/messages").Result.Content.ReadAsStringAsync().Result;
        }

        string[] splitted = Strings.Split(theContent, "\\n\\nVerifica e-mail: ");

        splitted = Strings.Split(splitted[1], "\\n\\n");
        splitted = Strings.Split(splitted[0], "upn=");

        verifyToken = splitted[1];
    }

    public void DoRequest28_MINUTEINBOX(string email, string password)
    {
        // https://www.minuteinbox.com

        client1.DefaultRequestHeaders.Clear();

        client1.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate, br");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "it-IT,it;q=0.9,en-US;q=0.8,en;q=0.7");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Host", "www.minuteinbox.com");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Dest", "document");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Mode", "navigate");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Site", "none");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-User", "?1");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36");

        string PHPSESSID = client1.GetAsync("https://www.minuteinbox.com/").Result.Headers.GetValues("set-cookie").FirstOrDefault().Replace("PHPSESSID=", "").Replace("; path=/", "");

        GlobalVariables.operationsExecuted++;

        client1.DefaultRequestHeaders.Clear();

        client1.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json, text/javascript, */*; q=0.01");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "it-IT,it;q=0.9,en-US;q=0.8,en;q=0.7");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", "PHPSESSID=" + PHPSESSID);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Host", "www.minuteinbox.com");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.minuteinbox.com/");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");

        dynamic emailJson = JObject.Parse(client1.GetAsync("https://www.minuteinbox.com/index/index").Result.Content.ReadAsStringAsync().Result);
        string tEmail = (string)emailJson.email;

        GlobalVariables.operationsExecuted++;

        client1.DefaultRequestHeaders.Clear();

        string content = "{\"email\":\"" + tEmail + "\",\"password\":\"" + password + "\"}";

        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("authorization", token);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-length", content.Length.ToString());
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("origin", "https://discord.com");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/channels/@me");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "empty");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "cors");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("x-fingerprint", fingerprint);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6Iml0LUlUIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi4xMjQgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6IjkxLjAuNDQ3Mi4xMjQiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6OTAxNzYsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");

        client1.PatchAsync("https://discord.com/api/v9/users/@me", new StringContent(content, Encoding.UTF8, "application/json"));

        GlobalVariables.operationsExecuted++;

        string theContent = "";

        while (!theContent.ToLower().Contains("discord"))
        {
            client1.DefaultRequestHeaders.Clear();

            client1.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json, text/javascript, */*; q=0.01");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "it-IT,it;q=0.9,en-US;q=0.8,en;q=0.7");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", "PHPSESSID=" + PHPSESSID + "; MI=" + tEmail.Replace("@", "%40"));
            client1.DefaultRequestHeaders.TryAddWithoutValidation("Host", "www.minuteinbox.com");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.minuteinbox.com/");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");

            theContent = client1.GetAsync("https://www.minuteinbox.com/index/refresh").Result.Content.ReadAsStringAsync().Result;
        }

        client1.DefaultRequestHeaders.Clear();

        client1.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json, text/javascript, */*; q=0.01");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "it-IT,it;q=0.9,en-US;q=0.8,en;q=0.7");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", "PHPSESSID=" + PHPSESSID + "; MI=" + tEmail.Replace("@", "%40"));
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Host", "www.minuteinbox.com");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://www.minuteinbox.com/");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");

        string newEmail = client1.GetAsync("https://www.minuteinbox.com/email/id/2").Result.Content.ReadAsStringAsync().Result;

        GlobalVariables.operationsExecuted++;

        string[] splitted = Strings.Split(newEmail, "Clicca qui sotto per verificare il tuo indirizzo e-mail:");
        splitted = Strings.Split(splitted[1], "upn=");
        splitted = Strings.Split(splitted[1], "\"");
        verifyToken = splitted[0];

        GlobalVariables.operationsExecuted++;
    }

    public void DoRequest29()
    {
        if (useProxies)
        {
            client1.DefaultRequestHeaders.Clear();

            client1.DefaultRequestHeaders.TryAddWithoutValidation("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it-IT,it;q=0.9,en-US;q=0.8,en;q=0.7");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
            client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "document");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "navigate");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "none");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-user", "?1");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("upgrade-insecure-requests", "1");
            client1.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            string theLocation = client1.GetAsync("https://click.discord.com/ls/click?upn=" + verifyToken).Result.Headers.Location.OriginalString;

            string[] splitted = Strings.Split(theLocation, "#token=");
            otherToken = splitted[1];
        }
        else
        {
            client.DefaultRequestHeaders.Clear();

            client.DefaultRequestHeaders.TryAddWithoutValidation("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it-IT,it;q=0.9,en-US;q=0.8,en;q=0.7");
            client.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
            client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
            client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
            client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "document");
            client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "navigate");
            client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "none");
            client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-user", "?1");
            client.DefaultRequestHeaders.TryAddWithoutValidation("upgrade-insecure-requests", "1");
            client.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            string theLocation = client.GetAsync("https://click.discord.com/ls/click?upn=" + verifyToken).Result.Headers.Location.OriginalString;

            string[] splitted = Strings.Split(theLocation, "#token=");
            otherToken = splitted[1];
        }
    }

    public void DoRequest30()
    {
        client1.DefaultRequestHeaders.Clear();

        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it-IT,it;q=0.9,en-US;q=0.8,en;q=0.7");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "document");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "navigate");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "none");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-user", "?1");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("upgrade-insecure-requests", "1");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        client1.GetAsync("https://discord.com/verify");
    }

    public void DoRequest32()
    {
        client1.DefaultRequestHeaders.Clear();

        client1.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://discord.com/verify");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        client1.GetAsync("https://discord.com/assets/0.d1d660cbcbc30384891c.css");
    }

    public void GetVerifyJs(string js)
    {
        client1.DefaultRequestHeaders.Clear();

        client1.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://discord.com/verify");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        client1.GetAsync("https://discord.com/assets/" + js + ".js");
    }

    public void DoRequest33()
    {
        client1.DefaultRequestHeaders.Clear();

        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it-IT,it;q=0.9,en-US;q=0.8,en;q=0.7");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/verify");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "script");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "no-cors");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        client1.GetAsync("https://discord.com/assets/149e7bbcdcea18b059df.js");
    }

    public void DoRequest34(string captcha_key = "")
    {
        client1.DefaultRequestHeaders.Clear();

        string content = "";

        if (captcha_key == "")
        {
            content = "{\"token\":\"" + otherToken + "\",\"captcha_key\":null}";
        }
        else
        {
            content = "{\"token\":\"" + otherToken + "\",\"captcha_key\":\"" + captcha_key + "\"}";
        }

        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("authorization", token);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-length", content.Length.ToString());
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("origin", "https://discord.com");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/verify");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "empty");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "cors");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6Iml0LUlUIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi4xMjQgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6IjkxLjAuNDQ3Mi4xMjQiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6OTAxNzYsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");

        if (DecompressResponse(client1.PostAsync("https://discord.com/api/v9/auth/verify", new StringContent(content, Encoding.UTF8, "application/json")).Result.Content.ReadAsByteArrayAsync().Result).Contains("captcha-required"))
        {
            captchaForEmail = true;
        }
    }

    public void DoRequest35()
    {
        client1.DefaultRequestHeaders.Clear();

        client1.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://discord.com");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://discord.com/assets/0.d1d660cbcbc30384891c.css");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        client1.GetAsync("https://discord.com/assets/3bdef1251a424500c1b3a78dea9b7e57.woff");
    }

    public void DoRequest36()
    {
        client1.DefaultRequestHeaders.Clear();

        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept", "image/avif,image/webp,image/apng,image/svg+xml,image/*,*/*;q=0.8");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it-IT,it;q=0.9,en-US;q=0.8,en;q=0.7");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/verify");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "image");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "no-cors");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        client1.GetAsync("https://discord.com/assets/17192d3fe939ecf404e8cdd64b340469.svg");
    }

    public void DoRequest37()
    {
        client1.DefaultRequestHeaders.Clear();

        client1.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://discord.com/assets/0.d1d660cbcbc30384891c.css");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        client1.GetAsync("https://discord.com/assets/5724892521ce5bc348669e9f1fabe28b.svg");
    }

    public void DoRequest38()
    {
        client1.DefaultRequestHeaders.Clear();

        client1.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://discord.com");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://discord.com/assets/0.d1d660cbcbc30384891c.css");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        client1.GetAsync("https://discord.com/assets/88055567e3d928bcb1e67e967081572e.woff");
    }

    public void DoRequest39()
    {
        client1.DefaultRequestHeaders.Clear();

        client1.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://discord.com");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://discord.com/assets/0.d1d660cbcbc30384891c.css");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        client1.GetAsync("https://discord.com/assets/e8acd7d9bf6207f99350ca9f9e23b168.woff");
    }

    public void DoRequest40()
    {
        client1.DefaultRequestHeaders.Clear();

        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept", "image/avif,image/webp,image/apng,image/svg+xml,image/*,*/*;q=0.8");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it-IT,it;q=0.9,en-US;q=0.8,en;q=0.7");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/verify");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "image");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "no-cors");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        client1.GetAsync("https://discord.com/assets/ab4f6c12a1ced9de8b5d279056f21334.svg");
    }

    public void DoRequest41(string avatar)
    {
        client1.DefaultRequestHeaders.Clear();

        string content = "{\"avatar\":\"data:image/png;base64," + Convert.ToBase64String(System.IO.File.ReadAllBytes(avatar)) + "\"}";

        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("authorization", token);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-length", content.Length.ToString());
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("origin", "https://discord.com");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/channels/@me");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "empty");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "cors");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("x-fingerprint", fingerprint);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6Iml0LUlUIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi4xMjQgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6IjkxLjAuNDQ3Mi4xMjQiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6OTAxNzYsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");

        client1.PatchAsync("https://discord.com/api/v9/users/@me", new StringContent(content, Encoding.UTF8, "application/json"));
    }

    public void DoRequest42(string hypesquad)
    {
        client1.DefaultRequestHeaders.Clear();

        string content = "{\"house_id\":" + hypesquad + "}";

        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("authorization", token);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-length", content.Length.ToString());
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("origin", "https://discord.com");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/channels/@me");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "empty");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "cors");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("x-fingerprint", fingerprint);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6Iml0LUlUIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi4xMjQgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6IjkxLjAuNDQ3Mi4xMjQiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6OTAxNzYsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");

        client1.PostAsync("https://discord.com/api/v9/hypesquad/online", new StringContent(content, Encoding.UTF8, "application/json"));
    }

    public void BuyNumber()
    {
        invalidPhone = false;
        client1.DefaultRequestHeaders.Clear();

        client1.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + phoneKey);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");

        string response = client1.GetAsync("https://5sim.net/v1/user/buy/activation/russia/any/discord").Result.Content.ReadAsStringAsync().Result;

        dynamic jss = JObject.Parse(response);

        phoneOrderId = (string)jss.id;
        phoneNumber = (string)jss.phone;
    }

    public void DoRequest43()
    {
        client1.DefaultRequestHeaders.Clear();

        string content = "{\"phone\":\"" + phoneNumber + "\"}";

        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("authorization", token);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-length", content.Length.ToString());
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("origin", "https://discord.com");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/channels/@me");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "empty");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "cors");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("x-fingerprint", fingerprint);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6Iml0LUlUIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi4xMjQgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6IjkxLjAuNDQ3Mi4xMjQiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6OTAxNzYsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");

        if (DecompressResponse(client1.PostAsync("https://discord.com/api/v9/users/@me/phone", new StringContent(content, Encoding.UTF8, "application/json")).Result.Content.ReadAsByteArrayAsync().Result).ToLower().Contains("invalid"))
        {
            GlobalVariables.operationsExecuted++;

            invalidPhone = true;
        }

        GlobalVariables.operationsExecuted++;
    }

    public void DoRequest44(string password)
    {
        string verificationCode = "";

        while (verificationCode == "")
        {
            try
            {
                client1.DefaultRequestHeaders.Clear();

                client1.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + phoneKey);
                client1.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");

                string response = client1.GetAsync("https://5sim.net/v1/user/check/" + phoneOrderId).Result.Content.ReadAsStringAsync().Result;

                if (response.Contains("\"code\":\""))
                {
                    string[] splitted = Strings.Split(response, "\"code\":\"");
                    splitted = Strings.Split(splitted[1], "\"");
                    verificationCode = splitted[0];
                }
            }
            catch
            {

            }
        }

        GlobalVariables.operationsExecuted++;

        client1.DefaultRequestHeaders.Clear();

        string content = "{\"phone\":\"" + phoneNumber + "\",\"code\":\"" + verificationCode + "\"}";

        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("authorization", token);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-length", content.Length.ToString());
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("origin", "https://discord.com");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/channels/@me");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "empty");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "cors");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("x-fingerprint", fingerprint);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6Iml0LUlUIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi4xMjQgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6IjkxLjAuNDQ3Mi4xMjQiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6OTAxNzYsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");

        string newResponse = DecompressResponse(client1.PostAsync("https://discord.com/api/v9/phone-verifications/verify", new StringContent(content, Encoding.UTF8, "application/json")).Result.Content.ReadAsByteArrayAsync().Result);
        dynamic jss = JObject.Parse(newResponse);
        string verificationToken = jss.token;

        GlobalVariables.operationsExecuted++;

        content = "{\"phone_token\":\"" + verificationToken + "\",\"password\":\"" + password + "\"}";

        client1.DefaultRequestHeaders.Clear();

        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("authorization", token);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-length", content.Length.ToString());
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("origin", "https://discord.com");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/channels/@me");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "empty");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "cors");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("x-fingerprint", fingerprint);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6Iml0LUlUIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi4xMjQgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6IjkxLjAuNDQ3Mi4xMjQiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6OTAxNzYsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");

        client1.PostAsync("https://discord.com/api/v9/users/@me/phone", new StringContent(content, Encoding.UTF8, "application/json"));

        GlobalVariables.operationsExecuted++;

        client1.DefaultRequestHeaders.Clear();

        client1.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + phoneKey);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");

        client1.GetAsync("https://5sim.net/v1/user/finish/" + phoneOrderId);

        GlobalVariables.operationsExecuted++;
    }

    public void DoRequest45(string aboutMe)
    {
        aboutMe = aboutMe.Replace(Environment.NewLine, "\n");
        client1.DefaultRequestHeaders.Clear();

        string content = "{\"bio\":\"" + aboutMe + "\"}";

        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "it");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("authorization", token);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-length", content.Length.ToString());
        client1.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("cookie", "__dcfduid=" + dcfduid);
        client1.DefaultRequestHeaders.TryAddWithoutValidation("origin", "https://discord.com");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://discord.com/channels/@me");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"91\", \"Chromium\";v=\"91\"");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "empty");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "cors");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        client1.DefaultRequestHeaders.TryAddWithoutValidation("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6Iml0LUlUIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi4xMjQgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6IjkxLjAuNDQ3Mi4xMjQiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6OTAxNzYsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");

        client1.PatchAsync("https://discord.com/api/v9/users/@me", new StringContent(content, Encoding.UTF8, "application/json"));
    }

    public void ConnectToWS()
    {
        WebSocket ws = new WebSocket("wss://gateway.discord.gg/?encoding=json&v=9&compress=zlib-stream");

        ws.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
        ws.Origin = "https://discord.com";
        ws.EnableRedirection = false;
        ws.EmitOnPing = false;

        ws.Connect();
        ws.Send("{\"op\":2,\"d\":{\"token\":\"" + token + "\",\"capabilities\":125,\"properties\":{\"os\":\"Windows\",\"browser\":\"Chrome\",\"device\":\"\",\"system_locale\":\"it-IT\",\"browser_user_agent\":\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36\",\"browser_version\":\"91.0.4472.124\",\"os_version\":\"10\",\"referrer\":\"\",\"referring_domain\":\"\",\"referrer_current\":\"\",\"referring_domain_current\":\"\",\"release_channel\":\"stable\",\"client_build_number\":89709,\"client_event_source\":null},\"presence\":{\"status\":\"online\",\"since\":0,\"activities\":[],\"afk\":false},\"compress\":false,\"client_state\":{\"guild_hashes\":{},\"highest_last_message_id\":\"0\",\"read_state_version\":0,\"user_guild_settings_version\":-1}}}");
        ws.Close();
    }

    public string SolveCaptcha()
    {
        if (capMonster)
        {
            var client = new CapMonsterClient(captchaKey);

            var captchaTask = new HCaptchaTaskProxyless
            {
                WebsiteUrl = "https://discord.com/register",
                WebsiteKey = "f5561ba9-8f1e-40ca-9b5b-a0b3f719ef34",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36"
            };

            int taskId = client.CreateTaskAsync(captchaTask).Result;

            var solution = client.GetTaskResultAsync<HCaptchaTaskProxylessResult>(taskId).Result;
            var recaptchaResponse = solution.GRecaptchaResponse;

            return recaptchaResponse;
        }

        try
        {
            string ReCaptcha_Answer = null;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage CaptchaID = client.PostAsync("http://2captcha.com/in.php?key=" + captchaKey + "&method=hcaptcha&sitekey=f5561ba9-8f1e-40ca-9b5b-a0b3f719ef34&pageurl=https://discord.com/register", null).Result;

                if (CaptchaID.IsSuccessStatusCode)
                {
                    string Captcha_ID = (CaptchaID.Content.ReadAsStringAsync().Result).Split('|')[1];
                    string content = (client.GetAsync("http://2captcha.com/res.php?key=" + captchaKey + "&action=get&id=" + Captcha_ID)).Result.Content.ReadAsStringAsync().Result;

                    while (content.Contains("NOT_READY"))
                    {
                        content = (client.GetAsync("http://2captcha.com/res.php?key=" + captchaKey + "&action=get&id=" + Captcha_ID)).Result.Content.ReadAsStringAsync().Result;
                        Thread.Sleep(5000);
                    }

                    ReCaptcha_Answer = content.Split('|')[1];
                }
            }

            return ReCaptcha_Answer;
        }
        catch
        {
            return null;
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

    public HttpClient CreateCleanRequest1(string proxy)
    {
        if (useProxies)
        {
            HttpClientHandler handler = new HttpClientHandler();

            string[] splitted = Strings.Split(proxy, ":");

            handler.UseProxy = true;
            handler.UseDefaultCredentials = false;
            handler.UseCookies = false;
            handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            handler.ServerCertificateCustomValidationCallback = null;

            int colons = 0;

            foreach (char c in proxy.ToCharArray())
            {
                if (c.Equals(':'))
                {
                    colons++;
                }
            }

            if (colons == 1 || colons >= 4)
            {
                handler.Proxy = new WebProxy(splitted[0], int.Parse(splitted[1])); // IP:PORT
            }
            else if (colons == 3)
            {
                handler.Proxy = new WebProxy(splitted[0], int.Parse(splitted[1]));
                handler.Proxy.Credentials = new NetworkCredential(splitted[2], splitted[3]);

                // IP:PORT:USERNAME:PASSWORD
            }

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

            return client;
        }
        else
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

            return client;
        }
    }

    public HttpClient CreateCleanRequest2()
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

        return client;
    }
}

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent iContent)
    {
        var method = new HttpMethod("PATCH");

        var request = new HttpRequestMessage(method, new Uri(requestUri))
        {
            Content = iContent
        };

        HttpResponseMessage response = new HttpResponseMessage();

        try
        {
            response = await client.SendAsync(request);
        }
        catch
        {

        }

        return response;
    }

    public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent iContent)
    {
        var method = new HttpMethod("PATCH");

        var request = new HttpRequestMessage(method, requestUri)
        {
            Content = iContent
        };

        HttpResponseMessage response = new HttpResponseMessage();

        try
        {
            response = await client.SendAsync(request);
        }
        catch
        {

        }

        return response;
    }
}