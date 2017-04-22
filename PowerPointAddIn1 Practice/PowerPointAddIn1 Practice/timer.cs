using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerPointAddIn1_Practice
{
    public partial class timer : Form
    {
        public int timeLeft;
        
        public timer(int timetolive)
        {
            timeLeft = timetolive;
            InitializeComponent();
        }

        private void timer_Load(object sender, EventArgs e)
        {

        }

        public int returnTime()
        {

            return timeLeft;
        }

        private void countdownTimer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                // counts down
                timeLeft = timeLeft - 1;
                //timeLabel.Text = timeLeft + " seconds";
                if (timeLeft >= 60 && timeLeft < 120) // for the minutes
                {
                    if (timeLeft < 70)
                    {
                        timeLabel.Text = "1:0" + timeLeft;
                    }
                    else
                    {
                        timeLabel.Text = "1:" + timeLeft;
                    }

                }
                else if (timeLeft >= 120 && timeLeft < 180)
                {
                    if (timeLeft < 130)
                    {
                        timeLabel.Text = "2:0" + timeLeft;
                    }
                    else
                    {
                        timeLabel.Text = "2:" + timeLeft;
                    }
                }
                else if (timeLeft >= 180 && timeLeft < 240)
                {
                    if (timeLeft < 190)
                    {
                        timeLabel.Text = "3:0" + timeLeft;
                    }
                    else
                    {
                        timeLabel.Text = "3:" + timeLeft;
                    }
                }
                else if (timeLeft >= 240 && timeLeft < 300)
                {
                    if (timeLeft < 250)
                    {
                        timeLabel.Text = "4:0" + timeLeft;
                    }
                    else
                    {
                        timeLabel.Text = "4:" + timeLeft;
                    }
                }
                else if (timeLeft >= 300 && timeLeft < 360)
                {
                    if (timeLeft < 310)
                    {
                        timeLabel.Text = "5:0" + timeLeft;
                    }
                    else
                    {
                        timeLabel.Text = "5:" + timeLeft;
                    }
                }
                else // under a minute
                {
                    if (timeLeft < 10)
                    {
                        timeLabel.Text = "0:0" + timeLeft;
                    }
                    else
                    {
                        timeLabel.Text = "0:" + timeLeft;
                    }
                    
                }
            }
            else
            {
                // If the user ran out of time, stop the timer, and close the dialog box
                countdownTimer.Stop();
               
                timeLabel.Text = "Time's up!";
                this.Close();
                //MessageBox.Show("You didn't finish in time.", "Sorry!");
                //sum.Value = addend1 + addend2;
                
            }

        }

      
    }
}
