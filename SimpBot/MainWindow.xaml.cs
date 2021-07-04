using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static PuppeteerSharp.Page page;
        public static Browser browser;

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {

            //make sure to give path to your chromium exe
            var options = new LaunchOptions
            {
                Headless = false,
                ExecutablePath = DONTPOST.PATH,
                Timeout = 320000,
                DefaultViewport = null

            };
            await StartBrowser(options);
        }

        /// <summary>
        /// start bullying
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task StartBrowser(LaunchOptions options)
        {
            browser = await Puppeteer.LaunchAsync(options);
            page = await browser.NewPageAsync();
            await page.GoToAsync("http://www.facebook.com", 120000);

            await page.Keyboard.PressAsync("Escape"); //idk what this does? but it does something

            //do login
            await page.TypeAsync("#email", username.Text); //enter email
            await page.TypeAsync("#pass", password.Password); //enter pasword
            await page.ClickAsync("[name = login]"); // clicks the login btn


            await page.WaitForNavigationAsync(); //wait for newsfeed to load



            //idk wtf this is for ._. but hey i trust the judgement of past me
            await page.Keyboard.PressAsync("Escape"); //hits esc button 
            await page.Keyboard.PressAsync("End");  //hits end button


            //starts an endless loop that keeps running this code
            while (true)
            {
                try
                {
                    //get a list of all the posts on the newsfeed
                    var posts = await page.QuerySelectorAllAsync("[data-visualcompletion='ignore-late-mutation'"); //get all posts                    

                    for (int i = 0; i < posts.Length; i++) //go through the list of posts
                    {
                        //run js that gets the name of the person who made the post
                        string js = $@"var n = document.querySelectorAll(""[data-visualcompletion='ignore-late-mutation'""); n[{i}].querySelectorAll('.qzhwtbm6.knvmm38d')[{0}].innerText;"; //get username string
                        var WhoMadePost = await page.EvaluateExpressionAsync(js); //get username of everyone who made a post


                        Console.WriteLine(WhoMadePost.ToString()); //output the name of the person who made the post to console 

                        //check to see if the person who made the post is the target
                        if (WhoMadePost.ToString() == Target.Text) 
                        {
                            //get all comments
                            var comments = await posts[i].QuerySelectorAsync(".oo9gr5id.lzcic4wl.jm1wdb64.l9j0dhe7.gsox5hk5.mdldhsdk.ii04i59q.notranslate"); 
                            Newtonsoft.Json.Linq.JToken check = null;

                            try
                            {
                                //check to see if already commented
                                check = await page.EvaluateExpressionAsync($@"var n = document.querySelectorAll(""[data-visualcompletion = 'ignore-late-mutation'""); n[{i}].querySelectorAll('.cwj9ozl2.tvmbv18p')[0].innerText;");
                            }
                            catch
                            {
                                //i think this is to handle null errors??? idk why it does it 
                            }

                            //if not commented run the code below
                            if (check == null) 
                            {
                                await comments.TypeAsync(Comment.Text); //type comment
                                await page.Keyboard.PressAsync("Enter"); //submit comment
                            }
                            else if (!check.ToString().Contains(FBusername.Text)) //im not sure what this check is for. all i know is the code works.
                            {
                                //check to see if already commented
                                check = await page.EvaluateExpressionAsync($@"var n = document.querySelectorAll(""[data-visualcompletion = 'ignore-late-mutation'""); n[{i}].querySelectorAll('.cwj9ozl2.tvmbv18p')[0].innerText;");

                                if (!check.ToString().Contains(FBusername.Text))
                                {
                                    await comments.TypeAsync(Comment.Text); //type comment
                                    await page.Keyboard.PressAsync("Enter"); //submit comment
                                }
                            }
                        }
                    }
                }
                catch
                {

                }
            }
        }


        /// <summary>
        /// close the browser when app closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (page != null) //checks to see if any pages are open
                {
                    page.CloseAsync(); // close it
                    browser.CloseAsync(); // close browser
                }
            }
            catch (NullReferenceException ex)
            { }
        }
    }
}
