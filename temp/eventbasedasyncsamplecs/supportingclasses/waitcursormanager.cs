//---------------------------------------------------------------------
//  This file is part of the Microsoft .NET Framework SDK Code Samples.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
// 
//This source code is intended only as a supplement to Microsoft
//Development Tools and/or on-line documentation.  See these other
//materials for detailed information regarding Microsoft code samples.
// 
//THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
//KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//PARTICULAR PURPOSE.
//---------------------------------------------------------------------
#region Using directives

using System;
using System.Windows.Forms;

#endregion

namespace Microsoft.Samples.Windows.Forms.EventBasedAsync
{
    
    internal class WaitCursorManager : IDisposable {

        private Form f = null;
        private Cursor stored = null;

        internal WaitCursorManager(Form f) {
            this.f = f;
            this.stored = f.Cursor;
            f.Cursor = Cursors.WaitCursor;
        }

        public void Dispose() {
            f.Cursor = stored;
            f = null;
            stored = null;
        }
    }
}
