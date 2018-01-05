using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Linq.Dynamic;
using System.IO;


namespace gcc_build_app
{
    public partial class fApp : Form
    {
        string m_gcc_PathEnvironment = "";

        void _init() {
            gcc_init();
            makefile_init();
            module_init();
        }

        #region [ === MAIN ==== ]

        public fApp()
        {
            InitializeComponent();
            app_init();
            _init();
        }

        private void fApp_Load(object sender, EventArgs e)
        {

        }

        void app_init() 
        {
            this.Text = _CONST.APP_NAME;
            this.Width = _CONST.APP_WIDTH;
            this.Height = _CONST.APP_HEIGHT;
        }

        #endregion

        #region [ === TAB: GCC === ]

        void gcc_init(){
            m_gcc_PathEnvironment = Environment.GetEnvironmentVariable("path").Split(';')
                .Where(x => x.ToLower().Contains("mingw"))
                .Take(1)
                .SingleOrDefault();
            gcc_Label_Path.Text = m_gcc_PathEnvironment;
            
        }

        #endregion

        #region [ === TAB: Makefile === ]

        void makefile_init(){
            makefile_Select_FileType.SelectedIndex = 0;
        }

        #endregion

        #region [ === TAB: Module === ]

        void module_init() 
        {
            module_Type_Select.SelectedIndex = 0;
        }

        private void module_Button_CloneMakefile_Click(object sender, EventArgs e)
        {

        }

        private void module_Button_CreateNewMakefile_Click(object sender, EventArgs e)
        {

        }

        private void module_Button_SelectModule_Click(object sender, EventArgs e)
        {

        }

        private void module_Button_UpdateModule_Click(object sender, EventArgs e)
        {

        }

        private void module_Button_RemoveModule_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region [ ==== ToolBarMain === ]

        private void tool_Button_CreateMakeFile_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = tabMakefile;
        }

        #endregion

        #region [ === TAB: Project === ]

        private void prj_option_Button_Select_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath);

                    System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
                }
            }
        }

        #endregion
    }
}
