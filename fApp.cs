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

        void _init()
        {
            gcc_init();
            makefile_init();
            define_BindUI();
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
            this.Top = 44;
            this.Text = _CONST.APP_NAME;
            this.Width = _CONST.APP_WIDTH;
            this.Height = _CONST.APP_HEIGHT;
        }

        #endregion

        #region [ === TAB: Define === ]

        void define_BindUI()
        {
            define_Name_Text.Text = "";
            var dfs = App.GetData().Defines;
            tabDefine.Tag = dfs;
            define_ListItem.Items.Clear();
            foreach (string fi in dfs)
                define_ListItem.Items.Add(fi, false);
        }

        private void define_Button_Add_Click(object sender, EventArgs e)
        {
            if (App.define_Add(define_Name_Text.Text.Trim()))
                define_BindUI();
        }

        private void define_Button_Remove_Click(object sender, EventArgs e)
        {
            if (App.define_Remove(define_Name_Text.Text.Trim()))
                define_BindUI();
        }

        private void define_Button_SAVE_Click(object sender, EventArgs e)
        {
            App.WriteFile();
        }

        private void define_Button_Cancel_Click(object sender, EventArgs e)
        {
            //App.GetData().Defines = App.get_DefineDefault();

        }
        private void define_ListItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            define_Name_Text.Text = define_ListItem.SelectedItem.ToString();
        }

        #endregion

        #region [ === TAB: GCC === ]

        oGCC gcc_LoadDefault()
        {
            string path = Environment.GetEnvironmentVariable("path").Split(';')
                .Select(x => x.ToLower())
                .Where(x => x.Contains("mingw"))
                .Take(1)
                .SingleOrDefault();
            if (!string.IsNullOrEmpty(path))
            {
                if (path.EndsWith("\\bin"))
                    path = path.Substring(0, path.Length - 4);
                return new oGCC(path);
            }
            return null;
        }

        void gcc_init()
        {
            oGCC gcc = null;
            if (App.GetData().GCCs.Count > 0)
                gcc = App.GetData().GCCs[0];
            else
            {
                string path = Environment.GetEnvironmentVariable("path").Split(';')
                    .Select(x => x.ToLower())
                    .Where(x => x.Contains("mingw"))
                    .Take(1)
                    .SingleOrDefault();
                if (!string.IsNullOrEmpty(path))
                {
                    if (path.EndsWith("\\bin"))
                        path = path.Substring(0, path.Length - 4);
                    gcc = gcc_LoadDefault();
                }
            }

            if (gcc != null)
                gcc_BindUI(gcc);
        }

        void gcc_BindUI(oGCC gcc)
        {
            tabGCC.Tag = gcc;
            m_gcc_PathEnvironment = gcc.PathRoot;
            gcc_Label_Path.Text = gcc.PathRoot;
            gcc_Select_List.Text = gcc.Name;

            gcc_Binaries_ListItems_Bind(gcc);
            gcc_Libraries_ListItems_Bind(gcc);
            gcc_CppIncludes_ListItems_Bind(gcc);
            gcc_CIncludes_ListItems_Bind(gcc);
        }

        void gcc_Binaries_ListItems_Bind(oGCC gcc)
        {
            gcc_Binaries_ListItems.DataSource = null;
            gcc_Binaries_ListItems.DataSource = gcc.Binaries;
            tabGCC.Tag = gcc;
        }
        void gcc_Libraries_ListItems_Bind(oGCC gcc)
        {
            gcc_Libraries_ListItem.DataSource = null;
            gcc_Libraries_ListItem.DataSource = gcc.Libraries;
            tabGCC.Tag = gcc;
        }
        void gcc_CIncludes_ListItems_Bind(oGCC gcc)
        {
            gcc_CIncludes_ListItem.DataSource = null;
            gcc_CIncludes_ListItem.DataSource = gcc.C_Includes;
            tabGCC.Tag = gcc;
        }
        void gcc_CppIncludes_ListItems_Bind(oGCC gcc)
        {
            gcc_CppIncludes_ListItem.DataSource = null;
            gcc_CppIncludes_ListItem.DataSource = gcc.Cpp_Includes;
            tabGCC.Tag = gcc;
        }

        private void gcc_Binaries_Add_Button_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    oGCC gcc = (oGCC)tabGCC.Tag;
                    gcc.Binaries.Add(fbd.SelectedPath.ToLower());
                    gcc_Binaries_ListItems_Bind(gcc);
                }
            }
        }

        private void gcc_Binaries_Remove_Button_Click(object sender, EventArgs e)
        {
            if (gcc_Binaries_ListItems.SelectedItem != null)
            {
                string _binaries = gcc_Binaries_ListItems.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(_binaries))
                {
                    oGCC gcc = (oGCC)tabGCC.Tag;
                    gcc.Binaries.Remove(_binaries);
                    gcc_Binaries_ListItems_Bind(gcc);
                }
            }
        }

        private void gcc_Button_SelectPath_Click(object sender, EventArgs e)
        {

        }

        private void gcc_Libraries_Add_Button_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    oGCC gcc = (oGCC)tabGCC.Tag;
                    gcc.Libraries.Add(fbd.SelectedPath.ToLower());
                    gcc_Libraries_ListItems_Bind(gcc);
                }
            }
        }

        private void gcc_Libraries_Remove_Button_Click(object sender, EventArgs e)
        {
            if (gcc_Binaries_ListItems.SelectedItem != null)
            {
                string _itemSelect = gcc_Libraries_ListItem.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(_itemSelect))
                {
                    oGCC gcc = (oGCC)tabGCC.Tag;
                    gcc.Libraries.Remove(_itemSelect);
                    gcc_Libraries_ListItems_Bind(gcc);
                }
            }
        }

        private void gcc_CIncludes_Add_Button_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    oGCC gcc = (oGCC)tabGCC.Tag;
                    gcc.C_Includes.Add(fbd.SelectedPath.ToLower());
                    gcc_CIncludes_ListItems_Bind(gcc);
                }
            }
        }

        private void gcc_CIncludes_Remove_Button_Click(object sender, EventArgs e)
        {
            if (gcc_Binaries_ListItems.SelectedItem != null)
            {
                string _itemSelect = gcc_CIncludes_ListItem.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(_itemSelect))
                {
                    oGCC gcc = (oGCC)tabGCC.Tag;
                    gcc.C_Includes.Remove(_itemSelect);
                    gcc_CIncludes_ListItems_Bind(gcc);
                }
            }
        }

        private void gcc_CppIncludes_Add_Button_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    oGCC gcc = (oGCC)tabGCC.Tag;
                    gcc.Cpp_Includes.Add(fbd.SelectedPath.ToLower());
                    gcc_CppIncludes_ListItems_Bind(gcc);
                }
            }
        }

        private void gcc_CppIncludes_Remove_Button_Click(object sender, EventArgs e)
        {
            if (gcc_Binaries_ListItems.SelectedItem != null)
            {
                string _itemSelect = gcc_CppIncludes_ListItem.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(_itemSelect))
                {
                    oGCC gcc = (oGCC)tabGCC.Tag;
                    gcc.Cpp_Includes.Remove(_itemSelect);
                    gcc_CppIncludes_ListItems_Bind(gcc);
                }
            }
        }

        private void gcc_SAVE_Button_Click(object sender, EventArgs e)
        {
            if (tabGCC.Tag != null)
            {
                App.gcc_Update((oGCC)tabGCC.Tag);
                App.WriteFile();
            }
        }

        private void gcc_Cancel_Button_Click(object sender, EventArgs e)
        {
            oGCC gcc = gcc_LoadDefault();
            gcc_BindUI(gcc);
            App.gcc_Update(gcc);
        }

        #endregion

        #region [ === TAB: Makefile === ]

        void makefile_init()
        {
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
                }
            }
        }

        #endregion






    }
}
