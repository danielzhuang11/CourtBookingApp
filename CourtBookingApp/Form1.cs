using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Globalization;

using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Entities;

namespace CourtBookingApp
{
    public partial class Form1 : Form
    {
        private IWebDriver m_driver;

        private bool completFlag = false;
        private string myCourtTime="";
        private string myCourtPeriod = "";
        private string notificationName = "";
        private string userNameStr;
        private string passWordStr;
        private string notificationStr;
        private string[] timeStr = new string[31];
        private string status = "Failed";

        public void generateTimeStr()
        {
            string tmpStr;
            int count = 0;

            // AM string
            for (int i = 7; i <= 11; i++)
            {
                tmpStr = i.ToString() + ":00 am";
                timeStr[count++] = tmpStr;

                tmpStr = i.ToString() + ":30 am";
                timeStr[count++] = tmpStr;
            }

            //PM string
            timeStr[count++] = "12:00 pm";
            timeStr[count++] = "12:30 pm";
            for (int i = 1; i <= 9; i++)
            {
                tmpStr = i.ToString() + ":00 pm";
                timeStr[count++] = tmpStr;

                tmpStr = i.ToString() + ":30 pm";
                timeStr[count++] = tmpStr;
            }
            timeStr[count++] = "10:00 pm";
        }

        public void startBrowser()
        {
            m_driver = new ChromeDriver("ChromeDriver");
        }

        public void openWebPage()
        {
            m_driver.Url = "https://cpac.clubautomation.com/index.php";
        }

        public void closeBrowser()
        {
            m_driver.Close();
        }

        public Form1()
        {
            InitializeComponent();

            // all time choice in string
            generateTimeStr();

            cbTime.Items.AddRange(timeStr);

            cbPeriod.Items.Add("30 minutes");
            cbPeriod.Items.Add("60 minutes");
            cbPeriod.Items.Add("90 minutes");
            cbPeriod.Items.Add("120 minutes");

            var tokenResult = CommonApi.GetToken("wxb52657827a246f81", "ce3f87fd605221d29cd799c21169d5c1");
            listNames(tokenResult);


        }

        private DateTime calculateTime()
        {
            completFlag = false;            
            
            //tbPassword.UseSystemPasswordChar = true;
            //userNameStr = tbUserName.Text;
            //passWordStr = tbPassword.Text;
           
            
            // calculate starting time
            DateTime dateTime = DateTime.ParseExact(myCourtTime,
                                    "h:mm tt", CultureInfo.InvariantCulture);
            DateTime current = DateTime.Now;
            TimeSpan timeCheck = dateTime.TimeOfDay - current.TimeOfDay;
            if (timeCheck < TimeSpan.Zero) // if input time passed current time, then it means next day
            {
                TimeSpan oneDay = new TimeSpan(24, 0, 0);
                dateTime = dateTime.Add(oneDay);
            }

            TimeSpan ts = new TimeSpan(2, 00, 15);
            DateTime startTime = dateTime.Subtract(ts);

            notificationStr = "Program will start from " + startTime.ToShortTimeString(); ;
            lbNotification.Text = notificationStr;
            lbNotification.Visible = true;

            return startTime;
        }

