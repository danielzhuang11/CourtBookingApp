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
        IWebDriver m_driver;

        bool completFlag = false;
       
        public void startBrowser()
        {
            //m_driver = new FirefoxDriver("D:\\DanielCode\\GeckoDriver");
            m_driver = new ChromeDriver("D:\\DanielCode\\ChromeDriver");
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
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            startBrowser();
            openWebPage();

            string myCourtTime = tbTime.Text;

            while (true)
            {
                if (myCourtTime == "")
                {
                    MessageBox.Show("Please fill the desired court time");
                }
                else
                {
                    break;
                }
            }

            DateTime dateTime = DateTime.ParseExact(myCourtTime,
                                    "hh:mm tt", CultureInfo.InvariantCulture);
            TimeSpan ts = new TimeSpan(2,0,0);
            DateTime startTime = dateTime.Subtract(ts);

            string tmpStr = "Program will start from " + startTime.ToShortTimeString(); ;
            lbNotification.Text = tmpStr;
            lbNotification.Visible = true;

            tbUserName.Text = "zhungtm@hotmail.com";
            tbPassword.Text = "Hello@123";
            tbPassword.UseSystemPasswordChar = true;
            string userNameStr = tbUserName.Text;
            string passWordStr = tbPassword.Text;

            IWebElement usernameTextBox = m_driver.FindElement(By.XPath(".//*[@name='login']"));

            IWebElement passwordTextBox = m_driver.FindElement(By.Id("password"));
            IWebElement signInButton = m_driver.FindElement(By.Id("signin_login_form"));
            usernameTextBox.SendKeys(userNameStr);


            IJavaScriptExecutor js = (IJavaScriptExecutor)m_driver;

            js.ExecuteScript(string.Format("document.getElementById('password').value='{0}';", passWordStr));

            signInButton.Submit();

            m_driver.Url = "https://cpac.clubautomation.com/event/reserve-court-new";

            // choose 30 minutes
            js.ExecuteScript(string.Format("document.getElementById('interval-30').checked='{0}';", "checked"));

            IWebElement searchBtn = m_driver.FindElement(By.Id("reserve-court-search"));
            searchBtn.Click();

            Thread.Sleep(5000);
           /* IWait<IWebElement> wait = new DefaultWait<IWebElement>(tr);
            wait.Timeout = TimeSpan.FromSeconds(5);
            wait.PollingInterval = TimeSpan.FromMilliseconds(300);
            */

           
            IWebElement timeChoices = m_driver.FindElement(By.Id("times-to-reserve"));

            foreach (var td in timeChoices.FindElements(By.XPath("tbody/tr/td")))
            {

                foreach(var option in td.FindElements(By.XPath("a")))
                {
                    string gmtOption = option.GetAttribute("t");
                    string locationOption = option.GetAttribute("l");
                    string timeOption = option.Text;

                    if (locationOption == "6" && timeOption == myCourtTime)
                    {
                        //   IWebElement timeLink = option.FindElement(By.LinkText(myCourtTime));
                        option.Click();
                        Thread.Sleep(1000);

                        m_driver.SwitchTo().Window(m_driver.WindowHandles.Last());
                        Thread.Sleep(2000);

                        IWebElement invoiceBox = m_driver.FindElement(By.ClassName("userbox"));
                        string invoiceText = invoiceBox.Text;
                        if (invoiceText.Contains("No Invoice"))
                        {
                            completFlag = true;
                            IWebElement btnLink = m_driver.FindElement(By.Id("cancel"));
                            btnLink.Click();
                            lbNotification.Text = "Booked " + myCourtTime + " court.";
                            lbNotification.Visible = true;
                            break;
                        }

                    }

                    if (completFlag)
                        break;
                }
            }

            closeBrowser();

        }
    }
}
