using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;



namespace PowerPointAddIn1_Practice
{
    public partial class loginDialog : Form
    {
        bool LoggedIn;
        string jwt;
        Dictionary<String, int> courses;
        Dictionary<int, String> seasons;
        Dictionary<int, String> years;
        Dictionary<String, int> coursesToTerm;
        Dictionary<int, String> seasonToTerm;
        public loginDialog()
        {
            LoggedIn = false;
            courses = new Dictionary<string, int>();
            seasons = new Dictionary<int, string>();
            years = new Dictionary<int, string>();
            coursesToTerm = new Dictionary<string, int>();
            seasonToTerm = new Dictionary<int, String>();
            jwt = "";

            InitializeComponent();
        }

        public string getJWT() { return jwt; }

        public Dictionary<String, int> getCourse() { return courses; }

        public Dictionary<String, int> getCoursesToTerm() { return coursesToTerm; }


        public Dictionary<int, String> getSeason() { return seasonToTerm; }


        public Dictionary<int, String> getYears() { return years; }

        public bool LoggedInFunction()
        {
            return LoggedIn;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string _cookie = "";
            this.IncorrectLoginLabel.Visible = false;
            var username = this.usernameField.Text;
            var password = this.passwordField.Text;
            Debug.WriteLine(password);
            WebRequest request = WebRequest.Create("http://52.41.106.241:1337/login");
            request.Credentials = CredentialCache.DefaultCredentials;


            try
            {

                //if (System.Web.HttpContext.Current.Session["cookie"] != null) 

                string result;
                string temp;
                Dictionary<String, Object> myJSON = null;
                using (WebClient client = new WebClient())
                {   
                    byte[] myData = client.UploadValues("http://52.41.106.241:1337/login",
                        new System.Collections.Specialized.NameValueCollection()
                        {
                    { "email", username },
                    { "password", password}
                        });
                    result = System.Text.Encoding.UTF8.GetString(myData);
                    myJSON = JsonConvert.DeserializeObject<Dictionary<String, Object>>(result);
                    jwt = (string) myJSON["jwt"];
                    //var step1 = JsonConvert.DeserializeObject<Dictionary<String, Object>>((string)myJSON["user"]);
                    //temp = (string)step1["title"];
                }

                //responsible for semester stuff
                string results;
               
                using (WebClient clients = new WebClient())
                {
                    clients.Headers.Add(HttpRequestHeader.Cookie, "jwt=" + jwt);
                    byte[] myDatas = clients.UploadValues("http://52.41.106.241:1337/term/multifind/", new System.Collections.Specialized.NameValueCollection()
                        {
                    { "1", "blank" },
                    { "2", "blank"}
                        });
                    results = System.Text.Encoding.UTF8.GetString(myDatas);
                    Debug.WriteLine(results);
                }
                Debug.WriteLine("Starting the Semester");
                var myJSONs = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(results);
                List<Dictionary<String, Object>> termFromJSON = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(results);
                foreach (Dictionary<String, Object> items in termFromJSON)
                {
                    string currentSeasonJSONString = items["season"].ToString();
                    Dictionary<String, Object> seasonFromJSON = JsonConvert.DeserializeObject<Dictionary<String, Object>>(currentSeasonJSONString);
                    string semester = seasonFromJSON["season"].ToString();

                    string currentYearJSONString = items["year"].ToString();
                    Dictionary<String, Object> yearFromJSON = JsonConvert.DeserializeObject<Dictionary<String, Object>>(currentYearJSONString);
                    semester += " " + yearFromJSON["year"].ToString();
                    int term = Convert.ToInt32(items["id"]);

                    Debug.WriteLine(semester + " " + term);

                    seasonToTerm.Add(term, semester);
                }

                //Debug.WriteLine(myJSONs.ToString());
                Debug.WriteLine("Ending the Semester");



                // responsible for course stuff
                string userFromJSONString = myJSON["user"].ToString();
                Dictionary<String, Object> usersFromJSON = JsonConvert.DeserializeObject<Dictionary<String, Object>>(userFromJSONString);
                string coursesFromJSONString = usersFromJSON["courses"].ToString();
                List<Dictionary<String, Object>> coursesFromJSON = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(coursesFromJSONString);
                foreach(Dictionary<String, Object> item in coursesFromJSON)
                {
                    string title = (string)item["title"];
                    int id = Convert.ToInt32(item["id"]);
                    int term = Convert.ToInt32(item["term"]);
                    coursesToTerm.Add(title, term);
                    courses.Add(title, id);
                }
                //Debug.WriteLine(temp);
                Debug.WriteLine("Start Login");
                Debug.WriteLine(result);

                this.LoggedIn = true;
                this.Close();
            } catch (System.Net.WebException exception)
            {
                Debug.WriteLine(exception.ToString());
                this.IncorrectLoginLabel.Visible = true;
            }
            

            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void passwordField_TextChanged(object sender, EventArgs e)
        {
            passwordField.PasswordChar = '*';
            passwordField.AcceptsTab = true;
        }

        private void usernameField_TextChanged(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           System.Diagnostics.Process.Start("http://52.41.106.241");

        }

        private void loginDialog_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
