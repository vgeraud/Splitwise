using System.Collections.Generic;

namespace Splitwise.Model
{
    public class SaveResultModel<T>
    {
        public T Model { get; set; }
        public bool Success { get; set; }
        public IList<string> ErrorMessages { get; set; }
    }
}
