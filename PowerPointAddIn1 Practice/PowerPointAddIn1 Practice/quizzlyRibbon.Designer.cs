namespace PowerPointAddIn1_Practice
{
    partial class quizzlyRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public quizzlyRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        public void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(quizzlyRibbon));
            this.quizzlyTab = this.Factory.CreateRibbonTab();
            this.loginGroup = this.Factory.CreateRibbonGroup();
            this.loginButton = this.Factory.CreateRibbonButton();
            this.LoggedInLabel = this.Factory.CreateRibbonLabel();
            this.quizzesGroup = this.Factory.CreateRibbonGroup();
            this.semesterDropDown = this.Factory.CreateRibbonDropDown();
            this.courseDropDown = this.Factory.CreateRibbonDropDown();
            this.sectionDropDown = this.Factory.CreateRibbonDropDown();
            this.quizDropDown = this.Factory.CreateRibbonDropDown();
            this.questionDropDown = this.Factory.CreateRibbonDropDown();
            this.addslideButton = this.Factory.CreateRibbonButton();
            this.helpGroup = this.Factory.CreateRibbonGroup();
            this.quizHelp = this.Factory.CreateRibbonLabel();
            this.sectionHelp = this.Factory.CreateRibbonLabel();
            this.quizzlyTab.SuspendLayout();
            this.loginGroup.SuspendLayout();
            this.quizzesGroup.SuspendLayout();
            this.helpGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // quizzlyTab
            // 
            this.quizzlyTab.Groups.Add(this.loginGroup);
            this.quizzlyTab.Groups.Add(this.quizzesGroup);
            this.quizzlyTab.Groups.Add(this.helpGroup);
            this.quizzlyTab.Label = "Quizzly";
            this.quizzlyTab.Name = "quizzlyTab";
            // 
            // loginGroup
            // 
            this.loginGroup.Items.Add(this.loginButton);
            this.loginGroup.Items.Add(this.LoggedInLabel);
            this.loginGroup.Name = "loginGroup";
            // 
            // loginButton
            // 
            this.loginButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.loginButton.Image = ((System.Drawing.Image)(resources.GetObject("loginButton.Image")));
            this.loginButton.Label = "LogIn";
            this.loginButton.Name = "loginButton";
            this.loginButton.ShowImage = true;
            this.loginButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.loginButton_Click);
            // 
            // LoggedInLabel
            // 
            this.LoggedInLabel.Label = "Logged In!";
            this.LoggedInLabel.Name = "LoggedInLabel";
            this.LoggedInLabel.Visible = false;
            // 
            // quizzesGroup
            // 
            this.quizzesGroup.Items.Add(this.semesterDropDown);
            this.quizzesGroup.Items.Add(this.courseDropDown);
            this.quizzesGroup.Items.Add(this.sectionDropDown);
            this.quizzesGroup.Items.Add(this.quizDropDown);
            this.quizzesGroup.Items.Add(this.questionDropDown);
            this.quizzesGroup.Items.Add(this.addslideButton);
            this.quizzesGroup.Label = "Quizzes";
            this.quizzesGroup.Name = "quizzesGroup";
            // 
            // semesterDropDown
            // 
            this.semesterDropDown.Label = "Semester";
            this.semesterDropDown.Name = "semesterDropDown";
            this.semesterDropDown.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.semesterDropDown_SelectionChanged);
            // 
            // courseDropDown
            // 
            this.courseDropDown.Label = "Course";
            this.courseDropDown.Name = "courseDropDown";
            this.courseDropDown.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.courseDropDown_SelectionChanged);
            // 
            // sectionDropDown
            // 
            this.sectionDropDown.Label = "Section";
            this.sectionDropDown.Name = "sectionDropDown";
            this.sectionDropDown.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.sectionDropDown_SelectionChanged);
            // 
            // quizDropDown
            // 
            this.quizDropDown.Label = "Quiz";
            this.quizDropDown.Name = "quizDropDown";
            this.quizDropDown.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.quizDropDown_SelectionChanged);
            // 
            // questionDropDown
            // 
            this.questionDropDown.Label = "Question";
            this.questionDropDown.Name = "questionDropDown";
            this.questionDropDown.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.questionDropDown_SelectionChanged);
            // 
            // addslideButton
            // 
            this.addslideButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.addslideButton.Enabled = false;
            this.addslideButton.Image = ((System.Drawing.Image)(resources.GetObject("addslideButton.Image")));
            this.addslideButton.Label = "Add Slide";
            this.addslideButton.Name = "addslideButton";
            this.addslideButton.ShowImage = true;
            this.addslideButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.addslideButton_Click);
            // 
            // helpGroup
            // 
            this.helpGroup.Items.Add(this.quizHelp);
            this.helpGroup.Items.Add(this.sectionHelp);
            this.helpGroup.Label = "Help";
            this.helpGroup.Name = "helpGroup";
            // 
            // quizHelp
            // 
            this.quizHelp.Label = "- Please select \'Semester\', \'Course\', \'Quiz\', \'Question\', and \'Section\' to add ne" +
    "w slide.";
            this.quizHelp.Name = "quizHelp";
            // 
            // sectionHelp
            // 
            this.sectionHelp.Label = "- Please select a \'Section\' to ask questions to before starting entered Presentat" +
    "ion Mode.";
            this.sectionHelp.Name = "sectionHelp";
            // 
            // quizzlyRibbon
            // 
            this.Name = "quizzlyRibbon";
            this.RibbonType = "Microsoft.PowerPoint.Presentation";
            this.Tabs.Add(this.quizzlyTab);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon2_Load);
            this.quizzlyTab.ResumeLayout(false);
            this.quizzlyTab.PerformLayout();
            this.loginGroup.ResumeLayout(false);
            this.loginGroup.PerformLayout();
            this.quizzesGroup.ResumeLayout(false);
            this.quizzesGroup.PerformLayout();
            this.helpGroup.ResumeLayout(false);
            this.helpGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab quizzlyTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup quizzesGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown courseDropDown;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown quizDropDown;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown questionDropDown;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown sectionDropDown;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton addslideButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup loginGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton loginButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel LoggedInLabel;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup helpGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel sectionHelp;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel quizHelp;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown semesterDropDown;
    }

    partial class ThisRibbonCollection
    {
        internal quizzlyRibbon Ribbon2
        {
            get { return this.GetRibbon<quizzlyRibbon>(); }
        }
    }
}
