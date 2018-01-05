using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Linq.Dynamic;


namespace gcc_build_app
{
    public partial class fApp : Form
    {
        string m_gcc_PathEnvironment = "";

        void _init() {
            gcc_init();
            makefile_init();
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
    }
}
