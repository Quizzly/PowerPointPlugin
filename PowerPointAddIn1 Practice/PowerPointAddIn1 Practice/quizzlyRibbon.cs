using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.Net;
using System.Diagnostics;
using System.Web;

using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.IO;

namespace PowerPointAddIn1_Practice
{
    public partial class quizzlyRibbon
    {
        static string selectedSection;
        static string jwt = "";
        string logoURL = "https://image.ibb.co/iMv06F/Capture.png";
        Dictionary<String, int> courses;
        Dictionary<String, int> coursesToTerm;
        Dictionary<int, String> seasons;
        Dictionary<String, int> quizzes;
        Dictionary<String, int> semesters;

        static string hello;
        List<String> mySeasonsInDropDown;
        static Dictionary<String, int> questions;
        static Dictionary<String, int> sections;
        static Dictionary<int, int> alreadyAsked;
        static Dictionary<int, int> idandduration;
        private void Ribbon2_Load(object sender, RibbonUIEventArgs e)
        {
            quizzes = new Dictionary<string, int>();
            questions = new Dictionary<string, int>();
            sections = new Dictionary<string, int>();
            semesters = new Dictionary<string, int>();
            alreadyAsked = new Dictionary<int, int>();

            mySeasonsInDropDown = new List<String>();
            idandduration = new Dictionary<int, int>();

            selectedSection = "";
        }


        private void loginButton_Click(object sender, RibbonControlEventArgs e)
        {

            
       
            loginDialog dlg = new loginDialog();
            dlg.ShowDialog();

            while (dlg.IsAccessible) ;

            if (dlg.LoggedInFunction())
            {
                this.LoggedInLabel.Visible = true;
                jwt = dlg.getJWT();
                courses = dlg.getCourse();
               

                seasons = dlg.getSeason();
                coursesToTerm = dlg.getCoursesToTerm();
                RibbonDropDownItem items = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                items.Label = "Select Semester"; // Select semester
                semesterDropDown.Items.Add(items);

                string result7;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://52.41.106.241:1337/metrics/1/1");
                request.Headers.Add(HttpRequestHeader.Cookie, "jwt=" + jwt);
                request.Method = "GET";
                String test = String.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    //response.Headers.Add(HttpRequestHeader.Cookie, "jwt=" + jwt);
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    test = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                }
                
                Debug.WriteLine("Start Graph Pull");
                Debug.WriteLine(test);



                foreach (KeyValuePair<String, int> entry in coursesToTerm)
                {
                    if (seasons.ContainsKey(entry.Value) && !mySeasonsInDropDown.Contains(seasons[entry.Value])) 
                    {
                        RibbonDropDownItem temp = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                        temp.Label = seasons[entry.Value];
                        semesterDropDown.Items.Add(temp);
                        mySeasonsInDropDown.Add(seasons[entry.Value]);
                    }
                }


            }

            
        }
      
 

        public static void askQuestion(string question, string section)
        {
            int questionID = questions[question];

            //grab the section from the ribbon
            string realsection = selectedSection;
            Debug.WriteLine("this is the real section: " + realsection);
            int sectionID = sections[realsection];

            if (!alreadyAsked.ContainsKey(sectionID))
            {
                string result;
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add(HttpRequestHeader.Cookie, "jwt=" + jwt);
                    byte[] myData = client.UploadValues("http://52.41.106.241:1337/question/ask/", new System.Collections.Specialized.NameValueCollection()
                        {
                    { "question", questionID.ToString() },
                    { "section", sectionID.ToString() }
                        });
                    result = System.Text.Encoding.UTF8.GetString(myData);
                }

                // show the timer 
                int dur = idandduration[questionID];
                timer time = new timer(dur);
                time.ShowDialog();

                Debug.WriteLine("the question was already asked");

                // pull the graph information for the server
                string url = String.Empty;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://52.41.106.241:1337/metrics/" + sectionID + "/" + questionID);
                request.Headers.Add(HttpRequestHeader.Cookie, "jwt=" + jwt);
                request.Method = "GET";
                String test = String.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    //response.Headers.Add(HttpRequestHeader.Cookie, "jwt=" + jwt);
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    url = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                }

