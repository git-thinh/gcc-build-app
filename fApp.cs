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
using ProtoBuf;


namespace gcc_build_app
{
    public partial class fApp : Form
    {
        List<oApp> _listApps = new List<oApp>() { };
        oApp _app = null;

        string m_compile_PathOutput = @"c:\gcc-build";

        string m_explorer_PathCurrent = "";
        string m_gcc_PathEnvironment = "";
        string m_build_PathRoot = "";
        string m_build_PathRootCpp = "";
        string m_build_PathFileCurrent = "";

        List<string> _listFileH = new List<string>() { };
        List<string> _listFileCpp = new List<string>() { };

        void _init()
        {
            if (!Directory.Exists(m_compile_PathOutput)) Directory.CreateDirectory(m_compile_PathOutput);
            m_build_PathRoot = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            gcc_init();
            makefile_init();
            define_BindUI();
            module_init();

            project_loadInit();
        }

        #region [ === MAIN ==== ]

        public fApp()
        {
            InitializeComponent();
            app_init();
            //this.Shown += (se, ev) => { this.Top = 44; };
        }

        private void fApp_Load(object sender, EventArgs e)
        {
            _init();
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
            if (define_ListItem.SelectedItem != null)
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

        #region [ === TAB: Project === ]

        void project_loadInit() {
            _listApps = Directory.GetFiles(m_build_PathRoot, "*.bgcc")
                .Select(x => Serializer.Deserialize<oApp>(File.OpenRead(x)))
                .Where(x => x != null && Directory.Exists(x.PathRootCpp))
                .ToList();
            file_Build_ListItem.DataSource = null;
            file_Build_ListItem.DataSource = _listApps.Select(x => x.Name).ToArray();
            if (_listApps.Count > 0)
            {
                _app = _listApps[0];
                project_BindUI();
            }
            else {
                fileBrowser1.SelectPath(@"C:\", true);
            }
        }

        void project_BindUI() {
            project_ItemSelect_PathRootCpp_TextBox.Text = _app.PathRootCpp;
            project_ItemSelect_Name_TextBox.Text = _app.Name;
            fileBrowser1.SelectPath(_app.PathRootCpp, true);
        }


        void project_getFiles(List<string> list, string path, string searchPattern)
        {
            string[] fs = Directory.GetFiles(path, searchPattern);
            list.AddRange(fs);
            foreach (string fr in Directory.GetDirectories(path))
                project_getFiles(list, fr, searchPattern);
        }

        private void project_Add_Button_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    string path = fbd.SelectedPath;
                    string projectName = Path.GetFileName(path);

                    TreeNode nodeProject = new TreeNode(projectName);
                    TreeNode nodeBuild = new TreeNode("Builds");
                    TreeNode nodeFiles = new TreeNode("Files");
                    TreeNode nodeFiles_H = new TreeNode("Header");
                    TreeNode nodeFiles_CPP = new TreeNode("Source");


                    nodeFiles_H.Nodes.AddRange(Directory.GetDirectories(path)
                        .Select(x => new TreeNode(Path.GetFileName(x)) { Tag = x })
                        .ToArray());
                    nodeFiles_CPP.Nodes.AddRange(Directory.GetDirectories(path)
                        .Select(x => new TreeNode(Path.GetFileName(x)) { Tag = x })
                        .ToArray());

                    nodeFiles_H.Nodes.AddRange(Directory.GetFiles(path, "*.h")
                        .Select(x => new TreeNode(Path.GetFileName(x)))
                        .ToArray());
                    nodeFiles_CPP.Nodes.AddRange(Directory.GetFiles(path, "*.cpp")
                        .Select(x => new TreeNode(Path.GetFileName(x)))
                        .ToArray());


                    List<string> list = new List<string>() { };
                    project_getFiles(list, path, "*.h");
                    nodeFiles.Nodes.AddRange(new TreeNode[] { nodeFiles_H, nodeFiles_CPP });

                    nodeProject.Nodes.AddRange(new TreeNode[] { nodeBuild, nodeFiles });
                    //nodeProject.ExpandAll();
                    //treeProject.Nodes.Add(nodeProject);
                }
            }
        }

        private void project_tree_save_button_Click(object sender, EventArgs e)
        {

        }

        private void build_AddNew_Button_Click(object sender, EventArgs e)
        {
            build_AddNew();
        }
         

        void build_AddNew() {
            if (!Directory.Exists(m_explorer_PathCurrent))
            {
                MessageBox.Show("Please chose folder contain source CPP in tab Explorer!", _CONST.APP_NAME);
                return;
            }
            string defValue = Path.GetFileName(m_explorer_PathCurrent);
            string name = Prompt.ShowDialog("Input build file name:", _CONST.APP_NAME, defValue);
            if (!string.IsNullOrEmpty(name)) {
                string _file = Path.Combine(m_build_PathRoot, name + ".bgcc");
                if (!File.Exists(_file))
                {
                    _app = new oApp() { Name = name, PathRootCpp = m_explorer_PathCurrent };
                    if(tabGCC.Tag != null) _app.update_GCC((oGCC)tabGCC.Tag);
                    if (tabDefine.Tag != null) _app.set_Define((List<string>)tabDefine.Tag);

                    using (Stream ms = new FileStream(_file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                        Serializer.Serialize(ms, _app);
                    project_loadInit();
                    int index = _listApps.FindIndex(x => x.Name == name);
                    if (index >= 0)
                    {
                        _app = _listApps[index];
                        project_BindUI();
                    }
                } else {
                    MessageBox.Show("File " + name + ".bgcc exist, Input other name..." , _CONST.APP_NAME);
                    build_AddNew();
                }
            }
        }

        void build_RUN() {
            if (File.Exists(m_build_PathFileCurrent))
            {

            }
            else
            {
                MessageBox.Show("Can not select file *.bgcc to build", _CONST.APP_NAME);
            }
        }

        private void fileBrowser1_SelectedFolderChanged(object sender, SelectedFolderChangedEventArgs e)
        {
            string path = e.Path;
            if (path[1] == ':' && path[2] == '\\')
            {
                m_explorer_PathCurrent = path; 
            }
        }


        private void file_Build_ListItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (file_Build_ListItem.SelectedItem != null)
            {
                string name = file_Build_ListItem.SelectedItem.ToString();
                int index = _listApps.FindIndex(x => x.Name == name);
                if (index >= 0) {
                    _app = _listApps[index];
                    project_BindUI();
                }
            }
        }

        private void build_Button_Compile_Click(object sender, EventArgs e)
        {

        }

        #endregion





    }
}
