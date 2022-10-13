using KaelsToolbox.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaelsToolbox.WinForms
{
    /// <summary>
    /// a better console implimentation with minimal effort to setup
    /// </summary>
    public class Console_Plus
    {
        /// <summary>
        /// event fired when valid input, 
        /// [StringEventArgs] is messege, 
        /// [sender] is curent name of window
        /// </summary>
        public event EventHandler<GenericEventArgs<string>> textSent;

        // strings from other threads that want to be written //
        private List<string> que_to_print = new List<string>();

        private RichTextBox textBox;
        private ComboBox inputBox;

        /// <summary>
        /// Wether or not to handle puting msg into console.
        /// Default = true
        /// </summary>
        public bool HandleInput = true;

        private string windowName;
        private string saveLocation = "";

        /// <summary>
        /// default color
        /// </summary>
        public Color textColor = Color.Black;

        /// <summary>
        /// Initialize the conosle
        /// </summary>
        /// <param name="textBox">the [rich text box] that will be the output</param>
        /// <param name="inputBox">where the user types</param>
        /// <param name="windowName">the name of the save-file.rtf</param>
        public Console_Plus(RichTextBox textBox, ComboBox inputBox, string windowName)
        {
            this.textBox = textBox;
            this.inputBox = inputBox;
            this.windowName = windowName;
            saveLocation = $"{Directory.GetCurrentDirectory()}\\Saves";

            // setup events
            textBox.TextChanged += new EventHandler(TextChanged);
            inputBox.KeyDown += new KeyEventHandler(KeyPressed);
            inputBox.KeyDown += new KeyEventHandler(Input_Clear);

            textBox.TabStop = false;

            Directory.CreateDirectory(saveLocation);
        }


        #region thredding
        /// <summary>
        /// only 1 timer allowed
        /// </summary>
        public bool HasTimer { get; private set; }
        /// <summary>
        /// Add timer to get input form other Threds every X millisecconds
        /// </summary>
        /// <param name="millisecconds">time between checks for other threds input</param>
        public void AddUpdateTimer(int millisecconds)
        {
            if (HasTimer) return;
            Timer timer = new Timer();
            timer.Interval = millisecconds;
            timer.Tick += Timer_Tick;
            timer.Start();
            HasTimer = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //if (que_to_print.Count > 0)
            //{
            //    InputText(que_to_print[0]);
            //    que_to_print.RemoveAt(0);
            //}

            int length = que_to_print.Count;
            string[] list = new string[length];
            for (int i = length - 1; i >= 0; i--)
            {
                list[i] = que_to_print[i];
                que_to_print.RemoveAt(i);
            }
        }

        /// <summary>
        /// adds text to print que, will send it when timer ticks
        /// </summary>
        /// <param name="text"></param>
        public void InputText_Thredded(string text)
        {
            que_to_print.Add(text);
        }
        #endregion

        /// <summary>
        /// WARNING!!! Changes what filename Save and Load look for
        /// </summary>
        public void ChangeWindowName(string newName)
        {
            windowName = newName;
        }

        private void Input_Clear(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                // clear the input feild
                inputBox.ResetText();
                // stops this event from being called alot by 
                // saying the key has finished pressing
                e.SuppressKeyPress = true;
            }
        }
        private void TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            textBox.SelectionStart = textBox.Text.Length;
            // scroll it automatically
            textBox.ScrollToCaret();
        }
        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (HandleInput)
            {
                if (e.KeyCode == Keys.Return && inputBox.Text.Length != 0)
                {
                    // write text to box
                    InputText(inputBox.Text);
                    textSent.Invoke(windowName, new GenericEventArgs<string>(inputBox.Text));
                    inputBox.Text = string.Empty;
                }
            }
        }



        /// <summary>
        /// directly send text to output with default color
        /// </summary>
        /// <param name="text">the text to send</param>
        public void InputText(string text)
        {
            int startingIndex = textBox.Text.Length;
            textBox.AppendText($"{text}\r\n");
            textBox.Select(startingIndex, text.Length);
            textBox.SelectionColor = textColor;
        }
        /// <summary>
        /// directly send text to output
        /// </summary>
        /// <param name="text">the text to send</param>
        /// <param name="color">the color to send it as</param>
        public void InputText(string text, Color color)
        {
            int startingIndex = textBox.Text.Length;
            textBox.AppendText($"{text}\r\n");
            textBox.Select(startingIndex, text.Length);
            textBox.SelectionColor = color;
        }

        /// <summary>
        /// clear the output box
        /// </summary>
        public void ClearConsole()
        {
            textBox.Clear();
        }

        /// <summary>
        /// save the output box to a file named after the window
        /// </summary>
        /// <param name="overwrite">wether or not to overwrite if file already exists</param>
        public void SaveWindow(bool overwrite = true)
        {
            textBox.SaveFile($"{saveLocation}\\{windowName}.rtf");
        }
        /// <summary>
        /// try and load a .rtf file named after the window
        /// </summary>
        public void LoadWindow()
        {
            if (File.Exists($"{saveLocation}\\{windowName}.rtf"))
            {
                textBox.LoadFile($"{saveLocation}\\{windowName}.rtf");
            }
        }
    }
}
