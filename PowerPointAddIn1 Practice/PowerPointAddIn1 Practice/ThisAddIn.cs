using System.Diagnostics;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;


namespace PowerPointAddIn1_Practice
{
    public partial class ThisAddIn
    {
        // 
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            ((PowerPoint.EApplication_Event)this.Application).NewPresentation +=
            new Microsoft.Office.Interop.PowerPoint.EApplication_NewPresentationEventHandler(ThisAddIn_NewPresentation);

            ((PowerPoint.EApplication_Event)this.Application).SlideShowNextSlide +=
            new Microsoft.Office.Interop.PowerPoint.EApplication_SlideShowNextSlideEventHandler(ThisAddIn_SlideShowNextSlide);

        }

        public void ThisAddIn_SlideShowNextSlide(PowerPoint.SlideShowWindow Wn)
        {
            char[] delimiters = { '&' };
            string[] words = this.Application.SlideShowWindows[1].View.Slide.Name.Split(delimiters);
            
            //string hi = quizzlyRibbon.semesterDropDown.SelectedItem.Label;
            if (words.Length > 0 && words[0] == "question") // asks a question
            {
                Debug.WriteLine("im asking a question now" + "section:" + words[2]);

                quizzlyRibbon.askQuestion(words[1], words[2]);
            }

        }

        

        void ThisAddIn_NewPresentation (Microsoft.Office.Interop.PowerPoint.Presentation Pres)
        {
            
        }
        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }
        // need this function because this you have to select a new Presentation
        
        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
