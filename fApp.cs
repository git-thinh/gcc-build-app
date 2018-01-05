using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace gcc_build_app
{
    public partial class fApp : Form
    {
        void _init() {
            tab_makefile_init();
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

        #region [ === TAB: Makefile === ]

        void tab_makefile_init(){
            makefile_Select_FileType.SelectedIndex = 0;
        }

        #endregion
    }
}
