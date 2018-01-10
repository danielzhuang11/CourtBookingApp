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

namespace CourtBookingApp
{
    public partial class Form1 : Form
    {
        private IWebDriver m_driver;

        private bool completFlag = false;
        private string myCourtTime="";
        private string myCourtPeriod = "";
        private string userNameStr;
        private string passWordStr;
        private string notificationStr;
        private string[] timeStr = new string[30];

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
        }

        private DateTime preProc()
        {
            completFlag = false;
            

            tbUserName.Text = "zhungtm@hotmail.com";
            tbPassword.Text = "Hello@123";
            tbPassword.UseSystemPasswordChar = true;
            userNameStr = tbUserName.Text;
            passWordStr = tbPassword.Text;
           
            
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

            TimeSpan ts = new TimeSpan(2, 00, 18);
            DateTime startTime = dateTime.Subtract(ts);

            notificationStr = "Program will start from " + startTime.ToShortTimeString(); ;
            lbNotification.Text = notificationStr;
            lbNotification.Visible = true;

            return startTime;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            myCourtTime = cbTime.GetItemText(cbTime.SelectedItem);
            myCourtPeriod = cbPeriod.GetItemText(cbPeriod.SelectedItem);

            if (myCourtTime == "" || myCourtPeriod == "")
            {
                MessageBox.Show("Please fill the desired court time and period");
            }
            else
            {
                DateTime startTime = preProc();
                SetUpTimer(startTime, false);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            myCourtTime = cbTime.GetItemText(cbTime.SelectedItem);
            myCourtPeriod = cbPeriod.GetItemText(cbPeriod.SelectedItem);

            if (myCourtTime == "" || myCourtPeriod == "")
            {
                MessageBox.Show("Please fill the desired court time and period");
            }
            else
            {
                DateTime startTime = preProc();
                SetUpTimer(startTime, true);
            }
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
                    notificationStr = "Booked " + myCourtTime + " court.";
                }
                else
                {
                    notificationStr = "Failed booking";
                }
                MessageBox.Show(notificationStr);
            
                return;
            }
            this.timer = new System.Threading.Timer(x =>
            {             
                if (this.CourtBooking(testFlag))
                {
                    notificationStr = "Booked " + myCourtTime + " court.";
                }
                else
                {
                    notificationStr = "Failed booking";
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
                closeBrowser();
                return false;
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
                        Thread.Sleep(1000);

                        m_driver.SwitchTo().Window(m_driver.WindowHandles.Last());
                        Thread.Sleep(2000);

                        IWebElement invoiceBox = m_driver.FindElement(By.ClassName("userbox"));
                        string invoiceText = invoiceBox.Text;
                        if (invoiceText.Contains("No Invoice"))
                        {
                            completFlag = true;
                            IWebElement btnLink;
                            if (testFlag)
                                btnLink = m_driver.FindElement(By.Id("cancel"));
                            else
                                btnLink = m_driver.FindElement(By.Id("confirm"));
                            btnLink.Click();
                      
                            break;
                        }

                    }

                    if (completFlag)
                        break;
                }
            }

         
            closeBrowser();
            return completFlag;
        }

        
    }
}
