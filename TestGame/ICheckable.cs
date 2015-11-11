using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGame
{
    public interface ICheckable
    {
        bool IsChecked(GameObject checkedObject);
        void OnChecked();
    }
}
