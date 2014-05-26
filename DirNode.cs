using System;
using System.Collections.Generic;
using System.Text;

namespace WeCode1._0
{
    public class DirNode : System.Windows.Forms.TreeNode
    {
        public DirNode()
        {

        }

        public bool SubDirectoriesAdded;
        public DirNode(string text)
            : base(text)
        {

        }
    }
}