                Debug.WriteLine("Start Graph Pull");
                Debug.WriteLine(url);

                //store the information
                alreadyAsked.Add(sectionID, questionID);
                Debug.WriteLine("Ask Question Info");
                Debug.WriteLine(result);

                //import that graph // might need to change for the timming of this to work well
                // need a function to return if its finished yet
                PowerPoint._Slide objSlideG;
                PowerPoint.TextRange objTextRngG;
                objSlideG = Globals.ThisAddIn.Application.ActivePresentation.Slides.Add(Globals.ThisAddIn.Application.SlideShowWindows[1].View.Slide.SlideIndex + 1, PowerPoint.PpSlideLayout.ppLayoutTitle);
                // objSlideG.Name = "graph&" + questionID.ToString();
                objTextRngG = objSlideG.Shapes[2].TextFrame.TextRange;
                objTextRngG.Text = "Question Statistics";
                objSlideG.Shapes.AddPicture(url, MsoTriState.msoTrue, MsoTriState.msoTrue, 250, 10, 400, 250);
            }
        }
   

        private void addslideButton_Click(object sender, RibbonControlEventArgs e)
        {
      
            string result;
            int questionID = questions[questionDropDown.SelectedItem.Label];
            Dictionary<string, string> answers = new Dictionary<string, string>();
            using (WebClient client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.Cookie, "jwt=" + jwt);
                byte[] myData = client.UploadValues("http://52.41.106.241:1337/question/find/" + questionID, new System.Collections.Specialized.NameValueCollection()
                        {
                    { "email", "blank" },
                    { "password", "blank"}
                        });
                result = System.Text.Encoding.UTF8.GetString(myData);
            }
            var myJSON = JsonConvert.DeserializeObject<Dictionary<String, Object>>(result);
            List<Dictionary<String, Object>> answerFromJSON = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(myJSON["answers"].ToString());
            foreach (Dictionary<String, Object> answerItem in answerFromJSON)
            {
                string title = (string)answerItem["text"];
                string choice = (string)answerItem["option"];
                answers.Add(choice, title);
            }

            Debug.WriteLine("Start Question Info");
            Debug.WriteLine(result);

 
            PowerPoint.Application objApp;
            PowerPoint.Presentations objPresSet;
            PowerPoint._Presentation objPres;
            PowerPoint.Slides objSlides;
            PowerPoint._Slide objSlide;
            PowerPoint._Slide objSlideB;
            PowerPoint.TextRange objTextRng;
            PowerPoint.TextRange objTextRngB;
            PowerPoint.TextRange objTextRngBB;
            PowerPoint.TextRange objTextRngAnswers;
            PowerPoint.Shapes objShapes;
            PowerPoint.Shape objShape;
            PowerPoint.SlideShowWindows objSSWs;
            PowerPoint.SlideShowTransition objSST;
            PowerPoint.SlideShowSettings objSSS;
            PowerPoint.SlideRange objSldRng;

            //Create a new presentation based on a template.
            objApp = new PowerPoint.Application();
            objApp.Visible = MsoTriState.msoTrue;
            objPresSet = objApp.Presentations;
     

            //Build Slide Before Question
            objSlideB = Globals.ThisAddIn.Application.ActivePresentation.Slides.Add(Globals.ThisAddIn.Application.ActiveWindow.View.Slide.SlideIndex + 1, PowerPoint.PpSlideLayout.ppLayoutTitle);
            objTextRngB = objSlideB.Shapes[1].TextFrame.TextRange;
            objTextRngB.Text = "The following question will be asked on the next slide:";
            objTextRngBB = objSlideB.Shapes[2].TextFrame.TextRange;
            objTextRngBB.Text = "\n" + questionDropDown.SelectedItem.Label;
            objTextRngBB.Font.Size = 30;
            

            objSlideB.Shapes.AddPicture(logoURL, MsoTriState.msoTrue, MsoTriState.msoTrue, 425,450,-1,-1);

            //Build Slide #1:
            //Add text to the slide, change the font and insert/position a 
            //button to ask question on the first slide.
            objSlide = Globals.ThisAddIn.Application.ActivePresentation.Slides.Add(Globals.ThisAddIn.Application.ActiveWindow.View.Slide.SlideIndex + 2, PowerPoint.PpSlideLayout.ppLayoutText);
            objSlide.Name = "question&" + questionDropDown.SelectedItem.Label + "&" + sectionDropDown.SelectedItem.Label;
            objTextRng = objSlide.Shapes[1].TextFrame.TextRange;
            objTextRngAnswers = objSlide.Shapes[2].TextFrame.TextRange;
            objTextRngAnswers.ParagraphFormat.Bullet.Type = PowerPoint.PpBulletType.ppBulletNone;


            objTextRng.Text = "\n" + questionDropDown.SelectedItem.Label + "\n";
            objTextRngAnswers.Text = "\n" + "A: " + answers["A"] + "\n" + "B: " + answers["B"] + "\n" +
                "C: " + answers["C"];
            objTextRng.Font.Name = "Calibri";
            objTextRng.Font.Size = 48;
            objTextRngAnswers.Font.Name = "Calibri";
            objTextRngAnswers.Font.Size = 36;

           
        }

        private void courseDropDown_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            
            quizDropDown.Items.Clear();
            questionDropDown.Items.Clear();
            sectionDropDown.Items.Clear();
            quizzes.Clear();
            questions.Clear();
            idandduration.Clear();
            sections.Clear();
           
            if (courseDropDown.SelectedItem.Label != "Select Course")
            {
                string result;
                int courseID = courses[courseDropDown.SelectedItem.Label];

                using (WebClient client = new WebClient())
                {
                    client.Headers.Add(HttpRequestHeader.Cookie, "jwt=" + jwt);
                    byte[] myData = client.UploadValues("http://52.41.106.241:1337/course/find/" + courseID, new System.Collections.Specialized.NameValueCollection()
                        {
                    { "email", "blank" },
                    { "password", "blank"}
                        });
                    result = System.Text.Encoding.UTF8.GetString(myData);
                }
                RibbonDropDownItem item = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                item.Label = "Select Quiz";
                quizDropDown.Items.Add(item);
                var myJSON = JsonConvert.DeserializeObject<Dictionary<String, Object>>(result);
                List<Dictionary<String, Object>> quizzesFromJSON = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(myJSON["quizzes"].ToString());
                Debug.WriteLine(quizzesFromJSON.ToString());
                foreach (Dictionary<String, Object> quizItem in quizzesFromJSON)
                {
                    string title = (string)quizItem["title"];
                    int id = Convert.ToInt32(quizItem["id"]);
                    quizzes.Add(title, id);
                    RibbonDropDownItem temp = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                    temp.Label = title;
                    quizDropDown.Items.Add(temp);
                }

                //section
                item = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                item.Label = "Select Section";
                sectionDropDown.Items.Add(item);
                List<Dictionary<String, Object>> sectionsFromJSON = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(myJSON["sections"].ToString());
                foreach (Dictionary<String, Object> sectionItem in sectionsFromJSON)
                {
                    string title = (string)sectionItem["title"];
                    int id = Convert.ToInt32(sectionItem["id"]);
                    sections.Add(title, id);
                    RibbonDropDownItem temp = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                    temp.Label = title;
                    sectionDropDown.Items.Add(temp);
                }

                Debug.WriteLine("Start Course Info");
                Debug.WriteLine(result);
            }
            else
            {
                RibbonDropDownItem blank2 = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                RibbonDropDownItem blank3 = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                RibbonDropDownItem blank4 = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                blank2.Label = " ";
                blank3.Label = " ";
                blank4.Label = " ";
                quizDropDown.Items.Add(blank2);
                questionDropDown.Items.Add(blank3);
                sectionDropDown.Items.Add(blank4);
            }

            // checks for anychanges on the ribbon
            if (semesterDropDown.SelectedItem.Label != "Select Semester" && courseDropDown.SelectedItem.Label != "Select Course" && quizDropDown.SelectedItem.Label != "Select Quiz" && questionDropDown.SelectedItem.Label != "Select Question" && sectionDropDown.SelectedItem.Label != "Select Section")
            {
                addslideButton.Enabled = true;
                Debug.WriteLine("Enabled");
            }
            if (semesterDropDown.SelectedItem.Label == "Select Semester" || courseDropDown.SelectedItem.Label == "Select Course" || quizDropDown.SelectedItem.Label == "Select Quiz" || questionDropDown.SelectedItem.Label == "Select Question" || sectionDropDown.SelectedItem.Label == "Select Section")
            {
                addslideButton.Enabled = false;
                Debug.WriteLine("Disabled");
            }
            if ((semesterDropDown.Items.Count > 0 && semesterDropDown.SelectedItem.Label == " ") ||
                   (courseDropDown.Items.Count > 0 && courseDropDown.SelectedItem.Label == " ") ||
                   (quizDropDown.Items.Count > 0 && quizDropDown.SelectedItem.Label == " ") ||
                   (questionDropDown.Items.Count > 0 && questionDropDown.SelectedItem.Label == " ") ||
                   (sectionDropDown.Items.Count > 0 && sectionDropDown.SelectedItem.Label == " "))
            {
                addslideButton.Enabled = false;
                Debug.WriteLine("Disabled");
            }
        }

        private void quizDropDown_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
           questionDropDown.Items.Clear();
           questions.Clear();
           idandduration.Clear();
            

            if (quizDropDown.SelectedItem.Label != "Select Quiz")
            {
                string result;
                int quizID = quizzes[quizDropDown.SelectedItem.Label];
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add(HttpRequestHeader.Cookie, "jwt=" + jwt);
                    byte[] myData = client.UploadValues("http://52.41.106.241:1337/quiz/find/" + quizID, new System.Collections.Specialized.NameValueCollection()
                        {
                    { "email", "blank" },
                    { "password", "blank"}
                        });
                    result = System.Text.Encoding.UTF8.GetString(myData);
                }

                RibbonDropDownItem item = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                item.Label = "Select Question";
                questionDropDown.Items.Add(item);
                var myJSON = JsonConvert.DeserializeObject<Dictionary<String, Object>>(result);
                List<Dictionary<String, Object>> questionsFromJSON = JsonConvert.DeserializeObject<List<Dictionary<String, Object>>>(myJSON["questions"].ToString());
                Debug.WriteLine(questionsFromJSON.ToString());
                foreach (Dictionary<String, Object> questionsItem in questionsFromJSON)
                {
                    string title = (string)questionsItem["text"];
                    int id = Convert.ToInt32(questionsItem["id"]);
                    int duration = Convert.ToInt32(questionsItem["duration"]);  // gets the duration of the question
                    questions.Add(title, id);
                    idandduration.Add(id, duration);
                    RibbonDropDownItem temp = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                    temp.Label = title;
                    questionDropDown.Items.Add(temp);
                }

                Debug.WriteLine("Start Quiz Info");
                Debug.WriteLine(result);
               
            }
            else
            {
                RibbonDropDownItem blank3 = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                blank3.Label = " ";
                questionDropDown.Items.Add(blank3);
            }

            // checks for changes on the ribbon
            if (semesterDropDown.SelectedItem.Label != "Select Semester" && courseDropDown.SelectedItem.Label != "Select Course" && quizDropDown.SelectedItem.Label != "Select Quiz" && questionDropDown.SelectedItem.Label != "Select Question" && sectionDropDown.SelectedItem.Label != "Select Section")
            {
                addslideButton.Enabled = true;
                Debug.WriteLine("Enabled");
            }
            if (semesterDropDown.SelectedItem.Label == "Select Semester" || courseDropDown.SelectedItem.Label == "Select Course" || quizDropDown.SelectedItem.Label == "Select Quiz" || questionDropDown.SelectedItem.Label == "Select Question" || sectionDropDown.SelectedItem.Label == "Select Section")
            {
                addslideButton.Enabled = false;
                Debug.WriteLine("Disabled");
            }
            if ((semesterDropDown.Items.Count > 0 && semesterDropDown.SelectedItem.Label == " ") ||
                   (courseDropDown.Items.Count > 0 && courseDropDown.SelectedItem.Label == " ") ||
                   (quizDropDown.Items.Count > 0 && quizDropDown.SelectedItem.Label == " ") ||
                   (questionDropDown.Items.Count > 0 && questionDropDown.SelectedItem.Label == " ") ||
                   (sectionDropDown.Items.Count > 0 && sectionDropDown.SelectedItem.Label == " "))
            {
                addslideButton.Enabled = false;
                Debug.WriteLine("Disabled");
            }

        }

        private void sectionDropDown_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            // checks for changes on the ribbon
            selectedSection = sectionDropDown.SelectedItem.Label;
            Debug.WriteLine("Section Change" + " " + selectedSection);
            if (semesterDropDown.SelectedItem.Label != "Select Semester" && courseDropDown.SelectedItem.Label != "Select Course" && quizDropDown.SelectedItem.Label != "Select Quiz" && questionDropDown.SelectedItem.Label != "Select Question" && sectionDropDown.SelectedItem.Label != "Select Section")
            {
                addslideButton.Enabled = true;
                Debug.WriteLine("Enabled");
            }
            if (semesterDropDown.SelectedItem.Label == "Select Semester" || courseDropDown.SelectedItem.Label == "Select Course" || quizDropDown.SelectedItem.Label == "Select Quiz" || questionDropDown.SelectedItem.Label == "Select Question" || sectionDropDown.SelectedItem.Label == "Select Section")
            {
                addslideButton.Enabled = false;
                Debug.WriteLine("Disabled");
            }
            if ((semesterDropDown.Items.Count > 0 && semesterDropDown.SelectedItem.Label == " ") ||
                   (courseDropDown.Items.Count > 0 && courseDropDown.SelectedItem.Label == " ") ||
                   (quizDropDown.Items.Count > 0 && quizDropDown.SelectedItem.Label == " ") ||
                   (questionDropDown.Items.Count > 0 && questionDropDown.SelectedItem.Label == " ") ||
                   (sectionDropDown.Items.Count > 0 && sectionDropDown.SelectedItem.Label == " "))
            {
                addslideButton.Enabled = false;
                Debug.WriteLine("Disabled");
            }
        }

        private void questionDropDown_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            // checks for changes on the ribbon
            if (semesterDropDown.SelectedItem.Label != "Select Semester" && courseDropDown.SelectedItem.Label != "Select Course" && quizDropDown.SelectedItem.Label != "Select Quiz" && questionDropDown.SelectedItem.Label != "Select Question" && sectionDropDown.SelectedItem.Label != "Select Section")
            {
                addslideButton.Enabled = true;
                Debug.WriteLine("Enabled");
            }
            if (semesterDropDown.SelectedItem.Label == "Select Semester" || courseDropDown.SelectedItem.Label == "Select Course" || quizDropDown.SelectedItem.Label == "Select Quiz" || questionDropDown.SelectedItem.Label == "Select Question" || sectionDropDown.SelectedItem.Label == "Select Section")
            {
                addslideButton.Enabled = false;
                Debug.WriteLine("Disabled");
            }
            if ((semesterDropDown.Items.Count > 0 && semesterDropDown.SelectedItem.Label == " ") ||
                   (courseDropDown.Items.Count > 0 && courseDropDown.SelectedItem.Label == " ") ||
                   (quizDropDown.Items.Count > 0 && quizDropDown.SelectedItem.Label == " ") ||
                   (questionDropDown.Items.Count > 0 && questionDropDown.SelectedItem.Label == " ") ||
                   (sectionDropDown.Items.Count > 0 && sectionDropDown.SelectedItem.Label == " "))
            {
                addslideButton.Enabled = false;
                Debug.WriteLine("Disabled");
            }
        }

        private void semesterDropDown_SelectionChanged(object sender, RibbonControlEventArgs e)
        {

            // if you make a change to the semester it should clear everything
            courseDropDown.Items.Clear();
            quizDropDown.Items.Clear();
            questionDropDown.Items.Clear();
            sectionDropDown.Items.Clear();
            quizzes.Clear();
            questions.Clear();
            idandduration.Clear();
            sections.Clear();

           
            //courseDropDown.Items.Clear();

            if (semesterDropDown.SelectedItem.Label != "Select Semester")
            {
                RibbonDropDownItem item = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                item.Label = "Select Course"; // Select course
                courseDropDown.Items.Add(item);
                int term = -1;
                foreach (KeyValuePair<int, String> entry in seasons)
                {
                    if (entry.Value.Equals(semesterDropDown.SelectedItem.Label))
                    {
                        term = entry.Key;
                        break;
                    }
                }
                foreach (KeyValuePair<String, int> entry in coursesToTerm)
                {
                    if (entry.Value == term)
                    {
                        RibbonDropDownItem temp = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                        temp.Label = entry.Key;
                        courseDropDown.Items.Add(temp);
                    }
                    
                }
            }
            else
            {
                RibbonDropDownItem blank1 = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                RibbonDropDownItem blank2 = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                RibbonDropDownItem blank3 = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                RibbonDropDownItem blank4 = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                blank1.Label = " "; // Select course
                blank2.Label = " ";
                blank3.Label = " ";
                blank4.Label = " ";
                courseDropDown.Items.Add(blank1);
                quizDropDown.Items.Add(blank2);
                questionDropDown.Items.Add(blank3);
                sectionDropDown.Items.Add(blank4);

            }

                // checks for changes in the ribbon
             if (semesterDropDown.SelectedItem.Label != "Select Semester" && courseDropDown.SelectedItem.Label != "Select Course" && quizDropDown.SelectedItem.Label != "Select Quiz" && questionDropDown.SelectedItem.Label != "Select Question" && sectionDropDown.SelectedItem.Label != "Select Section" ) 
            {
                addslideButton.Enabled = true;
                Debug.WriteLine("Enabled");
            }
            if (semesterDropDown.SelectedItem.Label == "Select Semester" || courseDropDown.SelectedItem.Label == "Select Course" || quizDropDown.SelectedItem.Label == "Select Quiz" || questionDropDown.SelectedItem.Label == "Select Question" || sectionDropDown.SelectedItem.Label == "Select Section")
            {
                addslideButton.Enabled = false;
                Debug.WriteLine("Disabled");
            }
            if ((semesterDropDown.Items.Count > 0 && semesterDropDown.SelectedItem.Label == " ") ||
                   (courseDropDown.Items.Count > 0 && courseDropDown.SelectedItem.Label == " ") ||
                   (quizDropDown.Items.Count > 0 && quizDropDown.SelectedItem.Label == " ") ||
                   (questionDropDown.Items.Count > 0 && questionDropDown.SelectedItem.Label == " ") ||
                   (sectionDropDown.Items.Count > 0 && sectionDropDown.SelectedItem.Label == " "))
            {
                addslideButton.Enabled = false;
                Debug.WriteLine("Disabled");
            }

        }
    }
}