        // input is: whether it is a testing 
        private void preProc(bool testFlag)
        {
            if (testFlag)
                tbTime.Visible = true;

            userNameStr = tbUserName.Text;
            passWordStr = tbPassword.Text;

            notificationName = cbName.GetItemText(cbName.SelectedItem);
            myCourtTime = cbTime.GetItemText(cbTime.SelectedItem);
            myCourtPeriod = cbPeriod.GetItemText(cbPeriod.SelectedItem);

            if (testFlag )
            {
                if (tbTime.Text != "")
                    myCourtTime = tbTime.Text;
            }

            if (userNameStr == "" || passWordStr == "")
            {
                MessageBox.Show("Please fill username and password");
            }
            else if (myCourtTime == "" || myCourtPeriod == "")
            {
                MessageBox.Show("Please fill the desired court time and period");
            }
            else
            {
                DateTime startTime = calculateTime();
                SetUpTimer(startTime, testFlag);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // test flag is FALSE
            preProc(false);          
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            // test flag is TRUE
            preProc(true);                   
        }

        private System.Threading.Timer timer;
        private void SetUpTimer(DateTime startTime, bool testFlag)
        {
            DateTime current = DateTime.Now;
            TimeSpan timeToGo = startTime.Subtract(current);
            if (timeToGo < TimeSpan.Zero)
            {
                if (this.CourtBooking(testFlag))
                {
                    notificationStr = "Booked " + myCourtTime + " court. Status message is " + status;
                }
                else
                {
                    notificationStr = "Failed booking. Status message is " + status;
                }
                MessageBox.Show(notificationStr);
            
                return;
            }
            this.timer = new System.Threading.Timer(x =>
            {             
                if (this.CourtBooking(testFlag))
                {
                    notificationStr = "Booked " + myCourtTime + " court. Status message is " + status;
                }
                else
                {
                    notificationStr = "Failed booking. Status message is " + status;
                }
                MessageBox.Show(notificationStr);
              
            }, null, timeToGo, (new TimeSpan(24,0,0)));
        }


        private bool CourtBooking(bool testFlag)
        {
            startBrowser();
            openWebPage();          

            IWebElement usernameTextBox = m_driver.FindElement(By.XPath(".//*[@name='login']"));

            IWebElement passwordTextBox = m_driver.FindElement(By.Id("password"));
            IWebElement signInButton = m_driver.FindElement(By.Id("signin_login_form"));
            usernameTextBox.SendKeys(userNameStr);


            IJavaScriptExecutor js = (IJavaScriptExecutor)m_driver;

            js.ExecuteScript(string.Format("document.getElementById('password').value='{0}';", passWordStr));

            signInButton.Submit();

            m_driver.Url = "https://cpac.clubautomation.com/event/reserve-court-new";

            switch (myCourtPeriod)
            {
                case "30 minutes":
                    // choose 30 minutes
                    js.ExecuteScript(string.Format("document.getElementById('interval-30').checked='{0}';", "checked"));
                    break;
                case "60 minutes":
                    // choose 30 minutes
                    js.ExecuteScript(string.Format("document.getElementById('interval-60').checked='{0}';", "checked"));
                    break;
                case "90 minutes":
                    // choose 30 minutes
                    js.ExecuteScript(string.Format("document.getElementById('interval-90').checked='{0}';", "checked"));
                    break;
                case "120 minutes":
                    // choose 30 minutes
                    js.ExecuteScript(string.Format("document.getElementById('interval-120').checked='{0}';", "checked"));
                    break;
            }
            IWebElement searchBtn = m_driver.FindElement(By.Id("reserve-court-search"));
            searchBtn.Click();

            // wait 5 seconds for retriving court information finished
            Thread.Sleep(5000);
            
            IWebElement timeChoices ;
            try
            {
                timeChoices = m_driver.FindElement(By.Id("times-to-reserve"));
            }
            catch
            {              
                completFlag = false;
                status = "No such court time. Failed booking.";
                teardown(completFlag);
                return completFlag;
            }

            foreach (var td in timeChoices.FindElements(By.XPath("tbody/tr/td")))
            {

                foreach (var option in td.FindElements(By.XPath("a")))
                {
                    string gmtOption = option.GetAttribute("t");
                    string locationOption = option.GetAttribute("l");
                    string timeOption = option.Text;

                    if (locationOption == "6" && timeOption == myCourtTime)
                    {
                        option.Click();
                        if (checkInvoice(testFlag))
                        {
                            // cancel it
                            IWebElement btnLink;
                            btnLink = m_driver.FindElement(By.Id("cancel"));
                            btnLink.Click();

                            // it might be caused by running too early, run it again
                            Thread.Sleep(2000);
                            option.Click();

                            if (checkInvoice(testFlag))
                            {
                                // still not good so cancel it
                                btnLink = m_driver.FindElement(By.Id("cancel"));
                                btnLink.Click();
                            }
                            else
                            {
                                status = "Booked successfully 2nd time.";
                            }
                        }
                        else // no invoice so book successfully
                        {
                            
                        }
                        break;
                    }
                    else
                    {
                        status = "No such court time.";
                    }

                    if (completFlag)
                        break;
                }
            }

            teardown(completFlag);

            return completFlag;
        }

        // return TRUE if there is invoice
        // return FALSE if there is no invoice
        private bool checkInvoice(bool testFlag)
        {
            Thread.Sleep(1000);

            m_driver.SwitchTo().Window(m_driver.WindowHandles.Last());
            Thread.Sleep(2000);

            IWebElement invoiceBox = m_driver.FindElement(By.ClassName("userbox"));
            IWebElement btnLink;
            if (invoiceBox.Text.Contains("No Invoice"))
            {
                completFlag = true;

                if (testFlag)
                    btnLink = m_driver.FindElement(By.Id("cancel"));
                else
                    btnLink = m_driver.FindElement(By.Id("confirm"));
                btnLink.Click();

                status = "Booked successfully";
                completFlag = true;
                return false;
            }
            else
            {
                status = "Court is not free.";
                completFlag = false;
                return true;
            }

        }

        

        private void teardown(bool resultFlag)
        {
            closeBrowser();
          
            var tokenResult = CommonApi.GetToken("wxb52657827a246f81", "ce3f87fd605221d29cd799c21169d5c1");
            
            SendToWeChat(tokenResult, resultFlag);
        }

        private bool isInNameList(string[] nameList, string name)
        {
            if (nameList.Count() == 0)
                return true; // if empty input, always send alarm to WeChat

            for (int i = 0; i < nameList.Count(); i++)
            {
                if (name.Trim() == nameList[i].Trim())
                {
                    return true;
                }
            }
            return false;
        }

        public void listNames(AccessTokenResult tokenResult)
        {
            string accessToken = "";
            try
            {
                if (tokenResult.access_token.Length > 0 && tokenResult.expires_in > 0)
                {
                    accessToken = tokenResult.access_token;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Exception {0}", ex.Message));
                return;
            }
            List<UserInfoJson> userInfoList = new List<UserInfoJson>();
            if (tokenResult.access_token.Length > 0 && tokenResult.expires_in > 0)
            {
                accessToken = tokenResult.access_token;
                foreach (var id in UserApi.Get(accessToken, "").data.openid)
                {
                    var userInfoResult = UserApi.Info(accessToken, id);
                    cbName.Items.Add(userInfoResult.nickname);
                    //userInfoList.Add(userInfoResult);
                    // Console.WriteLine(userInfoList.Count + ".添加：" + userInfoResult.nickname);

                }
            }

        }

        public bool SendToWeChat(AccessTokenResult tokenResult, bool completeFlag)
        {
          
            notificationName += ",Tom Z. Zhuang";          

            string[] recipientNames = notificationName.Split(',');

            string accessToken = "";
            try
            {
                if (tokenResult.access_token.Length > 0 && tokenResult.expires_in > 0)
                {
                    accessToken = tokenResult.access_token;
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Exception {0}", ex.Message));
                return false;
            }
            List<UserInfoJson> userInfoList = new List<UserInfoJson>();
            if (tokenResult.access_token.Length > 0 && tokenResult.expires_in > 0)
            {
                accessToken = tokenResult.access_token;
                foreach (var id in UserApi.Get(accessToken, "").data.openid)
                {
                    var userInfoResult = UserApi.Info(accessToken, id);
                    userInfoList.Add(userInfoResult);
                   
                }
            }

            bool nameFlag = false;
            
            foreach (var item in userInfoList)
            {
                    // check whether input name is in the user list
                    nameFlag = isInNameList(recipientNames, item.nickname);
                    
                    if (nameFlag) //only in the recipient name list
                    {
                        string openId = item.openid;

                     
                        string templateId = "WJ5AW-ANTT5pRbgkaGU6nZZmPYHhoIxGyQX0Z9Bf_eg";
                        string[] tarray = new string[2];
                        tarray[0] = myCourtTime + "\r\n";
                     /*   if (completeFlag)
                            tarray[1] = "Booked successfully.";
                        else
                            tarray[1] = "failed booking.";*/
                        tarray[1] = status;
                        var testData = new TestTemplateData()
                        {
                            Time = new TemplateDataItem(tarray[0].Trim()),
                            Result = new TemplateDataItem(tarray[1].Trim()),

                        };
                        try
                        {
                            var result = TemplateApi.SendTemplateMessage(accessToken, openId, templateId, "#FF0000", "", testData);
                        }
                        catch
                        {

                        }
                    }
                
            }
            return true;
        }

    }

   

    public class TestTemplateData
    {
        public TemplateDataItem Time { get; set; }
        public TemplateDataItem Result { get; set; }
        
    }
        
   
}
