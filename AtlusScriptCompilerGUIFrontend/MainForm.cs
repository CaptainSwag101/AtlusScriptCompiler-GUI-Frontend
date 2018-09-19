using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtlusScriptCompilerGUIFrontend
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            comboGame.DataSource = gamesDropdown;
            comboGame.SelectedIndex = 1;
        }

        public List<string> gamesDropdown = new List<string>()
        {
            "Persona 4",
            "Persona 5"
        };

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void CompileDecompileDragDrop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (string filePath in fileList)
            {
                if (!File.Exists("AtlusScriptCompiler.exe"))
                {
                    MessageBox.Show("Could not find AtlusScriptCompiler.exe. Put this program in the same folder and try running it again!", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string compileArg = "";
                string encodingArg = "";
                string libraryArg = "";
                string outFormatArg = "";

                string ext = Path.GetExtension(filePath.ToUpper());
                if (ext == ".MSG" || ext == ".FLOW")
                    compileArg = "-Compile";
                else if (ext == ".BMD" || ext == ".BF")
                    compileArg = "-Decompile";

                switch (comboGame.SelectedIndex)
                {
                    case 0: //P4
                        encodingArg = "-Encoding P4";
                        if (ext != ".BMD" && ext != ".MSG")
                            libraryArg = "-Library P4";
                        if (ext == ".MSG" || ext == ".FLOW")
                            outFormatArg = "-OutFormat V1";
                        break;
                    case 1: //P5
                        encodingArg = "-Encoding P5";
                        if (ext != ".BMD" && ext != ".MSG")
                            libraryArg = "-Library P5";
                        if (ext == ".MSG" || ext == ".FLOW")
                            outFormatArg = "-OutFormat V1BE";
                        break;
                }

                StringBuilder args = new StringBuilder();
                args.Append("AtlusScriptCompiler.exe ");
                args.Append($"\"{filePath}\" ");
                args.Append($"{compileArg} ");
                args.Append($"{outFormatArg} ");
                args.Append($"{libraryArg} ");
                args.Append($"{encodingArg} ");

                System.Diagnostics.Process.Start("cmd.exe", $"/c {args.ToString()}");
            }
        }

        private void CompileDecompileDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
    }
}
