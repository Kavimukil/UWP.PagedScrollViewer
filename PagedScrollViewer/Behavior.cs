using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace PagedScrollViewer
{
    public class Behavior<T> : DependencyObject, IBehavior where T : DependencyObject
    {
        DependencyObject IBehavior.AssociatedObject => AssociatedObject;
        public T AssociatedObject
        {
            get; set;
        }

        public virtual void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = (T)(associatedObject);
        }

        public virtual void Detach()
        {

        }
    }
}
