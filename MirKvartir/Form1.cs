using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
using Newtonsoft;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace MirKvartir
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Load+= OnLoad;
            AllCountry();
           
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            Activate act = new Activate();
            if (!File.Exists("key.it") || File.ReadAllText("key.it") != act.GetKey())
            {
                act.ShowDialog();
                if (!act.flag)
                    Close();
                else
                    File.WriteAllText("key.it", act.GetKey());
            }

        }

        private void AllCountry()
        {
            int schet = 0;
            var all_excel = Directory.GetFiles("Листы адресов");
            foreach (string s in all_excel)
            {
                schet++;
                panel1.Controls.Add(new Label() { Text = Path.GetFileName(s).Split('.')[0], ForeColor = Color.Red, Visible = true, Left = 50, Width = 200, Top = 30 * schet });
                ExcelPackage package = new ExcelPackage(new FileInfo(s));
                ExcelWorksheet w = package.Workbook.Worksheets.First();
                for (int i = 1; i <= w.Dimension?.Rows; i++)
                {
                    if (!string.IsNullOrEmpty(w.Cells[i, 1].Text))
                    {
                        schet++;
                        panel1.Controls.Add(new CheckBox()
                        {
                            Text = w.Cells[i, 1].Text,
                            Visible = true,
                            Left = 10,
                            Top = 30 * schet
                        });
                    }
                }
                package.Dispose();
            }

        }

        CookieContainer cookies = new CookieContainer();
        private void Autorization(string password,string email)
        {
            Random random = new Random();
            int ran = random.Next(1, 9999999);
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create("http://api-cabinet.mirkvartir.ru//AccountApi/Login");
            request1.Method = "POST";
            request1.CookieContainer = cookies;
            request1.Accept = "application/json, application/xml, text/plain, text/html, image/jpeg, *.*";
            request1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
            request1.ContentType = "multipart/form-data; boundary=----WebKitFormBoundary" + ran + "";
            string data = "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"jsonArgumentsSection\"" + "\r\n" + "" + "\r\n" + "{\"loginUserModel\":{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"rememberMe\":true,\"daysTimeout\":100}}" + "\r\n" + "------WebKitFormBoundary" + ran + "--";
            byte[] bytedata = Encoding.ASCII.GetBytes(data);
            request1.ContentLength = bytedata.Length;
            Stream stream = request1.GetRequestStream();
            stream.Write(bytedata, 0, bytedata.Length);
            HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();

        }

        private Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
             thread = new Thread(() =>
            {
                try
                {

                Autorization(textBoxPass.Text, textBoxLogin.Text);
                List<string> countryCheck = new List<string>();
                foreach (Control control in panel1.Controls)
                {
                    switch (control)
                    {
                        case CheckBox check:
                            if(check.Checked) countryCheck.Add(check.Text);
                            break;
                    }
                }
                var all_excel = Directory.GetFiles("Листы адресов");
                Random rnd = new Random();
                foreach (string s in all_excel)
                {
                    ExcelPackage package = new ExcelPackage(new FileInfo(s));
                    ExcelWorksheet w = package.Workbook.Worksheets.First();
                    for (int i = 1; i <= w.Dimension?.Rows; i++)
                    {
                        if (!string.IsNullOrEmpty(w.Cells[i, 1].Text)&& countryCheck.Contains(w.Cells[i,1].Text))
                        {
                            var all_address = w.Cells[i, 2].Text.Split('|').ToList();
                                string adrkom =all_address[rnd.Next(0,all_address.Count-1)];
                                all_address.Remove(adrkom);
                                string adrodn = all_address[rnd.Next(0, all_address.Count-1)];
                                all_address.Remove(adrodn);
                                string adrdvu = all_address[rnd.Next(0, all_address.Count-1)];
                                all_address.Remove(adrdvu);
                                {
                                string[] opisaniyeRoom = Properties.Resources.descriptionRoom.Split('\n', '\r').Where(f=> !string.IsNullOrEmpty(f)).ToArray();
                                string[] opisaniyeOnes = Properties.Resources.descriptionOnes.Split('\n', '\r').Where(f => !string.IsNullOrEmpty(f)).ToArray();
                                var all_foldersRooms = Directory.GetDirectories(textBoxRooms.Text);
                                var all_foldersOnes = Directory.GetDirectories(textBoxOnes.Text);
                                var all_foldersTwo = Directory.GetDirectories(textBoxTwo.Text);
                                toolStripStatusLabel1.Text = w.Cells[i, 1].Text + " " + adrkom.Replace("_", " ");
                                toolStripStatusLabel2.Text = "Комната";
                                Start(all_foldersRooms[rnd.Next(0, all_foldersRooms.Length - 1)], opisaniyeRoom[rnd.Next(0, opisaniyeRoom.Length - 1)], 0, w.Cells[i, 1].Text + " " + adrkom.Replace("_", " "), maskedTextBoxNumber.Text);
                                    toolStripStatusLabel1.Text = w.Cells[i, 1].Text + " " + adrodn.Replace("_", " ");
                                    toolStripStatusLabel2.Text = "Однушка";
                                Start(all_foldersOnes[rnd.Next(0, all_foldersOnes.Length - 1)], opisaniyeOnes[rnd.Next(0, opisaniyeOnes.Length - 1)], 1, w.Cells[i, 1].Text + " " + adrodn.Replace("_", " "), maskedTextBoxNumber.Text);
                                    toolStripStatusLabel1.Text = w.Cells[i, 1].Text + " " + adrdvu.Replace("_", " ");
                                    toolStripStatusLabel2.Text = "Двушка";
                                Start(all_foldersTwo[rnd.Next(0, all_foldersTwo.Length - 1)], opisaniyeOnes[rnd.Next(0, opisaniyeOnes.Length - 1)], 2, w.Cells[i, 1].Text + " " + adrdvu.Replace("_", " "), maskedTextBoxNumber.Text);
                            }
                        }
                    }
                    package.Dispose();
                    button1.Invoke(new Action(() => button1.Enabled = true));
                }

                }
                catch
                {
                    MessageBox.Show("Упс! Проверьте правильность заполненных полей!");
                    button1.Invoke(new Action(() => button1.Enabled = true));
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }
        private void Start(string directory, string opisanie, int komnat, string adres, string phone)
        {
            try
            {
                toolStripStatusLabel3.Text = "Шаг 1";
                DirectoryInfo info = new DirectoryInfo(directory);
                DirectoryInfo[] dirs = info.GetDirectories();
                FileInfo[] files = info.GetFiles();
                Random random = new Random();
                string etag = Convert.ToString(random.Next(1, 3));
                string ploshad = "0", price = "0";
                switch (komnat)
                {
                    case 1:
                        ploshad = Convert.ToString(random.Next(30, 40));
                        price = "10000";
                        break;
                    case 2:
                        ploshad = Convert.ToString(random.Next(38, 56));
                        price = "15000";
                        break;
                    case 0:
                        ploshad = Convert.ToString(random.Next(16, 24));
                        price = "6000";
                        break;
                }
                random = new Random();
                int ran = random.Next(1, 9999999);
                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create("http://cabinet.mirkvartir.ru/offers/manual");
                request1.CookieContainer = cookies;
                request1.Headers["Accept-Language"] = "en-US,en;q=0.8,ru;q=0.6";
                request1.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                request1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
                request1.Headers["Upgrade-Insecure-Requests"] = "1";
                HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();
                StreamReader streamReader = new StreamReader(response1.GetResponseStream());
                string result = streamReader.ReadToEnd();
                string name = Regex.Match(result, "(?<=data-reactid=\"44\">).*?(?=</a></)").Value;
                request1 = (HttpWebRequest)WebRequest.Create("http://api-cabinet.mirkvartir.ru//EstateOffersApi/ListEstates");
                request1.Method = "POST";
                request1.CookieContainer = cookies;
                request1.Headers["Origin"] = "http://cabinet.mirkvartir.ru";
                request1.Accept = "application/json, application/xml, text/plain, text/html, image/jpeg, *.*";
                request1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
                request1.ContentType = "multipart/form-data; boundary=----WebKitFormBoundary" + ran + "";
                request1.Referer = "http://cabinet.mirkvartir.ru/offers/manual";
                request1.Headers["Accept-Encoding"] = " gzip, deflate";
                request1.Headers["Accept-Language"] = "en-US,en;q=0.8,ru;q=0.6";
                string data = "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"jsonArgumentsSection\"" + "\r\n" + "" + "\r\n" + "{\"query\":{\"estateAndListingTypes\":[],\"currentPage\":1}}" + "\r\n" + "------WebKitFormBoundary" + ran + "--";
                byte[] bytedata = Encoding.ASCII.GetBytes(data);
                request1.ContentLength = bytedata.Length;
                Stream stream = request1.GetRequestStream();
                stream.Write(bytedata, 0, bytedata.Length);
                response1 = (HttpWebResponse)request1.GetResponse();
                streamReader = new StreamReader(response1.GetResponseStream());
                result = streamReader.ReadToEnd();
                request1 = (HttpWebRequest)WebRequest.Create("http://cabinet.mirkvartir.ru/additem/");
                request1.CookieContainer = cookies;
                request1.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                request1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
                request1.Headers["Upgrade-Insecure-Requests"] = "1";
                request1.Headers["Accept-Language"] = "en-US,en;q=0.8,ru;q=0.6";
                request1.Referer = "http://cabinet.mirkvartir.ru/offers/manual";
                response1 = (HttpWebResponse)request1.GetResponse();
                streamReader = new StreamReader(response1.GetResponseStream());
                result = streamReader.ReadToEnd();
                string estateid = Regex.Match(result, "(?<=estateId\":\").*?(?=\",\")").Value;
                string adresurl = HttpUtility.UrlEncode(adres);
                request1 = (HttpWebRequest)WebRequest.Create("http://api.mirkvartir.me/api/geosuggest/wizardsuggest/?query="+adresurl);
                request1.Referer = "http://cabinet.mirkvartir.ru/additem/";
                request1.Headers["Origin"] = "http://cabinet.mirkvartir.ru";
                request1.Accept = "application/json, text/javascript, */*; q=0.01";
                request1.Headers["Accept-Language"] = "en-US,en;q=0.8,ru;q=0.6";
                response1 = (HttpWebResponse)request1.GetResponse();
                streamReader = new StreamReader(response1.GetResponseStream());
                result = streamReader.ReadToEnd();
                ModelsAddres.AllAddres[] addresFind = JsonConvert.DeserializeObject<ModelsAddres.AllAddres[]>(result);
               var locat= addresFind.Last();
                string LocalIdHouse = locat.SuggestItem.LocationId.LocalId.ToString();
                string LocalIdStreet =locat.Locations.FirstOrDefault(f => f.MetaType == 1)?.LocalId.ToString();
                string LocalIdTown = locat.Locations.FirstOrDefault(f => f.MetaType == 4)?.LocalId.ToString();
                string LocalIdRegion =  locat.Locations.FirstOrDefault(f => f.MetaType == 5)?.LocalId.ToString();
                string LocalIdCountry = locat.Locations.FirstOrDefault(f => f.MetaType == 0)?.LocalId.ToString();
                Models.RootObject rootObject = JsonConvert.DeserializeObject<Models.RootObject>(Properties.Resources.jsonText);
                rootObject.estateId = estateid;
                rootObject.house = Convert.ToInt32(LocalIdHouse);
                rootObject.region = Convert.ToInt32(LocalIdRegion);
                rootObject.street = Convert.ToInt32(LocalIdStreet);
                rootObject.town = Convert.ToInt32(LocalIdTown);
                rootObject.name = name;
                rootObject.price = price;
                rootObject.location.label = adres;
                rootObject.location.cleanLabel = adres;
                rootObject.location.value.Locations[0].LocalId = Convert.ToInt32(LocalIdHouse);
                rootObject.location.value.Locations[1].LocalId = Convert.ToInt32(LocalIdStreet);
                rootObject.location.value.Locations[2].LocalId = Convert.ToInt32(LocalIdTown);
                rootObject.location.value.Locations[3].LocalId = Convert.ToInt32(LocalIdRegion);
                rootObject.location.value.Locations[4].LocalId = Convert.ToInt32(LocalIdCountry);
                rootObject.location.value.SuggestItem.LocationFullName = adres;
                rootObject.location.value.SuggestItem.LocationFullNameReversed = adres;
                rootObject.location.value.SuggestItem.LocationId.LocalId = Convert.ToInt32(LocalIdHouse);
                rootObject.locationAuto.label = adres;
                rootObject.locationAuto.cleanLabel = adres;
                rootObject.locationAuto.value.Locations[0].LocalId = Convert.ToInt32(LocalIdHouse);
                rootObject.locationAuto.value.Locations[1].LocalId = Convert.ToInt32(LocalIdStreet);
                rootObject.locationAuto.value.Locations[2].LocalId = Convert.ToInt32(LocalIdTown);
                rootObject.locationAuto.value.Locations[3].LocalId = Convert.ToInt32(LocalIdRegion);
                rootObject.locationAuto.value.Locations[4].LocalId = Convert.ToInt32(LocalIdCountry);
                rootObject.locationAuto.value.SuggestItem.LocationFullName = adres;
                rootObject.locationAuto.value.SuggestItem.LocationId.LocalId = Convert.ToInt32(LocalIdHouse);
                rootObject.selectedLocation.label = adres;
                rootObject.selectedLocation.value[0] = "MK_House|" + LocalIdHouse;
                rootObject.selectedLocation.value[1] = "MK_Street|" + LocalIdStreet;
                rootObject.selectedLocation.value[2] = "MK_Town|" + LocalIdTown;
                rootObject.selectedLocation.value[3] = "MK_Region|" + LocalIdRegion;
                rootObject.selectedLocation.value[4] = "MK_Country|" + LocalIdCountry;
                rootObject.townsForLead[0] = Convert.ToInt32(LocalIdTown);
                data = JsonConvert.SerializeObject(rootObject);
                data = HttpUtility.UrlEncode(data);
                data = "viewModelJson=" + data + "&ws1_ba=Rent&tradetype_price=" + price + "&clientFee=&ws1_price=" + adresurl + "&ws1_price=&ws1_price=&ws1_price=&ws1_price=";
                request1 = (HttpWebRequest)WebRequest.Create("http://cabinet.mirkvartir.ru/additem/");
                request1.Method = "POST";
                request1.CookieContainer = cookies;
                request1.Headers["Accept-Encoding"] = " gzip, deflate";
                request1.Headers["Accept-Language"] = "en-US,en;q=0.8,ru;q=0.6";
                request1.Headers["Origin"] = "http://cabinet.mirkvartir.ru";
                request1.Headers["Upgrade-Insecure-Requests"] = "1";
                request1.Headers["Cache-Control"] = "max-age=0";
                request1.Referer = "http://cabinet.mirkvartir.ru/additem/";
                request1.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                request1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
                request1.ContentType = "application/x-www-form-urlencoded";
                request1.CookieContainer.Add(new Cookie("step1", "true", "/", "cabinet.mirkvartir.ru"));
                bytedata = Encoding.ASCII.GetBytes(data);
                request1.ContentLength = bytedata.Length;
                stream = request1.GetRequestStream();
                stream.Write(bytedata, 0, bytedata.Length);
                response1 = (HttpWebResponse)request1.GetResponse();
                streamReader = new StreamReader(response1.GetResponseStream());
                result = streamReader.ReadToEnd();
                Models1.RootObject rootObject1 = JsonConvert.DeserializeObject<Models1.RootObject>(Properties.Resources.jsonText1);
                rootObject1.area = ploshad;
                rootObject1.description = opisanie;
                rootObject1.floor = etag;
                rootObject1.studio = false;
                rootObject1.name = name;
                rootObject1.price = price;
                rootObject1.roomDescription = Convert.ToString(komnat);
                rootObject1.roomDescriptionName = Convert.ToString(komnat);
                rootObject1.selectedLocation.label = adres;
                rootObject1.estateId = estateid;
                rootObject1.house = Convert.ToInt32(LocalIdHouse);
                rootObject1.region = Convert.ToInt32(LocalIdRegion);
                rootObject1.street = Convert.ToInt32(LocalIdStreet);
                rootObject1.town = Convert.ToInt32(LocalIdTown);
                rootObject1.location.label = adres;
                rootObject1.location.cleanLabel = adres;
                rootObject1.location.value.Locations[0].LocalId = Convert.ToInt32(LocalIdHouse);
                rootObject1.location.value.Locations[1].LocalId = Convert.ToInt32(LocalIdStreet);
                rootObject1.location.value.Locations[2].LocalId = Convert.ToInt32(LocalIdTown);
                rootObject1.location.value.Locations[3].LocalId = Convert.ToInt32(LocalIdRegion);
                rootObject1.location.value.Locations[4].LocalId = Convert.ToInt32(LocalIdCountry);
                rootObject1.location.value.SuggestItem.LocationFullName = adres;
                rootObject1.location.value.SuggestItem.LocationId.LocalId = Convert.ToInt32(LocalIdHouse);
                rootObject1.locationAuto.label = adres;
                rootObject1.locationAuto.cleanLabel = adres;
                rootObject1.locationAuto.value.Locations[0].LocalId = Convert.ToInt32(LocalIdHouse);
                rootObject1.locationAuto.value.Locations[1].LocalId = Convert.ToInt32(LocalIdStreet);
                rootObject1.locationAuto.value.Locations[2].LocalId = Convert.ToInt32(LocalIdTown);
                rootObject1.locationAuto.value.Locations[3].LocalId = Convert.ToInt32(LocalIdRegion);
                rootObject1.locationAuto.value.Locations[4].LocalId = Convert.ToInt32(LocalIdCountry);
                rootObject1.locationAuto.value.SuggestItem.LocationFullName = adres;
                rootObject1.locationAuto.value.SuggestItem.LocationId.LocalId = Convert.ToInt32(LocalIdHouse);
                rootObject1.selectedLocation.value[0] = "MK_House|" + LocalIdHouse;
                rootObject1.selectedLocation.value[1] = "MK_Street|" + LocalIdStreet;
                rootObject1.selectedLocation.value[2] = "MK_Town|" + LocalIdTown;
                rootObject1.selectedLocation.value[3] = "MK_Region|" + LocalIdRegion;
                rootObject1.selectedLocation.value[4] = "MK_Country|" + LocalIdCountry;
                rootObject1.townsForLead[0] = Convert.ToInt32(LocalIdTown);
                rootObject1.phone = phone;
                rootObject1.accountType = 2;
                rootObject1.roomCount = komnat;
                Models2.RootObject rootObject2 = JsonConvert.DeserializeObject<Models2.RootObject>(Properties.Resources.jsonText2);
                rootObject2.description = opisanie;
                rootObject2.floor = etag;
                rootObject2.studio = false;
                rootObject2.name = name;
                rootObject2.price = price;
                rootObject2.roomDescription = Convert.ToString(komnat);
                rootObject2.roomDescriptionName = Convert.ToString(komnat);
                rootObject2.selectedLocation.label = adres;
                rootObject2.estateId = estateid;
                rootObject2.house = Convert.ToInt32(LocalIdHouse);
                rootObject2.region = Convert.ToInt32(LocalIdRegion);
                rootObject2.street = Convert.ToInt32(LocalIdStreet);
                rootObject2.town = Convert.ToInt32(LocalIdTown);
                rootObject2.location.label = adres;
                rootObject2.location.cleanLabel = adres;
                rootObject2.location.value.Locations[0].LocalId = Convert.ToInt32(LocalIdHouse);
                rootObject2.location.value.Locations[1].LocalId = Convert.ToInt32(LocalIdStreet);
                rootObject2.location.value.Locations[2].LocalId = Convert.ToInt32(LocalIdTown);
                rootObject2.location.value.Locations[3].LocalId = Convert.ToInt32(LocalIdRegion);
                rootObject2.location.value.Locations[4].LocalId = Convert.ToInt32(LocalIdCountry);
                rootObject2.location.value.SuggestItem.LocationFullName = adres;
                rootObject2.location.value.SuggestItem.LocationId.LocalId = Convert.ToInt32(LocalIdHouse);
                rootObject2.locationAuto.label = adres;
                rootObject2.locationAuto.cleanLabel = adres;
                rootObject2.locationAuto.value.Locations[0].LocalId = Convert.ToInt32(LocalIdHouse);
                rootObject2.locationAuto.value.Locations[1].LocalId = Convert.ToInt32(LocalIdStreet);
                rootObject2.locationAuto.value.Locations[2].LocalId = Convert.ToInt32(LocalIdTown);
                rootObject2.locationAuto.value.Locations[3].LocalId = Convert.ToInt32(LocalIdRegion);
                rootObject2.locationAuto.value.Locations[4].LocalId = Convert.ToInt32(LocalIdCountry);
                rootObject2.locationAuto.value.SuggestItem.LocationFullName = adres;
                rootObject2.locationAuto.value.SuggestItem.LocationId.LocalId = Convert.ToInt32(LocalIdHouse);
                rootObject2.selectedLocation.value[0] = "MK_House|" + LocalIdHouse;
                rootObject2.selectedLocation.value[1] = "MK_Street|" + LocalIdStreet;
                rootObject2.selectedLocation.value[2] = "MK_Town|" + LocalIdTown;
                rootObject2.selectedLocation.value[3] = "MK_Region|" + LocalIdRegion;
                rootObject2.selectedLocation.value[4] = "MK_Country|" + LocalIdCountry;
                rootObject2.townsForLead[0] = Convert.ToInt32(LocalIdTown);
                rootObject2.phone = phone;
                rootObject2.accountType = 2;
                rootObject2.areaRoom = ploshad;
                rootObject2.photos[0].guid = "";
                for (int i = 0; i < files.Length; i++)
                {
                    if(i==4)
                        break;
                    toolStripStatusLabel3.Text = "Заливаем фото " + i+1;
                    request1 = (HttpWebRequest)WebRequest.Create("http://cabinet.mirkvartir.ru/photos/upload/");
                    request1.Method = "POST";
                    request1.Timeout = 1000000;
                    request1.ContinueTimeout = 1000000;
                    request1.CookieContainer = cookies;
                    request1.Accept = "text/plain, */*; q=0.01";
                    request1.Headers["X-Requested-With"] = "XMLHttpRequest";
                    request1.Headers["Accept-Language"] = "en-US,en;q=0.8,ru;q=0.6";
                    request1.Headers["Origin"] = "http://cabinet.mirkvartir.ru";
                    request1.Referer = "http://cabinet.mirkvartir.ru/Wizard/Step2?houseId=1951362";
                    request1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
                    request1.ContentType = "multipart/form-data; boundary=----WebKitFormBoundary" + ran;
                    request1.CookieContainer.Add(new Cookie("step1", "true", "/", "cabinet.mirkvartir.ru"));
                    request1.CookieContainer.Add(new Cookie("step2", "false", "/", "cabinet.mirkvartir.ru"));
                    byte[] image = File.ReadAllBytes(files[i].FullName);
                    data = "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"ws1_price\"" + "\r\n" + "" + "\r\n" + "" + "\r\n" + "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"ws1_price\"" + "\r\n" + "" + "\r\n" + "" + "\r\n" + "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"ws1_price\"" + "\r\n" + "" + "\r\n" + "" + "\r\n" + "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"ws1_price\"" + "\r\n" + "" + "\r\n" + "" + "\r\n" + "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"ws1_price\"" + "\r\n" + "" + "\r\n" + "" + "\r\n" + "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"flatinfo_square_all\"" + "\r\n" + "" + "\r\n" + "" + "\r\n" + "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"ws1_price\"" + "\r\n" + "" + "\r\n" + "" + "\r\n" + "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"ws1_price\"" + "\r\n" + "" + "\r\n" + "5" + "\r\n" + "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"ws1_price\"" + "\r\n" + "" + "\r\n" + "" + "\r\n" + "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"ws1_price\"" + "\r\n" + "" + "\r\n" + "" + "\r\n" + "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"ws1_price\"" + "\r\n" + "" + "\r\n" + "" + "\r\n" + "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"ws1_price\"" + "\r\n" + "" + "\r\n" + "" + "\r\n" + "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"ws1_price\"" + "\r\n" + "" + "\r\n" + "5" + "\r\n" + "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"ws1_price\"" + "\r\n" + "" + "\r\n" + "" + "\r\n" + "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"ta1\"" + "\r\n" + "" + "\r\n" + "" + "\r\n" + "------WebKitFormBoundary" + ran + "" + "\r\n" + "Content-Disposition: form-data; name=\"files[]\"; filename=\"" + Path.GetFileName(files[i].Name) + "\"" + "\r\n" + "Content-Type: image/jpeg" + "\r\n" + "\r\n";
                    bytedata = Encoding.ASCII.GetBytes(data);
                    bytedata = bytedata.Concat(image).ToArray().Concat(Encoding.ASCII.GetBytes("\r\n------WebKitFormBoundary" + ran + "--\r\n")).ToArray();
                    request1.ContentLength = bytedata.Length;
                    stream = request1.GetRequestStream();
                    stream.Write(bytedata, 0, bytedata.Length);
                    stream.Close();
                    response1 = (HttpWebResponse)request1.GetResponse();
                    streamReader = new StreamReader(response1.GetResponseStream());
                    result = streamReader.ReadToEnd();
                    string domainNumber = Regex.Match(result, "(?<=er\":).*?(?=,\")").Value;
                    string quid = Regex.Match(result, "(?<=guid\":\").*?(?=\",\")").Value;
                    string fileSize = Regex.Match(result, "(?<=fileSize\":).*?(?=})").Value;
                    Models1.Photo photo = new Models1.Photo();
                    Models2.Photo photo1 = new Models2.Photo();
                    photo1.fileSize = Convert.ToInt32(fileSize);
                    photo1.domainNumber = Convert.ToInt32(domainNumber);
                    photo1.guid = quid;
                    photo.fileSize = Convert.ToInt32(fileSize);
                    photo.domainNumber = Convert.ToInt32(domainNumber);
                    photo.guid = quid;
                    rootObject1.photos.Add(photo);
                    rootObject2.photos.Add(photo1);
                }
                if(komnat==0)
                    data = JsonConvert.SerializeObject(rootObject2);
                else
                    data = JsonConvert.SerializeObject(rootObject1);
                data = HttpUtility.UrlEncode(data);
                data = "viewModelJson=" + data;
                toolStripStatusLabel3.Text = "Шаг 2";
                request1 = (HttpWebRequest)WebRequest.Create("http://cabinet.mirkvartir.ru/wizard/step2/");
                request1.Method = "POST";
                request1.CookieContainer = cookies;
                request1.Headers["Accept-Encoding"] = " gzip, deflate";
                request1.Headers["Accept-Language"] = "en-US,en;q=0.8,ru;q=0.6";
                request1.Headers["Origin"] = "http://cabinet.mirkvartir.ru";
                request1.Headers["Upgrade-Insecure-Requests"] = "1";
                request1.Headers["Cache-Control"] = "max-age=0";
                request1.Referer = "http://cabinet.mirkvartir.ru/Wizard/Step2?houseId=" + LocalIdHouse;
                request1.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                request1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
                request1.ContentType = "application/x-www-form-urlencoded";
                request1.CookieContainer.Add(new Cookie("step1", "true", "/", "cabinet.mirkvartir.ru"));
                request1.CookieContainer.Add(new Cookie("step2", "true", "/", "cabinet.mirkvartir.ru"));
                bytedata = Encoding.ASCII.GetBytes(data);
                request1.ContentLength = bytedata.Length;
                stream = request1.GetRequestStream();
                stream.Write(bytedata, 0, bytedata.Length);
                response1 = (HttpWebResponse)request1.GetResponse();
                streamReader = new StreamReader(response1.GetResponseStream());
                result = streamReader.ReadToEnd();
                name = HttpUtility.UrlEncode(name);
                phone = HttpUtility.UrlEncode(phone);
                toolStripStatusLabel3.Text = "Решаем каптчу";
                string token = captcha();
               data = data + "&ws1_price=" + name + "&ws1_price=" + phone + "&AccountType=2&g-recaptcha-response=" + token;
                toolStripStatusLabel3.Text = "Шаг 3";

                request1 = (HttpWebRequest)WebRequest.Create("http://cabinet.mirkvartir.ru/wizard/step3/");
                request1.Method = "POST";
                request1.CookieContainer = cookies;
                request1.Headers["Accept-Encoding"] = " gzip, deflate";
                request1.Headers["Accept-Language"] = "en-US,en;q=0.8,ru;q=0.6";
                request1.Headers["Origin"] = "http://cabinet.mirkvartir.ru";
                request1.Headers["Upgrade-Insecure-Requests"] = "1";
                request1.Headers["Cache-Control"] = "max-age=0";
                request1.Referer = "http://cabinet.mirkvartir.ru/Wizard/Step2?houseId=" + LocalIdHouse;
                request1.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                request1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
                request1.ContentType = "application/x-www-form-urlencoded";
                request1.CookieContainer.Add(new Cookie("step1", "true", "/", "cabinet.mirkvartir.ru"));
                request1.CookieContainer.Add(new Cookie("step2", "true", "/", "cabinet.mirkvartir.ru"));
                request1.CookieContainer.Add(new Cookie("step3", "true", "/", "cabinet.mirkvartir.ru"));
                bytedata = Encoding.ASCII.GetBytes(data);
                request1.ContentLength = bytedata.Length;
                stream = request1.GetRequestStream();
                stream.Write(bytedata, 0, bytedata.Length);
                response1 = (HttpWebResponse)request1.GetResponse();
                streamReader = new StreamReader(response1.GetResponseStream());
               result = streamReader.ReadToEnd();

            }
            catch (Exception e)
            {
                
            }

        }
        public string captcha()
                {
                    string token = "";
                    string key = textBoxApi.Text;
                    HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create("http://rucaptcha.com/in.php?key=" + key + "&method=userrecaptcha&googlekey=6Lcg_f0SAAAAAHQ6yxbMkk7E8ZXIj780GarHe7dp&pageurl=http://cabinet.mirkvartir.ru/");
                    HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();
                    StreamReader StreamReader = new StreamReader(response1.GetResponseStream());
                    string result = StreamReader.ReadToEnd();
                    string id = result.Split('|')[1];
                    int i = 0;
                    while (i < 60)
                    {
                        request1 = (HttpWebRequest)WebRequest.Create("http://rucaptcha.com/res.php?key=" + key + "&action=get&id=" + id);
                        response1 = (HttpWebResponse)request1.GetResponse();
                        StreamReader = new StreamReader(response1.GetResponseStream());
                        result = StreamReader.ReadToEnd();
                        if (result.Length > 20)
                            break;
                        else
                        {
                            Thread.Sleep(5000);
                            i++;
                        }
                    }
                    if (i == 60)
                    {
                        captcha();
                    }
                    token = result.Split('|')[1];
                    return token;
                }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                switch (((Button)sender).Tag)
                {
                    case "0":
                        textBoxRooms.Text = dialog.SelectedPath;
                        break;
                    case "1":
                        textBoxOnes.Text = dialog.SelectedPath;
                        break;
                    case "2":
                        textBoxTwo.Text = dialog.SelectedPath;
                        break;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            thread?.Abort();
            button1.Enabled = true;
        }
    }
}
